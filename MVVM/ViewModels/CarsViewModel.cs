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
    public partial class CarsViewModel : ViewModelBase
    {
        public override string ViewName => "Cars View";

        public ICommand? NavigateBack { get; set; }

        public ICommand? RefreshCollection { get; set; }

        public CarsViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        protected override void InitializeButtons()
        {
            base.InitializeButtons();
            InitializeAddButton();
            InitializeUpdateButton();
            InitializeDeleteButton();

            NavigateBack = new ExecuteOnlyCommand((_) => {
                GetServiceCollection().GetNavService().Navigate(new HomeViewModel(GetServiceCollection()));
            });

            RefreshCollection = new ExecuteOnlyCommand((_) => {
                LoadCollections();
            });
        }

        protected override void InitializeProperties()
        {
            base.InitializeProperties();
            InitializeUpdateProperties();
            InitializeDeleteProperties();
        }

        private void LoadCollections()
        {
            LoadUpdateCarCollection();
            LoadDeleteCarCollection();
        }
    }

    public partial class CarsViewModel : ViewModelBase
    {
        public string AddCarType { get; set; }
        public string AddCarBrand { get; set; }
        public string AddCarModel { get; set; }
        public string AddCarDescription { get; set; }
        public string AddCarPlateNo { get; set; }

        public ICommand? AddCarButton { get; set; }

        private void InitializeAddButton()
        {
            AddCarButton = new ExecuteOnlyCommand((_) =>
            {
                // Check Inputs
                string ErrorList = "";

                if (String.IsNullOrEmpty(AddCarType)) ErrorList += "Car Type is Empty \n";
                if (String.IsNullOrEmpty(AddCarBrand)) ErrorList += "Car Brand is Empty \n";
                if (String.IsNullOrEmpty(AddCarModel)) ErrorList += "Car Model is Empty \n";
                if (String.IsNullOrEmpty(AddCarDescription)) ErrorList += "Car Description is Empty \n";
                if (String.IsNullOrEmpty(AddCarPlateNo)) ErrorList += "Car Plate No is Empty \n";

                if (!ErrorList.Equals(""))
                {
                    MessageBox.Show(ErrorList, "Add Car (Error Popup)");
                    return;
                }


                // Submit Inputs
                CarModel carModel = new CarModel()
                {
                    Type = AddCarType,
                    Brand = AddCarBrand,
                    Model = AddCarModel,
                    Description = AddCarDescription,
                    PlateNo = AddCarPlateNo
                };

                GetServiceCollection().GetDataService().GetCarRepository().AddCar(carModel);
                MessageBox.Show("Car Added Successfully", "Car Action");

                LoadCollections();
            });
        }
    }

    public partial class CarsViewModel : ViewModelBase
    {
        private string? _UpdateCarType;
        private string? _UpdateCarBrand;
        private string? _UpdateCarModel;
        private string? _UpdateCarDescription;
        private string? _UpdateCarPlateNo;

        public string? UpdateCarType { get => _UpdateCarType; set => SetProperty(ref _UpdateCarType, value); }
        public string? UpdateCarBrand { get => _UpdateCarBrand; set => SetProperty(ref _UpdateCarBrand, value); }
        public string? UpdateCarModel { get => _UpdateCarModel; set => SetProperty(ref _UpdateCarModel, value); }
        public string? UpdateCarDescription { get => _UpdateCarDescription; set => SetProperty(ref _UpdateCarDescription, value); }
        public string? UpdateCarPlateNo { get => _UpdateCarPlateNo; set => SetProperty(ref _UpdateCarPlateNo, value); }

        public ICommand? UpdateCarButton { get; set; }

        public ObservableCollection<CarModel> UpdateCarCollection { get; set; }

        private int _CurrentUpdateCollectionIndex;
        public int CurrentUpdateCollectionIndex
        {
            get
            {
                return this._CurrentUpdateCollectionIndex;
            }
            set
            {
                SetProperty(ref this._CurrentUpdateCollectionIndex, value);
                this._CurrentUpdateCollectionIndex = value;
                SelectedUpdateItemChanged();
            }
        }

        private CarModel SelectedUpdateCarModel;

        private void InitializeUpdateProperties()
        {
            CurrentUpdateCollectionIndex = -1;
            InitializeCollections();
            LoadUpdateCarCollection();
        }

        private void InitializeUpdateButton()
        {
            UpdateCarButton = new ExecuteOnlyCommand((_) => {
                // Check Inputs
                string ErrorList = "";

                if (String.IsNullOrEmpty(UpdateCarType)) ErrorList += "Car Type is Empty \n";
                if (String.IsNullOrEmpty(UpdateCarBrand)) ErrorList += "Car Brand is Empty \n";
                if (String.IsNullOrEmpty(UpdateCarModel)) ErrorList += "Car Model is Empty \n";
                if (String.IsNullOrEmpty(UpdateCarDescription)) ErrorList += "Car Description is Empty \n";
                if (String.IsNullOrEmpty(UpdateCarPlateNo)) ErrorList += "Car Plate No is Empty \n";

                if (SelectedUpdateCarModel == null) ErrorList += "No Selected Info From Table \n";

                if (!ErrorList.Equals(""))
                {
                    MessageBox.Show(ErrorList, "Update Car (Error Popup)");
                    return;
                }


                // Submit Inputs
                CarModel carModel = new CarModel()
                {
                    ID = SelectedUpdateCarModel.ID,
                    Type = UpdateCarType,
                    Brand = UpdateCarBrand,
                    Model = UpdateCarModel,
                    Description = UpdateCarDescription,
                    PlateNo = UpdateCarPlateNo
                };

                GetServiceCollection().GetDataService().GetCarRepository().UpdateCar(carModel);
                MessageBox.Show("Car Updated Successfully", "Car Action");

                LoadCollections();
            });
        }

        private void InitializeCollections()
        {
            UpdateCarCollection = new ObservableCollection<CarModel>();
        }

        private void LoadUpdateCarCollection()
        {
            // Pull All Cars
            List<CarModel> carModels = GetServiceCollection().GetDataService().GetCarRepository().GetBasicCar();

            if (carModels == null) return;

            // Clear Collection
            UpdateCarCollection.Clear();


            // Append All Cars
            foreach (CarModel carModel in carModels)
                UpdateCarCollection.Add(carModel);
        }

        private void SelectedUpdateItemChanged()
        {
            if(CurrentUpdateCollectionIndex < 0) return;

            SelectedUpdateCarModel = UpdateCarCollection[CurrentUpdateCollectionIndex];

            if(SelectedUpdateCarModel == null) return;

            UpdateCarType = SelectedUpdateCarModel.Type;
            UpdateCarBrand = SelectedUpdateCarModel.Brand;
            UpdateCarModel = SelectedUpdateCarModel.Model;
            UpdateCarDescription = SelectedUpdateCarModel.Description;
            UpdateCarPlateNo = SelectedUpdateCarModel.PlateNo;
        }
    }

    public partial class CarsViewModel : ViewModelBase
    {
        private string _DeleteCarID;
        private string _DeleteCarModel;
        private string _DeleteCarBrand;

        public string DeleteCarID { get => _DeleteCarID; set => SetProperty(ref _DeleteCarID, value); }
        public string DeleteCarModel { get => _DeleteCarModel; set => SetProperty(ref _DeleteCarModel, value); }
        public string DeleteCarBrand { get => _DeleteCarBrand; set => SetProperty(ref _DeleteCarBrand, value); }

        public ObservableCollection<CarModel> DeleteCarCollection { get; set; }

        public ICommand? DeleteCarButton { get; set; }

        private int _CurrentDeleteCollectionIndex;
        public int CurrentDeleteCollectionIndex
        {
            get
            {
                return this._CurrentDeleteCollectionIndex;
            }
            set
            {
                SetProperty(ref this._CurrentDeleteCollectionIndex, value);
                this._CurrentDeleteCollectionIndex = value;
                SelectedDeleteItemChanged();
            }
        }

        private CarModel SelectedDeleteCarModel;

        private void InitializeDeleteProperties()
        {
            DeleteCarCollection = new ObservableCollection<CarModel>();
            DeleteCarID = "?";
            DeleteCarModel = "?";
            DeleteCarBrand = "?";
            CurrentDeleteCollectionIndex = -1;
            LoadDeleteCarCollection();
        }

        private void InitializeDeleteButton()
        {
            DeleteCarButton = new ExecuteOnlyCommand((_) => {
                if(CurrentDeleteCollectionIndex < 0)
                {
                    MessageBox.Show("No Selected Item", "Delete Car (Error Popup)");
                    return;
                }

                CarModel carModel = new CarModel()
                {
                    ID = SelectedDeleteCarModel.ID
                };

                GetServiceCollection().GetDataService().GetCarRepository().DeleteCar(carModel);
                MessageBox.Show("Car Deleted Successfully", "Car Action");

                LoadDeleteCarCollection();
            });
        }

        private void LoadDeleteCarCollection()
        {
            // Pull All Cars
            List<CarModel> carModels = GetServiceCollection().GetDataService().GetCarRepository().GetBasicCar();

            if (carModels == null) return;

            // Clear Collection
            DeleteCarCollection.Clear();


            // Append All Cars
            foreach (CarModel carModel in carModels)
                DeleteCarCollection.Add(carModel);
        }

        private void SelectedDeleteItemChanged()
        {
            if (CurrentDeleteCollectionIndex < 0) return;

            SelectedDeleteCarModel = DeleteCarCollection[CurrentDeleteCollectionIndex];

            if (SelectedDeleteCarModel == null) return;

            DeleteCarID = SelectedDeleteCarModel.ID.ToString();
            DeleteCarModel = SelectedDeleteCarModel.Model;
            DeleteCarBrand = SelectedDeleteCarModel.Brand;

        }
    }
}
