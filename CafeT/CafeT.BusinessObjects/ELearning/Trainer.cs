using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BusinessObjects.ELearning
{
    public class Trainer
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public List<Course> Courses { set; get; }

        public Trainer()
        {
            Courses = new List<Course>();
        }
    }
}
