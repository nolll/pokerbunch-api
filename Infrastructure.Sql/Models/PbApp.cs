using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_app")]
public partial class PbApp
{
    [Key]
    [Column("app_id")]
    public int AppId { get; set; }

    [Column("app_key")]
    [StringLength(50)]
    public string AppKey { get; set; } = null!;

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PbApp")]
    public virtual PbUser User { get; set; } = null!;
}
