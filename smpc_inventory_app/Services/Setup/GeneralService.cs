using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup
{
    class GeneralService<T> : ServiceBase<T> where T : class
    {
        public GeneralService(string url) : base(url) { }
    }
}
