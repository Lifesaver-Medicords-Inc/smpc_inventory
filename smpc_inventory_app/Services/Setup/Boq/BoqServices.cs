using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Services.Setup.Model.Boq;

namespace smpc_inventory_app.Services.Setup.Boq
{
    internal static class BoqServices
    {
        public static async Task<ProjectComponentClass> GetAsDatatable()
        {
            // response?.Data: SendRequestAsync swallows every failure and returns
            // default(T) - null - so a request that failed (API restarting, connection
            // dropped) made this dereference throw. boq.cs already handles a null result
            // ("No data available."), it just never got one because this threw first.
            var response = await RequestToApi<ApiResponseModel<ProjectComponentClass>>.Get(ENUM_ENDPOINT.BOQ);
            ProjectComponentClass boq = response?.Data;

            return boq;
        }


        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.BOQ, data);

            return response;
        }

        public static async Task<ApiResponseModel> UpdateBoq(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.BOQ, data);
            return response; 
        }
        
    }
}
