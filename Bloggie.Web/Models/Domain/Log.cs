namespace Bloggie.Web.Models.Domain
{
    public class Log
    {
        public int Id { get; set; } // Primary Key
        public string Message { get; set; } // Log message
        public DateTime Timestamp { get; set; } // When the log entry was created
        public string LogLevel { get; set; } // Level of logging (Info, Error, etc.)
        public string? ExceptionDetails { get; set; } // Optional details about an exception
    }
}
