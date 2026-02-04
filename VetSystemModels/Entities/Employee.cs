using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.Entities;

[Index("UserId", Name = "UQ__Employee__1788CC4D2F2F7F0E", IsUnique = true)]
public partial class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string MiddleName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    public int SpecializationId { get; set; }

    public int? UserId { get; set; }
    public bool IsDeleted { get; set; } = false;

    [InverseProperty("Employee")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    [ForeignKey("SpecializationId")]
    [InverseProperty("Employees")]
    public virtual Specialization Specialization { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Employee")]
    public virtual User? User { get; set; }
}
