using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Item
{
    class ItemImageModel
    {
        public int id { get; set; }
        public int based_id { get; set; }
        public string image { get; set; }
    }
    class ItemImage
    {
        public List<ItemImageModel> itemimages { get; set; }
    }
}
