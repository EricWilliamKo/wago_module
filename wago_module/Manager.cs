using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace wago_module
{
    class Manager
    {
        WagoRemoteIO master;
        Queue<Action> command_queue;
        bool running;
        int blink_switch = 0;

        public Manager()
        {
            master = new WagoRemoteIO();
            command_queue = new Queue<Action>();
            master.StartMonitor();
            running = true;
            //master.SetBlinkList(new int[] { 1, 3, 5, 7, 9 });
            //Console.ReadKey();
            //master.SetBlinkList(new int[] { 2, 4, 6 });
        }

        public void BlinkOdd()
        {
            master.SetBlinkList(new int[] { 0, 2, 4, 6, 8, 10, 12 });
        }

        public void BlinkEven()
        {
            master.SetBlinkList(new int[] { 1, 3, 5, 7, 9, 11, 13 });
        }

        public void BlinkTrigger()
        {
            if(blink_switch%2 == 0)
            {
                BlinkEven();
            }
            else
            {
                BlinkOdd();
            }
            blink_switch ++;
            Thread.Sleep(3000);
        }

        public void RecieveCommand(Action command)
        {
            command_queue.Enqueue(command);
        }

        public void ListenBotton(int position)
        {
            int counter = 0;
            bool pushed = false;
            while (running)
            {
                bool[] input = master.GetInput();
                //Console.WriteLine(input);
                if (input[position])
                {
                    counter++;
                    if (counter > 10)
                        if (!pushed) {
                            pushed = true;
                            Console.WriteLine("Pushed");
                            counter = 0;
                            RecieveCommand(BlinkTrigger);
                        }
                }
                else
                {
                    counter = 0;
                    pushed = false;
                }
                Thread.Sleep(10);
            }
        }

        public void ExcuteCommand()
        {
            while (running)
            {
                if (command_queue.Count > 0)
                {
                    Action action = command_queue.Dequeue();
                    action();
                }
            }
        }

        public void StartListen()
        {
            Thread listener = new Thread(() => ListenBotton(3));
            Thread excutor = new Thread(ExcuteCommand);
            listener.Start();
            excutor.Start();
        }

        public void StopListen()
        {
            running = false;
            master.StopMonitor();
        }
    }
}
