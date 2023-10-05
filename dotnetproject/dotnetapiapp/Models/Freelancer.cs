using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetapiapp.Models
{
    public class Freelancer
    {
        [key]
        public int FreelancerID {get; set;}
        [Required]
        public int FreelancerName {get; set;}
        [Required]
        public int Specialization {get; set;}
        [Column(TypeName = "decimal(18,2)")]
        public int CommercialPerHour {get; set;}
        [Required]
        public int MailID {get; set;}
        public int ContactNumber {get; set;}
        public ICollection<Project> projects {get; set; }
    }
}