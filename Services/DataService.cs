using CarRentalManagementSystem.MVVM.Models;
using CarRentalManagementSystem.Repositories;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.Services
{
    public class DataService
    {
        private CarRepository? _CarRepository;
        private CustomerRepository? _CustomerRepository;
        private TransactionRepository? _TransactionRepository;

        public CarRepository GetCarRepository()
        {
            if (_CarRepository == null)
                _CarRepository = new CarRepository();
            return _CarRepository;
        }

        public CustomerRepository GetCustomerRepository()
        {
            if (_CustomerRepository == null)
                _CustomerRepository = new CustomerRepository();
            return _CustomerRepository;
        }

        public TransactionRepository GetTransactionRepository()
        {
            if (_TransactionRepository == null)
                _TransactionRepository = new TransactionRepository();
            return _TransactionRepository;
        }
    }
}
