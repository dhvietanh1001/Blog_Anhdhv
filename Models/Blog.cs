using System;
using System.Collections.Generic;

namespace MVC4.Models
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public string? Img { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public bool? Status { get; set; }
        public string? Location { get; set; }
        public DateTime? PublicDate { get; set; }

        public virtual CategoryBlog? Category { get; set; }
    }
}
