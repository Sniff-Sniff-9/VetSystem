using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.Entities;

[Index("ServiceName", Name = "UQ__Services__A42B5F997BAADBF0", IsUnique = true)]
public partial class Service
{
    [Key]
    public int ServiceId { get; set; }

    [StringLength(50)]
    public string ServiceName { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<EmployeeService> EmployeeServices { get; set; } = new List<EmployeeService>();

    public virtual ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
}
