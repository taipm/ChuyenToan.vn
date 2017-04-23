using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Managers
{
    public class ObjectManager
    {
        protected IUnitOfWorkAsync _unitOfWorkAsync;
        public string ErrorMessage { set; get; }

        public ObjectManager(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            ErrorMessage = "Developer is not good. Pls try and catch exception.";
        }

    }
}