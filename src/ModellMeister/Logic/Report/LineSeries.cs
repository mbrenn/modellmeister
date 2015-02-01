using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Logic.Report
{
    public class LineSeries
    {
        /// <summary>
        /// Title of the series
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the values of the 
        /// </summary>
        public List<double> Times
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the values of the 
        /// </summary>
        public List<double> Values
        {
            get;
            set;
        }

        public LineSeries()
        {
            this.Values = new List<double>();
            this.Times = new List<double>();
        }
    }
}
