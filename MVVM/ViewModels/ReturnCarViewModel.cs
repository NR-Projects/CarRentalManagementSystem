using CarRentalManagementSystem.Commands;
using CarRentalManagementSystem.MVVM.Models;
using CarRentalManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarRentalManagementSystem.MVVM.ViewModels
{
    public class ReturnCarViewModel : ViewModelBase
    {
        public override string ViewName => "Return Car View";

        private CarModel SelectedCarModel;

        private string _SelectedCarID;
        private string _SelectedCustomerName;
        public string SelectedCarID { get => _SelectedCarID; set => SetProperty(ref _SelectedCarID, value); }
        public string SelectedCustomerName { get => _SelectedCustomerName; set => SetProperty(ref _SelectedCustomerName, value); }

        public ObservableCollection<CarModel> CarCollection { get; set; }

        private int _CurrentCarSelectedIndex;
        public int CurrentCarSelectedIndex { get => _CurrentCarSelectedIndex; set { SetProperty(ref _CurrentCarSelectedIndex, value); OnItemSelectedChanged(); } }

        public ICommand? NavigateBack { get; set; }
        public ICommand? ReturnCarButton { get; set; }
        public ICommand? RefreshCollection { get; set; }

        public ReturnCarViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        protected override void InitializeProperties()
        {
            base.InitializeProperties();

            CurrentCarSelectedIndex = -1;

            CarCollection = new ObservableCollection<CarModel>();

            SelectedCarID = "?";
            SelectedCustomerName = "?";

            LoadCollection();
        }

        protected override void InitializeButtons()
        {
            base.InitializeButtons();

            NavigateBack = new ExecuteOnlyCommand((_) => {
                GetServiceCollection().GetNavService().Navigate(new HomeViewModel(GetServiceCollection()));
            });

            ReturnCarButton = new ExecuteOnlyCommand((_) => {
                // Check Inputs
                string ErrorList = "";

                if (SelectedCarModel == null || SelectedCarID.Equals("?") || SelectedCustomerName.Equals("?")) ErrorList += "No Selected Car For Return \n";

                if (!ErrorList.Equals(""))
                {
                    MessageBox.Show(ErrorList, "Return Car (Error Popup)");
                    return;
                }

                CarModel carModel = new CarModel()
                {
                    ID = SelectedCarModel.ID,
                    CarStatus = "Available"
                };

                GetServiceCollection().GetDataService().GetCarRepository().UpdateCarRentStatus(carModel);
                MessageBox.Show(String.Format("Car Under Customer: {0} Was Returned Successfully", SelectedCustomerName), "Return Car Action");

                SelectedCarModel = null;

                LoadCollection();
            });

            RefreshCollection = new ExecuteOnlyCommand((_) => {
                LoadCollection();
            });
        }

        private void LoadCollection()
        {
            List<CarModel> carModels = GetServiceCollection().GetDataService().GetCarRepository().GetFullCars();

            if (carModels == null) return;

            // Clear Collection
            CarCollection.Clear();

            // Append All Cars
            foreach (CarModel carModel in carModels)
            {
                if(carModel.CarStatus.Equals("Rented"))
                    CarCollection.Add(carModel);
            }
        }

        private void OnItemSelectedChanged()
        {
            SelectedCarID = "?";
            SelectedCustomerName = "?";

            if (CurrentCarSelectedIndex < 0) return;

            SelectedCarModel = CarCollection[CurrentCarSelectedIndex];

            if (SelectedCarModel == null) return;

            SelectedCarID = SelectedCarModel.ID.ToString();
            SelectedCustomerName = SelectedCarModel.RentCustomer.Name;
        }
    }
}
