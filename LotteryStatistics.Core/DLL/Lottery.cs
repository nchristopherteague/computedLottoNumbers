using System.Collections.Generic;
using System.Linq;
using System;

namespace LotteryStatistics.Core.DLL
{
    /// <summary>
    /// POCO Class containing Lottery for the lottery
    /// </summary>
    public class Lottery
    {
        public string Name { get; set; }
        public int HighestNumber { get; set; }
        public IList<Drawing> Drawings { get; set; }

        public IList<Statistics> LotteryNumberStatistics { get; set; } = new List<Statistics>();
        public IList<Statistics> BonusBallStatistics { get; set; } = new List<Statistics>();
        public IList<Statistics> PatternLevelStatistics { get; set; } = new List<Statistics>();
    }
}
