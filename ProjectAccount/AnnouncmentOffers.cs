using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAccount
{
    public class AnnouncmentOffers : IComparable<AnnouncmentOffers>
    {
        public int id { get; set; }
        public string nameprod { get; set; }
        public string nameprob { get; set; }
        public string typeprob { get; set; }
        public string state { get; set; }
        public string urgency { get; set; }

        public string worker_login { get; set; }

        public int deals { get; set; }

        public decimal price { get; set; }

        public int CompareTo(AnnouncmentOffers other)
        {
            if (this.Equals(other)) return 0;
            return (other.deals.CompareTo(this.deals));
        }
    }
}
