using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using QuestApplication.Models;

namespace QuestApplication.Entities
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]        
        public int Id { get; set; }

        public Student()
        {
            this.StudentAssignments = new HashSet<StudentAssignment>();
        }

        public virtual ApplicationUser User { get; set; }

        public ICollection<StudentAssignment> StudentAssignments { get; set; }
    }
}