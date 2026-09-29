
using Inventory_SMPC.Pages.Business_Partner_Info;
using Inventory_SMPC.Pages.Item;
using smpc_inventory_app.Pages.Setup;
using Inventory_SMPC.Pages.Setup;
using smpc_inventory_app.Pages.Business_Partner_Info;
using smpc_inventory_app.Pages.Purchasing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_inventory_app.Pages;
using smpc_inventory_app.Pages.Inventory;
//using smpc_sales_system.Pages.Sales;
//using smpc_sales_app.Pages.Sales;
using smpc_inventory_app.Pages.Item;
using smpc_inventory_app.Pages.Inventory.ReceivingReport2;
using smpc_inventory_app.Pages.Purchasing.PurchaseReturn;
using smpc_inventory_app.Pages.Hosted;

namespace smpc_inventory_app.Services.Helpers
{
    internal class RouteServices
    {

        // A factory per page, called for the page that was clicked. The table used to hold the
        // pages themselves, and since a RouteServices is made per sidebar click, every click
        // built every page in it - each with its own constructor-time work - to show one. With
        // the other apps' screens added (2026-09-29) that would also have built dispatching's
        // container and engineering's Item Request on every click. Each click still gets a new
        // page, as before.
        Dictionary<string, Func<Control>> _pages = new Dictionary<string, Func<Control>>()
        {
            //========================================================================
            // SETUP  
            { "ITEM ENTRY", () => new frm_Item_Entry() },

            { "BUSINESS PARTNER INFO", () => new BusinessPartnerInfo("")  },

            { "CANVASS SHEET", () => new CanvassSheet()},
            { "PURCHASING LIST", () => new NewPurchasingList() },
            { "PURCHASE ORDER", () => new frm_PurchaseOrder()},
            { "PURCHASE RETURN", () => new PurchaseReturnPage() },

            { "ITEM BRAND", () => new frm_item_brand_setup()  },
            { "ITEM NAME", () => new frm_item_name_setup()},
            { "ITEM TYPE", () => new frm_item_type_setup()  },
            { "ITEM CLASS", () => new frm_item_class_setup()  },
            { "ITEM MATERIAL", () => new frm_item_material_setup()  },
            { "ITEM PUMP COUNT", () => new frm_item_pump_count_setup()  },
            { "ITEM PUMP TYPE", () => new frm_item_pump_type_setup()  },
            { "UNIT OF MEASURE", () => new frm_unit_of_measure_setup()  },
            { "SOCIAL MEDIA", () => new frm_social_media_setup() },
            { "VALUATION METHOD", () => new frm_item_valuation_method_setup() },
             
            // OTHER SETUP
            { "PAYMENT TERMS", () => new Inventory_SMPC.Pages.Setup.frm_payment_terms_setup()  },
            { "ENTITY TYPE", () => new frm_entity_type() },
            { "INDUSTRIES", () => new frm_industries() },
            { "POSITION", () => new frm_position_setup() },
            { "BOM", () => new bom() },
            { "BOQ", () => new boq_wiring() },
            {"POSITIONS", () => new frm_position_setup() },

            //{ "INVENTORY", () => new frm_warehouse_name_setup() },
            { "WAREHOUSE USETYPE", () => new frm_warehouse_usetype_setup() },
            { "WAREHOUSE", () => new frm_warehouse_name_setup() },
            { "INVENTORY TRACKER", () => new InventoryTrackerPage() },
            { "INVENTORY LOGBOOK", () => new InventoryLogbookPage() },
            { "RECEIVING REPORT", () => new ReceivingReport2Page() },
            { "INVENTORY ITEM STOCKS", () => new ItemStocksPage() },
            { "PRODUCTION REPORT", () => new production() },

            // The Inventory Report builder, in a tab rather than a dialog (2026-09-29).
            { "INVENTORY REPORT LIST", () => new InventoryReportPage() },

            // The warehouse menu's lists that live in the other apps (see HostedScreens).
            { "PURCHASE REQUISITION", HostedScreens.PurchaseRequisition },
            { "ITEM REQUEST LIST", HostedScreens.ItemRequestList },
            { "ITEM RELEASE LIST", () => new HostedDispatchingPage("ITEM_RELEASE", "Item Release") },
            { "SALES ORDER LIST", () => new HostedDispatchingPage("SALES_ORDER", "Sales Order List") },
            { "LOGISTICS CALENDAR", () => new HostedDispatchingPage("LOGISTICS_CALENDAR", "Logistics Calendar") },
        };

        private string _selectedRoute;
        public RouteServices(string selectedRoute)
        {
            this._selectedRoute = selectedRoute;
        }
        public Control GetForm()
        {
            return _pages.First(v => v.Key == this._selectedRoute).Value();
        }

        public String GetTitle()
        {
            return _pages.First(v => v.Key == this._selectedRoute).Key;
        }
      }
}







