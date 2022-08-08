using CarRentalManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.MVVM.ViewModels
{
    public class TransactionsViewModel : ViewModelBase
    {
        public override string ViewName => "Transactions View";

        public TransactionsViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
        }
    }
}
