using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Item
{
   internal static class SocialMediaServices
    {
       

        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<BrandModel>>>.Get(ENUM_ENDPOINT.SOCIALS);
            DataTable itemBrands = JsonHelper.ToDataTable(response.Data);

            return itemBrands;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.SOCIALS, data);

            return response;
        }


        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<GeneralSetupModel>>.Delete(ENUM_ENDPOINT.SOCIALS, data);
            bool isSucccess = response.Success;

            return isSucccess;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {

            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.SOCIALS, data);
            return response;
        }
    }
}
