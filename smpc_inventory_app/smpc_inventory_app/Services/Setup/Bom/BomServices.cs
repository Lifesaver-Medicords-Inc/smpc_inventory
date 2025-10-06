// BomServices.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using smpc_inventory_app.Data;
using smpc_inventory_app.Services;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Services.Setup.Model.Item;

namespace smpc_inventory_app.Services.Setup.Item
{
    internal static class BomServices
    {
        // Get the BOMs as a DataTable
        public static async Task<DataTable> GetBomsAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<BomModel>>>.Get(ENUM_ENDPOINT.BOM);
            DataTable boms = JsonHelper.ToDataTable(response.Data);
            return boms;
        }

        // Insert a BOM (parent)
        public static async Task<bool> InsertBom(Dictionary<string, dynamic> data)
        {

            var response = await RequestToApi<ApiResponseModel<dynamic>>.Post(ENUM_ENDPOINT.BOM, data);
            bool responseData = response.Success;
            return responseData;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.BOM, data);

            return response;
        }


        //Delete a BOM(parent)
        public static async Task<bool> DeleteBom(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<PositionModel>>.Delete(ENUM_ENDPOINT.BOM, data);
            bool isSucccess = response.Success;

            return isSucccess;
        }

        // Update a BOM (parent)
        public static async Task<ApiResponseModel> UpdateBom(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.BOM, data);
            return response;  // Return ApiResponseModel
        }




    }
}

