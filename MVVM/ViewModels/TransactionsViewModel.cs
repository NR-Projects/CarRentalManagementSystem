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
    public class TransactionsViewModel : ViewModelBase
    {
        public override string ViewName => "Transactions View";

        private string _LabelFilterStr;
        public string LabelFilterStr { get => _LabelFilterStr; set { SetProperty(ref _LabelFilterStr, value); OnLabelFilterChanged(); } }

        private int _CurrentTransactionSelectedIndex;
        public int CurrentTransactionSelectedIndex { get; set; }

        public ObservableCollection<TransactionModel> TransactionCollection { get; set; }

        public ICommand? NavigateBack { get; set; }

        public ICommand? RefreshCollection { get; set; }


        public TransactionsViewModel(ServiceCollection serviceCollection) : base(serviceCollection)
        {
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

        protected override void InitializeProperties()
        {
            base.InitializeProperties();

            TransactionCollection = new ObservableCollection<TransactionModel>();

            LoadCollection();
        }

        private void LoadCollection()
        {
            List<TransactionModel> transactionModels = GetServiceCollection().GetDataService().GetTransactionRepository().GetFullTransactions();

            if (transactionModels == null) return;

            // Clear Collection
            TransactionCollection.Clear();

            // Append All Cars
            foreach (TransactionModel transactionModel in transactionModels)
                TransactionCollection.Add(transactionModel);
        }

        private void OnLabelFilterChanged()
        {
            if (LabelFilterStr.Equals(""))
            {
                LoadCollection();
                return;
            }

            List<TransactionModel> transactionModels = TransactionCollection.ToList();

            if (transactionModels == null) return;

            // Clear Collection
            TransactionCollection.Clear();

            foreach (TransactionModel transactionModel in transactionModels)
            {
                if (transactionModel.Label.Contains(LabelFilterStr))
                    TransactionCollection.Add(transactionModel);
            }
        }
    }
}
