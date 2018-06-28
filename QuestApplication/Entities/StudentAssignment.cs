using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuestApplication.Entities
{
    public class StudentAssignment
    {
        [Key, Column(Order = 0)]
        public int StudentId { get; set; }
        [Key, Column(Order = 1)]
        public int AssignmentId { get; set; }

        public virtual Student Student { get; set; }

        public virtual Assignment Assignment { get; set; }

        public int Point { get; set; }

    }
}