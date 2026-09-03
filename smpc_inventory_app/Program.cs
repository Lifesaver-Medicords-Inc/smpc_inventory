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
        public static string ApiBaseUrl { get; private set; }
        public static string WssBaseUrl { get; private set; }
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