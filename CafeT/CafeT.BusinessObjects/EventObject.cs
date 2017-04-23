using CafeT.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BusinessObjects
{
    public class EventObject:BaseObject
    {
        [Required]
        [MaxLength(100)]
        public string Title { set; get; }

        public string Content { set; get; }
        public Address Location { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { set; get; }

        /// <summary>
        /// Duration in minutes
        /// </summary>
        public int Duration { set; get; }

        public TimerObject Timer { set; get; }

        public EventObject():base()
        {

        }
        public bool IsEnable()
        {
            if(StartTime <= DateTime.Now)
            {
                return false;
            }
            return true;
        }
    }
}
