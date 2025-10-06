using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Sales
{
    internal static class ProjectService
    {
        static string url = "/sales/projects";
        static string url_conditions = "/sales/project_conditions";
        static string url_content = "/sales/project_content";
        static string url_bom = "/setup/bom";
        static string url_suppliers = "/BpiSuppliers";
        static string url_canvas = "/sales/salescanvas";
        static string url_pumps = "/sales/projects_pumps";
        public static async Task<SalesProjectList> GetProjects()
        {
            var response = await RequestToApi<ApiResponseModel<SalesProjectList>>.Get(url);
            SalesProjectList projectData = response.Data;
            return projectData;
        }
    }
}
