using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrixCashierLine
{
   
        internal class Customer
        {
        public int timeArrivedAtLine { get; set; }
        public int serviceTime { get; set; }
        public int CustomerId { get; set; }

            public Customer(int timeArrivedAtLine, int CustomerId)
            {
                this.CustomerId = CustomerId;
                this.timeArrivedAtLine = timeArrivedAtLine;
                Random rd = new Random();
                this.serviceTime = rd.Next(1, 5);

            }

          

        }
    }
