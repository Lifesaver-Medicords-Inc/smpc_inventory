using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Item
{
    public class BomModel
    {
        public int Id { get; set; }  
        public string ItemName { get; set; }  
        public string ItemCode { get; set; } 
        public string ItemModel { get; set; }  
        public int ProductionQty { get; set; }  
        public string Type { get; set; } 
        public string Labor { get; set; } 
        public int Price { get; set; }
    }

    public class BomDetailModel
    {
        public int ItemBomID { get; set; }  
        public string ItemCode { get; set; } 
        public string Description { get; set; }  
        public int Size { get; set; } 
        public int Quantity { get; set; }  
        public string OUM { get; set; }  
    }
}
