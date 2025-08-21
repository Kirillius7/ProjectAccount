using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAccount
{
    public class Workers_Offers
    {
        public int id { get; set; }
        public string nameprod { get; set; }
        public string nameprob { get; set; }
        public string typeprob { get; set; }
        public string state { get; set; }
        public string urgency { get; set; }

        public string login_worker { get; set; }

        public int num_deals { get; set; }

        public decimal price { get; set; }
    }
}
