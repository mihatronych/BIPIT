using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Лаба6async
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Console.ReadLine();//////////////
            }
        }

        static int N = 0;
        static int M = 0;
        static double x0;
        public static int amount;
        static bool checkfirst;
        static double delta;
        static double res;
        static double x;
        static double y;
        static double ssqleft;
        static double strap;
        static double[] vals;
        public static Thread[] th;
        public static List<Task> tasks;
        public static Action[] act;
        private async void button1_Click(object sender, EventArgs e)
        {
            double a = 0;
            double b = N;
            double analit = (1 / (-b * 2) + (a / (b * b)) * Math.Log(Math.Abs((a * 2 + b) / 2))) - (1 / (-b * 1) + (a / (b * b)) * Math.Log(Math.Abs((a * 1 + b) / 1)));

            res = 0;
            x = 0;
            y = 0;
            ssqleft = 0;
            strap = 0;
            vals = new double[Convert.ToInt32(numericUpDown1.Value)];
            
            checkfirst = true;
            a = 0;
            b = N;
            if (!int.TryParse(textBox1.Text, out N))
            {
                return;
            }
            amount = (int)N;
            delta = 1 / (double)amount;
            x0 = (double)a + (double)(b - a) / amount - delta * 2;
            M = Convert.ToInt32(numericUpDown1.Value);
            th = new Thread[M];

            res = 0;
            x = 0;
            y = 0;
            ssqleft = 0;
            strap = 0;
            vals = new double[Convert.ToInt32(numericUpDown1.Value)];
            checkfirst = true;
            a = 0;
            b = N;
            if (!int.TryParse(textBox1.Text, out N))
            {
                return;
            }
            amount = (int)N;
            delta = 1 / (double)amount;
            x0 = (double)a + (double)(b - a) / amount - delta * 2;

            M = Convert.ToInt32(numericUpDown1.Value);
            var summ = new Sum[M];
            for (int i = 0; i < summ.Length; i++)
            {
                summ[i] = new Sum(i);
            }
            tasks = new List<Task>();
            foreach(var val in summ)
            {
                tasks.Add(new Task(() => val.work()));
            }
            textBox2.Text += "Task start...";
            var st = new Stopwatch();
            st.Start();
            await task();

            textBox2.Text += " Done.\r\n";
            st.Stop();
            for (int i = 0; i < M; i++)
            {
                res += vals[i];
            }
            
            MessageBox.Show(res.ToString());


            try
            {
                SendMessageFromSocket(11000, "Результат= "+res + " # время = " + st.ElapsedMilliseconds.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Console.ReadLine();//////////////
            }
        }
        public static Socket sender;
        public static void doTask()
        {
            foreach (var t in tasks)
            {
                t.Start();
            }

            foreach (var t in tasks)
            {
                t.Wait();
            }
        }
        public static string answer = "";
        static void start(int port)
        {
            // Соединяемся с удаленным устройством
            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
            sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);
        }
        static void SendMessageFromSocket(int port, string textval)
        {
            start(11000);
            byte[] bytes = new byte[1024];
            //Console.Write("Введите сообщение: ");/////////////////////////
            string message = textval;
            //MessageBox.Show("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);
            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);
            // Получаем ответ от сервера
            int bytesRec = sender.Receive(bytes);
            answer = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            MessageBox.Show($"\nОтвет от сервера: {answer}\n\n");

            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()
            if (answer == "")
                SendMessageFromSocket(port, textval);
            // Освобождаем сокет
            closing();
        }

        public static void closing()
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        public static Task task()
        {
            Task f = Task.Run(() => doTask());
            return f;
        }

        public static void Summa(object input)
        {
            int number = (int)input;
            double a = 0;
            double b = N;
            for (int i = number; i < N; i += M)
            {
                x = x0 + (double)i * delta;
                y = x/(x*x*x+1);//1 / ((x * x) * ((double)a * x + (double)b));
                double yminone = (x - delta) / ((x - delta) * (x - delta) * (x - delta) + 1);
                    //1 / (((x - delta) * (x - delta)) * ((double)a * (x - delta) + (double)b));
                vals[number] += (y + yminone) / 2 * (delta);
            }
        }

        class Sum
        {
            int number;
            public Sum(object input)
            {
                number = (int)input;
            }

            public void work()
            {
                double a = 0;
                double b = 2;
                for (int i = number; i < N; i += M)
                {
                    x = x0 + (double)i * delta;
                    x = x0 + (double)i * delta;
                    y = x / (x * x * x + 1);//1 / ((x * x) * ((double)a * x + (double)b));
                    double yminone = (x - delta) / ((x - delta) * (x - delta) * (x - delta) + 1);
                    //1 / (((x - delta) * (x - delta)) * ((double)a * (x - delta) + (double)b));
                    vals[number] += (y + yminone) / 2 * (delta);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SendMessageFromSocket(11000, DateTime.Now.ToShortDateString() + "#" + DateTime.Now.ToShortTimeString() + "#" + textBox1.Text + "#" + textBox2.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Console.ReadLine();//////////////
            }
            /*if (answer != "")
                MessageBox.Show(answer);
            answer = "";*/
        }
    }
}
