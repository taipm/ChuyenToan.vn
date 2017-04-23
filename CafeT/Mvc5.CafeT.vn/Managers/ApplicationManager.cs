using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Managers
{
    public interface IApplicationManager
    {
        string GetConnectionString();
        void SetConnectionString();
        void Daily();
        void Weekly();
        void Monthly();
        void Yearly();
    }

    public class ApplicationManager : IApplicationManager
    {
        public void Daily()
        {
            throw new NotImplementedException();
        }

        public string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        public void Monthly()
        {
            throw new NotImplementedException();
        }

        public void SetConnectionString()
        {
            throw new NotImplementedException();
        }

        public void Weekly()
        {
            throw new NotImplementedException();
        }

        public void Yearly()
        {
            throw new NotImplementedException();
        }
    }
}