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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SMPC());
        }
    }
} 