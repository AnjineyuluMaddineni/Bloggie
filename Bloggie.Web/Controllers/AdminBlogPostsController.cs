using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;
        public AdminBlogPostsController(ITagRepository tagRepository,IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository= blogPostRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //Get tags from respository

            var tags=await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //Map view model to domain model
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
            };

            // Map selected tags
            var selectedTags = new List<Tag>();
            if (addBlogPostRequest.SeletedTags != null)
            {
                foreach (var selectedTagId in addBlogPostRequest.SeletedTags)
                {
                    var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                    var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                    if (existingTag != null)
                    {
                        selectedTags.Add(existingTag);
                    }
                }
            }

            blogPost.Tags = selectedTags;

            try
            {
                // Add blog post to repository
                await blogPostRepository.AddAsync(blogPost);

                // Redirect to the blog post list or success page
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully
                ModelState.AddModelError("", "An error occurred while saving the blog post.");

                // Reload the tags in case of error
                var tags = await tagRepository.GetAllAsync();
                addBlogPostRequest.Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() });

                return View(addBlogPostRequest);
            }
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery)
        {
            ViewBag.SearchQuery = searchQuery;

            //Call the respository
            var blogPost =await blogPostRepository.GetAllAsync(searchQuery);


            return View(blogPost);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Retrieve the result from the repository 
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                // map the domain model into the view model
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);

            }

            // pass data to view
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // Map view model back to domain model
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible
            };

            // Map tags into domain model
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainModel.Tags = selectedTags;

            // Submit information to repository to update
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updatedBlog != null)
            {
                // Show success notification
                TempData["SuccessMessage"] = "Blog post updated successfully!";
                return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
            }

            // Show error notification
            TempData["ErrorMessage"] = "Error updating blog post.";
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            // Talk to repository to delete this blog post and associated tags
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogPost != null)
            {
                // Set success notification
                TempData["SuccessMessage"] = "Blog post deleted successfully!";
                return RedirectToAction("List");
            }

            // Set failure notification
            TempData["ErrorMessage"] = "Error: Unable to delete the blog post.";
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }

    }
}
