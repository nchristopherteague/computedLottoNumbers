using LotteryStatistics.Core.BLL;
using LotteryStatistics.Core.DLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LotteryStatistics.Core;

namespace LotteryStatistics.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get all arguments
            var argumentManager = new ArgumentManager(args
                .Select(s => s.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(s => s[0].Substring(1), s => s[1]));
            var arguments = argumentManager.GetArgument("run", "GenerateNumbers").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var argIsFileOutput = argumentManager.CompareArgument("OutputMode", "file");
            var argNumberOfPatterns = argumentManager.GetArgument("NumberOfPatterns", "0");
            var argLotteryPicksPattern = argumentManager.GetArgument("pattern", "1,2,3,4,5");
            var argRandomSelectionCount = argumentManager.GetArgument("NumberOfRandomSelections", "0");

            // Get the Root Path and Output File Name
            var currentPath = Environment.CurrentDirectory.ToRootPath();
            var outputFileName = argIsFileOutput ? currentPath + "\\statistics\\" : string.Empty;
            
            // Load the Lottery/ies
            var lotteryNames = argumentManager.GetArgument("lotteries", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var lotteries = InitializeLottery(currentPath, lotteryNames);
            
            // if no lotteries exists, then just exit
            if (lotteries == null)
            {
                System.Console.WriteLine("No Lotteries Found");
                System.Console.ReadLine();
                return;
            }

            // Create a new Output Channel and run the commands
            using (var output = new OutputManager(argIsFileOutput, outputFileName))
            {
                foreach (var lottery in lotteries)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("*********************************************");
                    System.Console.WriteLine("{0} - Total Drawings: {1}", lottery.Name, lottery.Drawings.Count.ToString());
                    System.Console.WriteLine("*********************************************");

                    foreach (var argument in arguments)
                    {
                        System.Console.WriteLine();

                        switch (argument)
                        {
                            case "ShowNumberStatistics":
                                ShowNumberStatistics(lottery);
                                break;

                            case "ShowBonusBallStatistics":
                                ShowBonusBallStatistics(lottery);
                                break;

                            case "ShowPatternStatistics":
                                ShowPatternStatistics(lottery, argNumberOfPatterns.ToInt());
                                break;

                            case "ShowGeneratedNumbers":
                                ShowGeneratedNumbers(lottery, Convert.ToInt32(argRandomSelectionCount));
                                break;

                            case "ShowExistingLotteryNumbersPerPattern":
                                ShowExistingLotteryNumbersPerPattern(lottery, argLotteryPicksPattern);
                                break;

                            default:
                                break;
                        }
                    }
                }

                System.Console.WriteLine();
                System.Console.WriteLine();
            }

            System.Console.WriteLine();
            //System.Console.WriteLine("DONE");
            //System.Console.ReadLine();

        }

        #region Private Operations

        /// <summary>
        /// Initialize the Lottery
        /// </summary>
        /// <param name="currentPath">Current Path of application</param>
        /// <param name="lotteryNames">Names of lotteries</param>
        /// <returns></returns>
        private static IList<Lottery> InitializeLottery(string currentPath, IList<string> lotteryNames)
        {
            // Load the Lottery/ies
            var lotteryManager = InitializeLotteryManager(currentPath);

            if (lotteryManager == null || lotteryManager.Lotteries.Count < 1)
            {
                return null;
            }

            return lotteryNames.Count < 1 ? lotteryManager.Lotteries : lotteryManager.GetLotteries(lotteryNames);
        }

        /// <summary>
        /// Load the lottery
        /// </summary>
        /// <param name="currentPath"></param>
        /// <returns>Lottery Manager</returns>
        private static LotteryManager InitializeLotteryManager(string currentPath)
        {
            var lotteryDataPath = currentPath + "\\Data";
            return new LotteryManager(lotteryDataPath);
        }

        /// <summary>
        /// Outputs the statistics for a given lottery numbers
        /// </summary>
        /// <param name="lottery">The Lottery Object</param>
        private static void ShowNumberStatistics(Lottery lottery)
        {
            System.Console.WriteLine("*********************************************");
            System.Console.WriteLine("Number Statistics from {0} - {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            System.Console.WriteLine("*********************************************");

            foreach (var item in lottery.LotteryNumberStatistics)
            {
                System.Console.WriteLine("Lottery Number:  Number:{0} was Drawn {1} Times", item.Data, item.Count.ToString());
            }

            System.Console.WriteLine("*********************************************");
        }

        /// <summary>
        /// Outputs the statistics for a given lottery bonus balls
        /// </summary>
        /// <param name="lottery">The Lottery Object</param>
        private static void ShowBonusBallStatistics(Lottery lottery)
        {
            System.Console.WriteLine("*********************************************");
            System.Console.WriteLine("BonusBall Statistics from {0} - {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            System.Console.WriteLine("*********************************************");

            foreach (var item in lottery.BonusBallStatistics)
            {
                System.Console.WriteLine("BonusBall Number:  Number:{0} was Drawn {1} Times", item.Data, item.Count.ToString());
            }

            System.Console.WriteLine("*********************************************");
        }

        /// <summary>
        /// Outputs the statistics for a given lottery pattern level
        /// </summary>
        /// <param name="lottery">The Lottery Object</param>
        /// <param name="numberOfPatterns">Number of Patterns to show, 0 if show all</param>
        private static void ShowPatternStatistics(Lottery lottery, int numberOfPatterns = 0)
        {
            var patternStatistics = numberOfPatterns < 1
                ? lottery.PatternLevelStatistics
                : lottery.PatternLevelStatistics.Take(numberOfPatterns).ToList();

            System.Console.WriteLine("*********************************************");
            System.Console.WriteLine("Pattern Statistics from {0} - {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            System.Console.WriteLine("Pattern levels are assigned based on the lottery number drawn:");
            System.Console.WriteLine("Number Drawn between 00-09 = Pattern Level = 0");
            System.Console.WriteLine("Number Drawn between 10-19 = Pattern Level = 1");
            System.Console.WriteLine("Number Drawn between 20-29 = Pattern Level = 2");
            System.Console.WriteLine("Number Drawn between 30-39 = Pattern Level = 3");
            System.Console.WriteLine("etc, etc, etc.");
            System.Console.WriteLine("*********************************************");

            foreach (var item in patternStatistics)
            {
                var pattern = item.Data.Split(',').ToList<string>();
                System.Console.WriteLine("Pattern Levels:  {0} - {1}", item.Data, item.Count.ToString());
            }

            System.Console.WriteLine("*********************************************");
        }

        /// <summary>
        /// Outputs Random Generated Numbers for each pattern
        /// </summary>
        /// <param name="lottery">The Lottery Object</param>
        /// <param name="randomSelections">How many random selections for each pattern level</param>
        private static void ShowGeneratedNumbers(Lottery lottery, int randomSelections = 1)
        {
            System.Console.WriteLine("*********************************************");
            System.Console.WriteLine("Generated Picks from {0} - {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());

            foreach (var item in lottery.PatternLevelStatistics)
            {
                var pattern = item.Data.Split(',').ToList<string>();

                System.Console.WriteLine("*********************************************");
                System.Console.WriteLine("Pattern Levels:  {0}", string.Join(",", pattern));

                var mostPickedNumbers = NumberGenerator.SelectMostPickedNumbers(lottery.LotteryNumberStatistics, pattern);
                System.Console.WriteLine("Most Picked Numbers:  {0}", string.Join<int>(",", mostPickedNumbers));

                for (var i = 0; i < randomSelections; i++)
                {
                    var randomNumbers = NumberGenerator.SelectRandomNumbers(lottery.LotteryNumberStatistics, pattern);
                    System.Console.WriteLine("Random Selected Numbers:  {0}", string.Join<int>(",", randomNumbers));
                }
            }

            System.Console.WriteLine("*********************************************");
        }


        private static void ShowExistingLotteryNumbersPerPattern(Lottery lottery, string patternLevel)
        {
            System.Console.WriteLine("*********************************************");
            System.Console.WriteLine("Lottery Numbers Per Pattern {0} from {1} - {2}", patternLevel, DateTime.Now.ToShortDateString(),
                DateTime.Now.ToShortTimeString());
            System.Console.WriteLine("*********************************************");

            foreach (var drawing in lottery.Drawings)
            {
                var sortedNumbers = drawing.Numbers.OrderBy(a => a).ToList();
                var patternLevels = string.Join(",", drawing.PatternLevels);

                if (patternLevels.Equals(patternLevel))
                {
                    System.Console.WriteLine("{0} - {1},{2},{3},{4},{5}",
                        drawing.DateTime.ToString("MM/dd/yyyy"),
                        sortedNumbers[0].ToString(),
                        sortedNumbers[1].ToString(),
                        sortedNumbers[2].ToString(),
                        sortedNumbers[3].ToString(),
                        sortedNumbers[4].ToString());
                }
            }

            System.Console.WriteLine("*********************************************");
        }


        #endregion
    }
}

