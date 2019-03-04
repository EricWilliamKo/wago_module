using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using Modbus.Device;

namespace wago_module
{
    class WagoRemoteIO
    {
        string ip = "192.168.1.2";
        ModbusIpMaster master;
        bool[] input_coil_state;
        bool[] output_coil_state;
        bool[] next_output;
        bool running;
        bool blinker;
        int[] blink_list;

        public WagoRemoteIO()
        {
            input_coil_state = new bool[32];
            output_coil_state = new bool[16];
            next_output = new bool[16];
            TcpClient client = new TcpClient(ip, 502);
            master = ModbusIpMaster.CreateIp(client);
            running = true;
        }

        public void UpdateInputCoilState()
        {
            input_coil_state = master.ReadCoils(0, 32);
        }

        public void UpdateOutputCoilState()
        {
            output_coil_state = master.ReadCoils(512, 16);
            if (StateDifferent())
            {
                master.WriteMultipleCoils(512, next_output);
                Console.WriteLine("write output");
            }
        }

        public bool[] GetInput()
        {
            return input_coil_state;
        }

        public bool StateDifferent()
        {
            for(int i = 0; i < output_coil_state.Length; i++)
            {
                if(output_coil_state[i] != next_output[i])
                {
                    return true;
                }
            }
            return false;
        }

        public void StartBlink()
        {
            System.Timers.Timer mtimer = new System.Timers.Timer(500);
            mtimer.Elapsed += Blink;
            mtimer.AutoReset = true;
            mtimer.Enabled = true;
        }

        public void SetBlinkList(int[] list)
        {
            if(blink_list != null)
            {
                foreach (int i in blink_list)
                {
                    next_output[i] = false;
                }
            }
            
            blink_list = list;
        }

        public void Blink(Object source, ElapsedEventArgs e)
        {
            if (blinker)
                blinker = false;
            else
                blinker = true;

            foreach (int i in blink_list)
            {
                next_output[i] = blinker;
            }
        }

        public void Monitor()
        {
            while (running)
            {
                UpdateInputCoilState();
                UpdateOutputCoilState();
            }
        }

        public void StartMonitor()
        {
            Thread monitor = new Thread(Monitor);
            monitor.Start();
            StartBlink();
        }

        public void StopMonitor()
        {
            running = false;
        }


    }
}
