using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CafeT.BusinessObjects
{
    public abstract class BaseObject: EF6.Objects.BaseObject
    {
        [Key]
        public Guid Id { set; get; }

        [DisplayName("Created on")]
        [ScaffoldColumn(false)]
        
        public DateTime CreatedDate { set; get; }

        [ScaffoldColumn(false)]
        public string CreatedBy { set; get; }

        [ScaffoldColumn(false)]
        public DateTime? LastUpdatedDate { set; get; }

        [ScaffoldColumn(false)]
        public string LastUpdatedBy { set; get; }

        [ScaffoldColumn(false)]
        public int CountViews { set; get; } = 0;

        public IEnumerable<string> Viewers { set; get; }

        [ScaffoldColumn(false)]
        public DateTime? LastViewAt { set; get; }

        [ScaffoldColumn(false)]
        public string LastViewBy { set; get; }

        public BaseObject()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            CreatedBy = "Application";
            LastUpdatedBy = null;
            LastUpdatedDate = null;
            LastViewBy = null;
        }
    }
}
