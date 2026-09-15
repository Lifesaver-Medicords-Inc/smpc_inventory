using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace smpc_inventory_app.Services.Setup.Model.Item
{
    public class AdditionalSpecsModel
    {
        public int id { get; set; }
        public int based_id { get; set; }
        public int material_id { get; set; }
        public string suction_pressure { get; set; }
        public string driver_type { get; set; }
        public string motor_enclosure { get; set; }
        public string motor_manufacturer { get; set; }
        public string service_factor { get; set; }
        public string liquid_type { get; set; }
        public string connection_type { get; set; }

        // The API spells these "compatibility"; the property names keep the spelling the rest of
        // the app uses. Without the mapping they never filled, so an item's saved pump types and
        // pump count did not show on Item Entry (user-reported 2026-09-14).
        [JsonProperty("pump_type_compatibility_id")]
        public string pump_type_compatability_id { get; set; }
        [JsonProperty("pump_type_compatibility_names")]
        public string pump_type_compatability_names { get; set; }
        [JsonProperty("pump_count_compatibility_id")]
        public int pump_count_compatability_id { get; set; }

        public string size { get; set; }
        public decimal volume { get; set; }
        public int volume_unit_of_measure_id { get; set; }
        public decimal weight { get; set; }
        public int weight_unit_of_measure_id { get; set; }
        public string calibration { get; set; }
        //public decimal length { get; set; }
        //public int length_unit_of_measure_id { get; set; }
        //public decimal height { get; set; }
        //public int height_unit_of_measure_id { get; set; }
        public string long_description { get; set; }
    }
    class AdditionalSpecs
    {
        public List<AdditionalSpecsModel> additionalspecs { get; set; }
    }
}
