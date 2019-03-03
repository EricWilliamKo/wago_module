using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace wago_module
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            WagoRemoteIO master = new WagoRemoteIO();
            master.StartMonitor();
            master.SetBlinkList(new int[] { 1, 3, 5, 7, 9 });
            Console.ReadKey();
            master.SetBlinkList(new int[] { 2, 4, 6 });
            Console.ReadKey();
            master.StopMonitor();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
