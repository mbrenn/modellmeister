using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Logic.Reporting
{
    public class WatchList
    {
        /// <summary>
        /// Stores the watchlist
        /// </summary>
        private List<WatchListItem> items = new List<WatchListItem>();

        /// <summary>
        /// Gets the items of the watchlist
        /// </summary>
        public List<WatchListItem> Items
        {
            get { return this.items; }
        }

        public void Add(WatchListItem channelInformation)
        {
            if (channelInformation.Index == -1)
            {
                channelInformation.Index = this.Items.Count;
            }

            this.Items.Add(channelInformation);
        }
    }
}
