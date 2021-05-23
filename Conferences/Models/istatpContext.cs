using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Conferences
{
    public partial class istatpContext : DbContext
    {
        public istatpContext()
        {
        }

        public istatpContext(DbContextOptions<istatpContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Conference> Conferences { get; set; }
        public virtual DbSet<ConferencesAndParticipant> ConferencesAndParticipants { get; set; }
        public virtual DbSet<Form> Forms { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Organizer> Organizers { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<Work> Works { get; set; }
        public virtual DbSet<WorksAndParticipant> WorksAndParticipants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= DESKTOP-MSJIALE; Database=istatp; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation",  "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Conference>(entity =>
            {
                entity.HasIndex(e => e.ConferenceId, "DF_Conferences_ConferenceID_Unique")
                    .IsUnique();

                entity.Property(e => e.ConferenceId).HasColumnName("ConferenceID");

                entity.Property(e => e.Aim).IsRequired();

                entity.Property(e => e.DateAndTime).HasColumnType("datetime");

                entity.Property(e => e.FormId).HasColumnName("FormID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RequirementsForParticipants).IsRequired();

                entity.Property(e => e.RequirementsForWorks).IsRequired();

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.Topic).IsRequired();

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.Conferences)
                    .HasForeignKey(d => d.FormId)
                    .HasConstraintName("FK_Conferences_Forms");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Conferences)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Conferences_Locations");

                entity.HasOne(d => d.Organizer)
                    .WithMany(p => p.Conferences)
                    .HasForeignKey(d => d.OrganizerId)
                    .HasConstraintName("FK_Conferences_Organizers");
            });

            modelBuilder.Entity<ConferencesAndParticipant>(entity =>
            {
                entity.HasKey(e => e.ConferenceAndParticipantId)
                    .HasName("PK_ConferencesAndParticipants_ConferenceAndParticipantID");

                entity.HasIndex(e => e.ConferenceAndParticipantId, "DF_ConferencesAndParticipants_ConferenceAndParticipantID_Unique")
                    .IsUnique();

                entity.Property(e => e.ConferenceAndParticipantId).HasColumnName("ConferenceAndParticipantID");

                entity.Property(e => e.ConferenceId).HasColumnName("ConferenceID");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.HasOne(d => d.Conference)
                    .WithMany(p => p.ConferencesAndParticipants)
                    .HasForeignKey(d => d.ConferenceId)
                    .HasConstraintName("FK_ConferencesAndParticipants_Conferences");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.ConferencesAndParticipants)
                    .HasForeignKey(d => d.ParticipantId)
                    .HasConstraintName("FK_ConferencesAndParticipants_Participants");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasIndex(e => e.FormId, "DF_Forms_FormID_Unique")
                    .IsUnique();

                entity.Property(e => e.FormId).HasColumnName("FormID");

                entity.Property(e => e.AvailableAudienceSize).IsRequired();

                entity.Property(e => e.FullName).IsRequired();
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasIndex(e => e.LocationId, "DF_Locations_LocationID_Unique")
                    .IsUnique();

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.Country).IsRequired();
            });

            modelBuilder.Entity<Organizer>(entity =>
            {
                entity.HasIndex(e => e.OrganizerId, "DF_Organizers_OrganizerID_Unique")
                    .IsUnique();

                entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.Occupation).IsRequired();

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasIndex(e => e.ParticipantId, "DF_Participants_ParticipantID_Unique")
                    .IsUnique();

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.Institution).IsRequired();

                entity.Property(e => e.Occupation).IsRequired();

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Work>(entity =>
            {
                entity.HasIndex(e => e.WorkId, "DF_Works_WorkID_Unique")
                    .IsUnique();

                entity.Property(e => e.WorkId).HasColumnName("WorkID");

                entity.Property(e => e.ConferenceId).HasColumnName("ConferenceID");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.PublicationDate).HasColumnType("datetime");

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.Topic).IsRequired();
            });

            modelBuilder.Entity<WorksAndParticipant>(entity =>
            {
                entity.HasKey(e => e.WorkAndParticipantId)
                    .HasName("PK_WorksAndParticipants_WorkAndParticipantID");

                entity.HasIndex(e => e.WorkAndParticipantId, "DF_WorksAndParticipants_WorkAndParticipantID_Unique")
                    .IsUnique();

                entity.Property(e => e.WorkAndParticipantId).HasColumnName("WorkAndParticipantID");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.WorkId).HasColumnName("WorkID");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.WorksAndParticipants)
                    .HasForeignKey(d => d.ParticipantId)
                    .HasConstraintName("FK_WorksAndParticipants_Participants");

                entity.HasOne(d => d.Work)
                    .WithMany(p => p.WorksAndParticipants)
                    .HasForeignKey(d => d.WorkId)
                    .HasConstraintName("FK_WorksAndParticipants_WorkID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
