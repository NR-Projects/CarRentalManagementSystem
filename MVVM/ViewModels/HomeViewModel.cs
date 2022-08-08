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
                GetServiceCollection().GetNavService().Navigate(new AuthViewModel(GetServiceCollection()));
            });


            NavigateCars = new ExecuteOnlyCommand((_) => {
                GetServiceCollection().GetNavService().Navigate(new CarsViewModel(GetServiceCollection()));
            });

            NavigateCustomers = new ExecuteOnlyCommand((_) => {
                GetServiceCollection().GetNavService().Navigate(new CustomersViewModel(GetServiceCollection()));
            });

            NavigateRentCar = new ExecuteOnlyCommand((_) => {
                GetServiceCollection().GetNavService().Navigate(new RentCarViewModel(GetServiceCollection()));
            });

            NavigateReturnCar = new ExecuteOnlyCommand((_) => {
                GetServiceCollection().GetNavService().Navigate(new ReturnCarViewModel(GetServiceCollection()));
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
