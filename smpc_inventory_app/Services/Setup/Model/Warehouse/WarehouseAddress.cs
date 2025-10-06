using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Warehouse
{
    class WarehouseAddressOld
    {
        public int id { get; set; }
        public string code { get; set; }
        public int warehouse_address_id { get; set; }
        public string building_no { get; set; }
        public string street { get; set; }
        public string barangay_no { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string country { get; set; }
        public string contact_person { get; set; }
        public string contact_no { get; set; }
        
    }
}
