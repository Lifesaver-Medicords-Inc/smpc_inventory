using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Item
{
    class ItemModelModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int item_name_id { get; set; }
        public string related_name { get; set; }
        public int item_brand_id { get; set; }
        public string related_brand { get; set; }
        public string catalogue_year { get; set; }
        public bool is_active { get; set; }
    }
    class ItemModels
    {
        public List<ItemModelModel> itemmodels { get; set; }
    }

}