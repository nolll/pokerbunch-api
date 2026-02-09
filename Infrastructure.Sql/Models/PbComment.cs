using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_comment")]
public partial class PbComment
{
    [Key]
    [Column("comment_id")]
    public int CommentId { get; set; }

    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("date", TypeName = "timestamp without time zone")]
    public DateTime Date { get; set; }

    [Column("comment_text")]
    [StringLength(1024)]
    public string CommentText { get; set; } = null!;

    [ForeignKey("PlayerId")]
    [InverseProperty("PbComment")]
    public virtual PbPlayer Player { get; set; } = null!;

    [ForeignKey("CommentId")]
    [InverseProperty("Comment")]
    public virtual ICollection<PbCashgame> Cashgame { get; set; } = new List<PbCashgame>();

    [ForeignKey("CommentId")]
    [InverseProperty("Comment")]
    public virtual ICollection<PbTournament> Tournament { get; set; } = new List<PbTournament>();
}
