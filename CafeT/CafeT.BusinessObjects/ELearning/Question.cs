using System;
using System.ComponentModel.DataAnnotations;

namespace CafeT.BusinessObjects.ELearning
{
    public class Question:BaseObject
    {
        [MaxLength(250)]
        public string Title { set; get; }

        public string Content { set; get; }

        public string Authors { set; get; }
        public bool IsVerified { set; get; }
        public string VerifiedBy { set; get; }
        public DateTime? VerifiedDate { set; get; }

        public string Tags { set; get; }

        public Question():base()
        {
            Title = string.Empty;
            Content = string.Empty;
            IsVerified = false;
        }
    }
}
