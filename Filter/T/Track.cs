using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Global;

namespace Tracker.T
{
    public class Track
    {
        public bool init = false;
        Filter filter = Filter.Instance;
        //Logger logger = Logger.Instance;
        public bool Initialise()
        {
            //await Task.Run(() =>
            //{
            for (int i = 0; i < 4000; i+=1000)
            {
                Logger.Log("Awaiting initialise");
                Thread.Sleep(i);
            }
                filter.UpdatePath(Filter.Mode.initialise);
                init = true;
            //});
            return init;
        }
    }
}
