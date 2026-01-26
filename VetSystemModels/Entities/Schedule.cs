using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.Entities;

public partial class Schedule
{
    [Key]
    public int ScheduleId { get; set; }

    public DateOnly ScheduleDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int EmployeeId { get; set; }

    public bool IsAvailable { get; set; }

    [InverseProperty("Schedule")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [ForeignKey("EmployeeId")]
    [InverseProperty("Schedules")]
    public virtual Employee Employee { get; set; } = null!;
}
