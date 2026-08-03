using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model;
using smpc_inventory_app.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace smpc_inventory_app.Services.Setup.Item
{
     class GeneralSetupServices
    {
        private string EndPoint { get; }
        public  GeneralSetupServices(string api)
        {
            this.EndPoint = api;
        }

        public async Task<DataTable> GetAsDatatable()
        {
            try
            {
                var response = await RequestToApi<ApiResponseModel<List<GeneralSetupModel>>>.Get(this.EndPoint);

                // Bug #258: this used to return null whenever the API had nothing to return
                // (e.g. a freshly-seeded/empty database), which then got assigned straight
                // into CacheData.Industries / CacheData.BranchIndustries - overwriting the
                // safe empty-DataTable default those fields start with - and crashed every
                // caller that did .Copy()/.Rows.Count/etc. on them with a NullReferenceException.
                // Returning a properly-columned but empty table keeps that invariant intact.
                if (response == null || response.Data == null)
                    return JsonHelper.ToDataTable(new List<GeneralSetupModel>());

                DataTable responseData = JsonHelper.ToDataTable(response.Data);
                return responseData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GeneralSetupServices.GetAsDatatable] {this.EndPoint} - {ex.Message}");
                return JsonHelper.ToDataTable(new List<GeneralSetupModel>());
            }
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
