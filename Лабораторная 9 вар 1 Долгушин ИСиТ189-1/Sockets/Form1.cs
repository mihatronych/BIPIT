using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sockets
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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

        public static Socket sender;

        void SendMessageFromSocket(int port = 11000)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];
            start(port);
            // Соединяемся с удаленным устройством
            // Устанавливаем удаленную точку для сокета
            string message = textBox1.Text;
            //Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);
            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);
            // Получаем ответ от сервера
            int bytesRec = sender.Receive(bytes);
            string received = (Encoding.UTF8.GetString(bytes, 0, bytesRec));
            string[] entries = received.Split('#');
            WriteCameThings(entries);
            ShutYourMouth(sender);
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            SendMessageFromSocket();
        }

        public void ShutYourMouth(Socket sender)
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        public void WriteCameThings(string[] entries)
        {
            try
            {
                dataGridView1.RowCount = entries.Length - 1;
                for (int i = 1; i < entries.Length; i++)
                {
                    string[] subs = entries[i].Split('~');
                    dataGridView1.Rows[i - 1].Cells[0].Value = subs[0];
                    dataGridView1.Rows[i - 1].Cells[1].Value = subs[2];
                    dataGridView1.Rows[i - 1].Cells[2].Value = subs[1];
                    //dataGridView1.Rows.Add();
                }
            }
            catch
            {
                MessageBox.Show("There was a mistake");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}