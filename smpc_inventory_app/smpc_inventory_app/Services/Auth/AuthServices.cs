using smpc_inventory_app.Model;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Auth
{
    class AuthServices
    {
         static string url = "/login";
             
            // Login
            public static async Task<ApiResponseModel<CurrentUserModel>> Login(Dictionary<string, dynamic> data)
            {
                var response = await RequestToApi<ApiResponseModel<CurrentUserModel>>.Post(url, data);
                return response;
            }

            // Logout
            public static async Task<ApiResponseModel> Logout(Dictionary<string, dynamic> data)
            {
                var response = await RequestToApi<ApiResponseModel>.Post(url, data);
                return response;
            }
    }
}
