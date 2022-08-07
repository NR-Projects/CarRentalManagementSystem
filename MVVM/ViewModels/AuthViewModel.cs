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
    public class AuthViewModel : ViewModelBase
    {
        public override string ViewName => "Auth View";

        public string? UsernameStr { get; set; }
        public string? PasswordStr { get; set; }

        public ICommand? NavigateHome { get; set; }

        public AuthViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        protected override void InitializeButtons()
        {
            base.InitializeButtons();

            NavigateHome = new ExecuteOnlyCommand((_) => {
                // Check Inputs

                // Navigate To Main Menu
                GetServiceCollection().GetNavService().Navigate(new HomeViewModel(GetServiceCollection()));
            });
        }
    }
}
