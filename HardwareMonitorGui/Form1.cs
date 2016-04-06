using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CpuTemp;

namespace HardwareMonitorGui
{
    public partial class Form1 : Form
    {
        int chartCollum = 0;
        public Form1(CPU cpu)
        {
            InitializeComponent();
            cpu.notify += RecieveNotify;
            foreach (Temperature tempra in cpu.getTempHistory())
            {
                tempratureChart.Series["Temperature °C"].Points.AddXY(chartCollum, tempra.GetTemperatureDouble());
                label1.Text = tempra.ToString();
                chartCollum++;
            }
        }
        delegate void SetTextCallback(Temperature te);
        private void SetValue(Temperature te)
        {
            tempratureChart.Series["Temperature °C"].Points.AddXY(chartCollum, te.GetTemperatureDouble());
            tempratureChart.Series.FindByName("Temperature °C").Points.AddXY(chartCollum, te.GetTemperatureDouble());
            label1.Text = te.ToString();
            tempratureChart.Update();
        }
        private void RecieveNotify(Temperature te)
        {
            try
            {
                if(this.tempratureChart.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetValue);
                    this.Invoke(d, new object[] {
                    te
                });

                }
                else
                {
                    SetValue(te);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            chartCollum++;
        }

    }
}
