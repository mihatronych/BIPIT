using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace laba7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;
            
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void выводToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            новыйToolStripMenuItem_Click(sender, e);
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "xml files (*.xml)|*.xml";
            string fileName = "";
            if (open.ShowDialog() == DialogResult.OK)
            {
                fileName = open.FileName;
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(fileName);
                XmlElement xRoot = xDoc.DocumentElement;
                var i = 0;
                foreach (XmlNode xnode in xRoot)
                {
                    if (xnode.Attributes.Count > 0)
                    {
                        XmlNode attr = xnode.Attributes.GetNamedItem("id");
                        if (attr != null)
                        {
                            dataGridView1.Rows.Add();
                        }
                    }
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Name == "product")
                        {
                            dataGridView1.Rows[i].Cells[0].Value = childnode.InnerText;
                        }
                        if (childnode.Name == "prize")
                        {
                            dataGridView1.Rows[i].Cells[1].Value = childnode.InnerText;
                        }
                        if (childnode.Name == "count")
                        {
                            dataGridView1.Rows[i].Cells[2].Value = childnode.InnerText;
                        }
                        
                    }
                    i += 1;
                }
            }
            
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "xml files (*.xml)|*.xml";
            string fileName = "";

            if (save.ShowDialog() == DialogResult.OK)
            {
                fileName = save.FileName;
                XmlTextWriter xtw = new XmlTextWriter(fileName, Encoding.UTF8);
                xtw.WriteStartDocument();
                xtw.WriteStartElement("note");
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].IsNewRow) continue;

                    xtw.WriteStartElement("rec");
                    xtw.WriteAttributeString("id", i.ToString());
                    if (dataGridView1.Rows[i].Cells[0].Value == null)
                    {
                        dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Red;
                        MessageBox.Show("Ошибка сохранения");
                        break;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.White;
                    }
                    if (dataGridView1.Rows[i].Cells[1].Value == null)
                    {
                        dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.Red;
                        MessageBox.Show("Ошибка сохранения");
                        break;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.White;
                    }
                    if (dataGridView1.Rows[i].Cells[2].Value == null)
                    {
                        dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.Red;
                        MessageBox.Show("Ошибка сохранения");
                        break;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.White;
                    }
                    xtw.WriteStartElement("product");
                    //MessageBox.Show(dataGridView1[0, i].Value.ToString());
                    xtw.WriteString(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    xtw.WriteEndElement();

                    xtw.WriteStartElement("prize");
                    xtw.WriteString(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    xtw.WriteEndElement();

                    xtw.WriteStartElement("count");
                    xtw.WriteString(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    xtw.WriteEndElement();

                    xtw.WriteEndElement();
                }
                xtw.WriteEndElement();
                xtw.Close();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.O && e.Control)
            {
                открытьToolStripMenuItem_Click(sender,e);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.N && e.Control)
            {
                новыйToolStripMenuItem_Click(sender, e);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.S && e.Control)
            {
                сохранитьToolStripMenuItem_Click(sender, e);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Q && e.Control)
            {
                выводToolStripMenuItem_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
