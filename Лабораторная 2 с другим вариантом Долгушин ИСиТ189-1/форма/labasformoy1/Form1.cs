using System;
using System.Windows.Forms;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
namespace labasformoy1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "Оценка точности";
            label3.Text = "Рез-т вычислений";
            label4.Text = "Потрачено времени (мс)";
            label5.Text = "Потрачено времени (такт)";
            double a = 1;
            double b = 2;
            int amount = (int)numericUpDown1.Value;
            double[] xses = new double[amount+2];
            double[] ykas = new double[amount + 2];
            double[] Ssqright = new double[amount + 2];
            double[] Ssqleft = new double[amount + 2];
            double[] Strap = new double[amount + 2];
            //double sumsqright = 0;
            double sumsqleft = 0;
            double sumtrap = 0;
            double delta =1/(double)amount;
            Stopwatch st = Stopwatch.StartNew();
            double x0 = (double)a + (double)(b - a) / amount - delta * 2;
            double res = 0;
            var n = 2;
            for (int i = 0; i < amount+2; i++)
            {
                xses[i] = x0 + (double)i * delta;
                if (radioButton6.Checked) ykas[i] = Math.Round(1 / ((xses[i] * xses[i]) * ((double)a * xses[i] + (double)b)), 10); //- Вариант 5!
                if(radioButton7.Checked)ykas[i] = Math.Round((a*(xses[i]+b)* a * (xses[i] + b)), 10);//(ax+b)^n- Вариант 11! при n = 2, a =1, b =3

                if (i > 0)
                {
                    if (radioButton1.Checked)
                    {
                        Ssqleft[i] = ykas[i - 1] * delta;
                        sumsqleft = sumsqleft + Ssqleft[i];
                        res = sumsqleft;
                    }
                    else
                    {
                        Strap[i] = Math.Round((ykas[i] + ykas[i - 1]) / 2 * (delta), 10);
                        sumtrap = sumtrap + Strap[i];
                        res = sumtrap;
                    }
                }
            }
            double analit = 0.0;
            if (radioButton6.Checked)analit = (1 / (-b * 2) + (a / (b * b)) * Math.Log(Math.Abs((a * 2 + b) / 2))) - (1 / (-b * 1) + (a / (b * b)) * Math.Log(Math.Abs((a * 1 + b) / 1))); //вариант 5
            if (radioButton7.Checked) analit = (a * 2 + b) * (a * 2 + b) * (a * 2 + b) / (a * (3)) - (a * 1 + b) * (a * 1 + b) * (a * 1 + b) / (a * (3)); // вариант 11
            st.Stop();
            label2.Text += " - " + Math.Abs(analit - res);
            label3.Text += " - " + res;
            var sekCool = st.ElapsedTicks;
            var sek = st.ElapsedMilliseconds.ToString();
            label4.Text += " - " + sek + " мс";
            label5.Text += " - " + sekCool + " такт";
            if (radioButton5.Checked)
            {
                Word.Application word = new Word.Application();
                word.Documents.Add();
                word.ActiveDocument.Tables.Add(word.Selection.Range, amount+3, 7, 1, 0);
                word.ActiveDocument.Tables[1].Cell(1, 1).Range.Text = "Номер итерации";
                word.ActiveDocument.Tables[1].Cell(1, 2).Range.Text = "X";
                word.ActiveDocument.Tables[1].Cell(1, 3).Range.Text = "Y";
                word.ActiveDocument.Tables[1].Cell(1, 4).Range.Text = "Шаг итерации";
                for (int i = 0; i < amount + 2; i++)
                {
                    if (i > 0)
                {
                    if (radioButton1.Checked)
                    {
                        Ssqleft[i] = ykas[i - 1] * delta;
                        
                        sumsqleft = sumsqleft + Ssqleft[i];
                        res = sumsqleft;
                            word.ActiveDocument.Tables[1].Cell(i, 1).Range.Text = Convert.ToString(i-1);
                            word.ActiveDocument.Tables[1].Cell(i, 2).Range.Text = Convert.ToString(xses[i]);
                            word.ActiveDocument.Tables[1].Cell(i, 3).Range.Text = Convert.ToString(ykas[i]);
                            word.ActiveDocument.Tables[1].Cell(i, 4).Range.Text = Convert.ToString(Ssqleft[i]);
                    }
                    else
                    {
                        Strap[i] = Math.Round((ykas[i] + ykas[i - 1]) / 2 * (delta), 10);
                        sumtrap = sumtrap + Strap[i];
                        res = sumtrap;
                            word.ActiveDocument.Tables[1].Cell(i+1, 1).Range.Text = Convert.ToString(i - 1);
                            word.ActiveDocument.Tables[1].Cell(i+1, 2).Range.Text = Convert.ToString(xses[i]);
                            word.ActiveDocument.Tables[1].Cell(i+1, 3).Range.Text = Convert.ToString(ykas[i]);
                            word.ActiveDocument.Tables[1].Cell(i+1, 4).Range.Text = Convert.ToString(Strap[i]);
                        }
                }
                }
                word.ActiveDocument.Tables[1].Cell(1, 5).Range.Text = "Результат";
                word.ActiveDocument.Tables[1].Cell(1, 6).Range.Text = Convert.ToString(res);
                //word.ActiveDocument.Tables[1].Cell(1, 6).Range.Text = "Шаг итерации";
                word.Visible = true;
            }
            if (radioButton4.Checked)
            {
                Excel.Workbook xlwb;
                Excel.Worksheet xlws;
                object misValue = System.Reflection.Missing.Value;
                Excel.Application excel = new Excel.Application();
                xlwb = excel.Workbooks.Add(misValue);
                xlws = (Excel.Worksheet)xlwb.Worksheets.get_Item(1);
                excel.Visible = true;
                excel.Range["A1"].Value = "ID";
                excel.Range["B1"].Value = "X";
                excel.Range["C1"].Value = "Y";
                excel.Range["D1"].Value = "Результат";
                
                for (int i = 0; i < amount + 2; i++)
                {
                    if (i > 0)
                    {
                        if (radioButton1.Checked)
                        {
                            Ssqleft[i] = Math.Round(ykas[i - 1] * delta,6);

                            sumsqleft = sumsqleft + Ssqleft[i];
                            res = sumsqleft;
                            excel.Range["A"+(i+1)].Value = i - 1;
                            excel.Range["B" + (i + 1)].Value = xses[i];
                            excel.Range["C" + (i + 1)].Value = ykas[i];
                            excel.Range["D" + (i + 1)].Value = Ssqleft[i];

                            
                        }
                        else
                        {
                            Strap[i] = Math.Round((ykas[i] + ykas[i - 1]) / 2 * (delta), 6);
                            sumtrap = sumtrap + Strap[i];
                            res = sumtrap;
                            excel.Range["A" + (i + 1)].Value = i - 1;
                            excel.Range["B" + (i + 1)].Value = xses[i];
                            excel.Range["C" + (i + 1)].Value = ykas[i];
                            excel.Range["D" + (i + 1)].Value = Strap[i];

                        }
                    }
                }
                excel.Columns[1].AutoFit();
                excel.Columns[2].AutoFit();
                excel.Columns[3].AutoFit();
                excel.Columns[4].AutoFit();
                Excel.Range chartRange;

                Excel.ChartObjects xlc = (Excel.ChartObjects)xlws.ChartObjects(Type.Missing);
                Excel.ChartObject xlc1 = xlc.Add(10, 80, 300, 250);
                Excel.Chart chPage = xlc1.Chart;
                double f = amount + 3;
                chartRange = xlws.get_Range("D1", "D" + f);
                chPage.SetSourceData(chartRange, misValue);
            }


            }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
