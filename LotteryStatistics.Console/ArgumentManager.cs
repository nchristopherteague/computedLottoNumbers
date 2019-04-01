using System;
using System.Collections.Generic;

namespace LotteryStatistics.Console
{
    public class ArgumentManager
    {
        #region Properties

        public IDictionary<string, string> Arguments { get; set; }

        #endregion

        #region Constructors and Destructors

        public ArgumentManager()
        {
            this.Arguments = new Dictionary<string, string>();
        }

        public ArgumentManager(IDictionary<string, string> arguments)
        {
            this.Arguments = arguments;
        }

        #endregion

        #region Public Operations

        public string GetArgument(string key)
        {
            var exists = this.Arguments.TryGetValue(key, out var response);
            return response;
        }

        public string GetArgument(string key, string defaultValue)
        {
            var response = GetArgument(key);
            return string.IsNullOrWhiteSpace(response) ? defaultValue : response;
        }

        public bool CompareArgument(string key, string compare)
        {
            var value = GetArgument(key);
            return !string.IsNullOrWhiteSpace(value) && value.Equals(compare, StringComparison.CurrentCultureIgnoreCase) ? true : false;
        }

        #endregion
    }
}
