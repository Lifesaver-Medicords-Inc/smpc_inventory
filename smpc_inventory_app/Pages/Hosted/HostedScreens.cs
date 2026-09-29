using Newtonsoft.Json;
using smpc_inventory_app.Data;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Hosted
{
    // Screens of the other apps, opened from this app's sidebar (2026-09-29: the warehouse menu's
    // Purchase Requisition, Item Request List, Item Release List, Sales Order List and Logistics
    // Calendar had entries here and nothing behind them).
    //
    // Each one already exists in the app that owns it - one implementation, several entry points,
    // the arrangement the sales app uses to host this app's BPI and Item Entry, engineering's Item
    // Request and dispatching's Sales Calendar. The access catalogue names the owner of each:
    // Purchase Requisition is the Sales System's, Item Request (v2) is Engineering's, Item Release,
    // Sales Order List and Logistics Calendar are Dispatching's.
    //
    // Every app keeps its session in statics of its own. Nothing signs in to those apps when this
    // one runs, so each is handed this app's session just before its screen is built: the same
    // token (the API issues it per user, not per app) and the same user. Set each time rather than
    // once, because signing out and back in issues a new token. The signed-in user decides what
    // each screen offers - engineering's Item Request shows a warehouse user the forwarded
    // requests and the issue columns, dispatching's Item Release its warehouse side.
    internal static class HostedScreens
    {
        // Sales' Purchase Requisition (spec 5.5). Any department raises one.
        public static Control PurchaseRequisition()
        {
            ShareSessionWithSales();
            return new smpc_sales_system.Pages.Sales.PurchaseRequisition();
        }

        // Engineering's Item Request (spec 5.9) - the page the sales app hosts as well. Its
        // constructor reads the signed-in user, so the session goes first.
        public static Control ItemRequestList()
        {
            ShareSessionWithEngineering();
            return new smpc_engineering_app.Pages.ItemRequest2.ItemRequestPage2();
        }

        // Sales' own user type is this app's (the sales app signs in through this assembly's
        // model), so the user is shared as it is. Its API address is resolved from this process's
        // config when the sales app's Main has not run - the same keys, the same override file.
        private static void ShareSessionWithSales()
        {
            smpc_sales_app.Data.CacheData.SessionToken = CacheData.SessionToken ?? "";
            smpc_sales_app.Data.CacheData.CurrentUser = CacheData.CurrentUser;
        }

        // Engineering falls back to a local address when its Main has not run, so it is pointed
        // at this app's (the sales app does the same, Program.Configure). Its user is a type of
        // its own; the fields carry across by name.
        private static void ShareSessionWithEngineering()
        {
            smpc_engineering_app.Program.Configure(Program.ApiBaseUrl, Program.WssBaseUrl);
            smpc_engineering_app.Shared.CacheData.SessionToken = CacheData.SessionToken ?? "";
            smpc_engineering_app.Shared.CacheData.CurrentUser =
                Copy<smpc_engineering_app.Models.CurrentUserModel>(CacheData.CurrentUser);
        }

        // Another app's model of the same user, filled field by field from this app's. A field
        // that does not convert (position_id is text here and a number in dispatching's model) is
        // left at its default rather than failing the whole copy - the screens read the name,
        // department and position, not that.
        internal static T Copy<T>(object source) where T : class
        {
            if (source == null)
                return null;

            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => args.ErrorContext.Handled = true
            };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), settings);
        }
    }
}
