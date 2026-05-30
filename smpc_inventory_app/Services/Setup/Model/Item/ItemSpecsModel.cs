using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Item
{
    public class ItemSpecsModel
    {
        public int id { get; set; }
        public int based_id { get; set; }
        public string template { get; set; }
        public string fla_1 { get; set; }
        public string fla_2 { get; set; }
        public string volt_1 { get; set; }
        public string volt_2 { get; set; }
        public int impeller_id { get; set; }
        public string manufacturer_origin { get; set; }
        public ItemSpecstemplate[] item_specs_template { get; set; }
    }
    public class ItemSpecstemplate
    {
        public int id { get; set; }
        public int based_id { get; set; }
        public string title { get; set; }
        public string value { get; set; }
    }
    class ItemSpecs
    {
        public List<ItemSpecsModel> itemspecs { get; set; }
    }
}
