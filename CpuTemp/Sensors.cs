using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management; //Add System.Management to Referencesusi
//using Microsoft.SqlServer.Management;
using System.Timers;
namespace CpuTemp
{
    public static class Sensors
    {
        public static List<CPU> GetCpuTemp()
        {
            List<CPU> Cpus = new List<CPU>();
            ManagementObjectCollection enumeratorName = new ManagementObjectSearcher("root\\CIMV2",
                                "SELECT * FROM Win32_Processor").Get();
            try
            {
                foreach (ManagementObject tempObject in enumeratorName)
                {
                    Cpus.Add(new CPU(tempObject["Name"].ToString()));
                }
            }
            catch (Exception ex)
            {
                Cpus.Add(new CPU("NONE_INFO"));
            }
            return Cpus;
        }
    }

    public class CPU
    {
        Timer temperatureTick = new Timer();

        public event TemperatureNotify notify;
        public delegate void TemperatureNotify(Temperature ty);
        private string CpuName;

        public string CpuName1
        {
            get { return CpuName; }
            set {}
        }
        private Temperature Cputemp;

        public Temperature Cputemp1
        {
            get { return measureCPUTemp(); }
            set {}
        }

        private List<Temperature> historyTemp = new List<Temperature>();

        public Temperature[] getTempHistory()
        {
            return historyTemp.ToArray();
        }

        public Temperature measureCPUTemp()
        
        {
            try{
                foreach (ManagementObject tempObject in new ManagementObjectSearcher("root\\WMI",
                    "SELECT * FROM MSAcpi_ThermalZoneTemperature").Get())
                {
                    Temperature teptStiall = Temperature.CreateTemperature(tempObject["CurrentTemperature"].ToString(), Temperature.Temperatures.celsius);
                    historyTemp.Add(teptStiall);
                    if(notify != null)
                    notify(teptStiall);
                    return teptStiall;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("The current OS does not Support WMI", ex);
            }
            return null;
        }

        private void Elapsed(object source, System.Timers.ElapsedEventArgs no)
        {
            measureCPUTemp();
        }
        public CPU(string name, double timeIntervall)
        {
            
            this.CpuName = name;
            this.Cputemp = measureCPUTemp();
            this.temperatureTick.Interval = timeIntervall;
            this.temperatureTick.Elapsed += Elapsed;
            this.temperatureTick.Start();
        }

        public CPU(string name) : this(name, 1000) // Default 1 Second refresh
        {}

        public override string ToString()
        {
            return CpuName + ":  " + Cputemp.ToString();
        }
    }

    public class Temperature
    {
        public enum Temperatures { celsius, kelvin, farenheight };
        private double temp;
        private Temperatures format = Temperatures.celsius;
        private Temperature() { }

        public static Temperature CreateTemperature(string input, Temperatures format)
        {
            int raw = Int32.Parse(input);
            return CreateTemperature(raw, format);
        }

        public static Temperature CreateTemperature(double input, Temperatures format)
        {
            Temperature te = new Temperature();
            switch (format)
            {
                case Temperatures.farenheight:
                    te.temp = (Math.Truncate(((((input / 10) - 273.15) * 9 / 5 + 32) * 1.0) * 10) / 10);
                    break;
                case Temperatures.kelvin:
                    te.temp = (Math.Truncate(((input / 10) * 1.0) * 10) / 10);
                    break;
                case Temperatures.celsius:
                default:
                    te.temp = (Math.Truncate(((input / 10) - 273.15) * 10) / 10);
                    break;
            }
            te.format = format;
            return te;
        }

        public string GetTemperature()
        {
            switch (format)
            {
                case Temperatures.farenheight:
                    return temp + "°F";
                case Temperatures.kelvin:
                    return temp + "K";
                case Temperatures.celsius:
                default:
                    return temp + "°C";
            }
        }

        public double GetTemperatureDouble()
        {
            return temp;
        }

        public override string ToString()
        {
            return GetTemperature();
        }
    }
}

