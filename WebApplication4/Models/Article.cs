
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace blog_project.Models
{
    public class Article
    {
        [HiddenInput(DisplayValue = false)]
        public int ArticleID { get; set; }
        [Required(ErrorMessage = "Название блога обязательно")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Заполните содержимое блога")]
        public string Text { get; set; }

        public string Image { get; set; }

        public int TagID { get; set; }
        public Tag Tag { get; set; }

        public string ProfileID { get; set; }
        public  Profile Profile { get; set; }

        public  ICollection<Comment> Comments { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        public int Like { get; set; } = 0;
        public int Dislike { get; set; } = 0;

        public Article()
        {
            Comments = new List<Comment>();
        }

    }
}
