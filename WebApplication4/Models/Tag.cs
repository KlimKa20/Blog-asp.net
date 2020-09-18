using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog_project.Models
{
    public class Tag
    {
        public int TagID { get; set; }

        public string TagName { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public Tag()
        {
            Articles = new List<Article>();
        }
    }
}
