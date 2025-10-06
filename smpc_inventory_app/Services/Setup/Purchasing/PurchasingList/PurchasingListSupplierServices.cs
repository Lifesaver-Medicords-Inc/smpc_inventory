using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Purchasing
{
    class PurchasingListSupplierServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<PurchasingListSupplierModel>>>.Get(ENUM_ENDPOINT.PURCHASINGLISTSUPPLIER);

            DataTable purchasinglist = JsonHelper.ToDataTable(response.Data);

            return purchasinglist;
        }

    }
}
