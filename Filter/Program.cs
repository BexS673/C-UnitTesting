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
    private static Logger logger = Logger.Instance;
    private static Track track = new Track();

    private static List<Mail> mailCollect = new List<Mail>();
    private static bool mailboxFull = false;
    


    private static async Task Main(string[] args)
    {
        stopwatch.Start();
        logger.CleanFile();
        Task<bool> initialise = track.Initialise();

        
        while (track.init == false)
        {
            logger.Log("Waiting to initialise filter");
            Thread.Sleep(1000);
        }

        bool init = await initialise;
        Task start = Start();
        Task update = RandomUpdate();
        Task stopTimer = StopTimer();

        PrintMail();     
    } 

    public static async Task Start()
    {

        await Task.Run(() =>
        {
            for (int i = 0; i <= 10; i++)
            {
                Thread.Sleep(2000);
                filter.CallPositionUpdate();
                filter.Output(out testMail);
                logger.Log($"New mail sent to mailbox. Calling on thread {Thread.CurrentThread.ManagedThreadId}");
                mailCollect.Add(testMail);
            }
            
        });
       
    }

    public static async Task RandomUpdate()
    {
        await Task.Run(() =>
        {
            for (int i = 0; i <= 10; i++)
            {
                Thread.Sleep(random.Next(1000));
                filter.UpdatePath(Filter.Mode.update);
                logger.Log($"Update made to path. Calling on thread {Thread.CurrentThread.ManagedThreadId}");
            }
        });

    }

    private static async Task StopTimer()
    {
        await Task.Delay(random.Next(20000));
        mailboxFull = true;
    }
    private static void PrintMail()
    {
        while (mailboxFull == false)
        {
            Thread.Sleep(1000);
            if (mailCollect.Count == 0)
            {
                logger.Log("Awaiting mail");
            }
            else
            {
                logger.Log($"Mail no: {mailCollect.Count}. Range: {mailCollect.Last().Range}. Calling from {Thread.CurrentThread.ManagedThreadId}");
            }
        }          
    }

   
}
