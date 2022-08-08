using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.MVVM.Models
{
    public class CarModel : ModelBase
    {
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? PlateNo { get; set; }

        // Renting
        public double PricePerDay { get; set; }
        public string CarStatus { get; set; }
        public DateTime? RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public CustomerModel? RentCustomer { get; set; }
    }
}
