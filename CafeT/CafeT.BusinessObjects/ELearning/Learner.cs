using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BusinessObjects.ELearning
{
    public class Learner
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        
        ///Internal methods
        ///
        public List<Course> Courses { set; get; }
        public List<Article> Articles { set; get; }


        public Learner()
        {
            Courses = new List<Course>();
        }
        ///External methods
        ///

        public Examer ToExamer()
        {
            return new Examer();
        }
    }
}
