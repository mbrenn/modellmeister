using ModellMeister.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mbgi_gui.Models
{
    public class WatchModel
    {
        public SimulationClient Client
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the items
        /// </summary>
        public IEnumerable<WatchRowItem> Items
        {
            get
            {                
                var lastState = this.Client.SimulationResult.Result.LastOrDefault();
                if (lastState != null)
                {
                    var n = 1;
                    foreach (var item in lastState.Values)
                    {
                        if (item == null)
                        {
                            yield return new WatchRowItem("Value " + n.ToString(), "null");
                        }
                        else
                        {
                            yield return new WatchRowItem("Value " + n.ToString(), item.ToString());
                        }
                        n++;
                    }
                }
            }
        }

        public WatchModel(SimulationClient client)
        {
            this.Client = client;
        }
    }
}

