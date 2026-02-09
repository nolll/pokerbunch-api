using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

public partial class PokerBunchDbContext : DbContext
{
    public PokerBunchDbContext(DbContextOptions<PokerBunchDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PbApp> PbApp { get; set; }

    public virtual DbSet<PbBunch> PbBunch { get; set; }

    public virtual DbSet<PbCashgame> PbCashgame { get; set; }

    public virtual DbSet<PbCashgameCheckpoint> PbCashgameCheckpoint { get; set; }

    public virtual DbSet<PbComment> PbComment { get; set; }

    public virtual DbSet<PbEvent> PbEvent { get; set; }

    public virtual DbSet<PbJoinRequest> PbJoinRequest { get; set; }

    public virtual DbSet<PbLocation> PbLocation { get; set; }

    public virtual DbSet<PbPlayer> PbPlayer { get; set; }

    public virtual DbSet<PbRole> PbRole { get; set; }

    public virtual DbSet<PbTournament> PbTournament { get; set; }

    public virtual DbSet<PbTournamentPayout> PbTournamentPayout { get; set; }

    public virtual DbSet<PbTournamentResult> PbTournamentResult { get; set; }

    public virtual DbSet<PbUser> PbUser { get; set; }

    public virtual DbSet<PbUserSharing> PbUserSharing { get; set; }

    public virtual DbSet<PbUserTwitter> PbUserTwitter { get; set; }

    public virtual DbSet<PbVideo> PbVideo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PbApp>(entity =>
        {
            entity.HasKey(e => e.AppId).HasName("pb_app_pkey");

            entity.HasOne(d => d.User).WithMany(p => p.PbApp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<PbBunch>(entity =>
        {
            entity.HasKey(e => e.BunchId).HasName("pb_bunch_pkey");
        });

        modelBuilder.Entity<PbCashgame>(entity =>
        {
            entity.HasKey(e => e.CashgameId).HasName("pb_cashgame_pkey");

            entity.Property(e => e.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Bunch).WithMany(p => p.PbCashgame)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bunch");

            entity.HasOne(d => d.Location).WithMany(p => p.PbCashgame)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_location");

            entity.HasMany(d => d.Comment).WithMany(p => p.Cashgame)
                .UsingEntity<Dictionary<string, object>>(
                    "PbCashgameComment",
                    r => r.HasOne<PbComment>().WithMany()
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_comment"),
                    l => l.HasOne<PbCashgame>().WithMany()
                        .HasForeignKey("CashgameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_cashgame"),
                    j =>
                    {
                        j.HasKey("CashgameId", "CommentId").HasName("pb_cashgame_comment_pkey");
                        j.ToTable("pb_cashgame_comment");
                        j.IndexerProperty<int>("CashgameId").HasColumnName("cashgame_id");
                        j.IndexerProperty<int>("CommentId").HasColumnName("comment_id");
                    });
        });

        modelBuilder.Entity<PbCashgameCheckpoint>(entity =>
        {
            entity.HasKey(e => e.CheckpointId).HasName("pb_cashgame_checkpoint_pkey");

            entity.HasOne(d => d.Cashgame).WithMany(p => p.PbCashgameCheckpoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cashgame");

            entity.HasOne(d => d.Player).WithMany(p => p.PbCashgameCheckpoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_player");
        });

        modelBuilder.Entity<PbComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("pb_comment_pkey");

            entity.HasOne(d => d.Player).WithMany(p => p.PbComment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_player");
        });

        modelBuilder.Entity<PbEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("pb_event_pkey");

            entity.HasOne(d => d.Bunch).WithMany(p => p.PbEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bunch");

            entity.HasMany(d => d.Cashgame).WithMany(p => p.Event)
                .UsingEntity<Dictionary<string, object>>(
                    "PbEventCashgame",
                    r => r.HasOne<PbCashgame>().WithMany()
                        .HasForeignKey("CashgameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_cashgame"),
                    l => l.HasOne<PbEvent>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_event"),
                    j =>
                    {
                        j.HasKey("EventId", "CashgameId").HasName("pb_event_cashgame_pkey");
                        j.ToTable("pb_event_cashgame");
                        j.IndexerProperty<int>("EventId").HasColumnName("event_id");
                        j.IndexerProperty<int>("CashgameId").HasColumnName("cashgame_id");
                    });
        });

        modelBuilder.Entity<PbJoinRequest>(entity =>
        {
            entity.HasKey(e => e.JoinRequestId).HasName("pb_join_request_pkey");

            entity.HasOne(d => d.Bunch).WithMany(p => p.PbJoinRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bunch");

            entity.HasOne(d => d.User).WithMany(p => p.PbJoinRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<PbLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("pb_location_pkey");

            entity.HasOne(d => d.Bunch).WithMany(p => p.PbLocation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bunch");
        });

        modelBuilder.Entity<PbPlayer>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("pb_player_pkey");

            entity.HasOne(d => d.Bunch).WithMany(p => p.PbPlayer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bunch");
        });

        modelBuilder.Entity<PbRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("pb_role_pkey");
        });

        modelBuilder.Entity<PbTournament>(entity =>
        {
            entity.HasKey(e => e.TournamentId).HasName("pb_tournament_pkey");

            entity.HasOne(d => d.Bunch).WithMany(p => p.PbTournament)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bunch");

            entity.HasMany(d => d.Comment).WithMany(p => p.Tournament)
                .UsingEntity<Dictionary<string, object>>(
                    "PbTournamentComment",
                    r => r.HasOne<PbComment>().WithMany()
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_comment"),
                    l => l.HasOne<PbTournament>().WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_tournament"),
                    j =>
                    {
                        j.HasKey("TournamentId", "CommentId").HasName("pb_tournament_comment_pkey");
                        j.ToTable("pb_tournament_comment");
                        j.IndexerProperty<int>("TournamentId").HasColumnName("tournament_id");
                        j.IndexerProperty<int>("CommentId").HasColumnName("comment_id");
                    });
        });

        modelBuilder.Entity<PbTournamentPayout>(entity =>
        {
            entity.HasKey(e => new { e.TournamentId, e.Position }).HasName("pb_tournament_payout_pkey");

            entity.HasOne(d => d.Tournament).WithMany(p => p.PbTournamentPayout)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tournament");
        });

        modelBuilder.Entity<PbTournamentResult>(entity =>
        {
            entity.HasKey(e => new { e.TournamentId, e.PlayerId }).HasName("pb_tournament_result_pkey");

            entity.HasOne(d => d.Player).WithMany(p => p.PbTournamentResult)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_player");

            entity.HasOne(d => d.Tournament).WithMany(p => p.PbTournamentResult)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tournament");
        });

        modelBuilder.Entity<PbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("pb_user_pkey");

            entity.HasOne(d => d.Role).WithMany(p => p.PbUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role");
        });

        modelBuilder.Entity<PbUserSharing>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ServiceName }).HasName("pb_user_sharing_pkey");

            entity.HasOne(d => d.User).WithMany(p => p.PbUserSharing)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<PbUserTwitter>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("pb_user_twitter_pkey");

            entity.Property(e => e.UserId).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithOne(p => p.PbUserTwitter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<PbVideo>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("pb_video_pkey");

            entity.HasOne(d => d.Bunch).WithMany(p => p.PbVideo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bunch");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
