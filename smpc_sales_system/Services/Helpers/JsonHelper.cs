﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using smpc_sales_app.Models;
using smpc_sales_system.Services.Sales.Models;

namespace smpc_sales_app.Services.Helpers
{
    class JsonHelper
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
                        if (jsonObject[column.ColumnName] == null)
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
        public static DataTable ToDataTableFromJObject(JObject jObject)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Clear();

            foreach (var property in jObject.Properties())
            {
                dataTable.Columns.Add(property.Name);
            }

            var row = dataTable.NewRow();
            foreach (var property in jObject.Properties())
            {
                row[property.Name] = property.Value?.ToString() ?? DBNull.Value.ToString();
            }
            dataTable.Rows.Add(row);

            return dataTable;
        }


        public static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable();

            // Get all the properties of the model
            var properties = typeof(T).GetProperties();

            // Add columns to DataTable for each property
            foreach (var prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Add rows to the DataTable
            if(items != null)
            {
                foreach (var item in items)
                {
                    var row = dataTable.NewRow();
                    foreach (var prop in properties)
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(row);
                }
            }
            
            return dataTable; 
        }
    }
}
