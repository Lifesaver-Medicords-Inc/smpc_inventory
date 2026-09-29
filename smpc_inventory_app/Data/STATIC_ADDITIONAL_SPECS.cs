namespace smpc_inventory_app.Data
{
    // The fixed lists behind Item Entry's Additional Specs dropdowns (spec 4.2.3, 17.5).
    //
    // They MUST read exactly like the Advanced Conditions lists on a project quotation -
    // smpc_sales_system/Data/STATIC_ADVANCED_CONDITIONS.cs - so change one, change the other.
    // Item Entry stores the TEXT of the choice (tbl_setup_item_additional_specs.driver_type
    // and friends are nvarchar), where the quotation stores its position in the list; the
    // order here therefore does not matter to saved items, but keep it the same anyway.
    //
    // Hard-coded on the user's instruction (2026-09-26), from the same list the Advanced
    // Conditions were built from.
    static class STATIC_ADDITIONAL_SPECS
    {
        // What an unanswered dropdown shows. Never saved - see frm_Item_Entry.GetAdditionalSpecs.
        public const string SELECT_TEXT = "--SELECT--";

        public static readonly string[] DRIVER_TYPE = { "Electric Motor", "Engine" };

        public static readonly string[] MOTOR_ENCLOSURE = { "NEMA 2", "NEMA 3", "NEMA 4", "NEMA 12" };

        public static readonly string[] MOTOR_MANUFACTURER = { "Teco Motor", "Franklin Motor", "Calpeda Motor" };

        public static readonly string[] LIQUID_TYPE = { "Clean", "Sewage", "Seawater" };
    }
}
