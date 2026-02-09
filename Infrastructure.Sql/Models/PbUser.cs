using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_user")]
public partial class PbUser
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("token")]
    [StringLength(50)]
    public string? Token { get; set; }

    [Column("user_name")]
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [Column("password")]
    [StringLength(50)]
    public string? Password { get; set; }

    [Column("salt")]
    [StringLength(50)]
    public string? Salt { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("real_name")]
    [StringLength(50)]
    public string? RealName { get; set; }

    [Column("display_name")]
    [StringLength(50)]
    public string DisplayName { get; set; } = null!;

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<PbApp> PbApp { get; set; } = new List<PbApp>();

    [InverseProperty("User")]
    public virtual ICollection<PbJoinRequest> PbJoinRequest { get; set; } = new List<PbJoinRequest>();

    [InverseProperty("User")]
    public virtual ICollection<PbUserSharing> PbUserSharing { get; set; } = new List<PbUserSharing>();

    [InverseProperty("User")]
    public virtual PbUserTwitter? PbUserTwitter { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("PbUser")]
    public virtual PbRole Role { get; set; } = null!;
}
