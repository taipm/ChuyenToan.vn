using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CafeT.Objects
{
    public class TimerObject : BaseObject
    {
        public Timer Timer { set; get; }
        public TimerObject()
        {
            Timer = new Timer();
            Timer.Start();
            Timer.Elapsed += Timer_Elapsed;
        }
        public TimerObject(BaseObject t)
        {
            TimerObject _object = new TimerObject();
            _object.Id = t.Id;
            //foreach (var _item in t.Fields())
            //{
            //    foreach(var _i in _object.Fields())
            //    {
            //        if(_item.Key == _i.Key)
            //        {
            //        }
            //    }
            //}
            Timer = new Timer();
            Timer.Start();
            Timer.Elapsed += Timer_Elapsed;
            //this.Update();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.DoAction();
        }

        public void DoAction()
        {
        }
    }
}
