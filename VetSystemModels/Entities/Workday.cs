using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Entities
{
    public partial class Workday
    {
        public int WorkdayId { get; set; }

        public int EmployeeId { get; set; }

        public DateOnly WorkDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public TimeOnly LunchStart { get; set; }

        public TimeOnly LunchEnd { get; set; }

        public int SlotDuration { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Employee Employee { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
