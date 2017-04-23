using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CafeT.Text
{
    public static class ValidateStringHelper
    {
        #region Contains
        public static bool ContainsAny(this string text, string[] words)
        {
            foreach (string _word in words)
            {
                if (text.Contains(_word))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #region Object Validate
        public static bool IsCommand(this string text)
        {
            return false;
        }
        public static bool IsEmailMessage(this string text)
        {
            return false;
        }
        public static bool IsExistsRemoteFile(this string remoteUrl)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(remoteUrl) as HttpWebRequest;

                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";

                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                //Returns TRUE if the Status code == 200
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will return false.
                return false;
            }
        }

        #endregion
        #region Structure validate
        public static bool IsNullOrEmptyOrWhiteSpace(this string input)
        {
            return string.IsNullOrEmpty(input) || input.Trim() == string.Empty;
        }
        public static bool HasLetter(this string text)
        {
            return text.Any(x => char.IsLetter(x));
        }
        public static bool IsUrl(this string text)
        {
            if(!string.IsNullOrWhiteSpace(text))
            {
                Regex rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
                return rx.IsMatch(text);
            }
            return false;
        }

        public static bool IsImageUrl(this string text)
        {
            if (text.IsUrl() && text.EndsWith("jpg"))
            {
                return true;
            }
            return false;
        }

        public static bool IsIp(this string text)
        {
            return Regex.IsMatch(text,
                                @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");

        }

        public static bool IsEmail(this string text)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(text);
        }

        public static bool IsEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsDate(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                DateTime dt;
                return (DateTime.TryParse(input, out dt));
            }
            else
            {
                return false;
            }
        }
        public static bool IsUnicode(this string value)
        {
            int asciiBytesCount = System.Text.Encoding.ASCII.GetByteCount(value);
            int unicodBytesCount = System.Text.Encoding.UTF8.GetByteCount(value);

            if (asciiBytesCount != unicodBytesCount)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts the string representation of a Guid to its Guid 
        /// equivalent. A return value indicates whether the operation 
        /// succeeded. 
        /// </summary>
        /// <param name="s">A string containing a Guid to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the Guid value equivalent to 
        /// the Guid contained in <paramref name="s"/>, if the conversion 
        /// succeeded, or <see cref="Guid.Empty"/> if the conversion failed. 
        /// The conversion fails if the <paramref name="s"/> parameter is a 
        /// <see langword="null" /> reference (<see langword="Nothing" /> in 
        /// Visual Basic), or is not of the correct format. 
        /// </param>
        /// <value>
        /// <see langword="true" /> if <paramref name="s"/> was converted 
        /// successfully; otherwise, <see langword="false" />.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///        Thrown if <pararef name="s"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// Original code at https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=94072&wa=wsignin1.0#tabs
        /// 
        /// </remarks>
        public static bool IsGuid(this string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            Regex format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
            Match match = format.Match(s);

            return match.Success;
        }
        public static bool IsFullFilePath(this string text)
        {
            return false;
        }
        public static bool IsFullFolderPath(this string text)
        {
            return false;
        }
        public static bool IsExcelFile(this string text)
        {
            return false;
        }
        public static bool IsWordFile(this string text)
        {
            return false;
        }
        public static bool IsPdfile(this string text)
        {
            return false;
        }
        public static bool IsImageFile(this string text)
        {
            return false;
        }
        public static bool IsPowerPointFile(this string text)
        {
            return false;
        }
        public static bool IsXmlFile(this string text)
        {
            return false;
        }
        public static bool IsHtmlFile(this string text)
        {
            return false;
        }

        public static bool IsNumeric(this string text)
        {
            long retNum;
            return long.TryParse(text, System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        }

        public static bool IsBoolean(this string value)
        {
            var val = value.ToLower().Trim();
            if (val == "false")
                return false;
            if (val == "f")
                return false;
            if (val == "true")
                return true;
            if (val == "t")
                return true;
            if (val == "yes")
                return true;
            if (val == "no")
                return false;
            if (val == "y")
                return true;
            if (val == "n")
                return false;
            throw new ArgumentException("Value is not a boolean value.");
        }

        public static bool IsWhitespace(this char character)
        {
            return character == ' ' || character == 'n' || character == 't';
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        #endregion
    }
}
