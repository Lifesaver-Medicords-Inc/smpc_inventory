using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using smpc_app.Services.Helpers;

namespace smpc_inventory_app.Pages.Engineering.Bom
{
    public partial class SetupItemModal : Form
    {

        private string Title { get; }
        private string EndPoint { get; }
        private List<int> CurrentValues { get; }
        private List<string> CurrentGridValues { get; }
        private string placeHolderText = "BOM Search...";
        private int result { get; set; }
        //private DataView result { get; set; }
        private DataTable Dt { get; set; }

        public SetupItemModal(string title, string api, DataTable dt, List<int> currentValues, List<string> currentGridValues, int recordIndex = 0)
        {
            InitializeComponent();

            InitializeSearchBox();
            lbl_title.Text = title;
            this.Text = title;


            this.EndPoint = api;


            this.CurrentValues = currentValues;
            this.CurrentGridValues = (currentGridValues != null && recordIndex >= 0 && recordIndex < currentGridValues.Count && !string.IsNullOrEmpty(currentGridValues[recordIndex]))
                   ? new List<string>(currentGridValues[recordIndex].Split(','))
                   : new List<string>();
            this.Dt = dt;
        }

        private void SetupItemModal_Load(object sender, EventArgs e)
        {
            Helpers.Loading.ShowLoading(dg_item_bom, "Fetching data...");
            dg_item_bom.DataSource = this.Dt;

            foreach (DataGridViewColumn column in dg_item_bom.Columns)
            {
                if (column.Name != "item_code" && column.Name != "general_name" && column.Name != "item_model")
                {
                    column.Visible = false;
                }
            }

            Helpers.Loading.HideLoading(dg_item_bom);
        }

        public int GetResult()
        {
            return result;
        }

        private void dg_item_bom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // BUG (reported live): after a search, dg_item_bom is rebound to
                // Helpers.FilterDataTable's result - a brand-new DataTable built via
                // CopyToDataTable(), not a view over Dt - so e.RowIndex is a position
                // in that filtered copy, not in Dt. The caller (bom.cs's
                // btn_get_item_Click) always indexes the ORIGINAL bomItemList/Dt with
                // whatever GetResult() returns, so a raw filtered RowIndex silently
                // binds the wrong row - in practice almost always row 0 of Dt, i.e.
                // "always selects the first data" when a search narrows to one match.
                // Fix: resolve the clicked row back to its real index in Dt via the
                // stable item_id key before returning it, so GetResult()'s contract
                // (an index into Dt) holds whether or not the grid is filtered.
                DataRow clickedRow = (dg_item_bom.Rows[e.RowIndex].DataBoundItem as DataRowView)?.Row;

                if (clickedRow != null && Dt.Columns.Contains("item_id"))
                {
                    object itemId = clickedRow["item_id"];
                    int resolvedIndex = -1;

                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (Equals(Dt.Rows[i]["item_id"], itemId))
                        {
                            resolvedIndex = i;
                            break;
                        }
                    }

                    if (resolvedIndex >= 0)
                    {
                        this.result = resolvedIndex;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                }

                // Fallback (no item_id column, or DataBoundItem unavailable): grid is
                // presumably unfiltered and bound directly to Dt, so the clicked index
                // already matches Dt's own row order.
                this.result = e.RowIndex;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void InitializeSearchBox()
        {
            txt_search = Helpers.CreateSearchBox(placeHolderText, txt_search_TextChanged);
            this.Controls.Add(txt_search);
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string searchText = txt_search.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == placeHolderText)
            {
                dg_item_bom.DataSource = Dt;
            }
            else
            {
                var searchedData = Helpers.FilterDataTable(Dt, searchText, "general_name", "item_code", "item_model");
                dg_item_bom.DataSource = searchedData;
            }
        }
    }
}
