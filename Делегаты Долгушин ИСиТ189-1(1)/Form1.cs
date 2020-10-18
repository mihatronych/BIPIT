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

namespace Лаба4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static int N = 0;
        static int M = 0;
        static double x0;
        static int amount;
        static bool checkfirst;
        static double delta;
        static double res = 0;
        static double x = 0;
        static double y = 0;
        static double ssqleft;
        static double strap;
        static double[] vals;
        static double a = 1;
        static double b = 2;
        private void button1_Click(object sender, EventArgs e)
        {
            double analit = (1 / (-b * 2) + (a / (b * b)) * Math.Log(Math.Abs((a * 2 + b) / 2))) - (1 / (-b * 1) + (a / (b * b)) * Math.Log(Math.Abs((a * 1 + b) / 1)));
            string[] labelsSqr = new string[3];
            string[] labelsTrap = new string[3];
            if (comboBox1.SelectedItem != null)
            {
                label3.Text = "Значение интеграла методом левых прмоуг.:";
                label4.Text = "Оценка точности:";
                label5.Text = "Время вычислений:";
                label9.Text = "Значение интеграла методом трап.:";
                label10.Text = "Оценка точности:";
                label11.Text = "Время вычислений:";
                /*if (comboBox1.SelectedItem.ToString() == "Thread")
                {
                    labels = CutString((int)numericUpDown1.Value,analit,Thread, SummaSqr);

                }*/
                if (comboBox1.SelectedItem.ToString() == "Task")
                {
                    labelsSqr = CutString((int)numericUpDown1.Value, analit, TaskMethod, SummaSqr);
                    labelsTrap = CutString((int)numericUpDown1.Value, analit, TaskMethod, SummaTrap);
                }
                if (comboBox1.SelectedItem.ToString() == "Invoke")
                {
                    labelsSqr = CutString((int)numericUpDown1.Value, analit, Invoke, SummaSqr);
                    labelsTrap = CutString((int)numericUpDown1.Value, analit, Invoke, SummaTrap);
                }
                    label3.Text += " " + labelsSqr[0];
                    label4.Text += " " + labelsSqr[1];
                    label5.Text += " " + labelsSqr[2];
                label9.Text += " " + labelsTrap[0];
                label10.Text += " " + labelsTrap[1];
                label11.Text += " " + labelsTrap[2];
            }
            else
            {
                label3.Text = "Ошибка!";
                label4.Text = "Ошибка!";
                label5.Text = "Ошибка!";
                label9.Text = "Ошибка!";
                label10.Text = "Ошибка!";
                label11.Text = "Ошибка!";
            }
        }
        public delegate void summMethod(object input);



        static void SummaSqr(object input)
        {
            int number = (int)input;
            double a = 1;
            double b = 2;
            for (int i = number; i < N; i += M)
            {
                x = x0 + (double)i * delta;
                y = 1 / ((x * x) * ((double)a * x + (double)b));
                double yminone = 1 / (((x - delta) * (x - delta)) * ((double)a * (x - delta) + (double)b));
                vals[number] += yminone * delta;
            }
        }

        static void SummaTrap(object input)
        {
            int number = (int)input;
            double a = 1;
            double b = 2;
            for (int i = number; i < N; i += M)
            {
                x = x0 + (double)i * delta;
                y = 1 / ((x * x) * ((double)a * x + (double)b));
                double yminone = 1 / (((x - delta) * (x - delta)) * ((double)a * (x - delta) + (double)b));
                vals[number] += (y + yminone) / 2 * (delta);
            }
        }

        public static string[] CutString(int val, double analit,Method method, summMethod Summa)
        {
            string res = method(val, analit, Summa);
            return res.Split(';');
        }

        public delegate string Method(int val, double analit, summMethod Summa);

        public static string TaskMethod(int val, double analit, summMethod Summa)
        {
            res = 0;
            x = 0;
            y = 0;
            ssqleft = 0;
            strap = 0;
            vals = new double[8];
            Stopwatch st = Stopwatch.StartNew();
            checkfirst = true;
            a = 1;
            b = 2;
            amount = val;
            delta = 1 / (double)amount;
            x0 = (double)a + (double)(b - a) / amount - delta * 2;


            N = val;
            M = 8;
            Task[] task = new Task[]
                {
                        new Task(() => Summa(0)),
                        new Task(() => Summa(1)),
                        new Task(() => Summa(2)),
                        new Task(() => Summa(3)),
                        new Task(() => Summa(4)),
                        new Task(() => Summa(5)),
                        new Task(() => Summa(6)),
                        new Task(() => Summa(7)),
                };
            foreach (var t in task)
            {
                t.Start();
            }
            Task.WaitAll(task);
            st.Stop();
            for (int i = 0; i < M; i++)
            {
                res += vals[i];
            }
            string result;
            result = res + "";
            result += ";" + Math.Abs(analit - res) + "";
            result += ";" + st.ElapsedMilliseconds + " мс";
            return result;
        }

        public static string Invoke(int val, double analit,summMethod Summa)
        {
            res = 0;
            x = 0;
            y = 0;
            ssqleft = 0;
            strap = 0;
            vals = new double[8];
            Stopwatch st = Stopwatch.StartNew();
            checkfirst = true;
            a = 1;
            b = 2;
            amount = val;
            delta = 1 / (double)amount;
            x0 = (double)a + (double)(b - a) / amount - delta * 2;


            N = Convert.ToInt32(val);
            M = 8;
            Parallel.Invoke(() => Summa(1), () => Summa(2), () => Summa(3), () => Summa(4), () => Summa(5), () => Summa(6), () => Summa(7), () => Summa(0));
            st.Stop();
            for (int i = 0; i < M; i++)
            {
                res += vals[i];
            }
            string result;
            result = res + "";
            result += ";" + Math.Abs(analit - res) + "";
            result += ";" + st.ElapsedMilliseconds + " мс";
            return result;
        }

        public static string Thread(int val, double analit)
        {
            res = 0;
            x = 0;
            y = 0;
            ssqleft = 0;
            strap = 0;
            vals = new double[8];
            Stopwatch st = Stopwatch.StartNew();
            checkfirst = true;
            a = 1;
            b = 2;
            amount = val;
            delta = 1 / (double)amount;
            x0 = (double)a + (double)(b - a) / amount - delta * 2;


            N = Convert.ToInt32(val);
            M = 8;
            Thread[] th = new Thread[M];
            for (int i = 0; i < th.Count(); i++)
            {
                th[i] = new Thread(SummaSqr);
                th[i].Start(i);
            }
            for (int i = 0; i < th.Count(); i++)
            {
                th[i].Join();
            }
            st.Stop();
            for (int i = 0; i < M; i++)
            {
                res += vals[i];
            }
            string result;
            result = res + "";
            result +=";"+ Math.Abs(analit - res) + "";
            result += ";" + st.ElapsedMilliseconds + " мс";
            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

