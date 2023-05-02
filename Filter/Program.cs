using System;
using Tracker.Global;
using Tracker.T;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Mail;
using System.Timers;
using System.Reflection.PortableExecutable;
using System.Diagnostics;

class Program
{
    private static Stopwatch stopwatch = new Stopwatch();
    private static Mail testMail; //this should be new Mail object each time written to.

    private static Filter filter = Filter.Instance;

    private static List<Mail> mailCollect = new List<Mail>();
    private static void Main(string[] args)
    {
        stopwatch.Start();
        Filter filter = Filter.Instance;
        Init();
        Start();
        RandomUpdate();
        PrintMail();
    }
    
    public static void Init()
    {
        filter.UpdatePath(Filter.Mode.initialise);
    }

    public static async void Start()
    {

        for (int i = 0; i < 30; i++)
        {
            Thread.Sleep(1000);
            await Task.Run(() =>
            {
                filter.CallPositionUpdate();
                filter.Output(out testMail);
                mailCollect.Add(testMail);
            });
        }
     
        
    }

    public static async void RandomUpdate()
    {
      
        for (int i = 0; i < 30; i++)
        {
            Thread.Sleep(500);
            await Task.Run(() => { filter.UpdatePath(Filter.Mode.update); });
            //Console.WriteLine("Random update made");
        }       
    }
    private static void PrintMail()
    {

        for (int i = 0; i < 30; i++)
        {
            Thread.Sleep(2000);
            
            Console.WriteLine($"Time: {stopwatch.Elapsed}");
            Console.WriteLine($"Range: {mailCollect.Last().Range}");
          //  Console.WriteLine($"Bearing: {mailCollect.Last().Bearing}");
            
        }

    }

   
}
