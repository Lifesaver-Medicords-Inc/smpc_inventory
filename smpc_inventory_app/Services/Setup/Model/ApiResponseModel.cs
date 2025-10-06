using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup
{
    internal class ApiResponseModel<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string message { get; set; }
        public string token { get; set; }
    }

    internal class ApiResponseModel
    {
        public  bool Success { get; set; }
        public  string message { get; set; }
        public string Message { get;  set; }
        public dynamic Data { get; set; }
    }
}
