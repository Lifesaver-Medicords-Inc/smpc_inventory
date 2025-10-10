using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace smpc_inventory_app.Services.Setup.Inventory
{
    class InventoryTrackerView
    {
        [Column("id")]
        public int id { get; set; }

        [Column("pod_id")]
        public int pod_id { get; set; }

        [Column("item_code")]
        public string item_code { get; set; }

        [Column("general_name")]
        public string general_name { get; set; }

        [Column("brand")]
        public string brand { get; set; }

        [Column("item_desc")]
        public string item_desc { get; set; }

        [Column("location")]
        public string location { get; set; }

        [Column("qty")]
        public string qty { get; set; }

        [Column("uom")]
        public string uom { get; set; }

        [Column("warehouse_name")]
        public string warehouse_name { get; set; }

        [Column("remarks")]
        public string remarks { get; set; }

        [Column("rem_id")]
        public int rem_id { get; set; }

        [Column("warehouse_id")]
        public int warehouse_id { get; set; }
    }

    class InventoryLogbookView
    {
        [Column("id")]
        public int id { get; set; }

        [Column("pod_id")]
        public int pod_id { get; set; }

        [Column("general_name")]
        public string general_name { get; set; }

        [Column("brand")]
        public string brand { get; set; }

        [Column("item_desc")]
        public string item_desc { get; set; }

        [Column("location")]
        public string location { get; set; }

        [Column("qty_in")]
        public int qty_in { get; set; }

        [Column("qty_out")]
        public int qty_out { get; set; }

        [Column("date")]
        public string date { get; set; }

        [Column("rr_no")]
        public string rr_no { get; set; }

        [Column("po_no")]
        public string po_no { get; set; }

        [Column("supplier_name")]
        public string supplier_name { get; set; }
    }

    class WarehouseName
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    class InventoryTrackerModel
    {
        public int id { get; set; }
        public int rrd_id { get; set; }
        public int pod_id { get; set; }
        public string remarks { get; set; }
    }
}
