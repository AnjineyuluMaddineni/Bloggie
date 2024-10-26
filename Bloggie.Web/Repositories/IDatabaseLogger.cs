namespace Bloggie.Web.Repositories
{
    public interface IDatabaseLogger
    {
        Task LogAsync(string message, string logLevel, string exceptionDetails = null);
    }
}
