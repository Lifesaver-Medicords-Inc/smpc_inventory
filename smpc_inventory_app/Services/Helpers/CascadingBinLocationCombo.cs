using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using smpc_inventory_app.Model;

namespace smpc_inventory_app.Services.Helpers
{
    /// <summary>
    /// Same cascading zone -&gt; area -&gt; rack -&gt; level -&gt; bins picker as
    /// <see cref="BinLocationComboOverlay"/> (Receiving Report's grid-cell bin
    /// location column), applied to a single standalone ComboBox instead of a
    /// DataGridView cell overlay. Used anywhere a form (not a grid) needs the
    /// same "one bin, staged selection" behavior - ItemStockAddModal and
    /// ItemStockTransferModal, both of which previously flattened every
    /// "zone-area-rack-level-bins" combination into one long dropdown instead
    /// of matching Receiving Report's guided pick-one-stage-at-a-time UX.
    ///
    /// Selection-only, same as the overlay: typed characters are blocked, so a
    /// bin location can only ever be one of the values configured in Warehouse
    /// Setup - not free text. A warehouse with no bin-area setup yet simply
    /// shows no options at stage 0, the same as Receiving Report does.
    /// </summary>
    public class CascadingBinLocationCombo
    {
        private static readonly string[] Stages = { "zone", "area", "rack", "level", "bins" };
        private const string Sep = "-";

        private readonly ComboBox _combo;
        private List<ReceivingWarehouseAreaView> _areaData = new List<ReceivingWarehouseAreaView>();
        private readonly List<string> _chosen = new List<string>();
        private bool _suppress;

        public CascadingBinLocationCombo(ComboBox combo)
        {
            _combo = combo;
            _combo.DropDownStyle = ComboBoxStyle.DropDown;
            _combo.SelectedIndexChanged += OnSelected;
            _combo.KeyDown += OnKeyDown;
            _combo.KeyPress += OnKeyPress;
        }

        /// <summary>The fully-assembled "zone-area-rack-level-bins" value so far.</summary>
        public string Value => string.Join(Sep, _chosen);

        /// <summary>Load a new warehouse's area data and reset to stage 0 (zone).</summary>
        public void SetData(List<ReceivingWarehouseAreaView> areaData)
        {
            _areaData = areaData ?? new List<ReceivingWarehouseAreaView>();
            _chosen.Clear();
            PopulateCurrentStage();
        }

        /// <summary>Clear both the data and the current selection (e.g. warehouse deselected).</summary>
        public void Clear() => SetData(new List<ReceivingWarehouseAreaView>());

        private void PopulateCurrentStage()
        {
            var items = BuildItems();

            _suppress = true;
            _combo.Items.Clear();
            foreach (var item in items) _combo.Items.Add(item);
            _combo.Text = Value;
            _suppress = false;
        }

        private string[] BuildItems()
        {
            int stage = _chosen.Count;
            if (stage >= Stages.Length || _areaData.Count == 0) return Array.Empty<string>();

            IEnumerable<ReceivingWarehouseAreaView> rows = _areaData;
            for (int i = 0; i < stage; i++)
            {
                string chosen = _chosen[i];
                string prop = Stages[i];
                rows = rows.Where(r => string.Equals(PropValue(r, prop), chosen, StringComparison.OrdinalIgnoreCase));
            }

            return rows
                .Select(r => PropValue(r, Stages[stage]))
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(v => v, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        private static string PropValue(ReceivingWarehouseAreaView r, string prop)
        {
            switch (prop)
            {
                case "zone": return r.zone ?? string.Empty;
                case "area": return r.area ?? string.Empty;
                case "rack": return r.rack ?? string.Empty;
                case "level": return r.level ?? string.Empty;
                case "bins": return r.bins ?? string.Empty;
                default: return string.Empty;
            }
        }

        private void OnSelected(object sender, EventArgs e)
        {
            if (_suppress || _combo.SelectedItem == null) return;

            _chosen.Add(_combo.SelectedItem.ToString());
            PopulateCurrentStage();

            // Auto-drop the next stage, same as the grid overlay - keeps the pick
            // flowing without an extra click per segment.
            if (_chosen.Count < Stages.Length && _combo.Items.Count > 0)
            {
                _combo.BeginInvoke(new Action(() =>
                {
                    if (!_combo.IsDisposed) _combo.DroppedDown = true;
                }));
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && _chosen.Count > 0)
            {
                e.SuppressKeyPress = true;
                _chosen.RemoveAt(_chosen.Count - 1);
                PopulateCurrentStage();
                if (_combo.Items.Count > 0) _combo.DroppedDown = true;
            }
        }

        // Block all typed characters - the combo is selection-only, same as the
        // grid overlay. Backspace is already handled in KeyDown before KeyPress fires.
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
