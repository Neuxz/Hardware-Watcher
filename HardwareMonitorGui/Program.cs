using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HardwareMonitorGui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            List<CpuTemp.CPU> de = CpuTemp.Sensors.GetCpuTemp();
            foreach (CpuTemp.CPU cpu in de)
            {
                Application.Run(new Form1(cpu));
            }
        }
    }
}
