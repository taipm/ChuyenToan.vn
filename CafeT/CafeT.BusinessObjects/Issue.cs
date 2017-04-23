using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CafeT.BusinessObjects
{
    public class Issue:BaseObject
    {
        public Guid? ProjectId { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Start { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? End { set; get; }

        public string ExcuteBy { set; get; }
        public string VerifyBy { set; get; }
        public bool IsFinished { set; get; }
        public bool IsDoing { set; get; }
        
        public IEnumerable<FileObject> Files { set; get; }

        public Issue():base()
        {
            Start = DateTime.Now;
            End = Start.Value.AddMinutes(30);
        }

        public bool IsOverdued()
        {
            if(this.End.HasValue && this.End.Value < DateTime.Now && !this.IsFinished)
            {
                return true;
            }
            return false;
        }
        public bool IsFinish()
        {
            return IsFinished;
        }
        
        public void AutoAdjust()
        {
            if(DateTime.Compare(DateTime.Now, Start.Value) < 0)
            {
                Start = DateTime.Now;
                End = Start.Value.AddMinutes(30);
            }
        }
    }
}
