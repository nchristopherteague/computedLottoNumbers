using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using LotteryStatistics.Core.DLL;
using LotteryStatistics.Core.Data;

namespace LotteryStatistics.Core.BLL
{
    /// <summary>
    /// This acts as the Lottery Manager. This class will contain
    /// collection of lotteries and all logic around them.
    ///
    /// In a real world scenario this would also have an interface and dependency injector
    /// to get the components needed.
    /// </summary>
    public class LotteryManager
    {
        #region Members

        private readonly string dataPath;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the collection of Lotteries
        /// </summary>
        public IList<Lottery> Lotteries { get; set; } = new List<Lottery>();

        /// <summary>
        /// Gets the collection of Lottery Names
        /// </summary>
        public IList<string> LotteryNames
        {
            get
            {
                return this.Lotteries != null ? 
                    (from item in this.Lotteries select item.Name).OrderBy(a => a).ToList() : 
                    null;
            }
        }

        #endregion

        #region Constructors and Destructors

        public LotteryManager(string dataPath)
        {
            this.dataPath = dataPath;

            // Load the Raw Data
            this.LoadRawData();
        }

        #endregion

        #region Public Operations

        /// <summary>
        /// Return the Lottery based on a single name
        /// </summary>
        /// <param name="lotteryName">Name of the lottery</param>
        /// <returns>Single lottery object</returns>
        public Lottery GetLottery(string lotteryName)
        {
            return this.Lotteries.FirstOrDefault(l => l.Name.Equals(lotteryName));
        }

        /// <summary>
        /// Return a collection of lotteries based on multiple names
        /// </summary>
        /// <param name="lotteryNames">Collection of lottery names</param>
        /// <returns>Collection of lottery objects</returns>
        public IList<Lottery> GetLotteries(IList<string> lotteryNames)
        {
            var lotteries = this.Lotteries.Where(l => lotteryNames.Contains(l.Name));

            return lotteries.ToList();
        }

        #endregion

        #region Private Operations

        /// <summary>
        /// Load the Raw Lottery Data
        /// </summary>
        private void LoadRawData()
        {
            try
            {
                var lotteryFiles = Directory.GetFiles(this.dataPath, "*xml", SearchOption.AllDirectories);
                if(lotteryFiles == null)
                {
                    throw new Exception("TBD");
                }

                foreach (var file in lotteryFiles)
                {
                    using (TextReader reader = new StreamReader(file))
                    {
                        var serializer = new XmlSerializer(typeof(LotteryDto));
                        this.CreateLottery((LotteryDto)serializer.Deserialize(reader));
                    }
                }
            }
            catch(Exception ex) { throw ex; }
        }

        /// <summary>
        /// Transform the Lottery into reusable data
        /// </summary>
        /// <param name="lotteryDto"></param>
        private void CreateLottery(LotteryDto lotteryDto)
        {
            // First we need to create the Drawings
            var drawings = TransformDrawings(lotteryDto.Drawings);

            // Create the Lottery
            var lottery = new Lottery()
                {Name = lotteryDto.Name, HighestNumber = lotteryDto.HighestNumber, Drawings = drawings};

            // Run the statistics
            lottery.GenerateLotteryNumberStatistics();
            lottery.GenerateBonusBallStatistics();
            lottery.GeneratePatternLevelStatistics();

            // Then We need to populate the Lottery
            this.Lotteries.Add(lottery);
        }

        /// <summary>
        /// Parse each drawing and transform into useable parts
        /// </summary>
        /// <param name="rawDrawings"></param>
        private static IList<Drawing> TransformDrawings(IEnumerable<string> rawDrawings)
        {
            IList<Drawing> response = new List<Drawing>();
            foreach (var drawing in rawDrawings)
            {
                // Each drawing is a comma delimited list of data
                // We need to grab this and parse out what is needed
                IList<string> lineValues = drawing.Split(',').ToList();

                // Create a new instance of LotteryDrawings 
                // to add to this collection
                var dateTime = new DateTime(
                        lineValues[3].ToString().ToInt(0),
                        lineValues[1].ToString().ToInt(0),
                        lineValues[2].ToString().ToInt(0));

                // Lets get the numbers
                var numbers = new List<int>
                {
                    lineValues[4].ToString().ToInt(),
                    lineValues[5].ToString().ToInt(),
                    lineValues[6].ToString().ToInt(),
                    lineValues[7].ToString().ToInt(),
                    lineValues[8].ToString().ToInt()
                };
                numbers = (from item in numbers select item).OrderBy(a => a).ToList();

                // Get the Bonus Ball and Multiplier
                var bonusBall = lineValues.Count > 9 ? lineValues[9].ToString().ToInt() : 0;
                var multiplier = lineValues.Count > 10 ? lineValues[10].ToString().ToInt() : 0;
                var patternLevels = numbers.ToPatternLevels();

                // Add to the collection
                response.Add(new Drawing() { DateTime = dateTime, Numbers = numbers, BonusBall = bonusBall, Multiplier = multiplier, PatternLevels = patternLevels });
            }

            return response;
        }

        #endregion

    }
}
