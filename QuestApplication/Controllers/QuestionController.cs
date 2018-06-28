using QuestApplication.Entities;
using QuestApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestApplication.Controllers
{
    public class QuestionController : Controller
    {
        ApplicationDbContext context;
        static List<Question> questions;
        static int marks;

        public QuestionController()
        {
            ViewBag.Answer = null;
            context = new ApplicationDbContext();
        }
      
        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult Question(int? assessmentId, Question question= null)
        {
            if(question.Id > 0)
            {
                return View(question);
            }
            var asssignment = context.Assignments.FirstOrDefault(c => c.Id == assessmentId);
            if (asssignment != null)
            {
                questions = context.Questions.Where(c => c.Assignment.Id == assessmentId).ToList();
                questions.ForEach(c => c.SerialNo = questions.IndexOf(c) + 1);
                return View(questions.FirstOrDefault());
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public ActionResult Question(Question question)
        {
            if(question.OptionFour == question.Answer)
            {
                ViewBag.Answer = "Correct Answer";
                marks++;
            }
            else
            {
                ViewBag.Answer = "Wrong Answer";
                marks--;
            }
            return View(question);
        }

        [HttpGet]
        public ActionResult NextQuestion(int? serialNo)
        {
            if (serialNo <= questions.Count)
            {
                var question = questions.FirstOrDefault(c => c.SerialNo == serialNo);
                return RedirectToAction("Question", question);
            }
            else
            {                              
                return RedirectToAction("Result");
            }
            
        }

        public ActionResult Result()
        {            
            ViewBag.Remark = "Congratulations. You have scored " + marks + " out of " + questions.Count();
            return View();
        }
    }
}