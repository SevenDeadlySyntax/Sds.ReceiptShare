﻿using Sds.ReceiptShare.Data.Repository;

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
