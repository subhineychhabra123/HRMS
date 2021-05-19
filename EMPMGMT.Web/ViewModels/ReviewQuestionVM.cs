using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using EMPMGMT.Utility;

namespace EMPMGMT.Web.ViewModels
{
    public class ReviewQuestionVM
    {
        public int QuestionId { get; set; }
        public int VisibleToDesignationId { get; set; }
        public Enums.OptionType OptionType { get; set; }
        public string QuestionText { get; set; }
        public List<OptionVM> OptionsVM
        {
            get;
            set;
        }
    }


}