using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    public class Project
    {
         [Key]
        public int ProjectID {get; set;}
        [Required]
        public int ProjectName {get; set;}
        public int NumberofModules {get; set;}
        public DateTime SubmissionDate {get; set;}
        [ForeignKey("Freelancer")]
        public int FreelancerID {get; set;}
        public Freelancer Freelancer {get; set; }
    }