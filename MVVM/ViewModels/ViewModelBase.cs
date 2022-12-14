using CarRentalManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.MVVM.ViewModels
{
    public abstract partial class ViewModelBase : INotifyPropertyChanged
    {
        private ServiceCollection _ServiceCollection;
        public abstract string ViewName { get; }

        protected ServiceCollection GetServiceCollection()
        {
            return _ServiceCollection;
        }

        public virtual void OnEnterView()
        {
            _ServiceCollection.GetLogService().WriteLog(LogService.LogInfo, ViewName, "On Enter View is Called");
        }
        public virtual void OnExitView()
        {
            _ServiceCollection.GetLogService().WriteLog(LogService.LogInfo, ViewName, "On Exit View is Called");
        }

        private void SetupUI()
        {
            InitializeButtons();
            InitializeProperties();
        }

        protected virtual void InitializeButtons()
        {
            _ServiceCollection.GetLogService().WriteLog(LogService.LogInfo, ViewName, "Buttons are initialized for this ViewModel");
        }
        protected virtual void InitializeProperties()
        {
            _ServiceCollection.GetLogService().WriteLog(LogService.LogInfo, ViewName, "Properties are initialized for this ViewModel");
        }

        public ViewModelBase(ServiceCollection serviceCollection)
        {
            serviceCollection.GetLogService().WriteLog(LogService.LogInfo, ViewName + " [ViewModel]", "ViewModel Constructor Called");

            // Initialize Services
            _ServiceCollection = serviceCollection;

            // Initalize UI
            SetupUI();
        }
    }

    public abstract partial class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
