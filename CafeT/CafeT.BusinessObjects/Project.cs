using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeT.Objects;


namespace CafeT.BusinessObjects
{
    public class Project:BaseObject
    {
        public string Name { set; get; }
        public string Description { set; get; }

        public Project()
        {
        }
        public Project(string title)
        {
            Name = title;
        }
    }
}
