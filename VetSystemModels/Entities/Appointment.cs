using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.Entities;

public partial class Appointment
{
    [Key]
    public int AppointmentId { get; set; }

    public int EmployeeId { get; set; }

    public int PetId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalPriceAtMoment { get; set; }

    public int AppointmentStatusId { get; set; }

    public DateOnly AppointmentDate {  get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool IsDeleted { get; set; } = false;

    [ForeignKey("AppointmentStatusId")]
    [InverseProperty("Appointments")]
    public virtual AppointmentStatus AppointmentStatus { get; set; } = null!;

    [ForeignKey("PetId")]
    [InverseProperty("Appointments")]
    public virtual Pet Pet { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
}
