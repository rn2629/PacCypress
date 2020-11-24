using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PAC.Models
{
    
    public class DatePickerContext :IdentityDbContext
    {
        
        public DatePickerContext(DbContextOptions<DatePickerContext> options)
           : base(options)
        {
        }

        public DbSet<DatePickerEvent> tblSeanceCours { get; set; }
        public DbSet<DatePickerEventEtu> tblDisponibilites { get; set; }
        public DbSet<IdentityUser> AspNetUsers { get; set; }
        public DbSet<IdentityRole> AspNetRoles { get; set; }
        public DbSet<IdentityUserRole<string>> AspNetUserRole { get; set; }
        public DbSet<Enseignant> tblEnseignant { get; set; }
        public DbSet<Etudiant> tblEtudiant { get; set; }
        public DbSet<Rencontre> tblRencontre { get; set; }
        public DbSet<Evaluation> tblEvaluation { get; set; }
        public DbSet<Question> tblQuestion { get; set; }
        public DbSet<EvaluationQuestion> tblEvaluationQuestion { get; set; }
        public DbSet<AutoEvaluation> tblAutoEvaluation { get; set; }
        public DbSet<AutoEvaluationQuestion>tblAutoEvaluationQuestion { get; set; }
        public DbSet<AdminCommand> tblAdminCommand { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUserRole<string>>().HasKey(g => new { g.UserId});
            builder.Entity<EvaluationQuestion>().HasKey(c => new {  c.evaluationId,c.questionId });
            builder.Entity<AutoEvaluationQuestion>().HasKey(c => new { c.evaluationId, c.questionId });

        }
    }
}





