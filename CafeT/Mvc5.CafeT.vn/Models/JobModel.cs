using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mvc5.CafeT.vn.Models
{
    public class JobModel:JobObject
    {
        public IEnumerable<string> Likers { set; get; }
        public IEnumerable<string> Followers { set; get; }
        public IEnumerable<string> Applyers { set; get; }

        [Display(Name = "Hình đại diện")]
        public string AvatarPath { set; get; }

        [Display(Name = "Công ty")]
        public Guid? CompanyId { set; get; }
        public virtual CompanyModel Company { set; get; }

        public IEnumerable<QuestionModel> Questions { set; get; }
        public IEnumerable<FileModel> Files { set; get; }

        public JobModel():base()
        {
        }

        public bool HasQuestions()
        {
            if (Questions != null && Questions.Count() > 0) return true;
            return false;
        }
        public bool HasFiles()
        {
            if (Files != null && Files.Count() > 0) return true;
            return false;
        }
        public bool HasFollowers()
        {
            if (Followers != null && Followers.Count() > 0) return true;
            return false;
        }
        public bool HasApplyers()
        {
            if (Applyers != null && Applyers.Count() > 0) return true;
            return false;
        }
    }
}