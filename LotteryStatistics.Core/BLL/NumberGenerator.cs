using System.Collections.Generic;
using System.Linq;
using System;
using LotteryStatistics.Core.DLL;

namespace LotteryStatistics.Core.BLL
{

    /// <summary>
    /// 
    /// </summary>
    public static class NumberGenerator
    {
        public static IList<int> SelectMostPickedNumbers(IList<Statistics> lotteryNumbers, IList<string> patterns)
        {
            var response = new List<int>();

            foreach (var pattern in patterns)
            {
                response.Add(
                    lotteryNumbers
                    .Where(a => a.AdditionalData == pattern && !response.Contains(a.Data.ToInt()))
                     .OrderByDescending(o => o.Count)
                     .ThenBy(o => o.Data).First().Data.ToInt()
                    );
            }

            return response;
        }

        public static IList<int> SelectRandomNumbers(IList<Statistics> lotteryNumbers, IList<string> patterns)
        {
            var response = new List<int>();

            foreach (var pattern in patterns)
            {
                response.Add(
                     lotteryNumbers
                     .Where(a => a.AdditionalData == pattern && !response.Contains(a.Data.ToInt()))
                     .PickRandom().Data.ToInt()
                     );
            }

            return response;
        }
        
    }
}
