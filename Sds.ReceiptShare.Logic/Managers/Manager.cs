using Sds.ReceiptShare.Data.Repository;

namespace Sds.ReceiptShare.Logic.Managers
{
    public abstract class Manager
    {
        protected readonly IRepository _repository;

        public Manager(IRepository repository)
        {
            this._repository = repository;
        }
    }
}
