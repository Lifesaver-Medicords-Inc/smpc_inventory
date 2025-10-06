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
using smpc_inventory_app.Services.Setup.Model.Boq;

namespace smpc_inventory_app.Services.Setup.Boq
{
    internal static class BoqServices
    {
        public static async Task<ProjectComponentClass> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<ProjectComponentClass>>.Get(ENUM_ENDPOINT.BOQ);
            ProjectComponentClass boq = response.Data;

            return boq;
        }
        public static async Task<WiringNotes[]> GetAsDatatableNote()
        {
            var response = await RequestToApi<ApiResponseModel<WiringNotesResponse>>.Get(ENUM_ENDPOINT.WIRING_NOTES);
            return response.Data.wiring_notes;
        }
        public static async Task<QQView[]> GetQQView()
        {
            var response = await RequestToApi<ApiResponseModel<QQResponse>>.Get(ENUM_ENDPOINT.QQ_NOTES);
            return response.Data.qq_view;
        }

        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.BOQ, data);

            return response;
        }
        public static async Task<ApiResponseModel> InsertNote(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.WIRING_NOTES, data);

            return response;
        }

        public static async Task<ApiResponseModel> UpdateBoq(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.BOQ, data);
            return response; 
        }
        public static async Task<ApiResponseModel> UpdateNote(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.WIRING_NOTES, data);
            return response;
        }
    }
}
