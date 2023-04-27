using System;
using Tracker.Global;
using Tracker.T;
using System.Threading;
using System.Collections.Generic;
using System.Net.Mail;

class Program
{

    private static Mail testMail; //this should be new Mail object each time written to.

    private static Filter filter = Filter.Instance;

    private static List<Mail> mailCollect = new List<Mail>();
    
    public static void Main(string[] args)
    {
        
        //Filter filter =  Filter.Instance;
        filter.UpdatePath(Filter.Mode.initialise);
       

        Thread Thread1 = new Thread(new ThreadStart(Start));
        Thread Thread2 = new Thread(new ThreadStart(CollectMail));
        Thread Thread3 = new Thread(new ThreadStart(RandomUpdate));

        Thread1.Start();
        Thread3.Start();
        Thread2.Start();
        
    }

    public static void Start()
    {
        
        filter.UpdatePath(Filter.Mode.initialise);
       
        for (int i = 0; i < 10; i++)
        {
            
            filter.UpdatePath(Filter.Mode.update);
            filter.CallPositionUpdate();
            filter.Output(out testMail); 
            mailCollect.Add(testMail);          
        }
    }

    public static void RandomUpdate()
    {
        // Random randomTimer = new Random();
        for (int i = 0; i < 10; i++)
        {
            //Thread.Sleep(100);
            filter.UpdatePath(Filter.Mode.update);
            Console.WriteLine("Random update made");
        }
    }

    public static void CollectMail()
    {
        int i = 0;
        foreach (Mail mail in mailCollect)
        {
            i++;
            Console.WriteLine($" Range for mail {i} is: {mail.Range}");
            Console.WriteLine($" Bearing for mail {i} is: {mail.Bearing}");
        }
      
    }

    //public static void Write()
    //{
    //    for(int i = 0; i < 10; i++)
    //    {

    //        filter.UpdatePath(Filter.Mode.update);
    //        filter.CallPositionUpdate();
    //        filter.Output(out testMail);

    //        Console.WriteLine($"Thead2 Range is: {testMail.Range}");
    //        Console.WriteLine($"Thread2 Bearing is: {testMail.Bearing}");
    //    }
    //}
}
