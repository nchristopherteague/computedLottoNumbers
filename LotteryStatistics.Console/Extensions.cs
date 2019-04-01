using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace LotteryStatistics.Console
{
    static class Extensions
    {
        public static string ToRootPath(this string @source)
        {
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            return appPathMatcher.Match(@source).Value;
        }
    }
}
