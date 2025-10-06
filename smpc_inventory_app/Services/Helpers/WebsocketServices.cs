using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using smpc_inventory_app.Data;
using smpc_inventory_app.Properties;

namespace smpc_inventory_app.Services.Helpers
{
    internal static class WebSocketServices
    {
        private static ClientWebSocket _ws;
        private static CancellationTokenSource _cts;
        private static string lastEndpoint;
        private static Func<string, Task> lastCallback;

        public static event Action OnConnected;
        public static event Action<string> OnError;
        public static event Action OnDisconnected;

        public static bool IsConnected => _ws?.State == WebSocketState.Open;

        public static async Task ConnectAndDeserialize<T>(string endpoint, Action<T> onDeserialized)
        {
            string token = CacheData.SessionToken;
            //string url = $"ws://94e9-112-201-111-220.ngrok-free.app/api/ws{endpoint}?Authorization={token}";
            string url = $"ws://127.0.0.1:3000/api/ws{endpoint}?Authorization={token}";

            if (IsConnected) return;

            _ws = new ClientWebSocket();
            _cts = new CancellationTokenSource();
            lastEndpoint = endpoint;
            lastCallback = async (data) =>
            {
                try
                {
                    T result = JsonConvert.DeserializeObject<T>(data);
                    onDeserialized?.Invoke(result);
                }
                catch (Exception ex)
                {
                    OnError?.Invoke("Deserialization failed: " + ex.Message);
                }
            };

            try
            {
                await _ws.ConnectAsync(new Uri(url), _cts.Token);
                OnConnected?.Invoke();
                _ = ReceiveLoop(_cts.Token); // Start reading in background
            }
            catch (Exception ex)
            {
                OnError?.Invoke("Connection failed: " + ex.Message);
                StartReconnect();
            }
        }


        private static async Task ReceiveLoop(CancellationToken token)
        {
            var buffer = new byte[8192];
            var ms = new MemoryStream();

            try
            {
                while (_ws.State == WebSocketState.Open && !token.IsCancellationRequested)
                {
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                        ms.Write(buffer, 0, result.Count);
                    }
                    while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", token);
                        OnDisconnected?.Invoke();
                        StartReconnect();
                        break;
                    }

                    var message = Encoding.UTF8.GetString(ms.ToArray());

                    if (message.Length > 1_000_000)
                    {
                        OnError?.Invoke("Message exceeds 1MB.");
                        continue;
                    }

                    ms.SetLength(0); // Reset for next message

                    if (lastCallback != null)
                        await lastCallback.Invoke(message);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke("Receive error: " + ex.Message);
                OnDisconnected?.Invoke();
                StartReconnect();
            }

        }

        private static void StartReconnect()
        {
            Task.Run(async () =>
            {
                await Task.Delay(10000); // Wait 10 seconds
                if (!IsConnected && lastEndpoint != null && lastCallback != null)
                {
                    await ConnectAndDeserialize(lastEndpoint, async (string data) => await lastCallback(data));
                }
            });
        }

        public static async Task SendAsync(string message)
        {
            if (_ws != null && _ws.State == WebSocketState.Open)
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public static async Task DisconnectAsync()
        {
            if (_ws != null)
            {
                try
                {
                    _cts?.Cancel();
                    await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnected", CancellationToken.None);
                }
                catch { }
                finally
                {
                    _ws.Dispose();
                    _ws = null;
                }
            }
        }
    }
}
