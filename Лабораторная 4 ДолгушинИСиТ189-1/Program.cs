using System;
using System.Threading;
using System.Timers;

namespace BIPiT4
{
    class Program
    {
        public static Random rnd = new Random();//рандомайзер
        public static Thread que, masterFirst, masterSecond;//потоки для: пополнения очереди клиентами, и для обслуживания клиентов мастерами
        public static Semaphore Sem1 = new Semaphore(5, 5);//Семафор для защиты очереди от переполнения, 5 свободных ячеек, при заполнении 5ти - не добавляем
        public static Semaphore Sem2 = new Semaphore(0, 5);//Семафор для контроля доступа обслуживающих потоков, при нуле заявок в очереди - не извлекаем
        public static int index = 0;
        public static double[] queue = new double[5];
		
		public static void Main()
        {
            que = new Thread(ClientsThread);//поток клиентов
            masterFirst = new Thread(BarberThread);//поток первого мастера
            masterSecond = new Thread(BarberThread);//поток второго мастера
            que.Start();//Начать заполнение очереди
            masterFirst.Start();//Первому мастеру начать выполнение своей работы
            masterSecond.Start();//Второму мастеру начать выполнение своей работы
            Console.ReadLine();
        }
		
        public static void PutInQueue(double n)//Метод для записи клиента в очередь
        {
            Thread.Sleep((int)n);//запись клиента в очередь через то время, через которое он пришел
            index++;
            queue[index - 1] = n;//запись в массив
        }

        public static double ToBarber()//Метод для обслуживания мастером клиента из очереди
        {
            lock(typeof(Program))//защита от выполнения непотокобезопасного кода другими потоками, во время выполнения одним из потоков
            {
                double Result = queue[index - 1];//значение - время прихода извлеченного клиента
                Console.WriteLine("Длина очереди сейчас: " + index);
                index--;
                return Result;
            }
        }

        public static void BarberThread()
        {
            while (true)
            {
                Sem2.WaitOne();//Если в очереди 0 заявок, то парикмахеры отдыхают
                double timeForServe = rnd.Next(10000);//время обслуживания клиента
                Thread.Sleep((int)timeForServe);//мастер обслуживает какое то время
                Console.WriteLine("Клиент пришедший через {0} сек. - обслужен за {1} сек.", ToBarber() / 1000, timeForServe / 1000); //извлекаем из очереди время прихода клиента
                Sem1.Release();//Записать нового клиента в очередь после освобождения места
            }
        }
		
		public static void ClientsThread()
        {
            while (true)
            {
                Sem1.WaitOne();//Если очередь полная, прекратить ее пополнение
                double timeForCome = rnd.Next(5000);//Создание времени для прихода клиента
                PutInQueue(timeForCome);//Записать в очередь число, через которое клиент попал в очередь
                Console.WriteLine(timeForCome / 1000 + " сек. - время прихода клиента");
                Sem2.Release();//Обслужить клиента из очереди
            }
        }
    }
}

