using System;
using System.Collections.Generic;
using System.Linq;

namespace LotteryStatistics.Core.DLL
{
    /// <summary>
    /// This POCO class contain lottery drawing
    /// </summary>
    public class Drawing
    {
        public DateTime DateTime { get; set; }
        public IList<int> Numbers { get; set; }
        public int BonusBall { get; set; }
        public int Multiplier { get; set; }
        public IList<int> PatternLevels { get; set; }
    }
}
