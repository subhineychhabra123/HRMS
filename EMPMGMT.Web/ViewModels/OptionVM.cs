using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using EMPMGMT.Utility;

namespace EMPMGMT.Web.ViewModels
{
    public class OptionVM
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string OptionText { get; set; }
    }
}