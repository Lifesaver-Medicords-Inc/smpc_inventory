using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Item
{
    class ShipTypeServices
    {
        static string url = "/setup/shiptype";

        // GET
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ShipModel>>>.Get(url);
            DataTable shipItems = JsonHelper.ToDataTable(response.Data);
            return shipItems;
        }

        public static async Task<ShipModel[]> GetShip()
        {
            var response = await RequestToApi<ApiResponseModel<ShipModel[]>>.Get(url);
            var shipData = response.Data;

            return shipData;
        }


        // POST
        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(url, data);
            return response;
        }

        // DELETE
        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<ShipModel>>.Delete(url, data);
            bool isSucccess = response.Success;
            return isSucccess;
        }

        // UPDATE
        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(url, data);
            return response;
        }
    }
}
