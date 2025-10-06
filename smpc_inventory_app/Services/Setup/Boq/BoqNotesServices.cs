using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Boq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Boq
{
    class BoqNotesServices
    {
        public static async Task<DataTable> GetBoqNotesAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<ProjectComponentClass>>>.Get(ENUM_ENDPOINT.BOQ_NOTES);
            DataTable boqNotes = JsonHelper.ToDataTable(response.Data);
            return boqNotes;
        }

        // Insert a BOQ Note
        public static async Task<bool> InsertBoqNote(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel<dynamic>>.Post(ENUM_ENDPOINT.BOQ_NOTES, data);
            bool responseData = response.Success;
            return responseData;
        }

        // Insert a BOQ Note (with specific response type)
        public static async Task<ApiResponseModel> Insert(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Post(ENUM_ENDPOINT.BOQ_NOTES, data);
            return response;  // Return ApiResponseModel
        }

        
        // Update a BOQ Note
        public static async Task<ApiResponseModel> UpdateBoqNote(Dictionary<string, dynamic> data)
        {
            var response = await RequestToApi<ApiResponseModel>.Put(ENUM_ENDPOINT.BOQ_NOTES, data);
            return response; 
        }
        // Delete a BOQ Note
        //public static async Task<bool> DeleteBoqNote(Dictionary<string, dynamic> data)
        //{
        //    var response = await RequestToApi<ApiResponseModel<BoqNotesModel>>.Delete(ENUM_ENDPOINT.BOQ_NOTES, data);
        //    bool isSuccess = response.Success;
        //    return isSuccess;
        //}

    }
}
