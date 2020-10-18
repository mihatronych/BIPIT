using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace бипит
{
    class Program
    {
        static void Main(string[] args)
        {
            //1/(ln(x^3/(7+x))
            double a = 1;
            double b = 2;
            
            double[] xses = new double[102];
            double[] ykas = new double[102];
            double[] Ssqright = new double[102];
            double[] Ssqleft = new double[102];
            double[] Strap = new double[102];
            double sumsqright = 0;
            double sumsqleft = 0;
            double sumtrap = 0;
            double delta = 0.01;
            double x0 = (double)a + (double)(b - a) / 100 - delta*2;
            for (int i = 0; i < 102; i++)
            {
                    xses[i] = x0 + (double)i*delta;
                    ykas[i] = Math.Round(1 / ((xses[i] * xses[i]) * ((double)a * xses[i] + (double)b)), 10);
                //Ssqright[i] = ykas[i] * (xses[i] - xses[i - 1]);
                if (i > 0)
                {
                    Ssqleft[i] = ykas[i - 1] * delta;
                    Strap[i] = Math.Round((ykas[i] + ykas[i - 1]) / 2 * (delta), 10);
                    Console.WriteLine("x{0} - {1}, y{0} - {2}, Sслева - {3}, Sтрап  - {4}", i, xses[i], ykas[i], Ssqleft[i], Strap[i]);

                    sumsqright = sumsqright + Ssqright[i];
                    sumsqleft = sumsqleft + Ssqleft[i];
                    sumtrap = sumtrap + Strap[i];
                }
            }
            double analit = (1 / (-b * 2) + (a / (b * b)) * Math.Log(Math.Abs((a * 2 + b) / 2)))- (1 / (-b * 1) + (a / (b * b)) * Math.Log(Math.Abs((a * 1 + b) / 1)));
            Console.WriteLine("Сумма Sпрямоуг слева - {0}, Сумма Sтрапеции - {1}", sumsqleft, sumtrap);
            Console.WriteLine("оценка 1 - {0}, оценка 2 - {1}", Math.Abs(analit - sumsqleft), Math.Abs(analit - sumtrap));
            Console.ReadKey();
        }
    }
}
