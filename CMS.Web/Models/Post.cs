// In CMS.Web/Models/Post.cs
using System;

namespace CMS.Web.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }
    }
}
