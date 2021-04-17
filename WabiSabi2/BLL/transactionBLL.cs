using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WabiSabi2.BLL
{
    class transactionBLL
    {
        public int id { get; set; }
        public string type { get; set; }
        public string customer_name { get; set; }
        public string customer_contact { get; set; }
        public int grandTotal { get; set; }
        public DateTime transaction_date { get; set; }
        public decimal tax { get; set; }
        public int discount { get; set; }
        public string added_by { get; set; }
        public string billText { get; set; }
        public DataTable transactionDetails { get; set; }

    }
}
