using CarRentalManagementSystem.MVVM.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.Repositories
{
    public class CarRepository : RepositoryBase
    {
        public void AddCar(CarModel car)
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            string QueryStr = String.Format(
                "INSERT INTO Car VALUES (null, \"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"Available\", null, null, \"{5}\", null);",
                car.Type,
                car.Brand,
                car.Model,
                car.Description,
                car.PlateNo,
                car.PricePerDay
                );

            MySqlCommand mySqlCommand = new MySqlCommand(QueryStr, mySql);
            mySqlCommand.CommandType = System.Data.CommandType.Text;

            mySqlCommand.ExecuteNonQuery();
            mySql.Close();
        }

        public void UpdateCar(CarModel car)
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            string QueryStr = String.Format(
                "UPDATE Car SET CarType=\"{0}\", CarBrand=\"{1}\", CarModel=\"{2}\", CarDesc=\"{3}\", CarPlateNo=\"{4}\", CarPricePerDay={5} WHERE CarID={6}",
                car.Type,
                car.Brand,
                car.Model,
                car.Description,
                car.PlateNo,
                car.PricePerDay,
                car.ID
                );

            MySqlCommand mySqlCommand = new MySqlCommand(QueryStr, mySql);
            mySqlCommand.CommandType = System.Data.CommandType.Text;

            mySqlCommand.ExecuteNonQuery();
            mySql.Close();
        }

        public void DeleteCar(CarModel car)
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            string QueryStr = String.Format(
                "DELETE FROM Car WHERE CarID={0}",
                car.ID
                );

            MySqlCommand mySqlCommand = new MySqlCommand(QueryStr, mySql);
            mySqlCommand.CommandType = System.Data.CommandType.Text;

            mySqlCommand.ExecuteNonQuery();
            mySql.Close();
        }

        public List<CarModel> GetBasicCars()
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            List<CarModel> carModels = new List<CarModel>();

            MySqlCommand mySqlCommand = new MySqlCommand(
                "SELECT * FROM Car",
                mySql
                );

            mySqlCommand.CommandType = System.Data.CommandType.Text;
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            while (mySqlDataReader.Read())
            {
                CarModel carModel = new CarModel();
                carModel.ID = mySqlDataReader.GetInt32("CarID");
                carModel.Type = mySqlDataReader.GetString("CarType");
                carModel.Model = mySqlDataReader.GetString("CarModel");
                carModel.Brand = mySqlDataReader.GetString("CarBrand");
                carModel.Description = mySqlDataReader.GetString("CarDesc");
                carModel.PlateNo = mySqlDataReader.GetString("CarPlateNo");
                carModel.PricePerDay = mySqlDataReader.GetDouble("CarPricePerDay");
                carModel.CarStatus = mySqlDataReader.GetString("CarStatus");

                carModels.Add(carModel);
            }

            mySqlDataReader.Close();
            mySql.Close();

            return carModels;
        }

        public List<CarModel> GetFullCars()
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            List<CarModel> carModels = new List<CarModel>();

            MySqlCommand mySqlCommand = new MySqlCommand(
                "SELECT * FROM Car LEFT JOIN Customer ON Car.CarRentCustomer = Customer.CustID",
                mySql
                );

            mySqlCommand.CommandType = System.Data.CommandType.Text;
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            while (mySqlDataReader.Read())
            {
                CarModel carModel = new CarModel();
                carModel.ID = mySqlDataReader.GetInt32("CarID");
                carModel.Type = mySqlDataReader.GetString("CarType");
                carModel.Model = mySqlDataReader.GetString("CarModel");
                carModel.Brand = mySqlDataReader.GetString("CarBrand");
                carModel.Description = mySqlDataReader.GetString("CarDesc");
                carModel.PlateNo = mySqlDataReader.GetString("CarPlateNo");
                carModel.PricePerDay = mySqlDataReader.GetDouble("CarPricePerDay");
                carModel.CarStatus = mySqlDataReader.GetString("CarStatus");

                if (Convert.IsDBNull(mySqlDataReader["CarRentDate"]))
                    carModel.RentDate = null;
                else
                    carModel.RentDate = mySqlDataReader.GetDateTime("CarRentDate");

                if (Convert.IsDBNull(mySqlDataReader["CarReturnDate"]))
                    carModel.ReturnDate = null;
                else
                    carModel.ReturnDate = mySqlDataReader.GetDateTime("CarReturnDate");

                if (Convert.IsDBNull(mySqlDataReader["CarRentCustomer"]))
                    carModel.RentCustomer = null;
                else {
                    CustomerModel customerModel = new CustomerModel()
                    {
                        ID = mySqlDataReader.GetInt32("CustID"),
                        Name = mySqlDataReader.GetString("CustName"),
                        Gender = mySqlDataReader.GetString("CustGender"),
                        Address = mySqlDataReader.GetString("CustAddress"),
                        ContactNo = mySqlDataReader.GetString("CustContactNo"),
                        Email = mySqlDataReader.GetString("CustEmail")
                    };

                    carModel.RentCustomer = customerModel;
                }

                carModels.Add(carModel);
            }

            mySqlDataReader.Close();
            mySql.Close();

            return carModels;
        }

        public void UpdateCarRentStatus(CarModel car)
        {
            MySqlConnection mySql = new MySqlConnection(DatabaseConnectionString);
            mySql.Open();

            string QueryStr = "";

            if (car.CarStatus.Equals("Available"))
            {
                QueryStr = String.Format(
                    "UPDATE Car SET CarStatus=\"Available\", CarRentDate=null, CarReturnDate=null, CarRentCustomer=null WHERE CarID={0}",
                    car.ID
                    );
            }
            else if (car.CarStatus.Equals("Rented"))
            {
                if (car.RentDate == null) return;
                if (car.ReturnDate == null) return;

                DateTime RentDate = (DateTime)car.RentDate;
                DateTime ReturnDate = (DateTime)car.ReturnDate;

                QueryStr = String.Format(
                    "UPDATE Car SET CarStatus=\"Rented\", CarRentDate=\"{0}\", CarReturnDate=\"{1}\", CarRentCustomer={2} WHERE CarID={3}",
                    RentDate.ToString("yyyy-MM-dd"),
                    ReturnDate.ToString("yyyy-MM-dd"),
                    car.RentCustomer.ID,
                    car.ID
                    );
            }

            MySqlCommand mySqlCommand = new MySqlCommand(QueryStr, mySql);
            mySqlCommand.CommandType = System.Data.CommandType.Text;

            mySqlCommand.ExecuteNonQuery();
            mySql.Close();
        }
    }
}
