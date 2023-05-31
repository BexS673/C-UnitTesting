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
using System.Reflection.Metadata;

class Program
{
    private static Stopwatch stopwatch = new Stopwatch();
    private static Mail testMail; 
    public static Random random = new Random();

    private static Filter filter = Filter.Instance;
    private static Track track = new Track();

    private static List<Mail> mailCollect = new List<Mail>();
    private static bool mailboxFull = false;

     
    private static CustomTaskScheduler scheduler = new CustomTaskScheduler(2);
    private static TaskFactory factory = new TaskFactory(scheduler);
    


    private static async Task Main(string[] args)
    {
        stopwatch.Start();
        Logger.CleanFile();
         
        //Task<bool> initialise = Task<bool>.Run( () => track.Initialise());
        Console.WriteLine("Waiting for filter to initialise.");
        //await initialise;
        bool initialised = await Task.FromResult<bool>(track.Initialise());
        if (initialised == true)
        {
            Console.WriteLine("Initialised filter.");
        }

        //Task start = Task.Run( () => Start());
        Task start = factory.StartNew(() => Start(), CancellationToken.None);
        
        //Task update = factory.StartNew(() => RandomUpdate(), CancellationToken.None);
        Task update = Task.Run( () => RandomUpdate());
        Task stopTimer = Task.Run( () => StopTimer());

        //scheduler.PrintScheduledTask();

        LogMail();

        Task.WaitAll(start, update, stopTimer);
        Console.WriteLine("Filter ended");
        //scheduler.PrintScheduledTask();
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

public class CustomTaskScheduler : TaskScheduler
{
    [ThreadStatic]
    private static bool _currentThreadIsProcessingItems;
    private readonly LinkedList<Task> _tasks = new LinkedList<Task>();
    private readonly int _maxDegreeOfParellism;
    private int _delegatesQueueOrRunning = 0;
    public CustomTaskScheduler(int maxDegreeOfParellism)
    {
        if (maxDegreeOfParellism < 1) throw new ArgumentOutOfRangeException("maxDegressOfParellism");
        _maxDegreeOfParellism = maxDegreeOfParellism;        
    }

    protected sealed override void QueueTask(Task task)
    {
        lock (_tasks)
        {
            _tasks.AddLast(task);
            if (_delegatesQueueOrRunning < _maxDegreeOfParellism)
            {
                ++_delegatesQueueOrRunning;
            }
        }
    }

    protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {

        if (!_currentThreadIsProcessingItems) return false;
        if (taskWasPreviouslyQueued)
        {
            if (TryDequeue(task))
            {
                return base.TryExecuteTask(task);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return base.TryExecuteTask(task);
        }
        
    }

    protected sealed override bool TryDequeue(Task task)
    {
        lock (_tasks) return _tasks.Remove(task);
    }

    protected sealed override IEnumerable<Task> GetScheduledTasks()
    {
        bool lockTaken = false;
        try
        {
            Monitor.TryEnter(_tasks, ref lockTaken);
            if (lockTaken) return _tasks;
            else throw new NotSupportedException();
        }
        finally
        {
            if (lockTaken) Monitor.Exit(_tasks);
        }
    }

    public void PrintScheduledTask()
    {
        IEnumerable<Task> tasks = GetScheduledTasks();
        foreach (Task scheduledTask in tasks)
        {
            Console.WriteLine($"Task {scheduledTask.Id} status: scheduledTask.Status");
        }
    }
}
