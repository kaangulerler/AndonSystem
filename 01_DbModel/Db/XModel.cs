using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace _01_DbModel.Db
{
    public partial class XModel : DbContext
    {
        public XModel()
        {
        }

        public XModel(DbContextOptions<XModel> options)
            : base(options)
        {
        }

        public virtual DbSet<T3_Calisma> T3_Calisma { get; set; }
        public virtual DbSet<T3_CalismaIstasyonBosZaman> T3_CalismaIstasyonBosZaman { get; set; }
        public virtual DbSet<T3_CalismaPersonelBosZaman> T3_CalismaPersonelBosZaman { get; set; }
        public virtual DbSet<T3_CalismaPlanliDurus> T3_CalismaPlanliDurus { get; set; }
        public virtual DbSet<T3_DurusTip> T3_DurusTip { get; set; }
        public virtual DbSet<T3_Istasyon> T3_Istasyon { get; set; }
        public virtual DbSet<T3_IstasyonDurus> T3_IstasyonDurus { get; set; }
        public virtual DbSet<T3_IstasyonVardiya> T3_IstasyonVardiya { get; set; }
        public virtual DbSet<T3_Lokasyon> T3_Lokasyon { get; set; }
        public virtual DbSet<T3_Personel> T3_Personel { get; set; }
        public virtual DbSet<T3_PlanliCalisma> T3_PlanliCalisma { get; set; }
        public virtual DbSet<T3_PlanliDurus> T3_PlanliDurus { get; set; }
        public virtual DbSet<T3_Tabela> T3_Tabela { get; set; }
        public virtual DbSet<T3_Uretim> T3_Uretim { get; set; }
        public virtual DbSet<T3_UretimCalisma> T3_UretimCalisma { get; set; }
        public virtual DbSet<T3_UretimDurus> T3_UretimDurus { get; set; }
        public virtual DbSet<T3_UretimPersonel> T3_UretimPersonel { get; set; }
        public virtual DbSet<T3_UretimPlanliDurus> T3_UretimPlanliDurus { get; set; }
        public virtual DbSet<T3_Urun> T3_Urun { get; set; }
        public virtual DbSet<T3_UrunTip> T3_UrunTip { get; set; }
        public virtual DbSet<T3_UrunTipIstasyon> T3_UrunTipIstasyon { get; set; }
        public virtual DbSet<T3_Vardiya> T3_Vardiya { get; set; }
        public virtual DbSet<T3_VardiyaMola> T3_VardiyaMola { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("data source=192.168.1.250;initial catalog=T3Andon;user id=sa;password=T3Otomasyon2006;MultipleActiveResultSets=True;App=EntityFramework");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T3_Calisma>(entity =>
            {
                entity.HasIndex(e => e.IstasyonId, "IX_T3_Calisma_IstasyonId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Istasyon)
                    .WithMany(p => p.T3_Calisma)
                    .HasForeignKey(d => d.IstasyonId)
                    .HasConstraintName("FK_T3_Calisma_IstasyonId");
            });

            modelBuilder.Entity<T3_CalismaIstasyonBosZaman>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Calisma)
                    .WithMany(p => p.T3_CalismaIstasyonBosZaman)
                    .HasForeignKey(d => d.CalismaId)
                    .HasConstraintName("FK_T3_CalismaIstasyonBosZaman_T3_Calisma");
            });

            modelBuilder.Entity<T3_CalismaPersonelBosZaman>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Calisma)
                    .WithMany(p => p.T3_CalismaPersonelBosZaman)
                    .HasForeignKey(d => d.CalismaId)
                    .HasConstraintName("FK_T3_CalismaPersonelBosZaman_T3_Calisma");

                entity.HasOne(d => d.Personel)
                    .WithMany(p => p.T3_CalismaPersonelBosZaman)
                    .HasForeignKey(d => d.PersonelId)
                    .HasConstraintName("FK_T3_CalismaPersonelBosZaman_T3_Personel");
            });

            modelBuilder.Entity<T3_CalismaPlanliDurus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Calisma)
                    .WithMany(p => p.T3_CalismaPlanliDurus)
                    .HasForeignKey(d => d.CalismaId)
                    .HasConstraintName("FK_T3_CalismaPlanliDurus_T3_Calisma");
            });

            modelBuilder.Entity<T3_DurusTip>(entity =>
            {
                entity.HasIndex(e => e.DurusTipId, "IX_T3_DurusTip_DurusTipId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Barkod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DurusTipTree).IsRequired();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DurusTip)
                    .WithMany(p => p.InverseDurusTip)
                    .HasForeignKey(d => d.DurusTipId)
                    .HasConstraintName("FK_T3_DurusTip_T3_DurusTip");
            });

            modelBuilder.Entity<T3_Istasyon>(entity =>
            {
                entity.HasIndex(e => e.LokasyonId, "IX_T3_Istasyon_LokasyonId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Barkod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IpAdres)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LokasyonTree).IsRequired();

                entity.HasOne(d => d.Lokasyon)
                    .WithMany(p => p.T3_Istasyon)
                    .HasForeignKey(d => d.LokasyonId)
                    .HasConstraintName("FK_T3_Istasyon_LokasyonId");
            });

            modelBuilder.Entity<T3_IstasyonDurus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DurusTip).IsRequired();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DurusTipNavigation)
                    .WithMany(p => p.T3_IstasyonDurus)
                    .HasForeignKey(d => d.DurusTipId)
                    .HasConstraintName("FK_T3_IstasyonDurus_T3_DurusTip");

                entity.HasOne(d => d.Istasyon)
                    .WithMany(p => p.T3_IstasyonDurus)
                    .HasForeignKey(d => d.IstasyonId)
                    .HasConstraintName("FK_T3_IstasyonDurus_T3_Istasyon");
            });

            modelBuilder.Entity<T3_IstasyonVardiya>(entity =>
            {
                entity.HasIndex(e => e.IstasyonId, "IX_T3_IstasyonVardiya_IstasyonId");

                entity.HasIndex(e => e.VardiyaId, "IX_T3_IstasyonVardiya_VardiyaId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Istasyon)
                    .WithMany(p => p.T3_IstasyonVardiya)
                    .HasForeignKey(d => d.IstasyonId)
                    .HasConstraintName("FK_T3_IstasyonVardiya_IstasyonId");

                entity.HasOne(d => d.Vardiya)
                    .WithMany(p => p.T3_IstasyonVardiya)
                    .HasForeignKey(d => d.VardiyaId)
                    .HasConstraintName("FK_T3_IstasyonVardiya_VardiyaId");
            });

            modelBuilder.Entity<T3_Lokasyon>(entity =>
            {
                entity.HasIndex(e => e.UstId, "IX_T3_Lokasyon_UstId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Ust)
                    .WithMany(p => p.InverseUst)
                    .HasForeignKey(d => d.UstId)
                    .HasConstraintName("FK_T3_Lokasyon_T3_Lokasyon");
            });

            modelBuilder.Entity<T3_Personel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Barkod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Soyad)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<T3_PlanliCalisma>(entity =>
            {
                entity.HasIndex(e => e.IstasyonId, "IX_T3_PlanliCalisma_IstasyonId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Istasyon)
                    .WithMany(p => p.T3_PlanliCalisma)
                    .HasForeignKey(d => d.IstasyonId)
                    .HasConstraintName("FK_T3_PlanliCalisma_IstasyonId");
            });

            modelBuilder.Entity<T3_PlanliDurus>(entity =>
            {
                entity.HasIndex(e => e.IstasyonId, "IX_T3_PlanliDurus_IstasyonId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Istasyon)
                    .WithMany(p => p.T3_PlanliDurus)
                    .HasForeignKey(d => d.IstasyonId)
                    .HasConstraintName("FK_T3_PlanliDurus_IstasyonId");
            });

            modelBuilder.Entity<T3_Tabela>(entity =>
            {
                entity.HasKey(e => e.Zaman);

                entity.Property(e => e.Zaman).HasColumnType("datetime");
            });

            modelBuilder.Entity<T3_Uretim>(entity =>
            {
                entity.HasIndex(e => e.IstasyonId, "IX_T3_Uretim_CalismaId");

                entity.HasIndex(e => e.UrunId, "IX_T3_Uretim_UrunId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Barkod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Istasyon)
                    .WithMany(p => p.T3_Uretim)
                    .HasForeignKey(d => d.IstasyonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T3_Uretim_T3_Istasyon");

                entity.HasOne(d => d.Urun)
                    .WithMany(p => p.T3_Uretim)
                    .HasForeignKey(d => d.UrunId)
                    .HasConstraintName("FK_T3_Uretim_UrunId");
            });

            modelBuilder.Entity<T3_UretimCalisma>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Calisma)
                    .WithMany(p => p.T3_UretimCalisma)
                    .HasForeignKey(d => d.CalismaId)
                    .HasConstraintName("FK_T3_UretimCalisma_T3_Calisma");

                entity.HasOne(d => d.Uretim)
                    .WithMany(p => p.T3_UretimCalisma)
                    .HasForeignKey(d => d.UretimId)
                    .HasConstraintName("FK_T3_UretimCalisma_T3_Uretim");
            });

            modelBuilder.Entity<T3_UretimDurus>(entity =>
            {
                entity.HasIndex(e => e.DurusTipId, "IX_T3_UretimDurus_DurusTipId");

                entity.HasIndex(e => e.UretimId, "IX_T3_UretimDurus_UretimId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DurusTip).IsRequired();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DurusTipNavigation)
                    .WithMany(p => p.T3_UretimDurus)
                    .HasForeignKey(d => d.DurusTipId)
                    .HasConstraintName("FK_T3_UretimDurus_DurusTipId");

                entity.HasOne(d => d.Uretim)
                    .WithMany(p => p.T3_UretimDurus)
                    .HasForeignKey(d => d.UretimId)
                    .HasConstraintName("FK_T3_UretimDurus_UretimId");
            });

            modelBuilder.Entity<T3_UretimPersonel>(entity =>
            {
                entity.HasIndex(e => e.PersonelId, "IX_T3_UretimPersonel_PersonelId");

                entity.HasIndex(e => e.UretimId, "IX_T3_UretimPersonel_UretimId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Personel)
                    .WithMany(p => p.T3_UretimPersonel)
                    .HasForeignKey(d => d.PersonelId)
                    .HasConstraintName("FK_T3_UretimPersonel_PersonelId");

                entity.HasOne(d => d.Uretim)
                    .WithMany(p => p.T3_UretimPersonel)
                    .HasForeignKey(d => d.UretimId)
                    .HasConstraintName("FK_T3_UretimPersonel_UretimId");
            });

            modelBuilder.Entity<T3_UretimPlanliDurus>(entity =>
            {
                entity.HasIndex(e => e.UretimId, "IX_T3_UretimPlanliDurus_UretimId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Uretim)
                    .WithMany(p => p.T3_UretimPlanliDurus)
                    .HasForeignKey(d => d.UretimId)
                    .HasConstraintName("FK_T3_UretimPlanliDurus_UretimId");
            });

            modelBuilder.Entity<T3_Urun>(entity =>
            {
                entity.HasIndex(e => e.TipId, "IX_T3_Urun_TipId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Barkod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Bom)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Panel_No)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Switchgear)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Tip)
                    .WithMany(p => p.T3_Urun)
                    .HasForeignKey(d => d.TipId)
                    .HasConstraintName("FK_T3_Urun_TipId");
            });

            modelBuilder.Entity<T3_UrunTip>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Cap_Volt_Ind)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Es_Present)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Es_Type_Subs)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Panel_Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<T3_UrunTipIstasyon>(entity =>
            {
                entity.HasIndex(e => e.IstasyonId, "IX_T3_UrunTipIstasyon_IstasyonId");

                entity.HasIndex(e => e.TipId, "IX_T3_UrunTipIstasyon_TipId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Istasyon)
                    .WithMany(p => p.T3_UrunTipIstasyon)
                    .HasForeignKey(d => d.IstasyonId)
                    .HasConstraintName("FK_T3_UrunTipIstasyon_IstasyonId");

                entity.HasOne(d => d.Tip)
                    .WithMany(p => p.T3_UrunTipIstasyon)
                    .HasForeignKey(d => d.TipId)
                    .HasConstraintName("FK_T3_UrunTipIstasyon_TipId");
            });

            modelBuilder.Entity<T3_Vardiya>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<T3_VardiyaMola>(entity =>
            {
                entity.HasIndex(e => e.VardiyaId, "IX_T3_VardiyaMola_VardiyaId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Kod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Vardiya)
                    .WithMany(p => p.T3_VardiyaMola)
                    .HasForeignKey(d => d.VardiyaId)
                    .HasConstraintName("FK_T3_VardiyaMola_VardiyaId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
