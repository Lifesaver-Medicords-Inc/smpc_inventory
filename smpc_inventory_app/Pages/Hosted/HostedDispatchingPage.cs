using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using smpc_dispatching.Core.Interfaces;
using smpc_inventory_app.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Hosted
{
    // One of the dispatching app's screens - Item Release, Sales Order List, Logistics Calendar -
    // shown in a tab of this app (see HostedScreens for why they are hosted, not rebuilt).
    //
    // Dispatching builds every screen from a service container, so one is built here from its own
    // registrations (Program.BuildServiceProviderForHost), pointed at the API this app is signed
    // in to - exactly as the sales app hosts dispatching's Sales Calendar (SalesCalendarPage). The
    // container is built once and kept: rebuilding it per tab would open a second HttpClient and
    // re-read the configuration each time.
    //
    // The screen is built when the tab first loads, not when the page object is made, so nothing
    // of dispatching's is touched until the user actually opens one of these.
    public class HostedDispatchingPage : UserControl
    {
        private static IServiceProvider _dispatchingServices;

        private readonly string _routeCode;
        private readonly string _title;
        private bool _loaded;

        // routeCode is dispatching's own route code (its RouteService): ITEM_RELEASE, SALES_ORDER,
        // LOGISTICS_CALENDAR.
        public HostedDispatchingPage(string routeCode, string title)
        {
            _routeCode = routeCode;
            _title = title;
            Dock = DockStyle.Fill;
            BackColor = Color.White;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_loaded || DesignMode)
                return;

            _loaded = true;

            try
            {
                ShowView();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Hosted dispatching screen {Route} could not be opened", _routeCode);
                // Contained on purpose: a screen that cannot start says so in its own tab rather
                // than taking the rest of the app down with it.
                ShowMessage("The " + _title + " could not be opened." + Environment.NewLine
                            + Environment.NewLine + ex.Message);
            }
        }

        private void ShowView()
        {
            if (_dispatchingServices == null)
                _dispatchingServices = BuildDispatchingServices();

            // Dispatching's requests carry the token in a static of their own, and its Item
            // Release reads the signed-in user (department, name) from another when it is built.
            smpc_dispatching.Core.Services.HttpService.SessionToken = CacheData.SessionToken ?? "";
            smpc_dispatching.Core.Enum.CacheData.SessionToken = CacheData.SessionToken ?? "";
            smpc_dispatching.Core.Enum.CacheData.CurrentUser =
                HostedScreens.Copy<smpc_dispatching.Core.Models.UserModel>(CacheData.CurrentUser);

            // The calendar takes its department, title and detail panel from dispatching's
            // "selected route", which its own sidebar sets on a click. Nothing clicks it here.
            var routes = _dispatchingServices.GetRequiredService<IRouteService>();
            routes.SelectRoute(_routeCode);

            Control view = routes.GetForm(_routeCode);
            if (view == null)
                throw new InvalidOperationException("The dispatching app has no screen called " + _routeCode + ".");

            view.Dock = DockStyle.Fill;

            Controls.Clear();
            Controls.Add(view);
        }

        private static IServiceProvider BuildDispatchingServices()
        {
            // Dispatching's HttpService reads its base address from AppSettings:ApiBaseUrl. It is
            // given this app's resolved address (smpc.endpoints.xml, then App.config) in memory
            // rather than from a json file beside the exe, so the hosted screen can never end up
            // talking to a different API from the rest of this app.
            string apiBaseUrl = Program.ApiBaseUrl;

            if (string.IsNullOrWhiteSpace(apiBaseUrl))
                throw new InvalidOperationException(
                    "No API URL is configured for this app, so the screen has nothing to read from.");

            // BaseAddress only combines with a relative path when it ends in a slash.
            if (!apiBaseUrl.EndsWith("/"))
                apiBaseUrl += "/";

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "AppSettings:ApiBaseUrl", apiBaseUrl }
                })
                .Build();

            return smpc_dispatching.Program.BuildServiceProviderForHost(configuration);
        }

        private void ShowMessage(string text)
        {
            Controls.Clear();

            Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(24)
            });
        }
    }
}
