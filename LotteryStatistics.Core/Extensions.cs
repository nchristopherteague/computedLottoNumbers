using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System;
using LotteryStatistics.Core.DLL;

namespace LotteryStatistics.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Gets the safe attribute from XmlNode
        /// </summary>
        /// <param name="source"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetSafeAttribute(this XmlNode @source, string attributeName, string defaultValue = null)
        {
            var attribute = @source.Attributes[attributeName];
            return attribute != null
                       ? attribute.Value
                       : defaultValue;
        }

        /// <summary>
        /// Gets the safe value of a XmlNode
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetSafeNode(this XmlNode @source, string defaultValue = null)
        {
            return !string.IsNullOrWhiteSpace(@source.InnerText)
                ? @source.InnerText
                : defaultValue;
        }

        /// <summary>
        /// To Int
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToInt(this string @source)
        {
            return int.Parse(@source);
        }

        /// <summary>
        /// To Int
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string @source, int defaultValue)
        {
            return !int.TryParse(@source, out var value) ? defaultValue : value;
        }

        /// <summary>
        /// Initialize array with values
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        public static void InitializeArray(this int[] @source, int defaultValue)
        {
            // Initialize Lottery Stats
            //var lotteryStats = new int[this.HighestNumber + 1];
            for (int i = 1; i < @source.Count(); i++)
            {
                @source[i] = defaultValue;
            }
        }
        
        /// <summary>
        /// Sort dictionary
        /// </summary>
        /// <param name="source"></param>
        public static void SortDictionary(this IDictionary<int, int> @source)
        {
            @source = @source.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Convert to Pattern Level
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToPatternLevel(this int @source)
        {
            return @source / 10;
        }

        /// <summary>
        /// Convert to Pattern Levels
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IList<int> ToPatternLevels(this IEnumerable<int> @source)
        {
            return @source.Select(number => number.ToPatternLevel()).ToList();
        }

        /// <summary>
        /// Pick random number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T PickRandom<T>(this IEnumerable<T> @source)
        {
            return @source.PickRandom(1).Single();
        }

        /// <summary>
        /// Pick Random and take different counts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> @source, int count)
        {
            return @source.Shuffle().Take(count);
        }
        
        #region Private Operations

        /// <summary>
        /// Shuffle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static IEnumerable<T> Shuffle<T>(this IEnumerable<T> @source)
        {
            return @source.OrderBy(x => Guid.NewGuid());
        }

        /// <summary>
        /// Generates the Lottery Number Statistics
        /// </summary>
        /// <param name="source">The Lottery Object</param>
        public static void GenerateLotteryNumberStatistics(this Lottery @source)
        {
            var collection = new int[@source.HighestNumber + 1];
            collection.InitializeArray(0);

            // Convert to Array
            foreach (var drawing in @source.Drawings)
            {
                foreach (var number in drawing.Numbers)
                {
                    collection[number] = collection[number] + 1;
                }
            }

            @source.LotteryNumberStatistics = new List<Statistics>();
            for (var i = 1; i < collection.Count(); i++)
            {
                @source.LotteryNumberStatistics.Add(new Statistics() { Data = i.ToString(), Count = collection[i], AdditionalData = i.ToPatternLevel().ToString() });
            }

        }

        /// <summary>
        /// Generate the Bonus Ball Statistics
        /// </summary>
        /// <param name="source">The Lottery Object</param>
        public static void GenerateBonusBallStatistics(this Lottery @source)
        {
            var collection = new int[@source.HighestNumber + 1];
            collection.InitializeArray(0);

            foreach (var drawing in @source.Drawings)
            {
                collection[drawing.BonusBall] = collection[drawing.BonusBall] + 1;
            }

            @source.BonusBallStatistics = new List<Statistics>();
            for (var i = 1; i < collection.Count(); i++)
            {
                @source.BonusBallStatistics.Add(new Statistics()
                {
                    Data = i.ToString(),
                    Count = collection[i]
                });
            }
        }

        /// <summary>
        /// Generate the Pattern level Statistic
        /// </summary>
        /// <param name="source">The Lottery Object</param>
        public static void GeneratePatternLevelStatistics(this Lottery @source)
        {
            IDictionary<string, int> statistics = new Dictionary<string, int>();

            // Convert to Array
            foreach (var drawing in @source.Drawings)
            {
                var patternLevels = string.Join<int>(",", drawing.PatternLevels);
                if (statistics.ContainsKey(patternLevels))
                {
                    statistics[patternLevels] += 1;
                }
                else
                {
                    statistics.Add(patternLevels, 1);
                }
            }

            foreach (var stats in statistics)
            {
                @source.PatternLevelStatistics.Add(new Statistics() { Data = stats.Key, Count = stats.Value });
            }

            @source.PatternLevelStatistics = @source.PatternLevelStatistics
                .OrderByDescending(o => o.Count)
                .ThenBy(o => o.Data).ToList();
        }


        #endregion

    }
}
