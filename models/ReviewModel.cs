

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

   public class ReviewModel
        {


        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }


        [ForeignKey("ProductId")]
        public virtual ProductModel Product { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }
    }
