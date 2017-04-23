using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CafeT.Text
{
    public static class ExtractStringHelper
    {
        #region Extracts
        public static string GetYouTubeId(this string youTubeUrl)
        {
            //Setup the RegEx Match and give it 
            Match regexMatch = Regex.Match(youTubeUrl, "^[^v]+v=(.{11}).*",
                               RegexOptions.IgnoreCase);
            if (regexMatch.Success)
            {
                return "http://www.youtube.com/v/" + regexMatch.Groups[1].Value + "&hl=en&fs=1";
            }
            return youTubeUrl;
        }

        public static string GetInBetween(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            string[] result = { string.Empty, string.Empty };
            int iIndexOfBegin = strSource.IndexOf(strBegin);

            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired 
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;

                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired 
                    if (includeEnd)
                        iEnd += strEnd.Length;
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment 
                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd + strEnd.Length);
                }
            }
            else
                // stay where we are 
                result[1] = strSource;
            return result[0];
        }

        public static string[] ExtractAllBetween(this string text, string begin, string end)
        {
            List<string> _strs = new List<string>();
            string _str = text.ExtractFirstMinBetween(begin, end);

            while (!_str.IsNullOrEmptyOrWhiteSpace())
            {
                _strs.Add(_str);
                int _index = text.IndexOf(_str);

                if (_index != -1)
                {
                    text = text.Remove(_index, _str.Length);
                }
                _str = text.ExtractFirstMinBetween(begin, end);
            }
            return _strs.ToArray();
        }

        public static string Extract(this string value, string begin_text, string end_text, int occur = 1)
        {
            if (string.IsNullOrEmpty(value) == false)
            {
                // Search Begin
                int start = -1;
                // search with number of occurs
                for (int i = 1; i <= occur; i++)
                    start = value.IndexOf(begin_text, start + 1);

                if (start < 0)
                    return value;
                start += begin_text.Length;


                // Search End
                if (string.IsNullOrEmpty(end_text))
                    return value.Substring(start);
                int end = value.IndexOf(end_text, start);
                if (end < 0)
                    return value.Substring(start);

                end -= start;

                // End Final
                return value.Substring(start, end);
            }
            else
            {
                return value;
            }

        }

        public static void ToException(this string message)
        {
            throw new Exception(message);
        }

        public static string ToEmailMessage(this string text)
        {
            return string.Empty;
        }

        //public static string[] ExtractBetween(this string text, string a, string b)
        //{
        //    List<string> _lst = new List<string>();
        //    while (text.ExtractMaxBetween(a, b) != string.Empty)
        //    {
        //        _lst.Add(text.ExtractMinBetween(a, b));
        //        text = text.Replace(text.ExtractMinBetween(a, b), string.Empty);
        //    }
        //    return _lst.ToArray();
        //}

        /// <summary>
        /// Tested by: Phan Minh Tai
        /// </summary>
        /// <param name="text"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ExtractFirstMinBetween(this string text, string a, string b)
        {
            string _input = text;
            if (_input.After(a).Contains(b))
            {
                _input = _input.After(a);
                int _last = _input.IndexOf(b);
                return a + _input.Substring(0, _last) + b;
            }
            return string.Empty;
        }

        /// <summary>
        /// Max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ExtractMaxBetween(this string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1)
            {
                return string.Empty;
            }
            if (posB == -1)
            {
                return string.Empty;
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return string.Empty;
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        public static string[] ExtractEmails(this string str)
        {
            string RegexPattern = @"\b[A-Z0-9._-]+@[A-Z0-9][A-Z0-9.-]{0,61}[A-Z0-9]\.[A-Z.]{2,6}\b";

            // Find matches
            System.Text.RegularExpressions.MatchCollection matches
                = Regex.Matches(str, RegexPattern, RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            // add each match
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.ToString();
                c++;
            }

            return MatchList;
        }

        public static string[] ExtractUrlsWithoutHref(this string str)
        {
            string RegexPattern = @"\b(?:https?://|www\.)\S+\b";
            MatchCollection matches = Regex.Matches(str, RegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            //// Report on each match.
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.Value;
                c++;
            }

            return MatchList;
        }

        public static string[] ExtractAllMathText(this string text)
        {
            string[] _commands = text.ExtractAllBetween("$$","$$");
            return _commands;
        }

        public static string[] ExtractToYouTubeUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.ExtractUrls();
            foreach(string _url in _urls)
            {
                if(_url.ToLower().Contains("youtube.com"))
                {
                    _youtubeUrls.Add(_url);
                }
            }
            return _youtubeUrls.ToArray();
        }

        public static string[] ExtractToGoogleDriveUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.ExtractUrls();
            foreach (string _url in _urls)
            {
                if (_url.ToLower().Contains("https://drive.google.com/drive"))
                {
                    _youtubeUrls.Add(_url);
                }
            }
            return _youtubeUrls.ToArray();
        }

        public static string[] ExtractUrlsWithHref(this string str)
        {
            //match.Groups["name"].Value - URL Name
            // match.Groups["url"].Value - URI
            string RegexPattern = @"<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>";

            // Find matches.
            MatchCollection matches = Regex.Matches(str, RegexPattern, RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            // Report on each match.
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.Groups["url"].Value;
                c++;
            }

            return MatchList;
        }
        public static string[] ExtractUrls(this string str)
        {
            List<string> _urls = str.ExtractUrlsWithHref().ToList();
            List<string> _urls2  = str.ExtractUrlsWithoutHref().ToList();
            foreach(string _url in _urls2)
            {
                _urls.Add(_url);
            }
            return _urls.ToArray();
        }

        public static char[] ExtractSeparators(this string text)
        {
            List<char> separators = new List<char>();
            foreach (char character in text)
            {
                // If the character is not a letter,
                // then by definition it is a separator
                if (!char.IsLetter(character))
                {
                    separators.Add(character);
                }
            }
            return separators.ToArray();
        }

        public static char[] ExtractOperators(string expression)
        {
            string operatorCharacters = "+-*/";
            List<char> operators = new List<char>();
            foreach (char c in expression)
            {
                if (operatorCharacters.Contains(c))
                {
                    operators.Add(c);
                }
            }
            return operators.ToArray();
        }

        public static string[] ExtractNumbers(this string str)
        {
            // Find matches
            System.Text.RegularExpressions.MatchCollection matches
                = System.Text.RegularExpressions.Regex.Matches(str, @"(\d+\.?\d*|\.\d+)");

            string[] MatchList = new string[matches.Count];

            // add each match
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.ToString();
                c++;
            }

            return MatchList;
        }
        #endregion
    }
}
