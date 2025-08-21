using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAccount
{
    public class Deals : IComparable<Deals>
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

        public string date { get; set; }

        public int CompareTo(Deals other)
        {
            if(this.Equals(other)) return 0;
            return other.price.CompareTo(this.price);
        }
    }
}
