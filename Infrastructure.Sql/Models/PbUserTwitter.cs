using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_user_twitter")]
public partial class PbUserTwitter
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("twitter_name")]
    [StringLength(100)]
    public string TwitterName { get; set; } = null!;

    [Column("key")]
    [StringLength(100)]
    public string Key { get; set; } = null!;

    [Column("secret")]
    [StringLength(100)]
    public string Secret { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("PbUserTwitter")]
    public virtual PbUser User { get; set; } = null!;
}
