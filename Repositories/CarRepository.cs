using CarRentalManagementSystem.MVVM.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
                "INSERT INTO Car VALUES (null, \"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", null, null, null, null, null);",
                car.Type,
                car.Brand,
                car.Model,
                car.Description,
                car.PlateNo
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
                "UPDATE Car SET CarType=\"{0}\", CarBrand=\"{1}\", CarModel=\"{2}\", CarDesc=\"{3}\", CarPlateNo=\"{4}\" WHERE CarID=\"{5}\"",
                car.Type,
                car.Brand,
                car.Model,
                car.Description,
                car.PlateNo,
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
                "DELETE FROM Car WHERE CarID=\"{0}\"",
                car.ID
                );

            MySqlCommand mySqlCommand = new MySqlCommand(QueryStr, mySql);
            mySqlCommand.CommandType = System.Data.CommandType.Text;

            mySqlCommand.ExecuteNonQuery();
            mySql.Close();
        }

        public List<CarModel> GetBasicCar()
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

                carModels.Add(carModel);
            }

            mySqlDataReader.Close();
            mySql.Close();

            return carModels;
        }

        public List<CarModel> GetFullCar()
        {
            return null;
        }

        public void UpdateCarRentStatus()
        {
            //
        }
    }
}
