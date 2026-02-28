using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_join_request")]
public partial class PbJoinRequest
{
    [Key]
    [Column("join_request_id")]
    public int JoinRequestId { get; set; }

    [Column("bunch_id")]
    public int BunchId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("BunchId")]
    [InverseProperty("PbJoinRequest")]
    public virtual PbBunch Bunch { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("PbJoinRequest")]
    public virtual PbUser User { get; set; } = null!;
}
