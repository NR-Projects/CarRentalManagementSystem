using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.MVVM.Models
{
    public abstract class ModelBase
    {
        public int ID { get; set; }
        public string? Action { get; set; }
        public int? Status { get; set; }
    }
}
