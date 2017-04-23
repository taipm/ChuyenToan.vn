using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvc5.CafeT.vn.Models
{
    public class MilestoneModel:BaseObject
    {
        public string Name { set; get; }
        public string Description { set; get; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Start { set; get; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? End { set; get; }

        public MilestoneModel() : base() { }
    }

    public class CuriculumModel:BaseObject
    {
        public Guid? UserId{ set; get; }

        [Required]
        [EmailAddress]
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Tags { set; get; }
        public string Companies { set; get; }
        public string Skills { set; get; }
        public virtual IEnumerable<FileModel> Files { set; get; }
        //public decimal Salary { set; get; }

        public string Content { set; get; }

        public virtual IEnumerable<MilestoneModel> Milestones { set; get; }
        public CuriculumModel() : base() { }
    }
}