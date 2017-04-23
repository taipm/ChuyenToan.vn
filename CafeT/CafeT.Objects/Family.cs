using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CafeT.Objects
{
    public class Family
    {

    }

    public class SmartAge
    {
        public int Age { set; get; }
        public SmartAge(int age)
        {
            Age = age;
        }
        public void Update(int age)
        {
            Age = age;
            if(Age>20)
            {

            }
        }
    }

    public class People
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public DateTime Birthday { set; get; }
        public int Age { set; get; }
        public SmartAge SmAge { set; get; }

        protected Timer Clock { set; get; }

        public object State { set; get; }

        public People()
        {
            Id = Guid.NewGuid();
            Birthday = DateTime.Now;
            Age = 0;
            //SmAge = new SmartAge(Age);
            State = new Baby();
            Clock.Interval = 10000;
            Clock.Elapsed += Clock_Elapsed;
            Clock.Start();
        }

        private void Clock_Elapsed(object sender, ElapsedEventArgs e)
        {
            Age = Age + 1;
            Update();
            //SmAge.Update(Age);
            //throw new NotImplementedException();
        }
        public void Update()
        {

        }
    }

    public class Baby:People
    {
        public Baby()
        {

        }
        public object Grown()
        {
            if(Age > 10)
            {
                return new Boy(Id);
            }
            return this;
        }
    }

    public class Man : People
    {

    }

    public class Woman : People
    {

    }

    public class Boy:People
    {
        public Boy(Guid id)
        {
            Id = id;
        }
    }

    public class Girl : People
    {

    }
}
