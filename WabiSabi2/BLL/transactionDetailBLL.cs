using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WabiSabi2.BLL
{
    class transactionDetailBLL
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public string product_note { get; set; }
        public int rate { get; set; }
        public int qty { get; set; }
        public int total { get; set; }
        public string customer_name { get; set; }
        public string customer_contact { get; set; }
        public DateTime added_date { get; set; }
        public string added_by { get; set; }

    }
}
