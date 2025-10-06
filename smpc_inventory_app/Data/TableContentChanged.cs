using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Data
{
    public class TableContentChanged
    {
        public static bool WarehouseUseType { get; set; } = false;
        public static bool WarehouseName { get; set; } = false; 
        public static bool ReceivingReport { get; set; } = false;
    }

}
