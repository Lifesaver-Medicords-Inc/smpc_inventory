using smpc_app.Services.Helpers;
using smpc_inventory_app.Services.Setup.Item;
using smpc_inventory_app.Services.Setup.Model.Bom;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smpc_inventory_app.Pages.Engineering.Bom
{
    // The BOM screen's Search, done on the server.
    //
    // BomSearch filtered the BOM heads the screen had downloaded. That worked while the screen
    // held all 5,842; now that it holds 20, searching the local copy would only ever find the
    // page you are already on. This asks the API instead - 20 matches to a page, Previous/Next,
    // and a "Page x of y" count - and hands back the chosen BOM's id so the screen can open the
    // page starting at it. Same shape as Item Entry's ItemSearchModal.
    internal class BomPagedSearchModal : Form
    {
        // 0 when nothing was chosen.
        public int SelectedBomId { get; private set; }

        private readonly TextBox txt_search = new TextBox();
        private readonly DataGridView dgv_boms = new DataGridView();
        private readonly Button btn_prev = new Button();
        private readonly Button btn_next = new Button();
        private readonly Button btn_select = new Button();
        private readonly Label lbl_page = new Label();
        // Typing fires a request per keystroke otherwise - one settling pause instead.
        private readonly Timer _typingTimer = new Timer { Interval = 350 };

        private int _page = 1;
        private int _totalPages;
        private bool _isLoading;

        public BomPagedSearchModal()
        {
            Text = "Search BOM";
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(820, 520);
            MinimumSize = new Size(560, 380);

            Panel top = new Panel { Dock = DockStyle.Top, Height = 46, Padding = new Padding(10, 10, 10, 4) };
            Label lbl_search = new Label { Text = "SEARCH", AutoSize = true, Location = new Point(10, 15) };
            txt_search.Location = new Point(70, 11);
            txt_search.Width = top.ClientSize.Width - 90;
            txt_search.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            top.Controls.Add(lbl_search);
            top.Controls.Add(txt_search);

            dgv_boms.Dock = DockStyle.Fill;
            dgv_boms.AllowUserToAddRows = false;
            dgv_boms.AllowUserToDeleteRows = false;
            dgv_boms.ReadOnly = true;
            dgv_boms.MultiSelect = false;
            dgv_boms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_boms.AutoGenerateColumns = false;
            dgv_boms.RowHeadersVisible = false;
            dgv_boms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv_boms.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_boms.Columns.Add(Column("item_code", "ITEM CODE", 15));
            dgv_boms.Columns.Add(Column("general_name", "GENERAL NAME", 25));
            dgv_boms.Columns.Add(Column("item_model", "MODEL", 25));
            dgv_boms.Columns.Add(Column("short_desc", "DESCRIPTION", 35));

            Panel bottom = new Panel { Dock = DockStyle.Bottom, Height = 46, Padding = new Padding(10, 8, 10, 8) };
            btn_prev.Text = "<< PREV";
            btn_prev.Dock = DockStyle.Left;
            btn_prev.Width = 90;
            btn_next.Text = "NEXT >>";
            btn_next.Dock = DockStyle.Left;
            btn_next.Width = 90;
            lbl_page.Dock = DockStyle.Left;
            lbl_page.Width = 200;
            lbl_page.TextAlign = ContentAlignment.MiddleCenter;
            btn_select.Text = "SELECT";
            btn_select.Dock = DockStyle.Right;
            btn_select.Width = 100;
            // Added right-to-left: Dock=Left stacks in reverse of the order controls are added.
            bottom.Controls.Add(lbl_page);
            bottom.Controls.Add(btn_next);
            bottom.Controls.Add(btn_prev);
            bottom.Controls.Add(btn_select);

            Controls.Add(dgv_boms);
            Controls.Add(top);
            Controls.Add(bottom);

            txt_search.TextChanged += (s, e) => { _typingTimer.Stop(); _typingTimer.Start(); };
            _typingTimer.Tick += async (s, e) => { _typingTimer.Stop(); _page = 1; await LoadPage(); };
            btn_prev.Click += async (s, e) => { if (_page > 1) { _page--; await LoadPage(); } };
            btn_next.Click += async (s, e) => { if (_page < _totalPages) { _page++; await LoadPage(); } };
            btn_select.Click += (s, e) => SelectBom();
            dgv_boms.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) SelectBom(); };
            Load += async (s, e) => await LoadPage();
        }

        private static DataGridViewTextBoxColumn Column(string property, string header, int fillWeight)
        {
            return new DataGridViewTextBoxColumn
            {
                DataPropertyName = property,
                HeaderText = header,
                FillWeight = fillWeight,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
        }

        private async Task LoadPage()
        {
            if (_isLoading) return;
            _isLoading = true;

            Helpers.Loading.ShowLoading(dgv_boms);
            try
            {
                var result = await BomServices.Search(txt_search.Text, _page);

                List<BomHead> rows = result?.Data ?? new List<BomHead>();
                dgv_boms.DataSource = rows;

                _page = result?.Pagination?.page ?? _page;
                _totalPages = result?.Pagination?.total_pages ?? 0;

                lbl_page.Text = _totalPages == 0
                    ? "No matches"
                    : $"Page {_page} of {_totalPages}  ({result?.Pagination?.total ?? 0} BOMs)";

                btn_prev.Enabled = result?.Pagination?.has_prev ?? false;
                btn_next.Enabled = result?.Pagination?.has_next ?? false;
            }
            catch (Exception ex)
            {
                Helpers.ShowDialogMessage("error", "Failed to search BOMs.\n" + ex.Message);
            }
            finally
            {
                Helpers.Loading.HideLoading(dgv_boms);
                _isLoading = false;
            }
        }

        private void SelectBom()
        {
            if (dgv_boms.SelectedRows.Count == 0) return;

            BomHead selected = dgv_boms.SelectedRows[0].DataBoundItem as BomHead;
            if (selected == null) return;

            SelectedBomId = selected.id;
            DialogResult = DialogResult.OK;
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _typingTimer.Stop();
                _typingTimer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
