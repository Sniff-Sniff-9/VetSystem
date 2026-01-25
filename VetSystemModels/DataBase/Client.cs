using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.DataBase;

[Index("UserId", Name = "UQ__Clients__1788CC4DB4DF7A2C", IsUnique = true)]
public partial class Client
{
    [Key]
    public int ClientId { get; set; }

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string MiddleName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    public int? UserId { get; set; }

    [InverseProperty("Client")]
    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();

    [ForeignKey("UserId")]
    [InverseProperty("Client")]
    public virtual User? User { get; set; }
}
