using CarRentalManagementSystem.MVVM.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.Repositories
{
    public class CustomerRepository : RepositoryBase
    {
        public void AddCustomer(CustomerModel customer)
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            string QueryStr = String.Format("INSERT INTO Customer VALUES (null, \"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\")",
                customer.Name,
                customer.Gender,
                customer.Address,
                customer.ContactNo,
                customer.Email
                );

            MySqlCommand mySqlCommand = new MySqlCommand(QueryStr, mySql);
            mySqlCommand.CommandType = System.Data.CommandType.Text;

            mySqlCommand.ExecuteNonQuery();
            mySql.Close();
        }

        public List<CustomerModel> GetFullCustomers()
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            List<CustomerModel> customerModels = new List<CustomerModel>();

            MySqlCommand mySqlCommand = new MySqlCommand(
                "SELECT * FROM Customer",
                mySql
                );

            mySqlCommand.CommandType = System.Data.CommandType.Text;
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            while (mySqlDataReader.Read())
            {
                CustomerModel customerModel = new CustomerModel();

                customerModel.ID = mySqlDataReader.GetInt32("CustID");
                customerModel.Name = mySqlDataReader.GetString("CustName");
                customerModel.Gender = mySqlDataReader.GetString("CustGender");
                customerModel.Address = mySqlDataReader.GetString("CustAddress");
                customerModel.ContactNo = mySqlDataReader.GetString("CustAddress");
                customerModel.Email = mySqlDataReader.GetString("CustEmail");

                customerModels.Add(customerModel);
            }

            mySqlDataReader.Close();
            mySql.Close();

            return customerModels;
        }

        public void UpdateCustomer(CustomerModel customer)
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            string QueryStr = String.Format(
                "UPDATE Customer SET CustName=\"{0}\", CustGender=\"{1}\", CustAddress=\"{2}\", CustContactNo=\"{3}\", CustEmail=\"{4}\" WHERE CustID={5}",
                customer.Name,
                customer.Gender,
                customer.Address,
                customer.ContactNo,
                customer.Email,
                customer.ID
                );

            MySqlCommand mySqlCommand = new MySqlCommand(QueryStr, mySql);
            mySqlCommand.CommandType = System.Data.CommandType.Text;

            mySqlCommand.ExecuteNonQuery();
            mySql.Close();
        }

        public void DeleteCustomer(CustomerModel customer)
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            string QueryStr = String.Format(
                "DELETE FROM Customer WHERE CustID={0}",
                customer.ID
                );

            MySqlCommand mySqlCommand = new MySqlCommand(QueryStr, mySql);
            mySqlCommand.CommandType = System.Data.CommandType.Text;

            mySqlCommand.ExecuteNonQuery();
            mySql.Close();
        }
    }
}
