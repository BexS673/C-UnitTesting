using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.T;
using System.IO;

namespace Tracker.Global
{
    public class Logger
    {
        private static Logger instance = null;
        private static readonly object padlock = new object();
        Logger() { }

        public static Logger Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Logger();
                    }
                    return instance;
                }
            }
        }

        private string path = "C:\\Users\\rsaye\\OneDrive\\Documents\\Capgemini\\Capgemini\\unit-testing-using-nunit\\Filter\\log_test.txt";
        public void Log(string logMessage)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine($"{DateTime.Now.ToString()}");
                sw.WriteLine(logMessage);
                sw.WriteLine("___________________");
            }
            
        }

        public void CleanFile()
        {
            File.WriteAllText(path, string.Empty);
        }
    }
}
