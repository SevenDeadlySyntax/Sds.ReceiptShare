using Sds.ReceiptShare.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sds.ReceiptShare.Logic.Managers
{
    public abstract class Manager
    {
        protected IRepository Repository;

        public Manager(IRepository repository)
        {
            this.Repository = repository;
        }
    }
}
