using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrixCashierLine
{
    //worse complexity is O(n). worse space complexity is O(n).

    class Program
    {
        private static readonly ConcurrentQueue<Customer> queue = new ConcurrentQueue<Customer>();
        private static List<Cashier> cashiers = new List<Cashier>();
        private const int numOfCashier = 5;

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Welcome To My Store. To terminate, Press any key...");

                await Task.WhenAll( InitCashier(), RunCustomersLine());
                //have two tasks running in parallel. one adds customers to a queue, and the other tasks inits the list of the cashiers.

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }


            
            
        }

        private async static Task InitCashier()
        {
            for (int i = 0; i < numOfCashier; i++)
            {
                Cashier cashier = new Cashier(i+1, DateTime.Now);
                await cashier.Start(queue);
                cashiers.Add(cashier);
            }

        }

        private async static Task RunCustomersLine()
        {
            int ind = 0;
           
           await Task.Run(async () =>
            {
                while (!Console.KeyAvailable)
                {
                    AddToLine(ind);//add customer to queue
                    ind++;
                    await Task.Delay(1000);//every second
                }

                foreach (var cashier in cashiers)//when user stops the program, notify all cashiers that this is the end. only customers that are in the queue will be servered.
                {
                    cashier.toAbort = true;
                }
            });
           

        }

        private static void AddToLine(int CustomerId)
        {
            
            Console.WriteLine("Customer {0} joined the Line at: {1}", CustomerId, DateTime.Now.ToString("HH:mm:ss"));
            queue.Enqueue(new Customer(DateTime.Now.Second, CustomerId));
        }

    }
}
