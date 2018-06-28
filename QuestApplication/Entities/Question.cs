using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuestApplication.Entities
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Display(Name = "Question :")]
        public string Description { get; set; }

        public int SerialNo { get; set; }

        public string OptionOne { get; set; }

        public string OptionTwo { get; set; }
        public string OptionThree { get; set; }
        public string OptionFour { get; set; }

        public string Answer { get; set; }

        public int Mark { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}