using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Bpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Bpi
{
    internal static class BpiServices
    {
     
        //public static async Task<bool> Insert(Dictionary<string, dynamic> data)
        //{
        //    var response = await RequestToApi<ApiResponseModel<dynamic>>.Post(ENUM_ENDPOINT.BPI, data);
        //    bool responseData = response.Success;
        //    return responseData;
        //}

        public static async Task<bool> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<dynamic>>.Put(ENUM_ENDPOINT.BPI, data);
            bool responseData = response.Success;
            return responseData;
        }

        public static async Task<ApiResponseModel<dynamic>> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<dynamic>>.Post(ENUM_ENDPOINT.BPI, data);
            //   bool responseData = response.Success;
  
            return response;
        }



    }
}
