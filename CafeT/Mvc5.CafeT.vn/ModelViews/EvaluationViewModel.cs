using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class ExamView:BaseView
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public int CountViews { set; get; }

        public QuestionModel QuestionCreate { set; get; }
        public IEnumerable<QuestionModel> Questions { set; get; }
        public ExamView()
        {
            QuestionCreate = new QuestionModel();
        }
    }
}