
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public class DatabaseLogger : IDatabaseLogger
    {
        private readonly BloggieDbContext _context;
        public DatabaseLogger(BloggieDbContext context)
        {
            _context = context;
        }
        public async Task LogAsync(string message, string logLevel, string exceptionDetails = null)
        {
            var log = new Log
            {
                Message = message,
                Timestamp = DateTime.UtcNow,
                LogLevel = logLevel,
                ExceptionDetails = exceptionDetails ?? string.Empty // Set to empty string if null
            };

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
