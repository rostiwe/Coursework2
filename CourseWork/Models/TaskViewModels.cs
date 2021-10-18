using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CourseWork.Models
{
    public class StringModel
    { 
        public int StringModelId { get; set; }
        public string str { get; set; }
    }

    public class LookModel
    {
        public int TaskId { get; set; }
        public string TaskTexst { get; set; }
        public List<string> Pictures { get; set; }
        public decimal Rait { get; set; }
        public DateTime Date { get; set; }
        public string CreaterName { get; set; }
        public int UserRait { get; set; }
    }

    public class TaskCreateModel
    {
        public TaskCreateModel()
        {
            Pictures = new List<string>();
        }
        [Required]
        public string TaskTexst { get; set; }
        [Required]
        public string ans1 { get; set; }
        public string ans2 { get; set; }
        public string ans3 { get; set; }
        public List<string> Pictures { get; set; }

    }
    public class TaskViewModel
    {
        public TaskViewModel()
        {
            Pictures = new List<string>();
        }
        public int TaskId { get; set; }
        public string TaskTexst { get; set; }
        public string CreaterName { get; set; }
        public decimal Rait { get; set; }
        public DateTime Date { get; set; }
        public int UserRait { get; set; }
        public List<string> Pictures { get; set; }

    }
    public class RaitModel
    {
        public int UserTaskId { get; set; }
        public int Rait { get; set; }

    }
    //public partial class UserTask
    //{
    //    public int UserTaskId { get; set; }
    //    public string TaskTexst { get; set; }
    //    public decimal Rait { get; set; }
    //    public string ApplicationUserId { get; set; }
    //    public string ans1 { get; set; }
    //    public string ans2 { get; set; }
    //    public string ans3 { get; set; }
    //    public DateTime Date { get; set; }
    //    public virtual ICollection<Picture> Pictures { get; set; }
    //    public virtual ICollection<Comment> Comments { get; set; }
    //    public virtual ApplicationUser ApplicationUser { get; set; }
    //    public virtual ICollection<TasksRait> TasksRait { get; set; }
    //    public virtual ICollection<UserRightAns> UserRightAns { get; set; }
    //}
}