using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sell_Your_Books.Models
{
    public class BookInfo
    {
        [Key]
        [Required]
        public int BookId { get; set; }

        [Display(Name = "Book Image")]
        public string img { get; set; }

        [Required]
        [Display(Name = "Book Name")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Author Name")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Book Price")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Book Type")]
        public string isNew { get; set; }
        
        [Display(Name = "Condition Of The Book")]
        public string Condition { get; set; }
        public string userId { get; set; }
        public ApplicationUser User { get; set; }
    }
}