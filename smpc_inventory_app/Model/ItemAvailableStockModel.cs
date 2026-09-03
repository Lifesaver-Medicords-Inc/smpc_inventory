namespace smpc_inventory_app.Model
{
    // One row of GET /inventory/item_stocks/available - spec §8.11's stock figures for a
    // single item, already aggregated across every warehouse and bin by the API.
    //
    // `available` is the number to show a user deciding what to buy or promise: `physical`
    // still counts units that are spoken for. §8.11 defines TOTAL STOCK as excluding
    // reserved, and §8.12 makes that same figure the STOCK input to the purchasing
    // quantities formula.
    // Named ItemAvailableStockModel, not AvailableStockModel: the sales app already has
    // its own AvailableStockModel for this same endpoint, and it references this assembly
    // - so sharing the short name made every use of it in the sales app ambiguous between
    // the two namespaces. The two are separate copies on purpose; neither app owns a
    // shared model project.
    public class ItemAvailableStockModel
    {
        public int item_id { get; set; }
        public int physical { get; set; }
        public int reserved { get; set; }
        public int available { get; set; }
    }
}
