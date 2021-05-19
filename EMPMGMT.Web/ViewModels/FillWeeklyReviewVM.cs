using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using EMPMGMT.Utility;

namespace EMPMGMT.Web.ViewModels
{
    public class FillReviewVM
    {
        public int UserId { get { return SessionManagement.LoggedInUser.UserId; } }

        public int CurrentDesignation { get { return SessionManagement.LoggedInUser.CurrentDesignationId; } }

        [Required(ErrorMessage = "Week is required")]
        public DateTime FilledForWeek { get; set; }

        public List<ReviewQuestionVM> QuestionsVM { get; set; }
    }
}