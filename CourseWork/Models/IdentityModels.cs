using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CourseWork.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
        public ApplicationUser():base ()
        {
            this.UserTasks = new HashSet<UserTask>();
            this.TasksRaits = new HashSet<TasksRait>();
            this.UserRightAns = new HashSet<UserRightAns>();
            this.Comments = new HashSet<Comment>();
        }
        public virtual ICollection<UserTask> UserTasks { get; set; }
        public virtual ICollection<TasksRait> TasksRaits { get; set; }
        public virtual ICollection<UserRightAns> UserRightAns { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public bool IsInBan { get; set; }
        //public bool IsEnglish { get; set; }
        //public bool IsDark { get; set; }
    }

    public partial class UserTask
    {
        public UserTask()
        {
            this.Pictures = new HashSet<Picture>();
            this.TasksRait = new HashSet<TasksRait>();
            this.UserRightAns = new HashSet<UserRightAns>();
            this.Comments = new HashSet<Comment>();
        }

        public int UserTaskId { get; set; }
        public string TaskTexst { get; set; }
        public decimal Rait { get; set; }
        public string ApplicationUserId { get; set; }
        public string ans1 { get; set; }
        public string ans2 { get; set; }
        public string ans3 { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<TasksRait> TasksRait { get; set; }
        public virtual ICollection<UserRightAns> UserRightAns { get; set; }
    }
    public partial class UserRightAns
    {
        [Key, Column(Order = 1)]
        public string ApplicationUserId { get; set; }
        [Key, Column(Order = 2)]
        public int UserTaskId { get; set; }
        public virtual UserTask UserTasks { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public partial class TasksRait
    {
        [Key, Column(Order = 1)]
        public string ApplicationUserId { get; set; }
        [Key, Column(Order = 2)]
        public int UserTaskId { get; set; }
        public int Rait { get; set; }

        public virtual UserTask UserTasks { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
    public partial class Picture
    {
        public int Id { get; set; }
        public int UserTaskId { get; set; }
        public string Search { get; set; }

        public virtual UserTask UserTasks { get; set; }
    }
    public class Comment
    {
        public int Id { get; set; }
        public string text { get; set; }
        public int UserTaskId { get; set; }
        public virtual UserTask UserTasks { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public DateTime Date { get; set; }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<TasksRait> TasksRaits { get; set; }
        public DbSet<UserRightAns> UserRightAns { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<Comment> comments { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}