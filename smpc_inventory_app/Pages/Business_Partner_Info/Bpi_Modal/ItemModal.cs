
using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Helpers;
using smpc_inventory_app.Services.Setup;
using smpc_inventory_app.Services.Setup.Bpi;
using smpc_inventory_app.Services.Setup.Model.Bpi;
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
        private TextBox txt_search;
        private const string SearchPlaceholder = "Item Search...";

        // GetResult() used to return one item (a single click closed the modal).
        // Multi-select (requested): every checked row is now collected into a list
        // instead - see btn_add_selected_Click.
        private List<Dictionary<string, dynamic>> results { get; set; }

        // The rows on screen now - one page of 20, from the server. This used to be the
        // whole vw_bpi_item_list (3,707 rows once the Calpeda catalogue landed), filtered
        // in memory.
        private List<ItemBpiList> _rows = new List<ItemBpiList>();

        // Ticks live HERE, not in the grid, and they are keyed by item id with the whole
        // row kept alongside. Two reasons: a page change rebinds the grid and would
        // otherwise lose every tick, and "Add Selected Items" needs each ticked row's
        // price and descriptions even after you have paged away from it.
        private readonly Dictionary<int, ItemBpiList> _checked = new Dictionary<int, ItemBpiList>();

        private string _search = "";
        private int _page = 1;
        private int _totalPages;
        private bool _isLoading;
        private readonly Timer _typingTimer = new Timer { Interval = 350 };

        private readonly Button btn_page_prev = new Button { Text = "<< PREV", Dock = DockStyle.Left, Width = 90 };
        private readonly Button btn_page_next = new Button { Text = "NEXT >>", Dock = DockStyle.Left, Width = 90 };
        private readonly Label lbl_page = new Label { Dock = DockStyle.Left, Width = 250, TextAlign = ContentAlignment.MiddleCenter };

        public ItemModal()
        {
            InitializeComponent();
            InitializeSearchBox();
            BuildPager();
        }

        // Added to the existing footer in code, so ItemModal.Designer.cs and its .resx are
        // left alone.
        private void BuildPager()
        {
            pnl_footer.Controls.Add(lbl_page);
            pnl_footer.Controls.Add(btn_page_next);
            pnl_footer.Controls.Add(btn_page_prev);
            lbl_page.BringToFront();
            btn_page_next.BringToFront();
            btn_page_prev.BringToFront();

            btn_page_prev.Click += async (s, e) => { if (_page > 1) await LoadPage(_page - 1); };
            btn_page_next.Click += async (s, e) => { if (_page < _totalPages) await LoadPage(_page + 1); };

            _typingTimer.Tick += async (s, e) => { _typingTimer.Stop(); await LoadPage(1); };
            // Not a Dispose(bool) override - the Designer already declares one.
            FormClosed += (s, e) => { _typingTimer.Stop(); _typingTimer.Dispose(); };
        }

        private void InitializeSearchBox()
        {
            txt_search = Helpers.CreateSearchBox(SearchPlaceholder, txt_search_TextChanged);
            // Docked into pnl_dgv (not the form directly), above dg_ItemList (Dock=Fill).
            //
            // SendToBack, not BringToFront. WinForms lays docked controls out from the
            // back of the z-order to the front, and a Fill control takes whatever space
            // is left when its turn comes. BringToFront put the search box at the front,
            // so the grid docked first and filled the whole panel, and the search box
            // was then laid over the grid's top edge - hiding its header row
            // (user-reported 2026-09-14). At the back, the box takes the top strip
            // first and the grid fills the space below it.
            pnl_dgv.Controls.Add(txt_search);
            txt_search.SendToBack();
        }

        // Searches the whole catalogue on the server. Debounced, because every keystroke
        // is now a round trip rather than a filter over a table already in memory.
        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string searchText = txt_search.Text.Trim();

            // CreateSearchBox writes the placeholder into the box itself, which fires this
            // handler - treat it as an empty search rather than searching for its text.
            _search = (string.IsNullOrEmpty(searchText) || searchText == SearchPlaceholder)
                ? ""
                : searchText;

            _typingTimer.Stop();
            _typingTimer.Start();
        }

        private async Task LoadPage(int page)
        {
            if (_isLoading) return;
            _isLoading = true;

            Helpers.Loading.ShowLoading(this);
            try
            {
                var result = await ItemListBpiServices.GetPaged(_search, page);

                _rows = result?.Data ?? new List<ItemBpiList>();

                // Re-apply ticks: the rows are new objects on every page fetch, so a row
                // that was checked earlier arrives unchecked unless it is marked here.
                foreach (ItemBpiList row in _rows)
                    row.Selected = _checked.ContainsKey(row.id);

                // The Designer already declares every column with a DataPropertyName, so
                // auto-generation would add a second set of them.
                dg_ItemList.AutoGenerateColumns = false;
                dg_ItemList.DataSource = null;
                dg_ItemList.DataSource = _rows;

                _page = result?.Pagination?.page ?? page;
                _totalPages = result?.Pagination?.total_pages ?? 0;

                lbl_page.Text = _totalPages == 0
                    ? "No items found"
                    : $"Page {_page} of {_totalPages}  ({result?.Pagination?.total ?? 0} items, {_checked.Count} ticked)";

                btn_page_prev.Enabled = result?.Pagination?.has_prev ?? false;
                btn_page_next.Enabled = result?.Pagination?.has_next ?? false;
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", "Failed to load the item list.\n" + ex.Message);
            }
            finally
            {
                Helpers.Loading.HideLoading(this);
                _isLoading = false;
            }
        }

        private async void ItemModal_Load(object sender, EventArgs e)
        {
            await LoadPage(1);
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
            if (e.RowIndex < 0 || e.RowIndex >= _rows.Count)
                return;

            // Toggled on the bound object, not the cell: the grid is ReadOnly (Designer),
            // so a cell write would not push back through the binding. The old
            // EndEdit/CommitEdit dance existed for exactly that reason and is no longer
            // needed.
            ItemBpiList row = _rows[e.RowIndex];
            row.Selected = !row.Selected;

            if (row.Selected)
                _checked[row.id] = row;
            else
                _checked.Remove(row.id);

            dg_ItemList.InvalidateRow(e.RowIndex);

            if (_totalPages > 0)
            {
                lbl_page.Text = $"Page {_page} of {_totalPages}  ({_checked.Count} ticked)";
            }
        }

        private void btn_add_selected_Click(object sender, EventArgs e)
        {
            // Reads the tick register, not the grid - so items ticked on an earlier page
            // are still included after paging away from them.
            var selectedRows = new List<Dictionary<string, dynamic>>();

            foreach (ItemBpiList row in _checked.Values)
            {
                var data = new Dictionary<string, dynamic>();
                data.Add("item_id", row.id);
                data.Add("item_type", row.item_type ?? "");
                data.Add("item_code", row.item_code ?? "");
                data.Add("long_description", row.long_description ?? "");
                data.Add("item_price", row.item_price);
                // BusinessPartnerInfo.cs's dg_items_CellClick reads short_desc /
                // status_tangible / status_trade and threw KeyNotFoundException before
                // vw_bpi_item_list carried them - the paged endpoint returns the whole
                // row precisely so these keep working.
                data.Add("short_desc", row.short_desc ?? "");
                data.Add("status_tangible", row.status_tangible ?? "");
                data.Add("status_trade", row.status_trade ?? "");

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
