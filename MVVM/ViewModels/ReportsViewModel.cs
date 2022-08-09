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
using System.Windows.Input;

namespace CarRentalManagementSystem.MVVM.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        public override string ViewName => "Reports View";

        public string _SelectedFilter;
        public string SelectedFilter { get => _SelectedFilter; set { SetProperty(ref _SelectedFilter, value); OnSelectedFilterChanged(); } }

        private int _CurrentCarSelectedIndex;
        public int CurrentCarSelectedIndex { get => _CurrentCarSelectedIndex; set => SetProperty(ref _CurrentCarSelectedIndex, value); }


        private bool _ModelState;
        private bool _BrandState;
        private bool _PriceState;
        public bool ModelState { get => _ModelState; set => SetProperty(ref _ModelState, value); }
        public bool BrandState { get => _BrandState; set => SetProperty(ref _BrandState, value); }
        public bool PriceState { get => _PriceState; set => SetProperty(ref _PriceState, value); }


        private string _ModelFilterStr;
        private string _BrandFilterStr;
        private string _LowerPriceRange;
        private string _HigherPriceRange;

        public string ModelFilterStr { get => _ModelFilterStr; set { SetProperty(ref _ModelFilterStr, value); ModelFilterListener(); } }
        public string BrandFilterStr { get => _BrandFilterStr; set { SetProperty(ref _BrandFilterStr, value); BrandFilterListener(); } }
        public string LowerPriceRange { get => _LowerPriceRange; set { SetProperty(ref _LowerPriceRange, value); PriceFilterListener(); } }
        public string HigherPriceRange { get => _HigherPriceRange; set { SetProperty(ref _HigherPriceRange, value); PriceFilterListener(); } }

        public ObservableCollection<CarModel> CarCollection { get; set; }
        public ObservableCollection<string> FilterChoices { get; set; }

        public ICommand? NavigateBack { get; set; }
        public ICommand? RefreshCollection { get; set; }

        public ReportsViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        protected override void InitializeProperties()
        {
            base.InitializeProperties();
            CarCollection = new ObservableCollection<CarModel>();
            FilterChoices = new ObservableCollection<string>();

            FilterChoices.Add("Model");
            FilterChoices.Add("Brand");
            FilterChoices.Add("Price");

            LoadCollection();
        }

        protected override void InitializeButtons()
        {
            base.InitializeButtons();

            NavigateBack = new ExecuteOnlyCommand((_) => {
                GetServiceCollection().GetNavService().Navigate(new HomeViewModel(GetServiceCollection()));
            });

            RefreshCollection = new ExecuteOnlyCommand((_) => {
                LoadCollection();
            });
        }

        private void OnSelectedFilterChanged()
        {
            switch(SelectedFilter)
            {
                case "Model":
                    ModelState = true;
                    BrandState = false;
                    PriceState = false;
                    break;
                case "Brand":
                    ModelState = false;
                    BrandState = true;
                    PriceState = false;
                    break;
                case "Price":
                    ModelState = false;
                    BrandState = false;
                    PriceState = true;
                    break;
            }

            LoadCollection();
        }

        private void LoadCollection()
        {
            List<CarModel> carModels = GetServiceCollection().GetDataService().GetCarRepository().GetFullCars();

            if (carModels == null) return;

            // Clear Collection
            CarCollection.Clear();

            // Append All Cars
            foreach (CarModel carModel in carModels)
                CarCollection.Add(carModel);
        }

        private void ModelFilterListener()
        {
            if (ModelFilterStr.Equals(""))
            {
                LoadCollection();
                return;
            }

            List<CarModel> carModels = CarCollection.ToList();

            if (carModels == null) return;

            // Clear Collection
            CarCollection.Clear();

            foreach (CarModel carModel in carModels)
            {
                if(carModel.Model.Contains(ModelFilterStr))
                    CarCollection.Add(carModel);
            }
        }

        private void BrandFilterListener()
        {
            if (BrandFilterStr.Equals(""))
            {
                LoadCollection();
                return;
            }

            List<CarModel> carModels = CarCollection.ToList();

            if (carModels == null) return;

            // Clear Collection
            CarCollection.Clear();

            foreach (CarModel carModel in carModels)
            {
                if (carModel.Brand.Contains(BrandFilterStr))
                    CarCollection.Add(carModel);
            }
        }

        private void PriceFilterListener()
        {
            if (LowerPriceRange.Equals("") || HigherPriceRange.Equals(""))
            {
                LoadCollection();
                return;
            }

            try { double a = Double.Parse(LowerPriceRange); double b = Double.Parse(HigherPriceRange); } catch { return; }

            double LowerPrice = Double.Parse(LowerPriceRange);
            double HigherPrice = Double.Parse(HigherPriceRange);

            if (LowerPrice > HigherPrice) return;



            List<CarModel> carModels = CarCollection.ToList();

            if (carModels == null) return;

            // Clear Collection
            CarCollection.Clear();

            foreach (CarModel carModel in carModels)
            {
                if(carModel.PricePerDay >= LowerPrice && carModel.PricePerDay <= HigherPrice)
                    CarCollection.Add(carModel);
            }
        }
    }
}
