using smpc_inventory_app.Data;

using smpc_inventory_app.Model;

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

        public static async Task<List<CurrentUserModel>> GetBpiUsers(string employee_id)
        {
            var response = await RequestToApi<ApiResponseModel<List<CurrentUserModel>>>.Get(ENUM_ENDPOINT.EmployeeUsers + employee_id);
            
            return response.Data;
        }



        public static async Task<ApiResponseModel<dynamic>> Update(Dictionary<string, dynamic> data)
        {
            // Bug #263: this used to collapse the response down to a bare bool, throwing away
            // response.message - so a failed BPI update could only ever show a generic
            // "record update failed" with no indication of which field/validation actually
            // failed. Return the full response so the caller can surface it.
            var response = await RequestToApi<ApiResponseModel<dynamic>>.Put(ENUM_ENDPOINT.BPI, data);
            return response;
        }

        public static async Task<bool> UpdateMainBranch(List<Dictionary<string, dynamic>> data)
        {
            var response = await RequestToApi<ApiResponseModel<dynamic>>.Put(ENUM_ENDPOINT.BPIMain, data);
            bool responseData = response.Success;
            return responseData;
        }
        public static async Task<ApiResponseModel<dynamic>> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<dynamic>>.Post(ENUM_ENDPOINT.BPI, data);
            //   bool responseData = response.Success;
  
            return response;
        }
        public static async Task<ApiResponseModel<dynamic>> InsertNewBPI(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<dynamic>>.Post(ENUM_ENDPOINT.CreateBPI, data);
            bool responseData = response.Success;

            return response;
        }



    }
}
