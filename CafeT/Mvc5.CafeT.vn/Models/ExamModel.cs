using CafeT.BusinessObjects.ELearning;
using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class ExamModel:Exam
    {
        public IEnumerable<QuestionModel> Questions { set; get; }

        public ExamModel():base()
        {
        }
    }
}