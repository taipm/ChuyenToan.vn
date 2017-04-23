using CafeT.BusinessObjects;
using CafeT.BusinessObjects.ELearning;
using System;
using System.Collections;
using System.Collections.Generic;
using CafeT.Objects;
using System.Linq;

namespace Mvc5.CafeT.vn.Models
{
    public enum QuestionType
    {
        YesNo,
        Writting
    }

    public class QuestionModel:Question
    {
        public int? Level { set; get; }

        public int? Marks { set; get; }

        public Guid? ArticleId { set; get; }
        public Guid? ExamId { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? ProductId { set; get; }
        public Guid? JobId { set; get; }

        /// <summary>
        /// Minutes
        /// </summary>
        public int? EstimationTime { set; get; }

        public QuestionType Type { set; get; }
        public virtual IEnumerable<FileModel> Files { set; get; }
        public virtual IEnumerable<AnswerModel> Answers { set; get; }
        public virtual AnswerModel CorrectAnswer { set; get; }

        public QuestionModel():base()
        {
            EstimationTime = 45;
            Level = 0;
            Marks = 0;
        }

        public bool IsNoAnswer()
        {
            if (Answers == null) return true;
            return false;
        }

        public bool HasFiles()
        {
            if (Files != null && Files.Count() > 0) return true;
            return false;
        }
        public bool HasAnswer()
        {
            if (Answers != null && Answers.Count() > 0) return true;
            return false;
        }
        public override string ToString()
        {
            return this.PrintAllProperties();
        }
    }
}