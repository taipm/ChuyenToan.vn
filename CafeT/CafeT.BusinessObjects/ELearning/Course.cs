using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BusinessObjects.ELearning
{
    public class Course:BaseObject
    {
        public string Name { set; get; }
        public string Description { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate{ set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { set; get; }

        public bool IsEnable { set; get; }


        public List<Trainer> Trainers { set; get; }
        public List<Learner> Learners { set; get; }


        public Course()
        {
            Trainers = new List<Trainer>();
            Learners = new List<Learner>();
        }
        public Course(string name)
        {
            Name = name;
            Trainers = new List<Trainer>();
            Learners = new List<Learner>();
        }
    }
}
