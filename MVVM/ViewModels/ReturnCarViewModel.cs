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

        private DateTime _SetReturnDate;
        public DateTime SetReturnDate
        {
            get => _SetReturnDate;
            set
            {
                SetProperty(ref _SetReturnDate, value);
                OnCalculatePenalty();
            }
        }

        private string _SelectedCarID;
        private string _SelectedCustomerName;
        public string SelectedCarID { get => _SelectedCarID; set => SetProperty(ref _SelectedCarID, value); }
        public string SelectedCustomerName { get => _SelectedCustomerName; set => SetProperty(ref _SelectedCustomerName, value); }

        private int _CalculatedPenalty;
        public int CalculatedPenalty { get => _CalculatedPenalty; set => SetProperty(ref _CalculatedPenalty, value); }

        public ObservableCollection<CarModel> CarCollection { get; set; }

        private int _CurrentCarSelectedIndex;
        public int CurrentCarSelectedIndex { get => _CurrentCarSelectedIndex; set { SetProperty(ref _CurrentCarSelectedIndex, value); OnItemSelectedChanged(); OnCalculatePenalty(); } }

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

            SetReturnDate = DateTime.Now;

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

                // Check For Penalties (1 Day = CarPrice * 1.1)

                if (CalculatedPenalty != 0)
                {
                    // Show Popup
                    string PenaltyStr = String.Format("Late car return incurs penalty of amount P{0}, Press Cancel to Go Back", CalculatedPenalty);
                    MessageBoxResult mbres = MessageBox.Show(PenaltyStr, "Return Car (Penalty Popup)", MessageBoxButton.OKCancel);
                    if (mbres == MessageBoxResult.Cancel) return;
                }

                // Wrap up

                CarModel carModel = new CarModel()
                {
                    ID = SelectedCarModel.ID,
                    CarStatus = "Available"
                };

                GetServiceCollection().GetDataService().GetCarRepository().UpdateCarRentStatus(carModel);
                if (CalculatedPenalty != 0)
                {
                    GetServiceCollection().GetDataService().GetTransactionRepository().AddTransaction(
                    new TransactionModel()
                    {
                        Label = "Return Penalty",
                        Car = carModel,
                        Customer = SelectedCarModel.RentCustomer,
                        RentDate = ((DateTime)SelectedCarModel.RentDate),
                        ReturnDate = ((DateTime)SelectedCarModel.ReturnDate),
                        TotalCost = CalculatedPenalty
                    }
                );
                }
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

        private void OnCalculatePenalty()
        {
            if (SelectedCarModel == null) return;

            // Check Date
            int DaysGap = 0;
            
            if (SelectedCarModel.ReturnDate < SetReturnDate)
            {
                Console.WriteLine(SelectedCarModel.ReturnDate.ToString());
                Console.WriteLine(SetReturnDate.ToString());
                DaysGap = (SetReturnDate - ((DateTime)SelectedCarModel.ReturnDate)).Days;
            }

            // Check Car Price Per Day
            double CarCost = SelectedCarModel.PricePerDay;

            CalculatedPenalty = (int)(CarCost * DaysGap * 1.1);
        }
    }
}
