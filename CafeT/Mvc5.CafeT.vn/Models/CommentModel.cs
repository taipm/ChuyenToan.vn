using CafeT.BusinessObjects.ELearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class CommentModel:Comment
    {
        public Guid? ArticleId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? ExamId { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? ProductId { set; get; }

        public CommentModel():base()
        {
        }
    }
}