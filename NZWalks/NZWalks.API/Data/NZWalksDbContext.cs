using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext: DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions): base(dbContextOptions) 
            // here 500 error cause DbContext is non-generic type so pass type of dbcontext
            // so no ambiguity to which dbcontext is to be called
        {

            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        // when we run entityframework core migrations these properties create tables in database

        public DbSet<Images> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // seed the data for difficulties
            // Easy, Medium, Hard
            var difficulties = new List<Difficulty>()
            {
                // i opened c# interacctive window to generate new guid id to easy,medium,hard using Guid.NewGuid() from System.Guid

                new Difficulty()
                {
                    Id= Guid.Parse("c62caf1c-4b01-4ee9-b01f-22712605dfaa"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id= Guid.Parse("6e12cb01-ec23-43f3-be03-95d0a2e681ba"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id= Guid.Parse("60e28c55-c78e-418d-a864-741e9fd834eb"),
                    Name = "Hard"
                }

            };

            // seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            var regions = new List<Region>()
            {
                new Region()
                {
                    Id=Guid.Parse("b2964597-9765-4ef2-89e4-7510d4583f3d"),
                    Name="Ackland",
                    Code="AKL",
                    RegionImageUrl="https://dynamic-media-cdn.tripadvisor.com/media/photo-o/10/dc/e6/a1/enjoy-beautiful-views.jpg?w=600&h=-1&s=1"
                },
                 new Region()
                {
                    Id=Guid.Parse("3012669d-1fdf-43f8-a390-6d506826a3d1"),
                    Name="Queenstown",
                    Code="QT",
                    RegionImageUrl="https://unsplash.com/photos/aerial-view-of-city-near-lake-during-daytime-8T8UCBeWuUs"
                },
                new Region()
                {
                    Id=Guid.Parse("c2d724d7-83c1-4903-b68b-4d55c88cc2bb"),
                    Name="Wellington",
                    Code="WT",
                    RegionImageUrl="https://unsplash.com/photos/aerial-view-of-city-buildings-during-sunset-KRtXOMfS4oA"
                }

            };

            // insert data inside the regions table
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
