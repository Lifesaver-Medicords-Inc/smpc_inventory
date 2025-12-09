using smpc_inventory_app.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup
{
    public class SetupService<TModel> where TModel : class
    {
        private readonly string _endpoint;
        
        public SetupService(string endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task<DataTable> GetAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<TModel>>>.Get(_endpoint);
            return JsonHelper.ToDataTable(response.Data);
        }
        public async Task<TModel[]> GetAll()
        {
            var response = await RequestToApi<ApiResponseModel<TModel[]>>.Get(_endpoint);
            return response.Data;
        }

        internal async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            return await RequestToApi<ApiResponseModel>.Post(_endpoint, data);
        }

        internal async Task<ApiResponseModel> Update(Dictionary<string, dynamic> data)
        {
            return await RequestToApi<ApiResponseModel>.Put(_endpoint, data);
        }

        internal async Task<bool> Delete(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<TModel>>.Delete(_endpoint, data);
            return response.Success;
        }
    }
}
