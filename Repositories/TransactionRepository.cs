using CarRentalManagementSystem.MVVM.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.Repositories
{
    public class TransactionRepository : RepositoryBase
    {
        public void AddTransaction(TransactionModel transaction)
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            string QueryStr = String.Format(
                "INSERT INTO Transaction VALUES (null, \"{0}\", {1}, {2}, \"{3}\", \"{4}\", {5});",
                transaction.Label,
                transaction.Car.ID,
                transaction.Customer.ID,
                transaction.RentDate.ToString(),
                transaction.ReturnDate.ToString(),
                transaction.TotalCost
                );

            MySqlCommand mySqlCommand = new MySqlCommand(QueryStr, mySql);
            mySqlCommand.CommandType = System.Data.CommandType.Text;

            mySqlCommand.ExecuteNonQuery();
            mySql.Close();
        }

        public List<TransactionModel> GetFullTransactions()
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            List<TransactionModel> transactionModels = new List<TransactionModel>();

            MySqlCommand mySqlCommand = new MySqlCommand(
                "SELECT * FROM Transaction LEFT JOIN Car ON Transaction.TransacCarID = Car.CarID LEFT JOIN Customer ON Transaction.TransacCustID = Customer.CustID",
                mySql
                );

            mySqlCommand.CommandType = System.Data.CommandType.Text;
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            while (mySqlDataReader.Read())
            {
                TransactionModel transactionModel = new TransactionModel();

                transactionModel.ID = mySqlDataReader.GetInt32("TransacID");
                transactionModel.Label = mySqlDataReader.GetString("TransacLabel");

                CarModel carModel = new CarModel();
                carModel.ID = mySqlDataReader.GetInt32("CarID");
                carModel.Type = mySqlDataReader.GetString("CarType");
                carModel.Model = mySqlDataReader.GetString("CarModel");
                carModel.Brand = mySqlDataReader.GetString("CarBrand");

                CustomerModel customerModel = new CustomerModel();
                customerModel.ID = mySqlDataReader.GetInt32("CustID");
                customerModel.Name = mySqlDataReader.GetString("CustName");

                transactionModel.Car = carModel;
                transactionModel.Customer = customerModel;

                transactionModel.RentDate = mySqlDataReader.GetDateTime("TransacRentDate");
                transactionModel.ReturnDate = mySqlDataReader.GetDateTime("TransacReturnDate");
                transactionModel.TotalCost = mySqlDataReader.GetDouble("TransacTotalCost");

                transactionModels.Add(transactionModel);
            }

            mySqlDataReader.Close();
            mySql.Close();

            return transactionModels;
        }
    }
}
