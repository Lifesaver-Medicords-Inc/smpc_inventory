using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;


namespace smpc_inventory_app.Pages
{
    public partial class WebSocketForm : Form
    {
        private WebSocket ws;
        public WebSocketForm()
        {
            InitializeComponent();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            ws = new WebSocket("ws://127.0.0.1:3000/api/ws/purchasing/redboxlist");

            ws.OnOpen += (s, ev) =>
            {
                MessageBox.Show("Connected to server!");
            };

            ws.OnMessage += (s, ev) =>
            {
                // Handle incoming message
                var message = ev.Data;
                MessageBox.Show("Received: " + message);

                // Optionally: parse JSON and do something with the data
            };

            ws.OnError += (s, ev) =>
            {
                MessageBox.Show("Error: " + ev.Message);
            };

            ws.OnClose += (s, ev) =>
            {
                MessageBox.Show("Disconnected from server!");
            };

            ws.Connect();
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (ws != null && ws.IsAlive)
            {
                ws.Send("Hello from C#!");
            }
        }
    }
}
