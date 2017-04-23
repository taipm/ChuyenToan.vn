using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.BusinessObjects.ELearning
{
    public class Comment:BaseObject
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public Comment():base()
        { }
        public Comment(string title)
        {
            Title = title;
        }
    }
}
