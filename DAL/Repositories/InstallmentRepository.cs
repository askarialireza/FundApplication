using System.Linq;
using System;

namespace DAL
{
    public class InstallmentRepository : Repository<Models.Installment>, IInstallmentRepository
    {
        public InstallmentRepository(Models.DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
