using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.MathText
{
    public class Command
    {

    }

    public class Engine
    {
        public Dictionary<string, string> Commands { set; get; }

        public Engine()
        {

        }

        public void Init()
        {
            #region Các phép tính
            Commands.Add("+","+");
            Commands.Add("-", "-");
            Commands.Add(".", ".");
            Commands.Add(":", ":");
            #endregion
            #region So sánh
            Commands.Add("@{chia hết}", "");
            Commands.Add("@{không chia hết}", "+");
            Commands.Add("@{lớn hơn}", "+");
            Commands.Add("@{lớn hơn hoặc bằng}", "+");
            Commands.Add("@{nhỏ hơn}", "+");
            Commands.Add("@{nhỏ hơn hoặc bằng}", "+");

            #endregion
            #region Ký tự
            Commands.Add("@{Alpha}", "+");
            Commands.Add("@{Beta}", "+");
            Commands.Add("@{Gama}", "+");
            Commands.Add("@{}", "+");

            #endregion
            #region Giải tích
            Commands.Add("@{giới hạn}", "+");
            Commands.Add("@{tích phân không xác định}", "+");
            Commands.Add("@{tích phân xác định}", "+");
            #endregion
            #region Ký hiệu
            Commands.Add("@{Pi}", "+");
            Commands.Add("@{dương vô cùng}", "+");
            Commands.Add("@{âm vô cùng}", "+");
            Commands.Add("@{vô cùng}", @"\infty");
            #endregion
            #region Đại số
            Commands.Add("@{Alpha}", "+");
            #endregion
            #region Tập số
            Commands.Add("@{N}", @"\mathbb{N}");      //Số tự nhiên
            Commands.Add("@{Q}", @"\mathbb{Q}");      //Số hữu tỷ
            Commands.Add("@{I}", @"\mathbb{I}");      //Số vô tỷ
            Commands.Add("@{R}", @"\mathbb{R}");      //Số thực
            Commands.Add("@{C}", @"\mathbb{C}");      //Số phức
            #endregion
            #region Tập hợp
            Commands.Add("@{tồn tại}", @"\exits");
            Commands.Add("@{tồn tại}", @"\exits");
            Commands.Add("@{tồn tại}", @"\exits");
            Commands.Add("@{tồn tại}", @"\exits");
            #endregion
            #region Khung
            Commands.Add("@{Khung()()}", @"\exits");
            Commands.Add("@{tồn tại}", @"\exits");
            Commands.Add("@{tồn tại}", @"\exits");
            Commands.Add("@{tồn tại}", @"\exits");
            #endregion
        }

        //public Command GetCommandBy(string key)
        //{
        //    if
        //}
        public void BuildCommand()
        {

        }

        public bool IsCommand()
        {
            return true;
        }

        public string ToLatex(string command)
        {
            if(Commands.Select(t=>t.Key).Contains(command))
            {
                return Commands[command];
            }
            return null;
        }

        public string BuildFrame(string header, string content, string footer)
        {
            //Default: Full Width
            string _latex = string.Empty;

            string _template = @"\begin{frame}
                                    [Tên định lý]{dily:dinhly1}"
                                    + content +
                                @"\end{frame}";
            return _latex;

        }
    }
}
