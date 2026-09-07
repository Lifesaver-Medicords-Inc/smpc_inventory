using Inventory_SMPC.Pages;
using Serilog;
using smpc_inventory_app.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app
{
    static class Program
    {
        // Backing fields, so the getters below can fall back when Main() never ran.
        private static string _apiBaseUrl;
        private static string _wssBaseUrl;

        // Resolve on read rather than only in Main().
        //
        // Forms from this assembly are hosted by OTHER apps - the sales app opens
        // frm_Item_Entry through a reference to smpc_inventory_app.exe, and engineering does
        // the same with other screens. In that case this assembly's Main() never executes, so
        // the property stayed null and every URL built from it came out host-less:
        // "/vfile/1788749394091360800.jpg" instead of "http://127.0.0.1:3000/api/vfile/...".
        // Item images silently fell back to the placeholder the moment the form rebound them
        // from the server, which is why an image looked fine until the item was saved
        // (user-reported 2026-09-05).
        //
        // ConfigurationManager reads the HOST PROCESS's config, so a form running inside the
        // sales app resolves the sales app's ApiBaseUrl - which is the correct answer, and
        // stays correct if the host is later repointed. Main() still assigns these on startup;
        // this only covers the case where it has not run.
        public static string ApiBaseUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_apiBaseUrl))
                    _apiBaseUrl = ResolveFromConfig("ApiBaseUrl");

                return _apiBaseUrl;
            }
            private set { _apiBaseUrl = value; }
        }

        public static string WssBaseUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_wssBaseUrl))
                    _wssBaseUrl = ResolveFromConfig("WssBaseUrl");

                return _wssBaseUrl;
            }
            private set { _wssBaseUrl = value; }
        }

        // Deliberately returns null rather than throwing the way Main() does: a missing
        // setting at startup is a fatal misconfiguration worth stopping for, but the same
        // lookup on a property read happens deep inside a hosted form, where throwing would
        // take down a screen the user is in the middle of.
        private static string ResolveFromConfig(string key)
        {
            string env = System.Configuration.ConfigurationManager.AppSettings["Environment"] ?? "Development";
            return System.Configuration.ConfigurationManager.AppSettings[$"{key}.{env}"];
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LoggerConfig.Configure();
            // Read environment once at startup
            string env = System.Configuration.ConfigurationManager.AppSettings["Environment"] ?? "Development";

            // Resolve the correct API URL
            ApiBaseUrl = System.Configuration.ConfigurationManager.AppSettings[$"ApiBaseUrl.{env}"]
                         ?? throw new ConfigurationErrorsException($"No API URL configured for environment: {env}");

            // Resolve the correct API URL
            WssBaseUrl = System.Configuration.ConfigurationManager.AppSettings[$"WssBaseUrl.{env}"]
                         ?? throw new ConfigurationErrorsException($"No API URL configured for environment: {env}");

            Log.Information("Running in {Environment} environment", env);
            Log.Information("API URL: {Url}", ApiBaseUrl);
            Log.Information("WSS URL: {Url}", WssBaseUrl);

            // Set application-wide currency format to Philippine Peso
            CultureInfo culture = new CultureInfo("en-PH");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Global crash guard (mirrors dispatching's Program.cs). Without this, an
            // unhandled exception on the UI thread - e.g. an "Index out of range" during a
            // grid/list bind - showed the raw .NET Continue/Quit dialog. CatchException
            // routes UI-thread exceptions here so the app keeps running; the full stack is
            // written to the Serilog log for diagnosis and the user sees a clean message.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
            {
                try { Serilog.Log.Error(e.Exception, "Unhandled UI-thread exception"); } catch { }
                MessageBox.Show(
                    "Something went wrong and that action could not be completed." + Environment.NewLine + Environment.NewLine
                    + e.Exception.Message + Environment.NewLine + Environment.NewLine
                    + "The app will keep running. Full details were saved to the log.",
                    "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                try { Serilog.Log.Error(ex, "Unhandled non-UI exception"); } catch { }
                MessageBox.Show(
                    "A serious error occurred." + Environment.NewLine + Environment.NewLine
                    + (ex?.Message ?? "Unknown error") + Environment.NewLine + Environment.NewLine
                    + "Details were saved to the log.",
                    "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SMPC());
        }
    }
} 