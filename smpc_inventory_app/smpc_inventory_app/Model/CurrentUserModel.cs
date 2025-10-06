using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Model
{
    public class CurrentUserModel
    {
        public int id { get; set; }
        public string employee_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string department { get; set; }
        public string position { get; set; }
    }
}
