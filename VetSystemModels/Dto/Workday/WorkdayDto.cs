using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Workday
{
    public partial class WorkdayDto
    {
        public int WorkdayId { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = null!;

        public string DayOfWeek { get; set; } = null!;

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public TimeOnly LunchStart { get; set; }

        public TimeOnly LunchEnd { get; set; }

        public int SlotDuration { get; set; }
    }
}
