using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_tournament")]
public partial class PbTournament
{
    [Key]
    [Column("tournament_id")]
    public int TournamentId { get; set; }

    [Column("bunch_id")]
    public int BunchId { get; set; }

    [Column("buyin")]
    public int Buyin { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("duration")]
    public int Duration { get; set; }

    [Column("location")]
    [StringLength(50)]
    public string Location { get; set; } = null!;

    [Column("timestamp", TypeName = "timestamp without time zone")]
    public DateTime Timestamp { get; set; }

    [Column("published")]
    public bool Published { get; set; }

    [ForeignKey("BunchId")]
    [InverseProperty("PbTournament")]
    public virtual PbBunch Bunch { get; set; } = null!;

    [InverseProperty("Tournament")]
    public virtual ICollection<PbTournamentPayout> PbTournamentPayout { get; set; } = new List<PbTournamentPayout>();

    [InverseProperty("Tournament")]
    public virtual ICollection<PbTournamentResult> PbTournamentResult { get; set; } = new List<PbTournamentResult>();

    [ForeignKey("TournamentId")]
    [InverseProperty("Tournament")]
    public virtual ICollection<PbComment> Comment { get; set; } = new List<PbComment>();
}
