using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blogg.SemaphoreSlim
{
    class Program
    {
        private static readonly string[] Inputs = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
        
        static void Main(string[] args)
        {
            //Make sure to have the web app "Blogg.WebApp" running before starting this application.
            HammerTheApi();

            Console.ReadLine();
        }

        public static void HammerTheApi()
        {
            var rnd = new Random();

            var client = new HttpClient();
            
            var semaphore = new System.Threading.SemaphoreSlim(100);

            Parallel.ForEach(Enumerable.Range(0, 10000), async (_,_,_) =>
            {
                await semaphore.WaitAsync();

                var input = Inputs[rnd.Next(0, 6)];

                var response = await client.GetStringAsync($"https://localhost:44360/reverse?text={input}");

                Console.WriteLine($"{input} --> {response}");

                semaphore.Release();
            });
        }

        public static void HammerTheApiUnControlled()
        {
            //The web app "Blogg.WebApp" has a limit of 100 concurrent calls, this code will therefore not work since it is not throttled.
            var rnd = new Random();

            var client = new HttpClient();

            Parallel.ForEach(Enumerable.Range(0, 10000), async (_, _, _) =>
            {
                var input = Inputs[rnd.Next(0, 6)];

                var response = await client.GetStringAsync($"https://localhost:44360/reverse?text={input}");

                Console.WriteLine($"{input} --> {response}");
            });
        }
    }
}
