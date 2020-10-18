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
using System.Xml.Linq;
namespace laba8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Clean()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clean();
            var xd = XDocument.Load("UsAirport.xml");
            var doc = xd.Document.Elements();
            //var some = res.First().Elements().Elements().Take(10);
            var somesht = doc.First().Elements().Where(n => n.Element("LatitudeNpeerS").Value == "S").OrderByDescending(n => int.Parse(n.Element("LatitudeDegree").Value))
                .Where(n => int.Parse(n.Element("RunwayLengthFeet").Value) != 0).Take(20)
                .OrderBy(n => int.Parse(n.Element("RunwayLengthFeet").Value)).Select(n => new {
                    Code = n.Element("AirportCode").Value,
                    Name = n.Element("CityOrAirportName").Value,
                    Lat = $"{n.Element("LatitudeDegree").Value}\'{n.Element("LatitudeMinute").Value}\'\'{n.Element("LatitudeSecond").Value}{n.Element("LatitudeNpeerS").Value}",
                    Long = $"{n.Element("LongitudeDegree").Value}\'{n.Element("LongitudeMinute").Value}\'\'{n.Element("LongitudeSeconds").Value}{n.Element("LongitudeEperW").Value}",
                    RunwayLength = n.Element("RunwayLengthFeet").Value
                });

            DataGridViewTextBoxColumn dgvCode = new DataGridViewTextBoxColumn();
            dgvCode.Name = "К. аэроп.";
            DataGridViewTextBoxColumn dgvName = new DataGridViewTextBoxColumn();
            dgvName.Name = "Назв. аэроп.";
            DataGridViewTextBoxColumn dgvCoordLat = new DataGridViewTextBoxColumn();
            dgvCoordLat.Name = "Коорд. широт.";
            DataGridViewTextBoxColumn dgvCoordLong = new DataGridViewTextBoxColumn();
            dgvCoordLong.Name = "Коорд. долгот.";
            DataGridViewTextBoxColumn dgvRunway = new DataGridViewTextBoxColumn();
            dgvRunway.Name = "Длина полосы";
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { dgvCode,dgvName,dgvCoordLat,dgvCoordLong,dgvRunway });
            var count = 0;
            for (int i = 0; i < 10; i++)
            {
                dataGridView1.Rows.Add(somesht.ElementAt(count).Code, somesht.ElementAt(count).Name, somesht.ElementAt(count).Lat, somesht.ElementAt(count).Long, somesht.ElementAt(count).RunwayLength);
                count += 2;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clean();
            var xd = XDocument.Load("UsAirport.xml");
            var doc = xd.Document.Elements();
            //var some = res.First().Elements().Elements().Take(10);
            var somesht = doc.First().Elements().Where(n => n.Element("LatitudeNpeerS").Value == "S")
                .Select(n => new {
                    Code = n.Element("AirportCode").Value,
                    Name = n.Element("CityOrAirportName").Value,
                    Lat = $"{n.Element("LatitudeDegree").Value}\'{n.Element("LatitudeMinute").Value}\'\'{n.Element("LatitudeSecond").Value}{n.Element("LatitudeNpeerS").Value}",
                    Long = $"{n.Element("LongitudeDegree").Value}\'{n.Element("LongitudeMinute").Value}\'\'{n.Element("LongitudeSeconds").Value}{n.Element("LongitudeEperW").Value}",
                });
            var difsht = doc.First().Elements().Where(n => n.Element("CountryCode").Value == "1")
                .Where(n => int.Parse(n.Element("RunwayLengthFeet").Value) != 0)
                .Select(n => new {
                    Code = n.Element("AirportCode").Value,
                    Name = n.Element("CityOrAirportName").Value,
                    Lat = $"{n.Element("LatitudeDegree").Value}\'{n.Element("LatitudeMinute").Value}\'\'{n.Element("LatitudeSecond").Value}{n.Element("LatitudeNpeerS").Value}",
                    Long = $"{n.Element("LongitudeDegree").Value}\'{n.Element("LongitudeMinute").Value}\'\'{n.Element("LongitudeSeconds").Value}{n.Element("LongitudeEperW").Value}",
                });
            DataGridViewTextBoxColumn dgvCode = new DataGridViewTextBoxColumn();
            dgvCode.Name = "К. аэроп.";
            DataGridViewTextBoxColumn dgvName = new DataGridViewTextBoxColumn();
            dgvName.Name = "Назв. аэроп.";
            DataGridViewTextBoxColumn dgvCoordLat = new DataGridViewTextBoxColumn();
            dgvCoordLat.Name = "Коорд. широт.";
            DataGridViewTextBoxColumn dgvCoordLong = new DataGridViewTextBoxColumn();
            dgvCoordLong.Name = "Коорд. долгот.";
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { dgvCode, dgvName, dgvCoordLat, dgvCoordLong });
            //var len = somesht.Count() + difsht.Count();
            var count = 0;
            for (int i = 0; i < somesht.Count()/2; i++)
            {
                dataGridView1.Rows.Add(somesht.ElementAt(count).Code, somesht.ElementAt(count).Name, somesht.ElementAt(count).Lat, somesht.ElementAt(count).Long);
                count += 2;
            }
            count = 0;
            for (int i = 0; i < difsht.Count() / 2; i++)
            {
                dataGridView1.Rows.Add(difsht.ElementAt(count).Code, difsht.ElementAt(count).Name, difsht.ElementAt(count).Lat, difsht.ElementAt(count).Long);
                count += 2;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clean();
            var xd = XDocument.Load("UsAirport.xml");
            var doc = xd.Document.Elements();
            var somesht = doc.First().Elements().Where(n => n.Element("LatitudeNpeerS").Value == "N").Where(n=>int.Parse(n.Element("LatitudeDegree").Value)>numericUpDown1.Value).Take(40)
                .OrderBy(n => int.Parse(n.Element("RunwayLengthFeet").Value))
                .Select(n => new {
                    Code = n.Element("AirportCode").Value,
                    Name = n.Element("CityOrAirportName").Value,
                    Lat = $"{n.Element("LatitudeDegree").Value}\'{n.Element("LatitudeMinute").Value}\'\'{n.Element("LatitudeSecond").Value}{n.Element("LatitudeNpeerS").Value}",
                    Long = $"{n.Element("LongitudeDegree").Value}\'{n.Element("LongitudeMinute").Value}\'\'{n.Element("LongitudeSeconds").Value}{n.Element("LongitudeEperW").Value}",
                    RunwayLength = n.Element("RunwayLengthFeet").Value
                });
            DataGridViewTextBoxColumn dgvCode = new DataGridViewTextBoxColumn();
            dgvCode.Name = "К. аэроп.";
            DataGridViewTextBoxColumn dgvName = new DataGridViewTextBoxColumn();
            dgvName.Name = "Назв. аэроп.";
            DataGridViewTextBoxColumn dgvCoordLat = new DataGridViewTextBoxColumn();
            dgvCoordLat.Name = "Коорд. широт.";
            DataGridViewTextBoxColumn dgvCoordLong = new DataGridViewTextBoxColumn();
            dgvCoordLong.Name = "Коорд. долгот.";
            DataGridViewTextBoxColumn dgvRunway = new DataGridViewTextBoxColumn();
            dgvRunway.Name = "Длина полосы";
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { dgvCode, dgvName, dgvCoordLat, dgvCoordLong, dgvRunway });
            var count = 0;
            for (int i = 0; i < 20; i++)
            {
                dataGridView1.Rows.Add(somesht.ElementAt(count).Code, somesht.ElementAt(count).Name, somesht.ElementAt(count).Lat, somesht.ElementAt(count).Long, somesht.ElementAt(count).RunwayLength);
                count += 2;
            }
        }
    }
}
