using smpc_inventory_app.Data;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Model.Purchasing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Purchasing.PurchasingList
{
    class GuidingPriceServices
    {
        public static async Task<DataTable> GetAsDataTable()
        {
            var response = await RequestToApi<ApiResponseModel<List<GuidingPriceModel>>>.Get(ENUM_ENDPOINT.PURCHASING_GUIDING_PRICE);

            DataTable guidingPriceList = JsonHelper.ToDataTable(response.Data);

            return guidingPriceList;

        }
    }
}
