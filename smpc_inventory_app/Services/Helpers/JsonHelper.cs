using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using smpc_inventory_app.Services.Setup.Model.Bom;

namespace smpc_inventory_app.Services.Helpers
{
    internal class JsonHelper
    {
        public static DataTable ToDataTable(JArray jArray)
        {
            // Create a new DataTable
            DataTable dataTable = new DataTable();

            // If the JArray is not empty, get the first object to create columns
            if (jArray.Count > 0)
            {
                // Create columns based on the properties of the first object in the JArray
                foreach (JProperty property in jArray[0].ToObject<JObject>().Properties())
                {
                    dataTable.Columns.Add(property.Name);
                }

                // Add rows to the DataTable
                foreach (var item in jArray)
                {
                    var row = dataTable.NewRow();
                    var jsonObject = item.ToObject<JObject>();

                    // Add each value from the JObject to the corresponding column
                    foreach (DataColumn column in dataTable.Columns)
                    { 
                        // If the value is null, assign DBNull.Value; otherwise, convert to string
                        if(jsonObject[column.ColumnName] == null)
                        {
                            row[column] = DBNull.Value;
                        }
                        else
                        {
                            row[column] = jsonObject[column.ColumnName].ToString();
                        } 
                    }

                    // Add the row to the DataTable
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }
                                                             //column_name = desc / col_name=asc
        public static DataTable ToDataTable<T>(List<T> items, string sortBy = "")
        {
            var dataTable = new DataTable();

            // Get all the properties of the model
            var properties = typeof(T).GetProperties();

            // Add columns to DataTable for each property
            foreach (var prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            //to auto sort
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = sortBy.Replace(" ",""); //no space naming as it should be
                string[] parts = sortBy.Split('='); 
                string columnName = parts[0];
                string direction = (parts.Length > 1) ? parts[1].ToUpper() : "ASC";

                var prop = typeof(T).GetProperty(columnName,
                    System.Reflection.BindingFlags.IgnoreCase |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance);

                if (prop != null)
                {
                    Func<T, IComparable> keySelector = x =>
                    {
                        var val = prop.GetValue(x, null);
                        return val == null ? 0 : (IComparable)Convert.ChangeType(val, typeof(long));
                    };


                    if (direction.Contains("DESC"))
                        items = items.OrderByDescending(keySelector).ToList();
                    else
                        items = items.OrderBy(keySelector).ToList();
                }
            }
             
            // Add rows to the DataTable
            foreach (var item in items)
            {
                var row = dataTable.NewRow();
                foreach (var prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        internal static DataTable ToDataTable(BomClass data)
        {
            throw new NotImplementedException();
        }
    }

}
