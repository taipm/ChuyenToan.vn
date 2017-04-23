using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CafeT.BusinessObjects.ELearning
{
    public class Book:BaseObject
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(50)]
        public string ISBN { get; set; }
        [Required]
        [StringLength(100)]
        public string Author { get; set; }
    }
}
