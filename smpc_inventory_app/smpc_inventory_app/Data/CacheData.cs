using smpc_inventory_app.Model;
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Data
{
   public static class CacheData
    {
        public static DataTable Items { get; set; } = new DataTable();
        public static DataTable ItemBrand { get; set;} = new DataTable();
        public static DataTable ItemName { get; set; } = new DataTable();
        public static DataTable ItemType { get; set; } = new DataTable();
        public static DataTable ItemClass { get; set; } = new DataTable();
        public static DataTable Material { get; set; } = new DataTable();
        public static DataTable PumpCount { get; set; } = new DataTable();
        public static DataTable PumpType { get; set; } = new DataTable();
        public static DataTable UnitOfMeasurement { get; set; } = new DataTable();
        public static DataTable ItemModel { get; set; } = new DataTable();
        public static DataTable SocialMedia { get; set; } = new DataTable();
        public static DataTable Industries { get; set; } = new DataTable();
        public static DataTable BranchIndustries { get; set; } = new DataTable();
        public static DataTable Positions { get; set; } = new DataTable();
        public static DataTable Entity { get; set; } = new DataTable();

        public static DataTable PaymentTerms { get; set; } = new DataTable();
        public static String SessionToken { get; set; } = "";
        public static CurrentUserModel CurrentUser { get; set; } = null;
    }
}
