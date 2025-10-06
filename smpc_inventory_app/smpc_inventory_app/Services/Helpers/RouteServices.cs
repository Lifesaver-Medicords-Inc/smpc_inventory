
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
using smpc_inventory_app.Pages.Engineering.Boq;

namespace smpc_inventory_app.Services.Helpers
{
      class RouteServices
    {

        Dictionary<string, Control> _pages = new Dictionary<string, Control>()
        {
            //========================================================================
            // SETUP  
            { "ITEM ENTRY", new frm_Item_Entry() },
            { "BUSINESS PARTNER INFO", new BusinessPartnerInfo()  },
            { "CANVASS SHEET", new CanvassSheet()},
            { "PURCHASING LIST", new NewPurchasingList() },
           
            { "ITEM BRAND", new frm_item_brand_setup()  },
            { "ITEM NAME", new frm_item_name_setup()},
            { "ITEM TYPE", new frm_item_type_setup()  }, 
            { "ITEM CLASS", new frm_item_class_setup()  },
            { "ITEM MATERIAL", new frm_item_material_setup()  },
            { "ITEM PUMP COUNT", new frm_item_pump_count_setup()  },
            { "ITEM PUMP TYPE", new frm_item_pump_type_setup()  },
            { "UNIT OF MEASURE", new frm_unit_of_measure_setup()  },
            { "SOCIAL MEDIA", new frm_social_media_setup() },

            // OTHER SETUP
           { "PAYMENT TERMS", new Inventory_SMPC.Pages.Setup.frm_payment_terms_setup()  },
           { "ENTITY TYPE", new frm_entity_type() },
           { "INDUSTRIES", new frm_industries() },
           { "POSITION", new frm_position_setup() },
           { "BOM", new bom() },
           { "BOQ", new BOQ() },






        };

        private string _selectedRoute;
        public RouteServices(string selectedRoute)
        {
            this._selectedRoute = selectedRoute;
        }
        public Control GetForm()
        {
            return _pages.First(v => v.Key == this._selectedRoute).Value;
        }

        public String GetTitle()
        {
            return _pages.First(v => v.Key == this._selectedRoute).Key;
        }
    }
}







