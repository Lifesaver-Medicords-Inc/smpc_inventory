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

    internal static class PositionServices
    {

        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<PositionModel>>>.Get(ENUM_ENDPOINT.POSITION);
            DataTable positions = JsonHelper.ToDataTable(response.Data);

            return positions;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.POSITION, data);

            return response;
        }


        public static async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<PositionModel>>.Delete(ENUM_ENDPOINT.POSITION, data);
            bool isSucccess = response.Success;

            return isSucccess;
        }

        public static async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {

            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.POSITION, data);
            return response;
        }


        public static async Task<PositionModel[]> GetPosition()
        {
            var response = await RequestToApi<ApiResponseModel<PositionModel[]>>.Get(ENUM_ENDPOINT.POSITION);
            var positions = response.Data;

            return positions;
        }
    }
}
