using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostcommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment comment);
        Task<IEnumerable<BlogPostComment>> GetCommentsByIdAsync(Guid blogPostId);
    }
}
