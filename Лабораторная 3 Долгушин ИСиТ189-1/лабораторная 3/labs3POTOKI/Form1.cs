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

namespace labs3POTOKI
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
        static double res;
        static double x;
        static double y;
        static double ssqleft;
        static double strap;
        static double[] vals;
        /*private void button1_Click(object sender, EventArgs e)
        {
            res = 0;
            x = 0;
            y = 0;
            ssqleft = 0;
            strap = 0;
            vals = new double[(int)numericUpDown2.Value];
            Stopwatch st = Stopwatch.StartNew();
            checkfirst = radioButton1.Checked;
            double a = 1;
            double b = 2;
            amount = (int)numericUpDown1.Value;
            delta = 1 / (double)amount;
            x0 = (double)a + (double)(b - a) / amount - delta * 2;
            
            if (numericUpDown2.Value == 0)
            {
                for (int i = 0; i < amount + 2; i++)
                {
                    x= x0 + (double)i * delta;
                    y= Math.Round(1 / ((x * x) * ((double)a * x + (double)b)), 10);
                    if (i > 0)
                    {
                        if (radioButton1.Checked)
                        {
                            ssqleft = Math.Round(1 / (((x-delta) * (x-delta)) * ((double)a * x + (double)b)), 10) * delta;
                            res += ssqleft;
                        }
                        else
                        {
                            strap = Math.Round((y + Math.Round(1 / (((x - delta) * (x - delta)) * ((double)a * x + (double)b)), 10)) / 2 * (delta), 10);
                            res = strap;
                        }
                    }
                    }
                st.Stop();
            }
            else
            {
                N = Convert.ToInt32(numericUpDown1.Value);
                M = Convert.ToInt32(numericUpDown2.Value);
                Thread[] th = new Thread[M];

                for (int i = 0; i < th.Count(); i++)
                {
                    th[i] = new Thread(Summa);
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
            }
            
            textBox1.Text = "Значение интеграла: " + res+"\n\r"+"Время работы: "+st.ElapsedMilliseconds + " мс";
        }
        static void Summa(object input)
        {
            int number = (int)input;
            double a = 1;
            double b = 2;
            for (int i = number; i < N; i += M)
            {
                x = x0 + (double)i * delta;
                y = 1 / ((x * x) * ((double)a * x + (double)b));
                double yminone = 1 / (((x - delta) * (x - delta)) * ((double)a * (x - delta) + (double)b));
                    if (checkfirst)
                    {
                        vals[number] += yminone * delta;
                    }
                    else
                    {
                        vals[number] += (y + yminone) / 2 * (delta);
                    }
            }
        }*/
        private void button1_Click(object sender, EventArgs e)
        {
            res = 0;
            x = 0;
            y = 0;
            ssqleft = 0;
            strap = 0;
            vals = new double[(int)numericUpDown2.Value];

            checkfirst = radioButton1.Checked;
            double a = 1;
            double b = 2;
            amount = (int)numericUpDown1.Value;
            delta = 1 / (double)amount;
            x0 = (double)a + (double)(b - a) / amount - delta * 2;
            Stopwatch st1;
            if (numericUpDown2.Value == 0)
            {
                st1 = Stopwatch.StartNew();
                for (int i = 0; i < amount; i++)
                {
                    x = Math.Round(x0 + (double)i * delta, 10);
                    y = Math.Round(1 / ((x * x) * ((double)a * x + (double)b)), 10);
                    if (radioButton1.Checked)
                    {
                        ssqleft = Math.Round(1 / (((x - delta) * (x - delta)) * ((double)a * x + (double)b)), 10) * delta;
                        res += ssqleft;
                    }
                    else
                    {
                        strap = Math.Round((y + Math.Round(1 / (((x - delta) * (x - delta)) * ((double)a * (x - delta) + (double)b)), 10)) / 2 * (delta), 10);
                        res += strap;
                    }
                }
                st1.Stop();
            }
            else if (numericUpDown2.Value == 1)
            {
                N = Convert.ToInt32(numericUpDown1.Value);
                M = Convert.ToInt32(numericUpDown2.Value);
                st1 = Stopwatch.StartNew();
                Parallel.For(0, M, i =>
                {
                    int number = i;
                    for (int k = number; k < N; k += M)
                    {
                        x = x0 + (double)k * delta;
                        y = Math.Round(1 / ((x * x) * ((double)a * x + (double)b)),10);
                        if (checkfirst)
                        {
                            vals[number] += Math.Round((1 / (((x - delta) * (x - delta)) * ((double)a * (x - delta) + (double)b))) * delta,10);
                        }
                        else
                        {
                            vals[number] += Math.Round((y + (1 / (((x - delta) * (x - delta)) * ((double)a * (x - delta) + (double)b)))) / 2 * (delta),10);
                        }
                    }
                });
                st1.Stop();
                for (int i = 0; i < M; i++)
                {
                    res += vals[i];
                }
            }
            else
            {

                N = Convert.ToInt32(numericUpDown1.Value);
                M = Convert.ToInt32(numericUpDown2.Value);
                st1 = Stopwatch.StartNew();
                Parallel.For(0, M, i =>
                {
                    int number = i;
                    for (int k = number; k < N; k += M)
                    {
                        x = x0 + (double)k * delta;
                        y = 1 / ((x * x) * ((double)a * x + (double)b));
                        if (checkfirst)
                        {
                            vals[number] += (1 / (((x - delta) * (x - delta)) * ((double)a * (x - delta) + (double)b))) * delta;
                        }
                        else
                        {
                            vals[number] += (y + (1 / (((x - delta) * (x - delta)) * ((double)a * (x - delta) + (double)b)))) / 2 * (delta);
                        }
                    }
                });
                st1.Stop();
                for (int i = 0; i < M; i++)
                {
                    res += vals[i];
                }

            }
            textBox1.Text = "Значение интеграла: " + res + "\n\r" + "Время работы: " + st1.ElapsedMilliseconds + " мс";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
