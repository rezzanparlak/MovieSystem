using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Movies.APP.Domain
{
    public class MovieDbFactory : IDesignTimeDbContextFactory<MovieDB>
    {
        private const string ConnectionString = "data source=MovieDB";

        public MovieDB CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MovieDB>();
            optionsBuilder.UseSqlite(ConnectionString);
            return new MovieDB(optionsBuilder.Options);
        }
    }
}