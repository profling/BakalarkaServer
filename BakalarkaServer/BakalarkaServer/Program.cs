using System;
using System.Threading.Tasks;
using System.Threading;
namespace BakalarkaServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Helloo World!");
                await Task.Delay(1000);
            }
            
        }
    }
}
