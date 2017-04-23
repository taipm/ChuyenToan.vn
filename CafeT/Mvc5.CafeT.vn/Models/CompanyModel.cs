using CafeT.BusinessObjects;
using System.Collections.Generic;


namespace Mvc5.CafeT.vn.Models
{
    public class CompanyModel:Company
    {
        public IEnumerable<ImageModel> Images { set; get; }
        public IEnumerable<JobModel> Jobs { set; get; }

        public int? NumberOfStaffs { set; get; }

        public CompanyModel():base()
        {
            NumberOfStaffs = 0;
        }
    }
}