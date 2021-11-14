using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BrixCashierLine
{

    internal class Cashier
    {
        public Cashier(int cashierId, DateTime servingStart)
        {
            this.cashierId = cashierId;
            this.servingStart = servingStart;
        }


        public int cashierId { get; set; }
        public DateTime nextAvailable { get; set; }
        public DateTime servingStart { get; set; }

        public Boolean toAbort { get; set; }



        internal Task Start(ConcurrentQueue<Customer> queue)
        {
            object balanceLock = new object();

            var _ = Task.Run(async () =>
                 {
  
                     while (!toAbort || queue.Count > 0)
                     {
                         if (queue.Count > 0)//if there are items in the queue
                         {
                             servingStart = DateTime.Now;
                             Customer current;

                             if (queue.TryDequeue(out current))//thread safe!
                             {
                                 Console.WriteLine("Customer {0} is now being handled by cashier {1}. Serving Time is {2} seconds", current.CustomerId, cashierId, current.serviceTime);
                                 await Task.Delay(current.serviceTime * 1000);//handle next customer when service time is over.
                             }
                         }
                         else//wait a second and try to see if there is any customer in line
                         {
                             await Task.Delay(1000);
                         }
                     }

                 });
        
            return Task.CompletedTask;
        }
    }

       
    
}
