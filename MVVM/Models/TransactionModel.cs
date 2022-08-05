using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.MVVM.Models
{
    public class TransactionModel : ModelBase
    {
        public string? Label { get; set; }
        public CarModel? Car { get; set; }
        public CustomerModel? Customer { get; set; }
        public DateOnly? RentDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public double? TotalCost { get; set; }
    }
}
