using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CafeT.Text
{
    public static class StringHelper
    {
        public static string DeleteBeginTo(this string text, string to)
        {
            int _firstIndex = text.IndexOf(to);
            return text.Substring(_firstIndex);
        }

        public static string DeleteEndTo(this string text, string to)
        {
            int _lastIndex = text.LastIndexOf(to);
            return text.Substring(0, _lastIndex);
        }

        public static string GetFromBeginTo(this string text, string to)
        {
            return new NotImplementedException().Message;
        }
        public static string GetFromEndTo(this string text, string to)
        {
            return new NotImplementedException().Message;
        }
        public static string GetFromTo(this string text, string from, string to)
        {
            return new NotImplementedException().Message;
        }
        //#region Navigation
        //public static string[] GetNextTo(this string text, string word)
        //{
        //    return new string[] { };
        //}
        //public static string[] GetPrevTo(this string text, string word)
        //{
        //    return new string[] { };
        //}

        //#endregion
        #region Commands
        public static string[] IntersectWords(this string text, string[] anothers)
        {
            string _tmpStr = text;
            List<string> _words = new List<string>();
            foreach (string _str in anothers)
            {
                _words.AddRange(_tmpStr.IntersectWords(_str).ToList());
            }
            return _words.Distinct().ToArray();
        }
        public static string[] UnionWords(this string text, string another)
        {
            string[] _words = text.ToWords().Select(t => t.ToStandard()).ToArray();
            string[] _words2 = another.ToWords().Select(t => t.ToStandard()).ToArray();
            return _words.Union(_words2).Distinct().ToArray();
        }

        public static string[] IntersectWords(this string text, string another)
        {
            if(!text.IsNullOrEmptyOrWhiteSpace())
            {
                string[] _words = text.ToHtmlWords().Select(t => t.ToStandard()).ToArray();
                string[] _words2 = another.ToHtmlWords().Select(t => t.ToStandard()).ToArray();
                return _words.Intersect(_words2).ToArray();
            }
            return null;
        }
        #endregion
        
        #region Basic
        public static WordObject[] ToWordsObjects(this string text)
        {
            string[] _words = text.ToWords();
            List<WordObject> _wordObjects = new List<WordObject>();
            for(int i= 0; i < _words.Length; i++)
            {
                WordObject _word = new WordObject(_words[i]);
                _word.Index = i;
                _word.Value = _words[i];
                if(i<=0)
                {
                    _word.Before = null;
                }
                else
                {
                    _word.Before = new WordObject(_words[i-1]);
                }
                if (i >= _words.Length)
                {
                    _word.After = null;
                }
                else
                {
                    _word.After = new WordObject(_words[i + 1]);
                }
                _wordObjects.Add(_word);
            }
            return _wordObjects.ToArray();
        }
        
        public static string FirstWord(this string text)
        {
            string[] _words = text.ToWords();
            return _words[0];
        }
        public static void RemoveFirstWord(this string text)
        {
            text = text.Remove(0, text.FirstWord().Length);
        }

        public static string GetWord(this string text, int i)
        {
            if (text.IsNullOrEmptyOrWhiteSpace()) return null;
            else
            {
                string[] _words = text.ToWords();
                if (i <= _words.Count())
                {
                    return _words[i];
                }
                return string.Empty;
            }
        }

        public static WordObject GetWordObject(this string text, int n)
        {
            if (text.IsNullOrEmptyOrWhiteSpace()) return null;
            else
            {
                if (text.ToWords() != null && text.ToWords().Length <= n) return null;
                else
                {
                    string[] _words = text.ToWords();
                    WordObject _word = new WordObject();
                    _word.Index = n;
                    _word.Value = _words[n];
                    _word.Before = text.ToWordsObjects()[n - 1];
                    _word.After = text.ToWordsObjects()[n + 1];
                    return _word;
                }
            }
        }

        public static string[] NextWords(this string text, string word)
        {
            
            int[] _index = text.IndexAll(word);
            List<string> _nextWords = new List<string>();
            foreach (int i in _index)
            {
                try
                {
                    _nextWords.Add(text.GetWord(i + 1));
                }
                catch
                {
                    _nextWords.Add(string.Empty);
                }
            }
            return _nextWords.ToArray();
        }

        public static string[] PrveWords(this string text, string word)
        {
            int[] _index = text.IndexAll(word);
            List<string> _nextWords = new List<string>();

            foreach (int i in _index)
            {
                try
                {
                    _nextWords.Add(text.GetWord(i- 1));
                }
                catch
                {
                    _nextWords.Add(string.Empty);
                }
            }
            return _nextWords.ToArray();
        }

        public static int[] IndexAll(this string text, string word)
        {
            if (String.IsNullOrEmpty(word))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += word.Length)
            {
                index = text.IndexOf(word, index);
                if (index == -1)
                {
                    return indexes.ToArray();
                }
                indexes.Add(index);
            }
        }
        
        public static bool DoesNotStartWith(this string input, string pattern)
        {
            return string.IsNullOrEmpty(pattern) ||
                   input.IsNullOrEmptyOrWhiteSpace() ||
                   !input.StartsWith(pattern, StringComparison.CurrentCulture);
        }
        public static bool DoesNotEndWith(this string input, string pattern)
        {
            return string.IsNullOrEmpty(pattern) ||
                     input.IsNullOrEmptyOrWhiteSpace() ||
                     !input.EndsWith(pattern, StringComparison.CurrentCulture);
        }
        //public static bool IsDifference(this string text, string compare)
        //{
        //    return text.Equals(compare, StringComparison.Ordinal);
        //}

        public static string AddBefore(this string text, string before)
        {
            return before + text;
        }

        public static string AddAfter(this string text, string after)
        {
            return text + after;
        }

        public static string AddLineBefore(this string text, string before)
        {
            return before + "\n" + text;
        }
        public static string AddLineAfter(this string text, string after)
        {
            return text + "\n" + after;
        }

        public static string[] Select(this string[] texts, int min, int max)
        {
            return texts.Where(t => t.ToWords().Length <= max && t.ToWords().Length >= min).ToArray();
        }
        
        public static string[] ToWords(this string text)
        {
            if(string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            else
            {
                char[] separators = text.ExtractSeparators();
                string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                return words;
                //string[] _delimiters = new string[] { " ", ",", ".", "?", "!" };
                //return text.ToStandard().Split(_delimiters, StringSplitOptions.None)
                //    .Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
            }
        }

        public static bool IsUpper(this string word)
        {
            bool result = word.Equals(word.ToUpper());
            return result;
        }

        public static bool IsLower(this string word)
        {
            bool result = word.Equals(word.ToLower());
            return result;
        }

        //http://www.introprogramming.info/tag/evaluate-an-arithmetic-expression/
        //public static string[] ExtractWords(string text)
        //{
        //    char[] separators = text.ExtractSeparators();
        //    string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        //    return words;
        //}

        public static string[] ToHtmlWords(this string text)
        {
            string[] _strSplits = new string[] { " "};
            string[] _words =  text.ToStandard().Split(_strSplits, StringSplitOptions.None)
                .Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
            return _words;
        }

        public static string[] ToSentences(this string text)
        {
            string[] _strSplits = new string[] { ".", "?", "!" };
            return text.ToStandard().Split(_strSplits, StringSplitOptions.None);
        }

        public static string ToStandard(this string text)
        {
            if (text == null || string.IsNullOrWhiteSpace(text)) return string.Empty;
            while (text.StartsWith(" "))
            {
                text = text.Remove(0);
            }
            while (text.EndsWith(" "))
            {
                text = text.Remove(text.Length - 1);
            }
            while (text.Contains("\n"))
            {
                text = text.Replace("\n", string.Empty);
            }
            while (text.Contains("\t"))
            {
                text = text.Replace("\t", string.Empty);
            }
            //while (text.EndsWith(" ."))
            //{
            //    text = text.Remove(text.Length - 1);
            //}
            return text;
        }

        #endregion

        #region Strip
        /// <summary>
        /// Strip unwanted characters and replace them with empty string
        /// </summary>
        /// <param name="data">the string to strip characters from.</param>
        /// <param name="textToStrip">Characters to strip. Can contain Regular expressions</param>
        /// <returns>The stripped text if there are matching string.</returns>
        /// <remarks>If error occurred, original text will be the output.</remarks>
        public static string Strip(this string data, string textToStrip)
        {
            var stripText = data;

            if (string.IsNullOrEmpty(data)) return stripText;

            try
            {
                stripText = Regex.Replace(data, textToStrip, string.Empty);
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return stripText;
        }

        /// <summary>
        /// Strips unwanted characters on the specified string
        /// </summary>
        /// <param name="data">the string to strip characters from.</param>
        /// <param name="textToStrip">Characters to strip. Can contain Regular expressions</param>
        /// <param name="textToReplace">the characters to replace the stripped text</param>
        /// <returns>The stripped text if there are matching string.</returns>
        /// <remarks>If error occurred, original text will be the output.</remarks>
        public static string Strip(this string data, string textToStrip, string textToReplace)
        {
            var stripText = data;

            if (string.IsNullOrEmpty(data)) return stripText;

            try
            {
                stripText = Regex.Replace(data, textToStrip, textToReplace);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return stripText;
        }

        /// <summary>
        /// Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="text">string that will be truncated</param>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <returns>truncated string</returns>
        public static string Truncate(this string text, int maxLength)
        {
            // replaces the truncated string to a ...
            const string suffix = "...";
            string truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            int strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }

        #endregion
        
        #region ToHtml
        public static string ToImageHtml(this string text)
        {
            if(text.IsFullFilePath())
            {
                return string.Empty;
            }
            return string.Empty;
        }
        #endregion

        //http://www.code-kings.com/2012/08/reverse-words-in-given-string.html
        public static string ReverseWords(this string text)
        {
            string[] arySource = text.Split(new char[] { ' ' });
            string strReverse = string.Empty;

            for (int i = arySource.Length - 1; i >= 0; i--)
            {
                strReverse = strReverse + " " + arySource[i];
            }

            Console.WriteLine("Original String: \n\n" + text);

            Console.WriteLine("\n\nReversed String: \n\n" + strReverse);

            return strReverse;
        }

        public static bool HasPair(this string text, string first, string last)
        {
            if (text.After(first).Contains(last)) return true;
            return false;
        }

        public static bool ContainsAny(this string theString, char[] characters)
        {
            foreach (char character in characters)
            {
                if (theString.Contains(character.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        public static string[] GetSentences(this string text, string word)
        {
            List<string> _sentences = new List<string>();
            //Match Collection for every sentence
            MatchCollection matchSentences =
                Regex.Matches(text, @"([A-Z][^\.!?]*[\.!?])");
            //Alternative pattern :  @"(\S.+?[.!?])(?=\s+|$)"

            //counter for sentences.
            int foundSentenceWithWord = 0;
            foreach (Match sFound in matchSentences)
            {
                foreach (Capture capture in sFound.Captures)
                {
                    string current_sentence = capture.Value;
                    
                    //if you don't want to match for words like 'bank'er  or  'bank'ing
                    //use the word boundary "\b"
                    //change this pattern to   @"\b"+word+@"\b"
                    Match matchWordInSentence =
                        Regex.Match(capture.Value, word, RegexOptions.IgnoreCase);
                    if (matchWordInSentence.Success)
                    {
                        _sentences.Add(current_sentence);
                        foundSentenceWithWord++;
                    }
                }
            }
            return _sentences.ToArray();
        }

        

        private static readonly HashSet<char> DefaultNonWordCharacters
          = new HashSet<char> { ',', '.', ':', ';' };

        /// <summary>
        /// Returns a substring from the start of <paramref name="value"/> no 
        /// longer than <paramref name="length"/>.
        /// Returning only whole words is favored over returning a string that 
        /// is exactly <paramref name="length"/> long. 
        /// </summary>
        /// <param name="value">The original string from which the substring 
        /// will be returned.</param>
        /// <param name="length">The maximum length of the substring.</param>
        /// <param name="nonWordCharacters">Characters that, while not whitespace, 
        /// are not considered part of words and therefor can be removed from a 
        /// word in the end of the returned value. 
        /// Defaults to ",", ".", ":" and ";" if null.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <paramref name="length"/> is negative
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="value"/> is null
        /// </exception>
        public static string CropWholeWords(this string value, int length,
          HashSet<char> nonWordCharacters = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (length < 0)
            {
                throw new ArgumentException("Negative values not allowed.", "length");
            }

            if (nonWordCharacters == null)
            {
                nonWordCharacters = DefaultNonWordCharacters;
            }

            if (length >= value.Length)
            {
                return value;
            }
            int end = length;

            for (int i = end; i > 0; i--)
            {
                if (value[i].IsWhitespace())
                {
                    break;
                }

                if (nonWordCharacters.Contains(value[i])
                    && (value.Length == i + 1 || value[i + 1] == ' '))
                {
                    //Removing a character that isn't whitespace but not part 
                    //of the word either (ie ".") given that the character is 
                    //followed by whitespace or the end of the string makes it
                    //possible to include the word, so we do that.
                    break;
                }
                end--;
            }

            if (end == 0)
            {
                //If the first word is longer than the length we favor 
                //returning it as cropped over returning nothing at all.
                end = length;
            }

            return value.Substring(0, end);
        }

        
        

        /// <summary>
        /// Get string value after [first] a.
        /// </summary>
        public static string Before(this string value, string a)
        {
            int posA = value.IndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            return value.Substring(0, posA);
        }
       
        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string After(this string value, string a)
        {
            int posA = value.IndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }

        
        
        
        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>        
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string otherValue in stringValues)
                if (string.Compare(value, otherValue) == 0)
                    return true;

            return false;
        }

        /// <summary>
        /// Converts string to enum object
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
        }

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(0, length) : value;
        }

        /// <summary>
        ///  Replaces the format item in a specified System.String with the text equivalent
        ///  of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="arg0">An System.Object to format</param>
        /// <returns>A copy of format in which the first format item has been replaced by the
        /// System.String equivalent of arg0</returns>
        public static string Format(this string value, object arg0)
        {
            return string.Format(value, arg0);
        }

        /// <summary>
        ///  Replaces the format item in a specified System.String with the text equivalent
        ///  of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="args">An System.Object array containing zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the System.String
        /// equivalent of the corresponding instances of System.Object in args.</returns>
        public static string Format(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        public static string Inject(this string source, IFormatProvider formatProvider, params object[] args)
        {
            var objectWrappers = new object[args.Length];
            for (var i = 0; i < args.Length; i++)
            {
                objectWrappers[i] = new ObjectWrapper(args[i]);
            }

            return string.Format(formatProvider, source, objectWrappers);
        }

        public static string Inject(this string source, params object[] args)
        {
            return Inject(source, CultureInfo.CurrentUICulture, args);
        }

        /// <summary>
        /// Authors: Phan Minh Tai
        /// Email: taipm.vn@outlook.com
        /// Date: 19/05/2016
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Count of words in input string</returns>
        public static int GetCountWords(this string input)
        {
            var count = 0;
            try
            {
                // Exclude whitespaces, Tabs and line breaks
                var re = new Regex(@"[^\s]+");
                var matches = re.Matches(input);
                count = matches.Count;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }

        public static int CountOf(this string input, string word)
        {
            int _result = 0;
            int _index = input.IndexOf(word);
            while (_index >= 0)
            {
                input = input.Remove(_index, word.Length);
                _index = input.IndexOf(word);
                _result = _result + 1;
            }
            return _result;
        }
        /// <summary>
        /// JavaScript style Eval for simple calculations
        /// http://www.osix.net/modules/article/?id=761
        /// This is a safe evaluation.  IE will not allow for injection.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string Evaluate(this string e)
        {
            Func<string, bool> VerifyAllowed = e1 =>
            {
                string allowed = "0123456789+-*/()%.,";
                for (int i = 0; i < e1.Length; i++)
                {
                    if (allowed.IndexOf("" + e1[i]) == -1)
                    {
                        return false;
                    }
                }
                return true;
            };

            if (e.Length == 0) { return string.Empty; }
            if (!VerifyAllowed(e)) { return "String contains illegal characters"; }
            if (e[0] == '-') { e = "0" + e; }
            string res = "";
            try
            {
                res = Calculate(e).ToString();
            }
            catch
            {
                return "The call caused an exception";
            }
            return res;
        }

        /// <summary>
        /// JavaScript Eval Calculations for simple calculations
        /// http://www.osix.net/modules/article/?id=761
        /// This is an unsafe calculation. IE may allow for injection.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static double Calculate(this string e)
        {
            e = e.Replace(".", ",");
            if (e.IndexOf("(") != -1)
            {
                int a = e.LastIndexOf("(");
                int b = e.IndexOf(")", a);
                double middle = Calculate(e.Substring(a + 1, b - a - 1));
                return Calculate(e.Substring(0, a) + middle.ToString() + e.Substring(b + 1));
            }
            double result = 0;
            string[] plus = e.Split('+');
            if (plus.Length > 1)
            {
                // there were some +
                result = Calculate(plus[0]);
                for (int i = 1; i < plus.Length; i++)
                {
                    result += Calculate(plus[i]);
                }
                return result;
            }
            else
            {
                // no +
                string[] minus = plus[0].Split('-');
                if (minus.Length > 1)
                {
                    // there were some -
                    result = Calculate(minus[0]);
                    for (int i = 1; i < minus.Length; i++)
                    {
                        result -= Calculate(minus[i]);
                    }
                    return result;
                }
                else
                {
                    // no -
                    string[] mult = minus[0].Split('*');
                    if (mult.Length > 1)
                    {
                        // there were some *
                        result = Calculate(mult[0]);
                        for (int i = 1; i < mult.Length; i++)
                        {
                            result *= Calculate(mult[i]);
                        }
                        return result;
                    }
                    else
                    {
                        // no *
                        string[] div = mult[0].Split('/');
                        if (div.Length > 1)
                        {
                            // there were some /
                            result = Calculate(div[0]);
                            for (int i = 1; i < div.Length; i++)
                            {
                                result /= Calculate(div[i]);
                            }
                            return result;
                        }
                        else
                        {
                            // no /
                            string[] mod = mult[0].Split('%');
                            if (mod.Length > 1)
                            {
                                // there were some %
                                result = Calculate(mod[0]);
                                for (int i = 1; i < mod.Length; i++)
                                {
                                    result %= Calculate(mod[i]);
                                }
                                return result;
                            }
                            else
                            {
                                // no %
                                return double.Parse(e);
                            }
                        }
                    }
                }
            }
        }
    }
}
