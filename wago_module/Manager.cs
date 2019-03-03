using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wago_module
{
    class Manager
    {
        public Manager(){
            WagoRemoteIO master = new WagoRemoteIO();
            master.StartMonitor();
            //master.SetBlinkList(new int[] { 1, 3, 5, 7, 9 });
            //Console.ReadKey();
            //master.SetBlinkList(new int[] { 2, 4, 6 });
        }
        
    }
}
