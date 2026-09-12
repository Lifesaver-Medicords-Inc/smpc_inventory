using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Model
{
    public class CurrentUserModel
    {
        public int id { get; set; }
        public string employee_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        // The sales executive's name as BPI stores it in sales_id: first name and
        // last name, e.g. "Julie Cestona". sales_id holds the NAME, not the
        // employee id (management decision), so every ownership check compares
        // against this. Computed locally; never sent back to the API.
        [Newtonsoft.Json.JsonIgnore]
        public string full_name
        {
            get { return SalesOwner.Normalize((first_name ?? "") + " " + (last_name ?? "")); }
        }
        public string department { get; set; }
        public string position_id { get; set; }
        public UserPermissionModel permissions { get; set; }
        public PositionModel position { get; set; }
    }

    public class UserPermissionModel
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public bool can_create { get; set; }
        public bool can_update { get; set; }
        public bool can_delete { get; set; }
    }

    public class PositionModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<PositionAccessModel> access { get; set; } = new List<PositionAccessModel>();
        public ICollection<CurrentUserModel> users { get; set; } = new List<CurrentUserModel>();
    }

    public class PositionAccessModel
    {
        public int id { get; set; }
        public int position_id { get; set; }
        public string code { get; set; }
    }

    // BPI ownership is keyed on the sales executive's NAME - tbl_bpi.sales_id and
    // tbl_bpi_general.sales_id hold e.g. "Julie Cestona", not an employee id.
    // Every ownership check in inventory AND sales goes through here, so they all
    // normalise the same way; the sales app reaches it through its reference to
    // this assembly.
    //
    // Known weakness, accepted with that decision: a renamed user stops matching
    // the partners stamped with their old name, and two people with the same full
    // name would share ownership. Renaming a sales executive therefore means
    // renaming their partners in the same step.
    public static class SalesOwner
    {
        // Trims and collapses internal whitespace, so "J.  CESTONA " and
        // "J. CESTONA" compare equal.
        public static string Normalize(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;
            return string.Join(" ", name.Split((char[])null, StringSplitOptions.RemoveEmptyEntries));
        }

        // Case- and whitespace-insensitive. Two blanks are NOT a match, so an
        // unowned partner never counts as "owned by" a user with no name.
        public static bool Same(string a, string b)
        {
            string x = Normalize(a), y = Normalize(b);
            return x.Length > 0 && string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
        }

        // A partner with no owner at all - anyone who may edit BPI may edit it.
        // OFFICE is NOT shared: it is a user of its own (management, 2026-09-11),
        // so OFFICE partners belong to that user exactly like any other owner's.
        public static bool IsShared(string owner)
        {
            return Normalize(owner).Length == 0;
        }

        // True when this owner value belongs to this user.
        public static bool OwnedBy(string owner, CurrentUserModel user)
        {
            return user != null && Same(owner, user.full_name);
        }
    }

    // Who may open, view and edit Business Partner Info - the one place the BPI
    // role rules live (spec 3.2 and 4.1.10, as decided 2026-09-11). Inventory's
    // BPI page, both apps' sidebars, and every branch tab ask here.
    //
    //   Role         opens  views the contents of               edits
    //   Admin        yes    every record                        every record
    //   Manager      yes    every record                        own + shared
    //   Sales        yes    own + shared                        own + shared
    //   Purchasing   yes    supplier branches + own + shared    own + shared
    //   Accounting   yes    every record                        nothing
    //   anyone else  no
    //
    // Partner NAMES are always visible; these rules decide what a record shows.
    // "Shared" is SalesOwner.IsShared - a record with no owner at all.
    // The Sales Manager's quotation customer picker is NOT widened by this: it
    // still lists only partners they own (Quotation.cs, spec 5.1).
    //
    // Matched on position name first, then department, because that is what the
    // data holds: one Admin sits in the Engineering department, the purchasing
    // position is stored misspelled ("Purschasing"), and a department is only
    // as good as what was picked when the account was created.
    //
    // Client-side only, like the ownership lock it extends: GET /api/bpi still
    // returns every partner to any logged-in user.
    public static class BpiAccess
    {
        private enum Role { None, Admin, Manager, Sales, Purchasing, Accounting }

        // Entity codes from tbl_setup_bpi_entity that make a branch a supplier.
        // SUP and TSP are spec 17.3's; "SUP (TEMP)" is a further supplier type
        // present in the setup data.
        private static readonly string[] SupplierCodes = { "SUP", "TSP", "SUP (TEMP)" };

        private static bool Is(string value, params string[] names)
        {
            return names.Any(n => SalesOwner.Same(value, n));
        }

        private static Role RoleOf(CurrentUserModel user)
        {
            if (user == null) return Role.None;
            string position = user.position?.name;
            string department = user.department;

            if (Is(position, "Admin"))
                return Role.Admin;
            if (Is(position, "Sales Manager", "Chief Business Development Officer", "CBDO"))
                return Role.Manager;
            if (Is(position, "Sales Representatives") || Is(department, "Sales"))
                return Role.Sales;
            if (Is(position, "Purchasing", "Purschasing") || Is(department, "Purchasing"))
                return Role.Purchasing;
            if (Is(position, "Accounts Receivables", "Accounts Payable")
                || Is(department, "A/R", "A/P", "A/R\u2013A/P Cashier", "A/R-A/P Cashier"))
                return Role.Accounting;
            return Role.None;
        }

        // Whether the BPI page (and its sidebar entry) is available at all.
        public static bool CanOpen(CurrentUserModel user)
        {
            return RoleOf(user) != Role.None;
        }

        // Whether NEW, EDIT and the branch-tab menu are offered at all.
        // Accounting only views.
        public static bool CanEditAny(CurrentUserModel user)
        {
            Role role = RoleOf(user);
            return role == Role.Admin || role == Role.Manager || role == Role.Sales || role == Role.Purchasing;
        }

        public static bool CanCreate(CurrentUserModel user)
        {
            return CanEditAny(user);
        }

        // Whether a record's CONTENTS may be shown. entityCodes is the record's
        // entity_names, e.g. "SUP,CUS".
        public static bool CanView(CurrentUserModel user, string owner, string entityCodes)
        {
            switch (RoleOf(user))
            {
                case Role.Admin:
                case Role.Manager:
                case Role.Accounting:
                    return true;
                case Role.Purchasing:
                    return IsSupplier(entityCodes) || SalesOwner.IsShared(owner) || SalesOwner.OwnedBy(owner, user);
                case Role.Sales:
                    return SalesOwner.IsShared(owner) || SalesOwner.OwnedBy(owner, user);
                default:
                    return false;
            }
        }

        // Whether an existing record may be changed. Viewing never implies this:
        // a manager, or purchasing on a supplier, sees another executive's record
        // read-only.
        public static bool CanEdit(CurrentUserModel user, string owner)
        {
            Role role = RoleOf(user);
            if (role == Role.Admin) return true;
            if (role == Role.Manager || role == Role.Sales || role == Role.Purchasing)
                return SalesOwner.IsShared(owner) || SalesOwner.OwnedBy(owner, user);
            return false;
        }

        public static bool IsSupplier(string entityCodes)
        {
            if (string.IsNullOrWhiteSpace(entityCodes)) return false;
            return entityCodes.Split(',').Any(code => SupplierCodes.Any(s => SalesOwner.Same(code, s)));
        }

        // The hover text on a branch tab (spec 4.1.10): who to ask about a
        // branch the user cannot open.
        public static string OwnerLabel(string owner)
        {
            if (SalesOwner.Normalize(owner).Length == 0) return "No sales executive";
            return "Sales executive: " + SalesOwner.Normalize(owner);
        }
    }
}
