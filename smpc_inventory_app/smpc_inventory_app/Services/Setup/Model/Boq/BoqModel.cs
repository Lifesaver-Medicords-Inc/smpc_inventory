using smpc_inventory_app.Models;
using smpc_inventory_app.Models;
using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace smpc_inventory_app.Services.Setup.Model.Boq
    {
        public class BoqModel
        {
            public int ID { get; set; }
            public int ItemID { get; set; }
            public string Customer { get; set; }
            public string ProjectName { get; set; }
            public string ItemSetName { get; set; }
            public string DocRef { get; set; }
        }

        public class BoqDetailModel
        {
              
      
        public string components { get; set; }
        public string item_name { get; set; }
        public string short_desc { get; set; }
        public string size { get; set; }
        public float component_total { get; set; }
        public string unit_of_measure { get; set; }
        public int qty { get; set; }
        public string item_set_name { get; set; }
        public string customer_name { get; set; }
        public string remarks { get; set; }
        public string notes { get; set; }
        public string model { get; set; }

        public BoqDetailModel(string components, string itemName, string shortDesc, string size,
                              float componentTotal, string unitOfMeasure, int qty, string itemSetName, string customerName,
                              string remarks, string notes)
        {
            this.components = components;
            this.item_name = itemName;
            this.short_desc = shortDesc;
            this.size = size;
            this.component_total = componentTotal;
            this.unit_of_measure = unitOfMeasure;
            this.qty = qty;
            this.item_set_name = itemSetName;
            this.customer_name = customerName;
            this.remarks = remarks;
            this.notes = notes;
        }


    }

        // Move this outside the ProjectComponent class
        public class ProjectComponentClass
        {
            public List<ProjectComponent> project_component { get; set; }
        }
        public class QQResponse
        {
            public QQView[] qq_view { get; set; }
        }
        public class WiringNotesResponse
        {
            public WiringNotes[] wiring_notes { get; set; }
        }

        public class QQView
        {
        public int qq_id { get; set; }
        public int based_id { get; set; }
        public string components { get; set; }
        public string model { get; set; }
        public int item_id { get; set; }
        public int bom_id { get; set; }
        public bool is_child { get; set; }
        public int qty { get; set; }
        public string unit_of_measure { get; set; }
        public string remarks { get; set; }
        public string notes { get; set; }
        public string customer_name { get; set; }
        public int qq_note_id { get; set; }
    }
    public class WiringNotes
    {
        public int wiring_id { get; set; }
        public int based_id { get; set; }
        public string materials { get; set; }
        public string item_description { get; set; }
        public string num_of_wires_set { get; set; }
        public string num_of_qty_set { get; set; }
        public string distance_travelled_set { get; set; }
        public string allowance_wire_set { get; set; }
        public string num_of_sets { get; set; }
        public int total_qty { get; set; }
        public string wiring_note { get; set; }
        public int note_id { get; set; }
    }
        public class ProjectComponent
    {
            public int quotation_id { get; set; }
            public string project_name { get; set; }
            public int customer_id { get; set; }
            public int set_id { get; set; }
            public int item_set_based_on_quotation_id { get; set; }
            public string item_set_name { get; set; }
            public int bom_id { get; set; }
            public int items_id { get; set; }
            public string model { get; set; }
            public int item_id { get; set; }
            public int qty { get; set; }
            public int based_on_set_id { get; set; }
            public string components { get; set; }
            public uint node_id { get; set; }
            public string node_name { get; set; }
            public uint node_order { get; set; }
            public string node_type { get; set; }
            public uint parent_node_id { get; set; }  // New field
            public string item_name { get; set; }     // New field
            public string short_desc { get; set; }    // New field
            public string size { get; set; }          // New field
            public float component_total { get; set; }
            public string unit_of_measure { get; set; }
            public string customer_name { get; set; }
            public string remarks { get; set; }
            public string notes { get; set; }
            public uint boq_id { get; set; }   

        //wiring
        //public string materials { get; set; }
        //public int amp_req { get; set; }
        //public int wire_req { get; set; }
        //public string wiring_description { get; set; }
        //public int num_of_wires_set { get; set; }
        //public int num_of_qty_set { get; set; }
        //public int distance_travelled_set { get; set; }
        //public int allowance_wire_set { get; set; }
        //public int wiring_qty { get; set; }
        //public int num_of_sets { get; set; }
        //public int total_qty { get; set; }
        //public int cost { get; set; }
        //public int total_cost { get; set; }



        public ProjectComponent(int quotationId, string projectName, int customerId, int setId, int itemSetBasedOnQuotationId, string itemSetName,
                                int bomId, int itemsId, int qty, int basedOnSetId, string components, uint nodeId, string nodeName, uint nodeOrder,
                                string nodeType, int itemId, float componentTotal, string unitOfMeasure, string customerName, string model, string remarks, string notes)
                                //int ampReq,
                                //int wireReq, string wiringDescription, int numOfWiresSet, int numOfQtySet, int distanceTravelledSet, int allowanceWireSet, 
                                //int wiringQty, int numOfSets, int totalQty, int cost, int totalCost)
        {
            this.quotation_id = quotationId;
            this.project_name = projectName;
            this.customer_id = customerId;
            this.set_id = setId;
            this.item_set_based_on_quotation_id = itemSetBasedOnQuotationId;
            this.item_set_name = itemSetName;
            this.bom_id = bomId;
            this.items_id = itemsId;
            this.qty = qty;
            this.based_on_set_id = basedOnSetId;
            this.components = components;
            this.node_id = nodeId;
            this.node_name = nodeName;
            this.node_order = nodeOrder;
            this.node_type = nodeType;
            this.item_id = itemId;
            this.component_total = componentTotal;
            this.unit_of_measure = unitOfMeasure;
            this.customer_name = customerName;
            this.model = model;
            this.remarks = remarks;
            this.notes = notes;
            //WIRING
            //this.materials = materials;
            //this.amp_req = ampReq;
            //this.wire_req = wireReq;
            //this.wiring_description = wiringDescription;
            //this.num_of_wires_set = numOfWiresSet;
            //this.num_of_qty_set = numOfQtySet;
            //this.distance_travelled_set = distanceTravelledSet;
            //this.allowance_wire_set = allowanceWireSet;
            //this.wiring_qty = wiringQty;
            //this.num_of_sets = numOfSets;
            //this.total_qty = totalQty;
            //this.cost = cost;
            //this.total_cost = totalCost;


        }
    }

}



