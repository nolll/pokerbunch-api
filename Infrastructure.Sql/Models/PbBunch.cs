using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_bunch")]
public partial class PbBunch
{
    [Key]
    [Column("bunch_id")]
    public int BunchId { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("display_name")]
    [StringLength(50)]
    public string? DisplayName { get; set; }

    [Column("description")]
    [StringLength(50)]
    public string? Description { get; set; }

    [Column("timezone")]
    [StringLength(50)]
    public string Timezone { get; set; } = null!;

    [Column("default_buyin")]
    public int DefaultBuyin { get; set; }

    [Column("currency")]
    [StringLength(3)]
    public string Currency { get; set; } = null!;

    [Column("currency_layout")]
    [StringLength(20)]
    public string CurrencyLayout { get; set; } = null!;

    [Column("cashgames_enabled")]
    public bool CashgamesEnabled { get; set; }

    [Column("tournaments_enabled")]
    public bool TournamentsEnabled { get; set; }

    [Column("videos_enabled")]
    public bool VideosEnabled { get; set; }

    [Column("house_rules")]
    public string? HouseRules { get; set; }

    [InverseProperty("Bunch")]
    public virtual ICollection<PbCashgame> PbCashgame { get; set; } = new List<PbCashgame>();

    [InverseProperty("Bunch")]
    public virtual ICollection<PbEvent> PbEvent { get; set; } = new List<PbEvent>();

    [InverseProperty("Bunch")]
    public virtual ICollection<PbJoinRequest> PbJoinRequest { get; set; } = new List<PbJoinRequest>();

    [InverseProperty("Bunch")]
    public virtual ICollection<PbLocation> PbLocation { get; set; } = new List<PbLocation>();

    [InverseProperty("Bunch")]
    public virtual ICollection<PbPlayer> PbPlayer { get; set; } = new List<PbPlayer>();

    [InverseProperty("Bunch")]
    public virtual ICollection<PbTournament> PbTournament { get; set; } = new List<PbTournament>();

    [InverseProperty("Bunch")]
    public virtual ICollection<PbVideo> PbVideo { get; set; } = new List<PbVideo>();
}
