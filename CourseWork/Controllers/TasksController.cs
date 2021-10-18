using CourseWork.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourseWork.Controllers
{
    public class TasksController : Controller
    {
        private ApplicationUserManager _userManager;

        public TasksController()
        {
        }

        public TasksController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            List<UserTask> userTasks = dbContext.UserTasks.Where(x => x.ApplicationUserId == userId).OrderByDescending(x => x.Rait).ToList();
            List<TaskViewModel> TasksView = new List<TaskViewModel>();
            string userName = User.Identity.Name;
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
                    CreaterName = /*UserManager.FindById(t.ApplicationUserId).UserName*/ userName,
                    Date = t.Date,
                    Rait = t.Rait,
                    TaskTexst = t.TaskTexst,
                    Pictures = dbContext.Pictures.Where(x => x.UserTaskId == t.UserTaskId).Select(x => x.Search).ToList(),
                    UserRait = (int)rait
                });
            }

            return View(TasksView);
        }
        // GET: Tasks
        [Authorize]
        public ActionResult CreateNewTask()
        {
            TaskCreateModel t = new TaskCreateModel();
            t.ans2 = "asdas";
            return View(t);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewTask(TaskCreateModel taskCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(taskCreateModel);
            }
            UserTask userTask = new UserTask();
            userTask.ApplicationUserId = User.Identity.GetUserId();
            userTask.Rait = 0;
            userTask.TaskTexst = taskCreateModel.TaskTexst;
            userTask.ans1 = taskCreateModel.ans1;
            userTask.ans2 = taskCreateModel.ans2;
            userTask.ans3 = taskCreateModel.ans3;
            userTask.Date = DateTime.Now;
            var a = dbContext.UserTasks.Add(userTask);
            foreach (var p in taskCreateModel.Pictures)
            {
                if (p != "")
                {
                    dbContext.Pictures.Add(new Picture() { Search = p, UserTaskId = a.UserTaskId});
                }
            }
            dbContext.SaveChanges();
            return Redirect("/Tasks");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string getRait(/*RaitModel rait*/ int UserTaskId, int Rait)
        {
            //0 - Error
            //1 - New
            //2 - upload
            //3 - no Change
            if (Rait > 5)
                return "0";
            string userId = User.Identity.GetUserId();
            List<TasksRait> t = dbContext.TasksRaits.Where(x => x.ApplicationUserId == userId).ToList();
            if (t.Count == 0)
            {
                dbContext.TasksRaits.Add(new TasksRait() { Rait = Rait, ApplicationUserId = userId, UserTaskId = UserTaskId });
                try
                {
                    changeRait(UserTaskId);
                    return "1";
                }
                catch (Exception ex)
                {
                    return "0";
                }

            }
            t = t.Where(x => x.UserTaskId == UserTaskId).ToList();
            if (t.Count == 0)
            {
                dbContext.TasksRaits.Add(new TasksRait() { Rait = Rait, ApplicationUserId = userId, UserTaskId = UserTaskId });
                try
                {
                    changeRait(UserTaskId);
                    return "1";
                }
                catch (Exception ex)
                {
                    return "0";
                }
            }
            try
            {
                if (t[0].Rait == Rait)
                    return "3";
                dbContext.TasksRaits.Remove(t[0]);
                dbContext.TasksRaits.Add(new TasksRait() { Rait = Rait, ApplicationUserId = userId, UserTaskId = UserTaskId });
                changeRait(UserTaskId);
            }
            catch (Exception ex)
            {
                return "0";
            }


            return "2";
        }
        private void changeRait (int taskId)
        {
            dbContext.SaveChanges();
            List<TasksRait> r = dbContext.TasksRaits.Where(x => x.UserTaskId == taskId).ToList();
            List<int> i = r.Select(x => x.Rait).ToList();
            double RatingAverage = i.Average();
            dbContext.UserTasks.Find(taskId).Rait = Math.Round((decimal)RatingAverage,2);
            dbContext.SaveChanges();
        }
        public string AnsChk(string ans, int TaskId)
        {
            if (ans == null)
                return "1";
            UserTask tasks = dbContext.UserTasks.Find(TaskId);
            if (ans == tasks.ans1 || ans == tasks.ans2 || ans == tasks.ans3)
                return "0";
            return "1";
        }
        public ActionResult Look(int UserTaskId)
        {
            string userId = User.Identity.GetUserId();
            UserTask userTasks = dbContext.UserTasks.Find(UserTaskId);
            int? rait = 0;
            try
            {
                rait = dbContext.TasksRaits.Where(x => x.UserTaskId == UserTaskId).Where(x => x.ApplicationUserId == userId).Select(x => x.Rait).First();
            }
            catch
            {
                rait = 0;
            }
            LookModel lookModel = new LookModel()
            {
                TaskId = UserTaskId,
                CreaterName = dbContext.Users.Find(userTasks.ApplicationUserId).UserName,
                Date = userTasks.Date,
                Rait = userTasks.Rait,
                TaskTexst = userTasks.TaskTexst,
                Pictures = dbContext.Pictures.Where(x => x.UserTaskId == UserTaskId).Select(x => x.Search).ToList(),
                UserRait = (int)rait
            };
            
            return View(lookModel);

        }
    }
}