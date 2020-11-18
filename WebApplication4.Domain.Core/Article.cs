
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Domain.Core
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
        public Profile Profile { get; set; }

        public ICollection<Comment> Comments { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }

        public Article()
        {
            Comments = new List<Comment>();
        }

    }
}
