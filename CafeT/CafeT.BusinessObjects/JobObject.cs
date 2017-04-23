using System;
using System.ComponentModel.DataAnnotations;

namespace CafeT.BusinessObjects
{
    public class JobObject:BaseObject
    {
        [Display(Name ="Tiêu đề")]
        public string Title { set; get; }
        [Display(Name = "Mô tả")]
        public string Description { set; get; }
        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }
        public string Tags { set; get; }

        public string VerifiedBy { set; get; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? VerifiedDate { set; get; }
        
        public bool? IsPublished { set; get; }
        public bool? IsDrafted { set; get; }
        public bool? IsPublic { set; get; }
        public bool? IsProtect { set; get; }
        public bool? IsPrivate { set; get; }

        public IEquatable<string> Skills { set; get; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime? Start { set; get; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày kết thúc")]
        public DateTime? End { set; get; }

        [Display(Name = "Số lượng")]
        public int? Quantity { set; get; }
        [Display(Name = "Mức lương/ tháng")]
        public decimal? SalaryInMoth { set; get; }

        public JobObject():base()
        {
            Start = DateTime.Now;

            //Default: Every issue must to finish in 7 days
            End = Start.Value.AddDays(7);
            Quantity = 1;
            SalaryInMoth = 0;

            IsDrafted = true;
            IsPrivate = true;

            IsPublished = false;
            IsProtect = false;
            IsPublic = false;
        }

        public bool IsEnable()
        {
            if( Start != null && Start.HasValue && Start.Value <= DateTime.Now 
                && End != null && End.HasValue && End.Value >= DateTime.Now)
            {
                return true;
            }
            return false;
        }

        public void AddDays(int days)
        {
            if(End != null && End.HasValue)
            {
                End = End.Value.AddDays(days);
            }
        }
    }
}
