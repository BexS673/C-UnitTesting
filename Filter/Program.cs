using System;
using Tracker.Global;
using Tracker.T;

class Program
{
    public static void Main(string[] args)
    {
        Mail testMail;
        Filter filter = new Filter();
        filter.UpdatePath(Filter.Mode.initialise);
        filter.UpdatePath(Filter.Mode.update);

        filter.CallPositionUpdate();
        filter.Output(out testMail);

        Console.WriteLine(testMail.Range);
        Console.WriteLine(testMail.Bearing);

        

        filter.CallPositionUpdate();
    }
}
