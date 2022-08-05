﻿using CarRentalManagementSystem.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.Services
{
    public class NavigationService
    {
        private ViewModelBase? _CurrentViewModel;
        
        public ViewModelBase? CurrentViewModel { get => _CurrentViewModel; }

        public string? CurrentViewName { get; set; }

        public event Action? CurrentViewModelChanged;

        public void Navigate(ViewModelBase ViewModelToLoad)
        {

            // Call OnExitView of Last ViewModel
            if (_CurrentViewModel != null)
                _CurrentViewModel.OnExitView();

            // Set New ViewModel
            _CurrentViewModel = ViewModelToLoad;

            // Set New ViewModel Name
            CurrentViewName = ViewModelToLoad.ViewName;

            // Call OnEnterView of Loaded ViewModel
            _CurrentViewModel.OnEnterView();

            // Call ViewModel Change
            OnCurrentViewModelChanged();
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
