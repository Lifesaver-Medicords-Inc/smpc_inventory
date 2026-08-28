
using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Bpi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_sales_app.Pages
{
    public partial class ItemModal : Form
    {
        private DataTable _allItems;
        private TextBox txt_search;
        private const string SearchPlaceholder = "Item Search...";

        // GetResult() used to return one item (a single click closed the modal).
        // Multi-select (requested): every checked row is now collected into a list
        // instead - see btn_add_selected_Click.
        private List<Dictionary<string, dynamic>> results { get; set; }

        public ItemModal()
        {
            InitializeComponent();
            InitializeSearchBox();
        }

        private void InitializeSearchBox()
        {
            txt_search = Helpers.CreateSearchBox(SearchPlaceholder, txt_search_TextChanged);
            // Docked into pnl_dgv (not the form directly): dg_ItemList is Dock=Fill
            // inside that same panel, so a Dock=Top sibling added here sits above it
            // and the grid automatically shrinks to fit below - no manual layout math
            // needed, and no risk of landing above pnl_title at the form level.
            pnl_dgv.Controls.Add(txt_search);
            txt_search.BringToFront();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            if (_allItems == null || _allItems.Rows.Count == 0)
                return;

            string searchText = txt_search.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == SearchPlaceholder)
            {
                dg_ItemList.DataSource = _allItems;
            }
            else
            {
                DataTable filtered = Helpers.FilterDataTable(_allItems, searchText,
                    "general_name", "item_type", "item_model_name", "item_brand_name", "item_code", "long_description");
                dg_ItemList.DataSource = filtered;
            }
        }

        private async void GetItemList()
        {
            var data = await ItemListBpiServices.GetAsDatatable();

            // Multi-select (requested): a real bool column, not left typeless - the
            // "selected" DataGridViewCheckBoxColumn needs a bool-typed source to bind
            // cleanly. Columns.Add's DefaultValue only applies to rows created via
            // NewRow() afterwards, not the rows already in `data` from the API
            // response, so every existing row is set explicitly to false too.
            if (data != null && !data.Columns.Contains("Selected"))
            {
                data.Columns.Add("Selected", typeof(bool)).DefaultValue = false;
                foreach (DataRow row in data.Rows)
                {
                    row["Selected"] = false;
                }
            }

            _allItems = data;
            dg_ItemList.DataSource = data;
        }

        private void ItemModal_Load(object sender, EventArgs e)
        {
            GetItemList();
        }

        public List<Dictionary<string, dynamic>> GetResult()
        {
            return results;
        }

        // Multi-select (requested): clicking a row toggles its checkbox instead of
        // immediately closing the modal with that one item - picking is now a
        // separate, explicit step (btn_add_selected_Click) so more than one item can
        // be checked first.
        private void dgv_itemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Live crash (fixed earlier): NullReferenceException ("DataGridViewCell.
            // Value.get returned null") - dg_ItemList is bound directly to a DataTable,
            // so it still shows the trailing "add new row" placeholder by default.
            // Clicking that row fires CellClick with a real, non-negative RowIndex,
            // but every cell's Value is genuinely null (no DataRow backs it yet)
            // rather than DBNull - IsNewRow is what actually catches that case.
            if (e.RowIndex < 0 || e.RowIndex >= dg_ItemList.Rows.Count || dg_ItemList.Rows[e.RowIndex].IsNewRow)
                return;

            DataGridViewRow row = dg_ItemList.Rows[e.RowIndex];
            var checkboxCell = row.Cells["selected"];
            bool current = checkboxCell.Value as bool? ?? false;
            checkboxCell.Value = !current;

            // A checkbox cell's new Value doesn't commit to the bound DataTable until
            // the cell loses focus - without this, the very next click (e.g. on
            // btn_add_selected) would still see the old value.
            dg_ItemList.EndEdit();
            dg_ItemList.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btn_add_selected_Click(object sender, EventArgs e)
        {
            dg_ItemList.EndEdit();

            var selectedRows = new List<Dictionary<string, dynamic>>();

            foreach (DataGridViewRow row in dg_ItemList.Rows)
            {
                if (row.IsNewRow) continue;

                bool isChecked = row.Cells["selected"].Value as bool? ?? false;
                if (!isChecked) continue;

                int.TryParse(row.Cells["id"].Value?.ToString(), out int item_id);
                string item_type = row.Cells["item_type"].Value?.ToString() ?? "";
                string item_code = row.Cells["item_code"].Value?.ToString() ?? "";
                string long_description = row.Cells["long_description"].Value?.ToString() ?? "";
                float.TryParse(row.Cells["item_price"].Value?.ToString(), out float item_price);

                var data = new Dictionary<string, dynamic>();
                data.Add("item_id", item_id);
                data.Add("item_type", item_type);
                data.Add("item_code", item_code);
                data.Add("long_description", long_description);
                data.Add("item_price", item_price);

                selectedRows.Add(data);
            }

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Please check at least one item.", "Item List", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.results = selectedRows;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dg_ItemList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
