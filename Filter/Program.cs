using System;
using Tracker.Global;
using Tracker.T;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Mail;

class Program
{

    private static Mail testMail; //this should be new Mail object each time written to.

    private static Filter filter = Filter.Instance;

    private static List<Mail> mailCollect = new List<Mail>();
    
    public static void Main(string[] args)
    {

        Filter filter = Filter.Instance;
        filter.UpdatePath(Filter.Mode.initialise);


        Thread Thread1 = new Thread(new ThreadStart(Start));
        //Thread Thread2 = new Thread(new ThreadStart(CollectMail));
        Thread Thread3 = new Thread(new ThreadStart(RandomUpdate));

        Thread1.Start();
        Thread3.Start();
        
        //var task1 = Start();
        //var task2 = RandomUpdate();
        //Task.WaitAny(task1, task2);

    }

    public static void Start()
    {
       
        filter.UpdatePath(Filter.Mode.initialise);

        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(2000);
            // filter.UpdatePath(Filter.Mode.update);
            filter.CallPositionUpdate();
            filter.Output(out testMail);
            Console.WriteLine($"Range is: {testMail.Range}");
            Console.WriteLine($"Bearing is: {testMail.Bearing}");
            //mailCollect.Add(testMail);          
        }
        
    }

    public static void RandomUpdate()
    {
        // Random randomTimer = new Random();
        
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(1000 );
            filter.UpdatePath(Filter.Mode.update);
            Console.WriteLine("Random update made");
        }
        
    }

   
}
