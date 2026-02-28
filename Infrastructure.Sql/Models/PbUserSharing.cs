using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[PrimaryKey("UserId", "ServiceName")]
[Table("pb_user_sharing")]
public partial class PbUserSharing
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Key]
    [Column("service_name")]
    [StringLength(50)]
    public string ServiceName { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("PbUserSharing")]
    public virtual PbUser User { get; set; } = null!;
}
