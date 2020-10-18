using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace laba9_2
{
    public partial class Form1 : Form
    {
        public static Socket sender;
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
            if (answer=="")
                SendMessageFromSocket(port, textval);
            // Освобождаем сокет
            closing();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SendMessageFromSocket(11000, DateTime.Now.ToShortDateString()+"#"+ DateTime.Now.ToShortTimeString() + "#"+textBox1.Text+"#"+textBox2.Text);
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public static void closing()
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
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
    }
}
