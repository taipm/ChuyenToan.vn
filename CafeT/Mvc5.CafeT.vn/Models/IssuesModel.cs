using CafeT.BusinessObjects;
using System;
using CafeT.DateTimes;
using System.Timers;

namespace Mvc5.CafeT.vn.Models
{
    public class IssueModel:Issue
    {
        public Timer Clock { set; get; }

        public bool? IsDaily { set; get; }
        public bool? IsWeekly { set; get; }
        public bool? IsMonthly { set; get; }
        public bool? IsYearly { set; get; }

        public int? ReminderBefore { set; get; }
        public int? MaxCountReminder { set; get; }
        private int CountRemider { set; get; }

        public IssueModel() : base()
        {
            ReminderBefore = 0;
            Clock = new Timer();
            Clock.Enabled = true;
            Clock.Interval = 300000;
            Clock.Elapsed += Clock_Elapsed;
            MaxCountReminder = 2;
            CountRemider = 0;
            IsDaily = false;
        }

        private void Clock_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(IsRemider())
            {
                Notify();
            }
        }

        public void Notify()
        {
            var _emailService = new EmailService();
            _emailService.SendAsync(this);
        }

        public bool IsRemider()
        {
            if (CountRemider >= MaxCountReminder) return false;
            if(End.HasValue && ReminderBefore != null && End.Value.AddMinutes(ReminderBefore.Value) > DateTime.Now)
            {
                CountRemider = CountRemider + 1;
                return true;
            }
            return false;
        }

        public bool IsToday()
        {
            if(End.HasValue && End.Value.IsToday())
            {
                return true;
            }
            return false;
        }

        public bool IsYesterday()
        {
            if (End.HasValue && End.Value.IsYesterday())
                return true;
            return false;
        }
        public bool IsNextWorking()
        {
            return true;
        }

        public bool IsTomorrow()
        {
            if (End.HasValue && End.Value.IsTomorrow())
                return true;
            return false;
        }
        public bool IsThisWeek()
        {
            return true;
        }
        public bool IsLastWeek()
        {
            return true;
        }

        public bool IsNextWeek()
        {
            return true;
        }

        public bool IsThisMonth()
        {
            return true;
        }

        public bool IsLastMonth()
        {
            return true;
        }

        public bool IsNextMonth()
        {
            return true;
        }
    }
}