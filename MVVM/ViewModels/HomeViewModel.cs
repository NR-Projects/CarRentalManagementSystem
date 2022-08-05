using CarRentalManagementSystem.Commands;
using CarRentalManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarRentalManagementSystem.MVVM.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public override string ViewName => "Home View";

        public ICommand? NavigateSettings { get; set; }
        public ICommand? NavigateLogout { get; set; }

        public ICommand? NavigateCars { get; set; }
        public ICommand? NavigateCustomers { get; set; }
        public ICommand? NavigateRentCar { get; set; }
        public ICommand? NavigateReturnCar { get; set; }
        public ICommand? NavigateReports { get; set; }
        public ICommand? NavigateTransactions { get; set; }

        public HomeViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        protected override void InitializeButtons()
        {
            base.InitializeButtons();

            NavigateSettings = new ExecuteOnlyCommand((_) => {
                //
            });

            NavigateLogout = new ExecuteOnlyCommand((_) => {
                // Clear Session

                // Send Back To Auth View
                _ServiceCollection.GetNavService().Navigate(new AuthViewModel(_ServiceCollection));
            });


            NavigateCars = new ExecuteOnlyCommand((_) => {
                _ServiceCollection.GetNavService().Navigate(new CarsViewModel(_ServiceCollection));
            });

            NavigateCustomers = new ExecuteOnlyCommand((_) => {
                //
            });

            NavigateRentCar = new ExecuteOnlyCommand((_) => {
                //
            });

            NavigateReturnCar = new ExecuteOnlyCommand((_) => {
                //
            });

            NavigateReports = new ExecuteOnlyCommand((_) => {
                //
            });

            NavigateTransactions = new ExecuteOnlyCommand((_) => {
                //
            });
        }
    }
}
