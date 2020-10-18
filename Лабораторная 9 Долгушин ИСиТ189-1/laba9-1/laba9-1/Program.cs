using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace laba9_1
{
    class Program
    {

        static void Main(string[] args)
        {
            // Устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);
            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                // Начинаем слушать соединения
                Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                while (true)
                {
                    //Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                    // Программа приостанавливается, ожидая входящее соединение
                    /*Socket handler = sListener.Accept();
                    string data = null;
                    // Мы дождались клиента, пытающегося с нами соединиться
                    Console.WriteLine("Клиент подключен");
                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    // Показываем данные на консоли
                    Console.Write("Полученный текст: " + data + "\n\n");
                    // Отправляем ответ клиенту\
                    //string reply = "Спасибо за запрос в " + data.Length.ToString()
                    //+ " символов";
                    StreamWriter sw = new StreamWriter("log.txt", true);
                    sw.WriteLineAsync(data);
                    sw.Close();
                    byte[] msg = Encoding.UTF8.GetBytes("Успешно");
                    handler.Send(msg);
                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Сервер завершил соединение с клиентом.");
                        break;
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();*/
                    waitconnect(sListener);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static Socket Handler;

        public static async void waitconnect(Socket sListener)
        {
            Socket Handler = await sListener.AcceptAsync();
            string data = null;
            // Мы дождались клиента, пытающегося с нами соединиться
            Console.WriteLine("Клиент подключен");
            byte[] bytes = new byte[1024];
            int bytesRec = Handler.Receive(bytes);
            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            // Показываем данные на консоли
            Console.Write("Полученный текст: " + data + "\n\n");
            // Отправляем ответ клиенту\
            //string reply = "Спасибо за запрос в " + data.Length.ToString()
            //+ " символов";
            StreamWriter sw = new StreamWriter("log.txt", true);
            await sw.WriteLineAsync(data);
            sw.Close();
            byte[] msg = Encoding.UTF8.GetBytes("Успешно");
            Handler.Send(msg);
        }

        public static async void connect(Socket handler)
        {
            string data = null;
            // Мы дождались клиента, пытающегося с нами соединиться
            Console.WriteLine("Клиент подключен");
            byte[] bytes = new byte[1024];
            int bytesRec = handler.Receive(bytes);
            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            // Показываем данные на консоли
            Console.Write("Полученный текст: " + data + "\n\n");
            // Отправляем ответ клиенту\
            //string reply = "Спасибо за запрос в " + data.Length.ToString()
            //+ " символов";
            StreamWriter sw = new StreamWriter("log.txt", true);
            await sw.WriteLineAsync(data);
            sw.Close();
            byte[] msg = Encoding.UTF8.GetBytes("Успешно");
            handler.Send(msg);
            /*if (data.IndexOf("<TheEnd>") > -1)
            {
                Console.WriteLine("Сервер завершил соединение с клиентом.");
                break;
            }*/
            //handler.Shutdown(SocketShutdown.Both);
            //handler.Close();
        }
    }
}
