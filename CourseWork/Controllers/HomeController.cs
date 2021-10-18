using CourseWork.Models;
using Fissoft.EntityFramework.Fts;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWork.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            List<UserTask> userTasks = dbContext.UserTasks.OrderByDescending(x => x.Rait).ToList();
            List<TaskViewModel> TasksView = new List<TaskViewModel>();
            foreach (UserTask t in userTasks)
            {
                int? rait = 0;
                try
                {
                    rait = dbContext.TasksRaits.Where(x => x.UserTaskId == t.UserTaskId).Where(x => x.ApplicationUserId == userId).Select(x => x.Rait).First();
                }
                catch
                {
                    rait = 0;
                }
                TasksView.Add(new TaskViewModel()
                {
                    TaskId = t.UserTaskId,
                    CreaterName = /*UserManager.FindById(t.ApplicationUserId).UserName*/ dbContext.Users.Find(t.ApplicationUserId).UserName,
                    Date = t.Date,
                    Rait = t.Rait,
                    TaskTexst = t.TaskTexst,
                    Pictures = dbContext.Pictures.Where(x => x.UserTaskId == t.UserTaskId).Select(x => x.Search).ToList(),
                    UserRait = (int)rait
                });
            }

            return View(TasksView);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        [HttpPost]
        public ActionResult search(string SearchString)
        {
            string userId = User.Identity.GetUserId();
            var text = FullTextSearchModelUtil.Contains(SearchString);
            //db.Tables.Where(c => c.Fullname.Contains(text));
            List<UserTask> userTasks = dbContext.UserTasks.Where(x=>x.TaskTexst.Contains(SearchString)).OrderByDescending(x=>x.Rait).ToList();
            List<TaskViewModel> TasksView = new List<TaskViewModel>();
            foreach (UserTask t in userTasks)
            {
                int? rait = 0;
                try
                {
                    rait = dbContext.TasksRaits.Where(x => x.UserTaskId == t.UserTaskId).Where(x => x.ApplicationUserId == userId).Select(x => x.Rait).First();
                }
                catch
                {
                    rait = 0;
                }
                TasksView.Add(new TaskViewModel()
                {
                    TaskId = t.UserTaskId,
                    CreaterName = /*UserManager.FindById(t.ApplicationUserId).UserName*/ dbContext.Users.Find(t.ApplicationUserId).UserName,
                    Date = t.Date,
                    Rait = t.Rait,
                    TaskTexst = t.TaskTexst,
                    Pictures = dbContext.Pictures.Where(x => x.UserTaskId == t.UserTaskId).Select(x => x.Search).ToList(),
                    UserRait = (int)rait
                });
            }

            return View(TasksView);
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                string filePath = Guid.NewGuid() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath("~/UploadedFiles"), filePath));
                //Here you can write code for save this information in your database if you want
            }
            return Json("You are gay");
        }
        //public ActionResult Disk()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Disk(HttpPostedFileBase file)
        //{
        //    GoogleDriveAPIHelper.UplaodFileOnDrive(file);
        //    ViewBag.Success = "File Uploaded on Google Drive";
        //    return View();
        //}
        //public List<string> AddOne()
    }
}