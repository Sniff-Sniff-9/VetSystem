using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Entities
{
    public partial class EmployeeService
    {
        public int EmployeeServiceId { get; set; }

        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }

        public Employee Employee { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}
