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
using System.Runtime.CompilerServices;
using System.Text;

class Program
{
    private static Stopwatch stopwatch = new Stopwatch();
    private static Mail testMail; 
    public static Random random = new Random();

    private static Filter filter = Filter.Instance;
    private static Track track = new Track();

    private static List<Mail> mailCollect = new List<Mail>();
    private static bool mailboxFull = false;
    


    private static async Task Main(string[] args)
    {
        stopwatch.Start();
        Logger.CleanFile();
         
        Task<bool> initialise = new Task<bool>(() => track.Initialise());
        Task start = new Task (() => Start());
        Task update = new Task(() => RandomUpdate());
        Task stopTimer = new Task(() => StopTimer());

        initialise.Start();
        Console.WriteLine("Waiting for filter to initialise.");
        await initialise;
        Console.WriteLine("Initialised filter.");

        start.Start();
        update.Start();
        stopTimer.Start();

        LogMail();     
    } 

    public static void Start()
    {

        //await Task.Run(() =>
        //{
            for (int i = 0; i <= 10; i++)
            {
                Thread.Sleep(2000);
                filter.CallPositionUpdate();
                filter.Output(out testMail);
                Logger.Log($"New mail sent to mailbox. Calling on thread {Task.CurrentId}");
                mailCollect.Add(testMail);
            }
            
        //});
       
    }

    public static void RandomUpdate()
    {
        //await Task.Run(() =>
        //{
            for (int i = 0; i <= 10; i++)
            {
                Thread.Sleep(random.Next(1000));
                filter.UpdatePath(Filter.Mode.update);
                Logger.Log($"Update made to path. Calling on thread {Task.CurrentId}");
            }
        //});

    }

    private static void StopTimer()
    {
        Thread.Sleep(random.Next(20000));
        mailboxFull = true;
    }
    private static void LogMail()
    {
        while (mailboxFull == false)
        {
            Thread.Sleep(1000);
            Console.WriteLine("Logging mail to logger");
            if (mailCollect.Count == 0)
            {
                Logger.Log("Awaiting mail");
            }
            else
            {
                Logger.Log($"Mail no: {mailCollect.Count}. Range: {mailCollect.Last().Range}. Calling from {Thread.CurrentThread.ManagedThreadId}");
            }
        }          
    }

   
}
