﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.Services
{
    public class ServiceCollection
    {
        private NavigationService? _NavigationService;
        private DataService? _DataService;

        public ServiceCollection() { }

        public NavigationService GetNavService()
        {
            if (_NavigationService == null)
                _NavigationService = new NavigationService();
            return _NavigationService;
        }

        public DataService GetDataService()
        {
            if(_DataService == null)
                _DataService = new DataService();
            return _DataService;
        }
    }
}
