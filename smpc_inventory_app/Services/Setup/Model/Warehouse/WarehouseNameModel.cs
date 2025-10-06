using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Warehouse
{
    public class WarehouseNameModel
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string warehouse_manager { get; set; }
        public bool? is_inactive { get; set; }   
    }

    public class WarehouseAddressModel
    {
        public int id { get; set; }
        public int warehouse_name_id { get; set; } 
        public string building_no { get; set; }
        public string street { get; set; }
        public string barangay_no { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string country { get; set; }
        public string contact_person { get; set; }
        public string contact_no { get; set; }
    }

    public class WarehouseAreaModel
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
     
    public class WarehouseList
    {
        public List<WarehouseNameModel> warehouse_name { get; set; }
        public List<WarehouseAddressModel> warehouse_address { get; set; }
        public List<WarehouseAreaModel> warehouse_area { get; set; }
    }

    public class WarehouseHierarchy
    {
        public string Zone { get; set; }
        public List<WarehouseAreaHierarchy> Areas { get; set; } = new List<WarehouseAreaHierarchy>();
    }

    public class WarehouseAreaHierarchy
    {
        public string Area { get; set; }
        public List<WarehouseRackHierarchy> Racks { get; set; } = new List<WarehouseRackHierarchy>();
    }

    public class WarehouseRackHierarchy
    {
        public string Rack { get; set; }
        public List<WarehouseLevelHierarchy> Levels { get; set; } = new List<WarehouseLevelHierarchy>();
    }

    public class WarehouseLevelHierarchy
    {
        public string Level { get; set; }
        public List<string> Bins { get; set; } = new List<string>();
    }
}
