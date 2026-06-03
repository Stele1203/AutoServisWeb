using System.Data.Entity;

namespace AutoServisWeb.Models
{
    public class AutoDnevnik_DBEntities : DbContext
    {
        public AutoDnevnik_DBEntities()
            : base("name=AutoDnevnik_DBEntities")
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Uloge> Uloges { get; set; }
        public virtual DbSet<Korisnici> Korisnicis { get; set; }
        public virtual DbSet<Marke> Markes { get; set; }
        public virtual DbSet<Vozila> Vozilas { get; set; }
        public virtual DbSet<Mehanicari> Mehanicaris { get; set; }
        public virtual DbSet<KategorijeServisa> KategorijeServisas { get; set; }
        public virtual DbSet<Servisi> Servisis { get; set; }
        public virtual DbSet<Delovi> Delovis { get; set; }
        public virtual DbSet<ServisStavke> ServisStavkes { get; set; }
        public virtual DbSet<Registracije> Registracijes { get; set; }
        public virtual DbSet<Komentari> Komentaris { get; set; }
        public virtual DbSet<LogBrisanihServisa> LogBrisanihServisas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Korisnici>()
                .HasOptional(k => k.Uloge)
                .WithMany(u => u.Korisnicis)
                .HasForeignKey(k => k.UlogaID);

            modelBuilder.Entity<Vozila>()
                .HasOptional(v => v.Korisnici)
                .WithMany(k => k.Vozilas)
                .HasForeignKey(v => v.KorisnikID);

            modelBuilder.Entity<Vozila>()
                .HasOptional(v => v.Marke)
                .WithMany(m => m.Vozilas)
                .HasForeignKey(v => v.MarkaID);

            modelBuilder.Entity<Servisi>()
                .HasOptional(s => s.Vozila)
                .WithMany(v => v.Servisis)
                .HasForeignKey(s => s.VoziloID);

            modelBuilder.Entity<Servisi>()
                .HasOptional(s => s.Mehanicari)
                .WithMany(m => m.Servisis)
                .HasForeignKey(s => s.MehanicarID);

            modelBuilder.Entity<Servisi>()
                .HasOptional(s => s.KategorijeServisa)
                .WithMany(k => k.Servisis)
                .HasForeignKey(s => s.KategorijaID);

            modelBuilder.Entity<ServisStavke>()
                .HasOptional(ss => ss.Servisi)
                .WithMany(s => s.ServisStavkes)
                .HasForeignKey(ss => ss.ServisID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServisStavke>()
                .HasOptional(ss => ss.Delovi)
                .WithMany(d => d.ServisStavkes)
                .HasForeignKey(ss => ss.DeoID);

            modelBuilder.Entity<Registracije>()
                .HasOptional(r => r.Vozila)
                .WithMany(v => v.Registracijes)
                .HasForeignKey(r => r.VoziloID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Komentari>()
                .HasOptional(k => k.Servisi)
                .WithMany(s => s.Komentaris)
                .HasForeignKey(k => k.ServisID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Komentari>()
                .HasOptional(k => k.Korisnici)
                .WithMany()
                .HasForeignKey(k => k.KorisnikID);
        }
    }
}