using CarRentalManagementSystem.Commands;
using CarRentalManagementSystem.MVVM.Models;
using CarRentalManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarRentalManagementSystem.MVVM.ViewModels
{
    public class RentCarViewModel : ViewModelBase
    {
        public override string ViewName => "Rent Car View";

        private CarModel SelectedCarModel;
        private CustomerModel SelectedCustomerModel;

        private string _SelectedCarID;
        private DateTime _SetRentDate;
        private DateTime _SetReturnDate;
        private string _TotalCost;
        public string SelectedCarID { get => _SelectedCarID; set => SetProperty(ref _SelectedCarID, value); }
        public DateTime SetRentDate {
            get => _SetRentDate;
            set {
                SetProperty(ref _SetRentDate, value);
                OnCostsChanged();
            }
        }
        public DateTime SetReturnDate {
            get => _SetReturnDate;
            set
            {
                SetProperty(ref _SetReturnDate, value);
                OnCostsChanged();
            }
        }
        public string TotalCost { get => _TotalCost; set => SetProperty(ref _TotalCost, value); }

        private string _SelectedCustomerID;
        private string _SelectedCustomerName;
        public string SelectedCustomerID { get => _SelectedCustomerID; set => SetProperty(ref _SelectedCustomerID, value); }
        public string SelectedCustomerName { get => _SelectedCustomerName; set => SetProperty(ref _SelectedCustomerName, value); }

        public int _CurrentCarSelectedIndex;
        public int _CurrentCustomerSelectedIndex;
        public int CurrentCarSelectedIndex {
            get => _CurrentCarSelectedIndex;
            
            set {
                SetProperty(ref _CurrentCarSelectedIndex, value);
                OnCarSelectedChanged();
            }
        }
        public int CurrentCustomerSelectedIndex
        {
            get => _CurrentCustomerSelectedIndex;

            set
            {
                SetProperty(ref _CurrentCustomerSelectedIndex, value);
                OnCustomerSelectedChanged();
            }
        }

        public ObservableCollection<CarModel> CarCollection { get; set; }
        public ObservableCollection<CustomerModel> CustomerCollection { get; set; }

        public ICommand? OpenCustomersViewButton { get; set; }
        public ICommand? RentCarButton { get; set; }
        public ICommand? RefreshCollections { get; set; }

        public ICommand? NavigateBack { get; set; }

        public RentCarViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        protected override void InitializeProperties()
        {
            base.InitializeProperties();

            CurrentCarSelectedIndex = -1;
            CurrentCustomerSelectedIndex = -1;

            SetRentDate = DateTime.Now;
            SetReturnDate = DateTime.Now;

            CarCollection = new ObservableCollection<CarModel>();
            CustomerCollection = new ObservableCollection<CustomerModel>();

            SelectedCarID = "?";
            TotalCost = "?";

            SelectedCustomerID = "?";
            SelectedCustomerName = "?";

            LoadCarCollection();
            LoadCustomerCollection();
        }

        protected override void InitializeButtons()
        {
            base.InitializeButtons();

            NavigateBack = new ExecuteOnlyCommand((_) => {
                GetServiceCollection().GetNavService().Navigate(new HomeViewModel(GetServiceCollection()));
            });

            OpenCustomersViewButton = new ExecuteOnlyCommand((_) => {
                GetServiceCollection().GetNavService().Navigate(new CustomersViewModel(GetServiceCollection()));
            });

            RentCarButton = new ExecuteOnlyCommand((_) => {
                // Check Inputs

                string ErrorList = "";

                if (SelectedCarModel == null) ErrorList += "No Selected Car For Rental \n";
                if (String.IsNullOrEmpty(TotalCost) || TotalCost.Equals("?")) ErrorList += "Dates Invalid \n"; // If Total Cost are Valid, then Dates should be Valid
                if (SelectedCustomerModel == null) ErrorList += "No Selected Customer For Rental \n";

                if(!ErrorList.Equals(""))
                {
                    MessageBox.Show(ErrorList, "Rent Car (Error Popup)");
                    return;
                }

                CustomerModel customerModel = new CustomerModel()
                {
                    ID = SelectedCustomerModel.ID
                };

                CarModel carModel = new CarModel()
                {
                    ID = SelectedCarModel.ID,
                    CarStatus = "Rented",
                    RentDate = SetRentDate,
                    ReturnDate = SetReturnDate,
                    RentCustomer = customerModel
                };

                GetServiceCollection().GetDataService().GetCarRepository().UpdateCarRentStatus(carModel);
                GetServiceCollection().GetDataService().GetTransactionRepository().AddTransaction(
                    new TransactionModel()
                    {
                        Label = "Rent",
                        Car = carModel,
                        Customer = customerModel,
                        RentDate = SetRentDate,
                        ReturnDate = SetReturnDate,
                        TotalCost = Double.Parse(TotalCost.Substring(3))
                    }
                );
                MessageBox.Show(String.Format("Car Rented Successfully Under Customer: {0}", SelectedCustomerName), "Rent Car Action");

                SelectedCarModel = null;
                SelectedCustomerModel = null;

                LoadCarCollection();
                LoadCustomerCollection();
            });

            RefreshCollections = new ExecuteOnlyCommand((_) => {
                LoadCarCollection();
                LoadCustomerCollection();
            });
        }

        private void LoadCarCollection()
        {
            List<CarModel> carModels = GetServiceCollection().GetDataService().GetCarRepository().GetBasicCars();

            if (carModels == null) return;

            // Clear Collection
            CarCollection.Clear();

            // Append All Cars
            foreach (CarModel carModel in carModels)
            {
                if(carModel.CarStatus.Equals("Available"))
                    CarCollection.Add(carModel);
            }
        }

        private void LoadCustomerCollection()
        {
            List<CustomerModel> customerModels = GetServiceCollection().GetDataService().GetCustomerRepository().GetFullCustomers();

            if (customerModels == null) return;

            // Clear Collection
            CustomerCollection.Clear();

            // Append All Customers
            foreach (CustomerModel customerModel in customerModels)
                CustomerCollection.Add(customerModel);
        }

        private void OnCarSelectedChanged()
        {
            SelectedCarID = "?";

            if (CurrentCarSelectedIndex < 0) return;

            SelectedCarModel = CarCollection[CurrentCarSelectedIndex];

            if (SelectedCarModel == null) return;

            SelectedCarID = SelectedCarModel.ID.ToString();

            OnCostsChanged();
        }

        private void OnCustomerSelectedChanged()
        {
            if (CurrentCustomerSelectedIndex < 0) return;

            SelectedCustomerModel = CustomerCollection[CurrentCustomerSelectedIndex];

            if (SelectedCustomerModel == null) return;

            SelectedCustomerID = SelectedCustomerModel.ID.ToString();
            SelectedCustomerName = SelectedCustomerModel.Name;
        }

        private void OnCostsChanged()
        {
            TotalCost = "?";

            // Check Selected Car
            if (SelectedCarModel == null) return;

            // Check Two Dates
            int DaysInBetween = (SetReturnDate - SetRentDate).Days;

            if (DaysInBetween <= 0) return;

            if (DaysInBetween <= 0)
            {
                TotalCost = "?";
                return;
            }

            double TotalPrice = Math.Round(SelectedCarModel.PricePerDay * DaysInBetween, 2);

            TotalCost = "Php " + TotalPrice.ToString();
        }
    }
}
