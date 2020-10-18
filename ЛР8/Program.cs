using System;
using System.Threading;

namespace ЛР8
{
    class Program
    {
        const int sleep = 1000;
        static Semaphore full;
        static Semaphore empty;
        static Mutex mut = new Mutex();
        static int pot = 0; //Счетчик для очереди
        static int capacity; //Размер очереди

        static void Queue(int i)
        {
            while(true)
            {
                empty.WaitOne();
                //Люди приходят в очередь
                mut.WaitOne();
                if (pot + i <= capacity)
                {
                    pot += i;
                    mut.ReleaseMutex();
                    Console.WriteLine("{0} зашел по счету {1}", Thread.CurrentThread.Name, pot);
                    full.Release();
                } else
                {
                    mut.ReleaseMutex();
                    Console.WriteLine("Клиент под номером {0} не успел в очередь, размер очереди {1}", Thread.CurrentThread.Name, pot);
                    full.Release();
                    Thread.Sleep(sleep);
                }
            }
        }

        static void Barber(int i)
        {
            while (true)
            {
                full.WaitOne();
                
                mut.WaitOne();
                if (pot - i >= 0)
                {
                    pot -= i;
                    mut.ReleaseMutex();
                    Console.WriteLine("Клиент ушел стричься, размер очереди {0}", pot);
                    //Уменьшаем очередь
                    empty.Release();
                } else
                {
                    //Очередь пуста
                    mut.ReleaseMutex();
                    Console.WriteLine("{0}, парикмахеры отдыхают", pot);
                    empty.Release();
                    Thread.Sleep(TimeSpan.Zero);
                }
            }
        }
        static void Main(string[] args)
        {
            //Console.Write("Количество парикмахеров (они обслуживают 2 клиентов за раз): ");
            int N = 2;
            full = new Semaphore(0,5);
            empty = new Semaphore(0, 2);
            capacity = 5;           
            //Console.Write("Сколько человек заходит в парикмахерскую? ");
            int y = 5;
            Thread hipstersThread = new Thread(() => Queue(y));
            for (int i = 0; i < N; i++)
            {
                Thread hipster = new Thread(() => Barber(N));
                hipster.Name = "Клиент №" + i.ToString();
                hipster.Start();
            }
            hipstersThread.Start();
            hipstersThread.Join();
        }
    }
}
