using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuestApplication.Entities;
using QuestApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestApplication.Controllers
{
    [Authorize]
    public class AssignmentController : Controller
    {
        ApplicationDbContext context;

        public AssignmentController()
        {
            context = new ApplicationDbContext();            
        }

        #region Assignment        

        public void IsTeacherUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                ViewBag.IsTeacher = s[0].ToString() == "Teacher";
            }            
        }

        // GET: Assignment
        public ActionResult Index()
        {
            IsTeacherUser();
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;                                               
            }
            else            
                ViewBag.Name = "Not Logged IN";            

            return View(context.Assignments);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult Create(int? id)
        {
            var assignment = context.Assignments.FirstOrDefault(c => c.Id == id);
            assignment = assignment ?? new Assignment();
            return View(assignment);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult Create(Assignment assignment)
        {
            if (!ModelState.IsValid)
                return View(assignment);

            if (assignment.Id > 0)
            {
                var dbAssign = context.Assignments.FirstOrDefault(c => c.Id == assignment.Id);
                if(dbAssign!=null)
                {
                    dbAssign.Name = assignment.Name;
                    dbAssign.MaxTime = assignment.MaxTime;
                }
            }
            else
                context.Assignments.Add(assignment);

            context.SaveChanges();
            TempData["AssignmentId"] = assignment.Id;
            return RedirectToAction("Details");                        
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if(!id.HasValue)
                id = Convert.ToInt32(TempData["AssignmentId"]);
            
            IsTeacherUser();
            var assignment = context.Assignments.FirstOrDefault(c=>c.Id == id);
            assignment = assignment ?? new Assignment();
            assignment.Questions = context.Questions.Where(c => c.Assignment.Id == assignment.Id).ToList();
            TempData["AssignmentId"] = assignment.Id;
            return View("Details", assignment);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var assignment = context.Assignments.FirstOrDefault(c => c.Id == id);
            if(assignment!=null)
            {
                context.Assignments.Remove(assignment);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Question

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult CreateQuestion(int? questionId)
        {
            var question = context.Questions.FirstOrDefault(c => c.Id == questionId);
            question = question ?? new Question();
            return View(question);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult CreateQuestion(Question question)
        {
            if (ModelState.IsValid)
            {
                if (question.Id > 0)
                {
                    var questionEntity = context.Questions.FirstOrDefault(c => c.Id == question.Id);
                    if (questionEntity != null)
                    {
                        MapQuestionPropertiesToDatabase(questionEntity, question);         
                    }
                }
                else
                {
                    question.Assignment = GetAssignment();
                    context.Questions.Add(question);
                }
                context.SaveChanges();
            }
           
            return RedirectToAction("Details");
        }



        //[Authorize(Roles = "Student")]
        //public ActionResult Question(int? id)
        //{
        //    var asssignment = context.Assignments.FirstOrDefault(c => c.Id == id);
        //    if (asssignment != null)
        //    {
        //        asssignment.Questions = context.Questions.Where(c => c.Assignment.Id == id).ToList();
        //        var question = asssignment.Questions.Last();

        //        return View(question);
        //    }
        //    return RedirectToAction("Index");
        //}

        //[Authorize(Roles = "Student")]
        //[HttpPost]
        //public ActionResult Question(Question question)
        //{
        //    //var optionFour = collection["OptionFour"];
        //    //var id = collection["Id"];
        //    return RedirectToAction("Index");
        //}

        private void MapQuestionPropertiesToDatabase(Question questionEntity, Question question)
        {
            questionEntity.Mark = question.Mark;
            questionEntity.Description = question.Description;
            questionEntity.OptionOne = question.OptionOne;
            questionEntity.OptionTwo = question.OptionTwo;
            questionEntity.OptionThree = question.OptionThree;
            questionEntity.Answer = question.Answer;

        }

        private Assignment GetAssignment()
        {
            var assessmentId = Convert.ToInt32(TempData["AssignmentId"]);
            var assignment = context.Assignments.FirstOrDefault(c => c.Id == assessmentId);
            return assignment;
        }

        

        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteQuestion(int? questionId)
        {
            var question = context.Questions.FirstOrDefault(c => c.Id == questionId);
            if(question!=null)
            {
                context.Questions.Remove(question);
                context.SaveChanges();
            }
            return RedirectToAction("Details");
        }

        #endregion
    }
}