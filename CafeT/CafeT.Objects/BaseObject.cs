using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace CafeT.Objects
{
    public class BaseObject
    {
        [Key]
        public Guid Id { set; get; } 
        
        public string PathTextDb { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime? LastUpdatedDate { set; get; }

        
        public BaseObject()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            CreatedBy = "Application";
            PathTextDb = string.Empty;
        }

        public TimerObject ToTimerObject()
        {
            return new TimerObject(this);
        }
        public string GetObjectInfo()
        {
            return this.GetObjectAllFields();
        }
        public void Print()
        {
            this.PrintAllProperties();
        }
    }
}
