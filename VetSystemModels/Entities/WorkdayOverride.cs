using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Entities
{
    public partial class WorkdayOverride
    {
        public int WorkdayOverrideId { get; set; }

        public int EmployeeId { get; set; }

        public DateOnly WorkdayOverrideDate {  get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Employee Employee { get; set; } = null!;
    }
}
