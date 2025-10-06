using System;
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

namespace smpc_inventory_app.Services.Setup
{
    internal static class ItemBrandServices
    {


        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<BrandModel>>>.Get(ENUM_ENDPOINT.BRAND);
            DataTable itemBrands = JsonHelper.ToDataTable(response.Data);

            return itemBrands;  
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {   
           var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.BRAND, data);
          
           return response;
        }


        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<BrandModel>>.Delete(ENUM_ENDPOINT.BRAND, data);
            bool isSucccess = response.Success;
        
            return isSucccess;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {

            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.BRAND, data);
            return response;
        }

        //   <--- TEST API FOR FRONT END --->

        public static async Task<BrandModel[]> GetBrand()
        {
            var response = await RequestToApi<ApiResponseModel<BrandModel[]>>.Get(ENUM_ENDPOINT.BRAND);
            var itemBrands = response.Data;

            return itemBrands;
        }

        //public static async Task<ApiResponseModel> Add(Dictionary<string, dynamic> data)
        //{

        //    ApiResponseModel response = new ApiResponseModel();
        //    try
        //    {
        //        response = await RequestToApi<ApiResponseModel>.Post(url, data);

        //    }
        //    catch (Exception e)
        //    {
        //        response.Success = false;
        //        response.message = "An error occurred: " + response.message;
        //    }
        //    return response;

        //    ApiResponseModel response = new ApiResponseModel();
        //    response = await RequestToApi<ApiResponseModel>.Post(url, data);
        //    return response;
        //}

    }
}
