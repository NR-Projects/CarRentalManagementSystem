using CarRentalManagementSystem.Commands;
using CarRentalManagementSystem.MVVM.Models;
using CarRentalManagementSystem.Services;
using System;
using System.Collections.Generic;
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

        public CarsViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        protected override void InitializeButtons()
        {
            base.InitializeButtons();
            InitializeAddButton();
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

        private void InitializeAddButton() {
            AddCarButton = new ExecuteOnlyCommand((_) => {
                // Check Inputs
                string ErrorList = "";

                if (String.IsNullOrEmpty(AddCarType))           ErrorList += "Car Type is Empty \n";
                if (String.IsNullOrEmpty(AddCarBrand))          ErrorList += "Car Brand is Empty \n";
                if (String.IsNullOrEmpty(AddCarModel))          ErrorList += "Car Model is Empty \n";
                if (String.IsNullOrEmpty(AddCarDescription))    ErrorList += "Car Description is Empty \n";
                if (String.IsNullOrEmpty(AddCarPlateNo))        ErrorList += "Car Plate No is Empty \n";

                if (!ErrorList.Equals(""))
                {
                    MessageBox.Show(ErrorList, "Add Account Error Popup");
                    return;
                }


                // Submit Inputs
                CarModel carModel = new CarModel()
                {
                    Type = AddCarType,
                    Brand = AddCarBrand,
                    Model = AddCarModel,
                    Description = AddCarDescription,
                    PlateNo = AddCarPlateNo,
                };
            });
        }
    }
}
