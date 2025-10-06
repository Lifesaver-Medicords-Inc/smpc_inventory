using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smpc_inventory_app.Services.Setup.Model.Bpi
{
    class Bpi
    {

        public int id { get; set; }

        public string sales_id { get; set; }

        public string name { get; set; }

        public string main_website { get; set; }

        public string tin { get; set; }

        public string main_tel_no { get; set; }

        public string industry_ids { get; set; }

        public string industry_names { get; set; }


    }

    class BpiGeneral
    {

        public int general_id { get; set; }

        public int general_based_id { get; set; }
        public string branch_sales_id { get; set; }
        public int social_id { get; set; }
        public string branch_name { get; set; }


        public string transaction_type { get; set; }

        public string class_name { get; set; }

        public string branch_tel_no { get; set; }
        public string branch_website { get; set; }
        public string customer_code { get; set; }
        public string supplier_code { get; set; }

        public string fax_no { get; set; }
        public string notes { get; set; }
        public string entity_ids { get; set; }
        public string entity_names { get; set; }


        public string branch_industry_names { get; set; }
        public string branch_industry_ids { get; set; }



    }

    class BpiContacts
    {

        public int contacts_id { get; set; }
        public int contacts_based_id { get; set; }
        public int branch_id { get; set; }
        public string number { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string preferences { get; set; }
        public string contact_notes { get; set; }
        public int position { get; set; }
        public bool is_default_contact { get; set; }

        public BpiContacts(int contactId, int contact_based_id, string number, string name, string email, string preferences, int positions, int branchId,string contactNotes,bool isDefaultContact)
        {
            this.contacts_id = contactId;
            this.contacts_based_id = contact_based_id;
            this.number = number;
            this.name = name;
            this.email = email;
            this.preferences = preferences;
            this.position = positions;
            this.branch_id = branchId;
            this.contact_notes = contactNotes;
            this.is_default_contact = isDefaultContact;
        }

    }

    class BpiAddress
    {
        public int address_ids { get; set; }
        public int address_based_id { get; set; }
        public int address_branch_id { get; set; }
        public string location { get; set; }
        public bool address_is_deleted { get; set; }

        public BpiAddress(int addressId, int addressBasedId, string location, int branchId, bool isDeleted)
        {
            this.address_ids = addressId;
            this.address_based_id = addressBasedId;
            this.location = location;
            this.address_branch_id = branchId;
            this.address_is_deleted = isDeleted;
        }

    }
    class BpiFinance
    {
        public int finance_id { get; set; }
        public int finance_based_id { get; set; }
        public int finance_payment_terms_id { get; set; }
        public int finance_account_id { get; set; }
        public int finance_branch_id { get; set; }

    }

    class BpiFinancePending
    {
        public int customer_id { get; set; }
        public int finance_pending_branch_id { get; set; }
        public string date { get; set; }
        public string qoute_ref { get; set; }
        public float total_price { get; set; }
        public string stage { get; set; }
        public string status { get; set; }

    }
    class BpiAccreditation
    {
        public int bpi_accreditation_id { get; set; }
        public int bpi_accreditation_based_id { get; set; }
        public int bpi_accreditation_branch_id { get; set; }
        public string date_added { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public string accreditation_added_by { get; set; }
        public BpiAccreditation(int bpiAccreditationId, int bpiAccreditationBranchId, string dateAdded, string filePath, int bpiAccreditationBasedId, string fileName, string accreditationAddedBy)
        {

            this.bpi_accreditation_id = bpiAccreditationId;
            this.bpi_accreditation_based_id = bpiAccreditationBasedId;
            this.bpi_accreditation_branch_id = bpiAccreditationBranchId;
            this.date_added = dateAdded;
            this.file_path = filePath;
            this.file_name = fileName;
            this.accreditation_added_by = accreditationAddedBy;
        }

    }

    class BpiItems
    {
        public int bpi_item_id { get; set; }
        public int bpi_item_branch_id { get; set; }
        public int bpi_item_based_id { get; set; }
        public int payment_terms_id { get; set; }
        public int item_account_id { get; set; }
        public string tax_code { get; set; }
        public string item_tax_code { get; set; }
        public float price { get; set; }
        public string notes { get; set; }
        public int item_id { get; set; }
        public string item_code { get; set; }
        public string short_desc { get; set; }
        public string status_tangible { get; set; }
        public string status_trade { get; set; }
        public bool item_is_deleted { get; set; }

        public BpiItems(int bpi_item_id, int bpi_item_based_id, int paymentTermsId, int itemId, string taxCode, string item_tax_code, float price, string notes, int itemAccountId, bool isDeleted)
        {
            this.bpi_item_id = bpi_item_id;
            this.bpi_item_based_id = bpi_item_based_id;
            this.item_account_id = itemAccountId;
            this.payment_terms_id = paymentTermsId;
            this.item_id = itemId;
            this.tax_code = taxCode;
            this.item_tax_code = item_tax_code;
            this.price = price;
            this.notes = notes;
            this.item_is_deleted = isDeleted;

        }

    }

    // For List and Singe Class

    class Bpi_Class
    {
        public List<Bpi> bpi { get; set; }
        public List<BpiGeneral> general { get; set; }
        public List<BpiContacts> contacts { get; set; }
        public List<BpiAddress> address { get; set; }
        public List<BpiItems> items { get; set; }
        public List<BpiFinance> finance { get; set; }
        public List<BpiFinancePending> finance_pending { get; set; }

        public List<BpiAccreditation> accreditations { get; set; }
        public List<BpiHistory> history { get; set; }
    }


    class BpiHistory
    {
        public int based_id { get; set; }
        public int branch_id { get; set; }
        public string at_date { get; set; }
        public string actions { get; set; }
        public string edit_by { get; set; }
        public string edit_history { get; set; }
       

    }
    class SingleBpi_Class
    {
        public Bpi bpi { get; set; }
        public BpiNewGeneral general { get; set; }
        public BpiNewContacts contacts { get; set; }
        public BpiAddress address { get; set; }
        public BpiItems items { get; set; }
        public BpiFinance finance { get; set; }



    }
    class BpiNewGeneral
    {
        public int id { get; set; }

        public int based_id { get; set; }
        public string branch_sales_id { get; set; }
        public int social_id { get; set; }
        public string branch_name { get; set; }

        public string transaction_type { get; set; }

        public string class_name { get; set; }

        public string branch_tel_no { get; set; }
        public string branch_website { get; set; }
        public string customer_code { get; set; }
        public string supplier_code { get; set; }

        public string fax_no { get; set; }
        public string notes { get; set; }
        public string entity_ids { get; set; }
        public string entity_names { get; set; }

        public string branch_industry_names { get; set; }
        public string branch_industry_ids { get; set; }

    }



    class BpiNewContacts
    {

        public int id { get; set; }
        public int based_id { get; set; }
        public int branch_id { get; set; }
        public string number { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string preferences { get; set; }
        public int position { get; set; }
        public string notes { get; set; }



        public BpiNewContacts(int contactId, int contact_based_id, string number, string name, string email, string preferences, int positions, int branchId)
        {
            this.id = contactId;
            this.based_id = contact_based_id;
            this.number = number;
            this.name = name;
            this.email = email;
            this.preferences = preferences;
            this.position = positions;
            this.branch_id = branchId;

        }

    }

}
