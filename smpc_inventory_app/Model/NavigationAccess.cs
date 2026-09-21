using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace smpc_inventory_app.Model
{
    // Hides the sidebar entries this user's position has no access to.
    //
    // The grants live in tbl_position_access, come back on the signed-in user as
    // position.access, and are seeded per position from the access-level workbook's nine
    // "FOR <DEPARTMENT>" menus (ERP_API's SeedPositionAccess). Admin's Access Control screen
    // is what changes them afterwards. This is the same check SMPC_Admin already applies to
    // its own navigation - a position that cannot open a screen never sees it listed.
    //
    // Two deliberate ways of NOT hiding something:
    //
    //   * A node with no code below is left visible. The catalogue does not yet cover every
    //     screen (Production Report and Purchase Return have no code at all), and hiding a
    //     screen because nobody has written down what opens it would be a guess.
    //   * A user whose position carries no grants at all sees the whole sidebar. That is not
    //     a position locked out of everything - it is a position nobody has set up, or a
    //     login that came back without its access list, and blanking the app would turn a
    //     configuration gap into a support call.
    internal static class NavigationAccess
    {
        // Sidebar node name (Layout.Designer.cs TreeNode.Name) -> the access codes that open
        // it (tbl_access_modules.code). More than one code means any of them is enough: the
        // Receiving Report exists twice in the catalogue, once per version of the screen.
        private static readonly Dictionary<string, string[]> NodeCodes =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "ITEM ENTRY", new[] { "Item.Item Entry (main)" } },
                { "BUSINESS PARTNER INFO", new[] { "Business Partner Info.Business Partner Info (main)" } },
                { "PURCHASING LIST", new[] { "Purchasing.Purchasing List (new)", "Purchasing.Purchasing List (old)" } },
                { "PURCHASE ORDER", new[] { "Purchasing.Purchase Order" } },
                { "PURCHASE REQUISITION", new[] { "Purchasing.Purchase Requisition", "Purchasing.Purchase Requisition Card (dashboard)" } },
                // Both had a sidebar entry but no catalogue code, so they could never be
                // hidden - and Purchase Return kept the whole Purchasing heading alive for
                // a warehouse user, who has no purchasing screens at all bar the requisition.
                { "PURCHASE RETURN", new[] { "Purchasing.Purchase Return" } },
                { "PRODUCTION REPORT", new[] { "Inventory.Production Report" } },
                { "BOM", new[] { "Engineering.BOM" } },
                { "BOQ", new[] { "Engineering.BOQ" } },
                { "RECEIVING REPORT", new[] { "Inventory.Receiving Report (v2)", "Inventory.Receiving Report (v1)" } },
                { "INVENTORY LOGBOOK", new[] { "Inventory.Inventory Logbook" } },
                { "INVENTORY TRACKER", new[] { "Inventory.Inventory Tracker" } },
                { "INVENTORY ITEM STOCKS", new[] { "Inventory.Item Stocks" } },
                // Listed by the workbook's warehouse menu but not built here yet (see
                // Layout.NotYetBuilt). They are gated on the codes of the screens they
                // stand in for, which live in dispatching and engineering today, so the
                // position that will use them is the position that sees them.
                { "INVENTORY REPORT LIST", new[] { "Inventory.Inventory Report (modal)" } },
                { "ITEM REQUEST LIST", new[] { "Item Request.Item Request", "Item Request.Item Request (v2)" } },
                { "ITEM RELEASE LIST", new[] { "Item Release.Item Release" } },
                // The list lives in the engineering app as well as dispatching; either grant
                // is enough to see the entry here.
                { "SALES ORDER LIST", new[] { "Sales Order.Order List", "Sales Order.Sales Order (Engineering)" } },
                { "LOGISTICS CALENDAR", new[] { "Logistics Calendar.Calendar View" } },
                { "ITEM BRAND", new[] { "Setup.Item Brand Setup" } },
                { "ITEM CLASS", new[] { "Setup.Item Class Setup" } },
                { "ITEM MATERIAL", new[] { "Setup.Item Material Setup" } },
                { "ITEM NAME", new[] { "Setup.Item Name Setup" } },
                { "ITEM PUMP COUNT", new[] { "Setup.Item Pump Count Setup" } },
                { "ITEM PUMP TYPE", new[] { "Setup.Item Pump Type Setup" } },
                { "ITEM TYPE", new[] { "Setup.Item Type Setup" } },
                { "UNIT OF MEASURE", new[] { "Setup.Unit of Measure Setup" } },
                { "PAYMENT TERMS", new[] { "Setup.Payment Terms Setup" } },
                { "SOCIAL MEDIA", new[] { "Setup.Social Media Setup" } },
                { "ENTITY TYPE", new[] { "Setup.Entity Type" } },
                { "INDUSTRIES", new[] { "Setup.Industries" } },
                { "POSITION", new[] { "Setup.Position Setup" } },
                { "WAREHOUSE USETYPE", new[] { "Setup.Warehouse Use Type Setup" } },
                { "WAREHOUSE", new[] { "Setup.Warehouse Name Setup" } },
                { "VALUATION METHOD", new[] { "Setup.Item Valuation Method Setup" } },
            };

        private static HashSet<string> GrantedCodes()
        {
            var granted = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var access = Data.CacheData.CurrentUser?.position?.access;

            if (access == null)
                return granted;

            foreach (var row in access)
            {
                if (!string.IsNullOrWhiteSpace(row?.code))
                    granted.Add(row.code.Trim());
            }

            return granted;
        }

        public static bool HasAccess(params string[] codes)
        {
            if (codes == null || codes.Length == 0)
                return true;

            var granted = GrantedCodes();

            // See the note above: no grants at all is a configuration gap, not a lockout.
            if (granted.Count == 0)
                return true;

            return codes.Any(code => granted.Contains(code.Trim()));
        }

        // Removes what this position cannot open, then any grouping node left with nothing
        // under it - a "Setup" heading over an empty list is worse than no heading.
        public static void Apply(TreeView sidebar)
        {
            if (sidebar == null)
                return;

            var granted = GrantedCodes();
            if (granted.Count == 0)
                return;

            RemoveUngranted(sidebar.Nodes, granted);
        }

        private static void RemoveUngranted(TreeNodeCollection nodes, HashSet<string> granted)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                TreeNode node = nodes[i];

                RemoveUngranted(node.Nodes, granted);

                string[] codes;
                bool isGrouping = node.Nodes.Count > 0
                                  || string.Equals(node.Name, "parent", StringComparison.OrdinalIgnoreCase);

                if (NodeCodes.TryGetValue(node.Name ?? "", out codes))
                {
                    if (!codes.Any(code => granted.Contains(code.Trim())))
                        nodes.RemoveAt(i);

                    continue;
                }

                // A heading whose children have all gone takes itself with them. A leaf with
                // no code stays - see the note above.
                if (isGrouping && node.Nodes.Count == 0 && HadChildren(node))
                    nodes.RemoveAt(i);
            }
        }

        // A grouping node is one the designer gave the name "parent"; by the time its
        // children have been removed it looks like a leaf, so the name is what tells them
        // apart rather than the (now empty) child collection.
        private static bool HadChildren(TreeNode node)
        {
            return string.Equals(node.Name, "parent", StringComparison.OrdinalIgnoreCase);
        }
    }
}
