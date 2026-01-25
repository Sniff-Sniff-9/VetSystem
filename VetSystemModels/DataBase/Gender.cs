using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.DataBase;

[Index("GenderName", Name = "UQ__Genders__F7C17715E5B7D3F8", IsUnique = true)]
public partial class Gender
{
    [Key]
    public int GenderId { get; set; }

    [StringLength(10)]
    public string GenderName { get; set; } = null!;

    [InverseProperty("Gender")]
    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
