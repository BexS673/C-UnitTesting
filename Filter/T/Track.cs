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
        Logger logger = Logger.Instance;
        public async Task<bool> Initialise()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(4000);
                filter.UpdatePath(Filter.Mode.initialise);
                logger.Log($"Initialised filter. Calling from {Thread.CurrentThread.ManagedThreadId}");
                init = true;
            });
            return init;
        }
    }
}
