using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using QuestApplication.Models;

namespace QuestApplication.Entities
{
   
    public class Assignment
    {
        public Assignment()
        {
            this.StudentAssignments = new HashSet<StudentAssignment>();
            this.Questions = new HashSet<Question>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        
        [Display(Name = "Duration (Minutes)")]
        public decimal MaxTime { get; set; }
        
        public ICollection<StudentAssignment> StudentAssignments { get; set; }

        public ICollection<Question> Questions { get; set; }

    }
}