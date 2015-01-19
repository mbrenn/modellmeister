using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Library.Sink
{
    public partial class LinePlot
    {
        public double GetChannelValue ( int channelNr )
        {
            switch (channelNr)
            {
                case 1:
                    return this.Channel1;
                case 2:
                    return this.Channel2;
                case 3:
                    return this.Channel3;
                case 4:
                    return this.Channel4;
                case 5:
                    return this.Channel5;
                case 6:
                    return this.Channel6;
                default:
                    throw new InvalidOperationException("channelNr not supported: " + channelNr);
            }
        }

        partial void DoExecute(ModellMeister.Runtime.StepInfo info)
        {
            var channelCount = this.ChannelCount ;
            var result = new object[channelCount];

            for (var n = 1; n <= channelCount; n++)
            {
                result[n - 1] = this.GetChannelValue(n);
            }

            info.Debug.AddResult(result);
        }
    }
}
