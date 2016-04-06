using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace CpuTemp
{
    class DebugConsole
    {
        static void Main(string[] args)
        {
            int whaitSecond = 0;
            try
            {
                string edit = args[0].Replace(",", ".");
                whaitSecond = (int)(Double.Parse(edit) * 1000);
            }
            catch
            {
                whaitSecond = 500;//Default Tick
            }
            List<CpuTemp.CPU> de = CpuTemp.Sensors.GetCpuTemp();
            while (true)
            {
                System.Threading.Thread.Sleep(whaitSecond);
                Console.Write("\r{0}", de[0].ToString());
            }
            
            Console.ReadKey();
        }
    }
}
