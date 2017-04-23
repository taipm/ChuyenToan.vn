using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class AnswerReviewModel:BaseObject
    {
        public Guid? AnswerId { set; get; }
        public string ReviewBy { set;get;}

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        public DateTime? ReviewDate { set; get; }

        public int? Marks { set; get; }
        public bool IsCorrect { set; get; }
        public string Content { set; get; }
        public virtual AnswerModel Answer { set; get; }
        public AnswerReviewModel() : base() { }
    }
}