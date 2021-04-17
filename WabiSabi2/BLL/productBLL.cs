using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WabiSabi2.BLL
{
    class productBLL
    {
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string size { get; set; }
        public int rate { get; set; }
        public int qty { get; set; }
        public DateTime added_date { get; set; }
        public string added_by { get; set; }

    }
}
