using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.T;
using System.IO;

namespace Tracker.Global
{
    public static class Logger
    {

        private static string path = "C:\\Users\\rsaye\\OneDrive\\Documents\\Capgemini\\Capgemini\\unit-testing-using-nunit\\Filter\\logTest.txt";
        public static void Log(string logMessage)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine($"{DateTime.Now.ToString()}");
                sw.WriteLine(logMessage);
                sw.WriteLine("___________________");
            }
            
        }

        public static void CleanFile()
        {
            File.WriteAllText(path, string.Empty);
        }
    }
}
