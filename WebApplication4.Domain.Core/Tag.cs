using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Domain.Core
{
    public class Tag
    {
        public int TagID { get; set; }

        public string TagName { get; set; }
        public ICollection<Article> Articles { get; set; }
        
    }
}
