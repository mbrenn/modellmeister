using ModellMeister.Logic.Reporting;
using ModellMeister.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Logic
{
    /// <summary>
    /// The report logic wraps the information from the simulation or execution
    /// and prepares it for a simple access on reporting
    /// </summary>
    public class ReportLogic
    {
        /// <summary>
        /// Stores the line series
        /// </summary>
        private List<LineSeries> lineSeries = new List<LineSeries>();

        /// <summary>
        /// Gets the line series
        /// </summary>
        public List<LineSeries> LineSeries
        {
            get { return this.lineSeries; }
        }

        /// <summary>
        /// Initializes a new instance of the simulation result
        /// </summary>
        /// <param name="result">Result to be executed</param>
        public ReportLogic(SimulationResult result)
        {
            this.ParseSimulationResult(result);
        }

        /// <summary>
        /// Parses the simulation results and stores them into the local
        /// </summary>
        /// <param name="result">Result to be parsed</param>
        private void ParseSimulationResult(SimulationResult result)
        {            
            // Retrieves the number of results
            var numberOfSeries = result.Result.Count == 0 ? 0 : result.Result.Max(x => x.Values.Length);
            for (var n = 0; n < numberOfSeries; n++)
            {
                this.lineSeries.Add(new LineSeries());
            }

            // Parses the results
            var m = 0; 
            foreach (var line in result.Result)
            {
                for (var n = 0; n < numberOfSeries; n++)
                {
                    if (n < line.Values.Length)
                    {
                        // The current line has enough values
                        this.lineSeries[n].Times.Add(line.AbsoluteTime.TotalSeconds);
                        this.lineSeries[n].Values.Add(Convert.ToDouble(line.Values[n]));
                    }
                    else
                    {
                        // Filling them with a zero value
                        this.lineSeries[n].Values.Add(0);
                    }
                }

                m++;
            }
        }
    }
}
