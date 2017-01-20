namespace Photography.Data
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class PhotographyContext : DbContext
    {
        public PhotographyContext()
            : base("name=PhotographyContext")
        {
        }

        public DbSet<Accessory> Accessories { get; set; }

        public DbSet<Camera> Cameras {get;set;}

        public DbSet<Len> Lens { get; set; }

        public DbSet<Photographer> Photographers { get; set; }

        public DbSet<Workshop> Workshops { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photographer>()
               .HasRequired(phot => phot.PrimaryCamera)
               .WithMany(camera => camera.PrimeryPhotographers)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Photographer>()
                .HasRequired(phot => phot.SecondaryCamera)
                .WithMany(camera => camera.SecondaryPhotographers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Workshop>()
                .HasRequired(work => work.Trainer)
                .WithMany(tr => tr.TrainWorkshops)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Workshop>()
                .HasMany(work => work.Participants)
                .WithMany(photog => photog.ParticipWorkshops)
                .Map(configuration =>
                {
                    configuration.MapLeftKey("WorkshopId");
                    configuration.MapRightKey("PartisipantId");
                    configuration.ToTable("WorkshopParticipants");
                });

            modelBuilder.Entity<Camera>()
             .Map<DslrCamera>(m =>
             m.Requires("Type")
                 .HasValue("DSLR"))
             .Map<MirrorlessCamera>(m =>
             m.Requires("Type")
                 .HasValue("Mirrorless"));


            base.OnModelCreating(modelBuilder);
        }
    }
    
}