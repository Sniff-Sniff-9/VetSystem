using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VetSystemModels.Entities;

[Index("GenderName", Name = "UQ__Genders__F7C17715E5B7D3F8", IsUnique = true)]
public partial class Gender
{
    [Key]
    public int GenderId { get; set; }

    [StringLength(10)]
    public string GenderName { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("Gender")]
    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
