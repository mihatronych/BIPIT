using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace potoki1
{
    class Program
    {
        static Stopwatch st;
        static double x;
        static double y;
        static double delta;
        static double S = 0;
        static double N = 0;
        static int M = 0;
        static double sum;
        static double x0;
        static double[] vals;
        static void Main(string[] args)
        {
            Console.WriteLine("l=интеграл от 0 до 2 (x^2)dx");
            Console.WriteLine("Введите N - ");
            N = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Введите M - ");
            M = Convert.ToInt32(Console.ReadLine());
            Thread[] th = new Thread[M];
            //int i = 0;
            vals = new double[M];
            delta = 2 / N;
            x0 = 0;
            S = 0;
            sum = new double();
            st= Stopwatch.StartNew();
                for (int i=0; i<th.Count();i++)
            {
                th[i] = new Thread(Summa);
                th[i].Start(i);
                //th[i].Join();
            }
            for (int i = 0; i < th.Count(); i++)
            {
                th[i].Join();
                sum += vals[i];
            }
            st.Stop();
            Console.WriteLine("При N = {0}, M = {1} Sigma = {2} время = {3}", N, M, sum, st.ElapsedMilliseconds);
            Console.ReadKey();
        }
        
        static void Summa(object i)
        {
            int number = (int)i;
            for (int k = number; k < N; k += M)
            {
                x = x0 + (double)k * delta;
                y = x * x;
                vals[number] += (x-delta)*(x-delta)*delta;

            }
        }
    }
}
