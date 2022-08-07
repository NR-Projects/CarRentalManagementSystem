using CarRentalManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public override string ViewName => "Main";
        public ViewModelBase? CurrentViewModel
        {
            get
            {
                if (GetServiceCollection() != null)
                    return GetServiceCollection().GetNavService().CurrentViewModel;
                return null;
            }
        }

        public string? CurrentViewName
        {
            get
            {
                if (GetServiceCollection() != null)
                    return GetServiceCollection().GetNavService().CurrentViewName;
                return null;
            }
        }

        public MainViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
            GetServiceCollection().GetNavService().CurrentViewModelChanged += OnCurrectViewModelChanged;
        }

        private void OnCurrectViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
            OnPropertyChanged(nameof(CurrentViewName));
        }
    }
}
