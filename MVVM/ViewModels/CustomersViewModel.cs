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
    public partial class CustomersViewModel : ViewModelBase
    {
        public override string ViewName => "Customers View";

        public ICommand? NavigateBack { get; set; }

        public ICommand? RefreshCollection { get; set; }

        public CustomersViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        protected override void InitializeButtons()
        {
            base.InitializeButtons();
            InitializeAddButton();
            InitializeUpdateButton();
            InitializeDeleteButton();

            NavigateBack = new ExecuteOnlyCommand((_) =>
            {
                GetServiceCollection().GetNavService().Navigate(new HomeViewModel(GetServiceCollection()));
            });

            RefreshCollection = new ExecuteOnlyCommand((_) =>
            {
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
            LoadUpdateCustomerCollection();
            LoadDeleteCustomerCollection();
        }
    }

    public partial class CustomersViewModel : ViewModelBase
    {
        public string AddCustomerName { get; set; }
        public string AddCustomerGender { get; set; }
        public string AddCustomerAddress { get; set; }
        public string AddCustomerContactNo { get; set; }
        public string AddCustomerEmail { get; set; }

        public ICommand? AddCustomerButton { get; set; }

        public void InitializeAddButton()
        {
            AddCustomerButton = new ExecuteOnlyCommand((_) => {
                // Check Inputs
                string ErrorList = "";

                if (String.IsNullOrEmpty(AddCustomerName)) ErrorList += "Customer Name is Empty \n";
                if (String.IsNullOrEmpty(AddCustomerGender)) ErrorList += "Customer Gender is Empty \n";
                if (String.IsNullOrEmpty(AddCustomerAddress)) ErrorList += "Customer Address is Empty \n";
                if (String.IsNullOrEmpty(AddCustomerContactNo)) ErrorList += "Customer Contact No is Empty \n";
                if (String.IsNullOrEmpty(AddCustomerEmail)) ErrorList += "Customer Email is Empty \n";

                if (!ErrorList.Equals(""))
                {
                    MessageBox.Show(ErrorList, "Add Customer (Error Popup)");
                    return;
                }

                CustomerModel customerModel = new CustomerModel()
                {
                    Name = AddCustomerName,
                    Gender = AddCustomerGender,
                    Address = AddCustomerAddress,
                    ContactNo = AddCustomerContactNo,
                    Email = AddCustomerEmail
                };

                GetServiceCollection().GetDataService().GetCustomerRepository().AddCustomer(customerModel);
                MessageBox.Show("Customer Added Successfully", "Customer Action");

                LoadCollections();
            });
        }
    }

    public partial class CustomersViewModel : ViewModelBase
    {
        private string _UpdateCustomerName;
        private string _UpdateCustomerGender;
        private string _UpdateCustomerAddress;
        private string _UpdateCustomerContactNo;
        private string _UpdateCustomerEmail;

        public string UpdateCustomerName { get => _UpdateCustomerName; set => SetProperty(ref _UpdateCustomerName, value); }
        public string UpdateCustomerGender { get => _UpdateCustomerGender; set => SetProperty(ref _UpdateCustomerGender, value); }
        public string UpdateCustomerAddress { get => _UpdateCustomerAddress; set => SetProperty(ref _UpdateCustomerAddress, value); }
        public string UpdateCustomerContactNo { get => _UpdateCustomerContactNo; set => SetProperty(ref _UpdateCustomerContactNo, value); }
        public string UpdateCustomerEmail { get => _UpdateCustomerEmail; set => SetProperty(ref _UpdateCustomerEmail, value); }

        public ICommand? UpdateCustomerButton { get; set; }

        public ObservableCollection<CustomerModel> UpdateCustomerCollection { get; set; }

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

        private CustomerModel SelectedUpdateCustomerModel;

        private void InitializeUpdateProperties()
        {
            CurrentUpdateCollectionIndex = -1;
            InitializeCollections();
            LoadUpdateCustomerCollection();
        }

        private void InitializeUpdateButton()
        {
            UpdateCustomerButton = new ExecuteOnlyCommand((_) => {
                // Check Inputs
                string ErrorList = "";

                if (String.IsNullOrEmpty(UpdateCustomerName)) ErrorList += "Customer Name is Empty \n";
                if (String.IsNullOrEmpty(UpdateCustomerGender)) ErrorList += "Customer Gender is Empty \n";
                if (String.IsNullOrEmpty(UpdateCustomerAddress)) ErrorList += "Customer Address is Empty \n";
                if (String.IsNullOrEmpty(UpdateCustomerContactNo)) ErrorList += "Customer Contact No is Empty \n";
                if (String.IsNullOrEmpty(UpdateCustomerEmail)) ErrorList += "Customer Email is Empty \n";

                if (SelectedUpdateCustomerModel == null) ErrorList += "No Selected Info From Table \n";

                if (!ErrorList.Equals(""))
                {
                    MessageBox.Show(ErrorList, "Update Customer (Error Popup)");
                    return;
                }

                CustomerModel customerModel = new CustomerModel()
                {
                    ID = SelectedUpdateCustomerModel.ID,
                    Name = UpdateCustomerName,
                    Gender = UpdateCustomerGender,
                    Address = UpdateCustomerAddress,
                    ContactNo = UpdateCustomerContactNo,
                    Email = UpdateCustomerEmail
                };

                GetServiceCollection().GetDataService().GetCustomerRepository().UpdateCustomer(customerModel);
                MessageBox.Show("Customer Updated Successfully", "Customer Action");

                LoadCollections();
            });
        }

        private void InitializeCollections()
        {
            UpdateCustomerCollection = new ObservableCollection<CustomerModel>();
        }

        private void LoadUpdateCustomerCollection()
        {
            // Pull All Cars
            List<CustomerModel> customerModels = GetServiceCollection().GetDataService().GetCustomerRepository().GetFullCustomers();

            if (customerModels == null) return;

            // Clear Collection
            UpdateCustomerCollection.Clear();


            // Append All Cars
            foreach (CustomerModel customerModel in customerModels)
                UpdateCustomerCollection.Add(customerModel);
        }

        private void SelectedUpdateItemChanged()
        {
            if (CurrentUpdateCollectionIndex < 0) return;

            SelectedUpdateCustomerModel = UpdateCustomerCollection[CurrentUpdateCollectionIndex];

            if (SelectedUpdateCustomerModel == null) return;

            UpdateCustomerName = SelectedUpdateCustomerModel.Name;
            UpdateCustomerGender = SelectedUpdateCustomerModel.Gender;
            UpdateCustomerAddress = SelectedUpdateCustomerModel.Address;
            UpdateCustomerContactNo = SelectedUpdateCustomerModel.ContactNo;
            UpdateCustomerEmail = SelectedUpdateCustomerModel.Email;
        }
    }

    public partial class CustomersViewModel : ViewModelBase
    {
        private string _DeleteCustomerID;
        private string _DeleteCustomerName;

        public string DeleteCustomerID { get => _DeleteCustomerID; set => SetProperty(ref _DeleteCustomerID, value); }
        public string DeleteCustomerName { get => _DeleteCustomerName; set => SetProperty(ref _DeleteCustomerName, value); }

        public ObservableCollection<CustomerModel> DeleteCustomerCollection { get; set; }

        public ICommand? DeleteCustomerButton { get; set; }

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

        private CustomerModel SelectedDeleteCustomerModel;

        private void InitializeDeleteProperties()
        {
            DeleteCustomerCollection = new ObservableCollection<CustomerModel>();
            DeleteCustomerID = "?";
            DeleteCustomerName = "?";
            CurrentDeleteCollectionIndex = -1;
            LoadDeleteCustomerCollection();
        }

        private void InitializeDeleteButton()
        {
            DeleteCustomerButton = new ExecuteOnlyCommand((_) => {
                if (CurrentDeleteCollectionIndex < 0)
                {
                    MessageBox.Show("No Selected Item", "Delete Customer (Error Popup)");
                    return;
                }

                CustomerModel customerModel = new CustomerModel()
                {
                    ID = SelectedDeleteCustomerModel.ID
                };

                GetServiceCollection().GetDataService().GetCustomerRepository().DeleteCustomer(customerModel);
                MessageBox.Show("Customer Deleted Successfully", "Customer Action");

                LoadDeleteCustomerCollection();
            });
        }

        private void LoadDeleteCustomerCollection()
        {
            // Pull All Cars
            List<CustomerModel> customerModels = GetServiceCollection().GetDataService().GetCustomerRepository().GetFullCustomers();

            if (customerModels == null) return;

            // Clear Collection
            DeleteCustomerCollection.Clear();


            // Append All Cars
            foreach (CustomerModel customerModel in customerModels)
                DeleteCustomerCollection.Add(customerModel);
        }

        private void SelectedDeleteItemChanged()
        {
            if (CurrentDeleteCollectionIndex < 0) return;

            SelectedDeleteCustomerModel = DeleteCustomerCollection[CurrentDeleteCollectionIndex];

            if (SelectedDeleteCustomerModel == null) return;

            DeleteCustomerID = SelectedDeleteCustomerModel.ID.ToString();
            DeleteCustomerName = SelectedDeleteCustomerModel.Name;

        }
    }
}
