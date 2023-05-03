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

class Program
{
    private static Stopwatch stopwatch = new Stopwatch();
    private static Mail testMail; 
    public static Random random = new Random();

    private static Filter filter = Filter.Instance;

    private static List<Mail> mailCollect = new List<Mail>();
    private static bool mailboxFull = false;
    private static async Task Main(string[] args)
    {
        stopwatch.Start();
        await Task.Run(() => { filter.UpdatePath(Filter.Mode.initialise); });
        // Task.Run(() => { Start(); });
        //Task.Run(() => { RandomUpdate(); });
        Task start =  Start();
        Task update = RandomUpdate();
        Task stopTimer = StopTimer();
        Console.WriteLine(start.Status);
        Console.WriteLine(update.Status);
        Console.WriteLine(stopTimer.Status);
        PrintMail();
        Console.WriteLine(start.Status);
        Console.WriteLine(update.Status);
        Console.WriteLine(stopTimer.Status);
       
    } 

    public static async Task Start()
    {

        await Task.Run(() =>
        {
            for (int i = 0; i <= 10; i++)
            {

                //await Task.Run(() =>
                //    {
                Thread.Sleep(1000);
                filter.CallPositionUpdate();
                filter.Output(out testMail);
                mailCollect.Add(testMail);
                //   });
            }
            
        });
        
    }

    public static async Task RandomUpdate()
    {
        await Task.Run(() =>
        {
            for (int i = 0; i <= 10; i++)
            {
                //Thread.Sleep(500);
                //  await Task.Run(() => 
                //  {
                Thread.Sleep(random.Next(500));
                filter.UpdatePath(Filter.Mode.update);
                // });
                //filter.UpdatePath(Filter.Mode.update);

                //Console.WriteLine("Random update made");
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
        //Thread.Sleep(2000);
        while (mailboxFull == false)
        {
            Thread.Sleep(500);
            Console.WriteLine($"Mail no: {mailCollect.Count}");
            Console.WriteLine($"Time: {stopwatch.Elapsed}");
            if (mailCollect.Count == 0)
            {
                Console.WriteLine("Awaiting mail");
            }
            else
            {
                Console.WriteLine($"Range: {mailCollect.Last().Range}");
            }
            //mailCollect.Remove(mailCollect.First());
            //  Console.WriteLine($"Bearing: {mailCollect.Last().Bearing}");
        }          
    }

   
}
