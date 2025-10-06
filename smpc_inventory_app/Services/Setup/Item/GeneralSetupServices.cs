using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Item
{
     class GeneralSetupServices
    {
        private string EndPoint { get; }
        public  GeneralSetupServices(string api)
        {
            this.EndPoint = api;
        }
      
        public  async Task<DataTable> GetAsDatatable()
        {      
            var response = await RequestToApi<ApiResponseModel<List<GeneralSetupModel>>>.Get(this.EndPoint);
            DataTable responseData = JsonHelper.ToDataTable(response.Data);

            return responseData;
        }

        public async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(this.EndPoint, data);

            return response;
        }

        public  async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<GeneralSetupModel>>.Delete(this.EndPoint, data);
            bool isSuccess = response.Success;

            return isSuccess;
        }

        public  async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(this.EndPoint, data);
            return response;
        }



    }
}
