using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Warehouse
{
    class WarehouseAreasModel //not used
    {
        public int id { get; set; }
        public int warehouse_name_id { get; set; }
        public string use_type { get; set; }
        public string zone { get; set; }
        public string area { get; set; }
        public string rack { get; set; }
        public string level { get; set; }
        public string bins { get; set; }
        public string location_code { get; set; }
        public string notes { get; set; }
    }
}
