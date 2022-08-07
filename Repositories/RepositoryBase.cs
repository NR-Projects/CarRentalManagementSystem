using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.Repositories
{
    public abstract class RepositoryBase
    {
        protected string DatabaseConnectionString = "datasource=localhost;port=3306;username=dummy;password=dummy_password;database=crms";
    }
}
