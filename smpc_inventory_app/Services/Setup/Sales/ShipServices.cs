using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Sales
{
    class ShipServices
    {
        // GET
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ShipModel>>>.Get(ENUM_ENDPOINT.SHIPTYPE);
            DataTable shipItems = JsonHelper.ToDataTable(response.Data);
            return shipItems;
        }

        public static async Task<ShipModel[]> GetShip()
        {
            var response = await RequestToApi<ApiResponseModel<ShipModel[]>>.Get(ENUM_ENDPOINT.SHIPTYPE);
            var shipData = response.Data;

            return shipData;
        }


        // POST
        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.SHIPTYPE, data);
            return response;
        }

        // DELETE
        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ShipModel>>.Delete(ENUM_ENDPOINT.SHIPTYPE, data);
            bool isSucccess = response.Success;
            return isSucccess;
        }

        // UPDATE
        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.SHIPTYPE, data);
            return response;
        }
    }
}
