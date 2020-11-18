using System;
using System.ComponentModel.DataAnnotations;


namespace WebApplication4.Domain.Core
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Text { get; set; }
        public int ArticleID { get; set; }
        public string ProfileID { get; set; }
        public Profile Profile { get; set; }
        public Article Article { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }

    }
}
