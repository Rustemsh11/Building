using Building.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Building.DAL
{
    public partial class BuildingContext : DbContext
    {
        public BuildingContext()
        {
        }

        public BuildingContext(DbContextOptions<BuildingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BuildingSite> BuildingSites { get; set; } = null!;
        //public virtual DbSet<Characteristic> Characteristics { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<EmployeesBuilding> EmployeesBuildings { get; set; } = null!;
        public virtual DbSet<Material> Materials { get; set; } = null!;
        //public virtual DbSet<MaterialCharacteristic> MaterialCharacteristics { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<Query> Queries { get; set; } = null!;
        public virtual DbSet<QueryDetail> QueryDetails { get; set; } = null!;
        public virtual DbSet<Catalog> Catalogs { get; set; } = null!;
        //public virtual DbSet<Image> Images { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(x => x.EnableRetryOnFailure());    
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuildingSite>(entity =>
            {
                entity.HasKey(e => e.BuildingID);

                entity.Property(e => e.BuildingID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BuildingID");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Photo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Deadline)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            #region charateristic

            //modelBuilder.Entity<Characteristic>(entity =>
            //{
            //    entity.Property(e => e.CharacteristicId)
            //        .ValueGeneratedNever()
            //        .HasColumnName("CharacteristicID");

            //    entity.Property(e => e.Additional)
            //        .HasMaxLength(250)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Measure)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Name)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);
            //});
            #endregion

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.Birthday)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Idposition).HasColumnName("IDPosition");

                entity.Property(e => e.Login)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone).HasMaxLength(12);

                entity.Property(e => e.SecondName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdpositionNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Idposition)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_Positions");
            });

            modelBuilder.Entity<EmployeesBuilding>(entity =>
            {
                entity.Property(e => e.EmployeesBuildingID)
                .ValueGeneratedOnAdd()
                .HasColumnName("EmployeeBuildingID");

                entity.ToTable("EmployeesBuilding");

                entity.Property(e => e.BuildingId).HasColumnName("BuildingID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.HasOne(d => d.Building)
                    .WithMany()
                    .HasForeignKey(d => d.BuildingId)
                    .HasConstraintName("FK_EmployeesBuilding_BuildingSites"); 

                entity.HasOne(d => d.Employee)
                    .WithMany()
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_EmployeesBuilding_Employees");
            });                         

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.MaterialId)
                    .ValueGeneratedNever()
                    .HasColumnName("MaterialID");
                entity.Property(e => e.IdCatalog)
                .HasColumnName("CatalogID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.HasOne(m => m.Catalog)
                    .WithMany(c => c.Materials)
                    .HasForeignKey(m => m.IdCatalog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Materials_Catalog");
                entity.HasMany(d => d.QueryDetails)
               .WithOne(c => c.Material)
               .HasForeignKey(d => d.MaterialId)
               .OnDelete(DeleteBehavior.Cascade);

            });
            #region MaterialCharact

            //modelBuilder.Entity<MaterialCharacteristic>(entity =>
            //{
            //    entity.HasNoKey();

            //    entity.Property(e => e.CharacId).HasColumnName("CharacID");

            //    entity.Property(e => e.MaterialId).HasColumnName("MaterialID");

            //    entity.HasOne(d => d.Charac)
            //        .WithMany()
            //        .HasForeignKey(d => d.CharacId)
            //        .HasConstraintName("FK_MaterialCharacteristics_Characteristics");

            //    entity.HasOne(d => d.Material)
            //        .WithMany()
            //        .HasForeignKey(d => d.MaterialId)
            //        .HasConstraintName("FK_MaterialCharacteristics_Materials");
            //});
            #endregion


            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.Idposition);

                entity.Property(e => e.Idposition)
                    .ValueGeneratedNever()
                    .HasColumnName("IDPosition");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Query>(entity =>
            {
                entity.Property(e => e.QueryId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("QueryID");

               

                entity.Property(e => e.Deadline)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                               

                

                entity.Property(e => e.ProrabId).HasColumnName("ProrabID");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.SnabId).HasColumnName("SnabID");

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);               
                  

                entity.HasOne(d => d.Prorab)
                    .WithMany(p => p.QueryProrabs)
                    .HasForeignKey(d => d.ProrabId)
                    .HasConstraintName("FK_Queries_Employees");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Queries)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK_Queries_BuildingSites");

                entity.HasOne(d => d.Snab)
                    .WithMany(p => p.QuerySnabs)
                    .HasForeignKey(d => d.SnabId)
                    .HasConstraintName("FK_Queries_Employees1");
                entity.HasMany(d => d.QueryDetails)
                .WithOne(c => c.Query)
                .HasForeignKey(d => d.QueryId)
                .OnDelete(DeleteBehavior.Cascade);
                

            });
            modelBuilder.Entity<QueryDetail>(entity =>
            {
                entity.Property(e => e.ID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.QueryId).HasColumnName("QueryId");

                //entity.Property(e => e.MaterialId)
                //entity.Property(e => e.CatalogId)
                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);


                //entity.HasOne(d => d.Query)
                //    .WithMany()
                //    .HasForeignKey(d => d.QueryId)
                //    .HasConstraintName("FK_QueryDetails_Queries");

                //entity.HasOne(d => d.Material)
                //    .WithMany()
                //    .HasForeignKey(d => d.MaterialId)
                //    .HasConstraintName("FK_QueryDetails_Materials")
                //    .OnDelete(DeleteBehavior.Cascade);

                //entity.HasOne(d => d.Material)
                //    .WithMany()
                //    .HasForeignKey(d => d.MaterialId)
                //    .HasConstraintName("FK_QueryDetails_Materials")
                //    .OnDelete(DeleteBehavior.Cascade);


            });

            modelBuilder.Entity<Catalog>(entity =>
            {
                entity.HasKey(e => e.CatalogID);

                entity.Property(e => e.CatalogID)
                    .ValueGeneratedNever()
                    .HasColumnName("CatalogID");
                    

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
                //modelBuilder.Entity<Image>(entity =>
                //{
                //    entity.Property(e => e.Id)
                //        .HasColumnName("Id");
                //    entity.HasOne(d => d.Query)
                //        .WithMany()
                //        .HasForeignKey(d => d.QueryId)
                //        .OnDelete(DeleteBehavior.Cascade)
                //        .HasConstraintName("FK_Images_Queries");
                //});

                OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
