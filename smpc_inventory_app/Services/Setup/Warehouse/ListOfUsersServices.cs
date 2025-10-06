using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using smpc_inventory_app.Model;

namespace smpc_inventory_app.Services.Setup
{
    class ListOfUsersServices
    {
        public static async Task<DataTable> GetAsDatatable()
        {
            var response = await RequestToApi<ApiResponseModel<List<CurrentUserModel>>>.Get(ENUM_ENDPOINT.USERS);
            DataTable users = JsonHelper.ToDataTable(response.Data);

            return users;
        }
    }
}
