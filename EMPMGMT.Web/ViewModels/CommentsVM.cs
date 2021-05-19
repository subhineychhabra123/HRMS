using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class CommentsVM
    {

        public int CommentId { get; set; }
        [Required(ErrorMessage = "Comment is required")]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        public Nullable<System.DateTime> CommentDate { get; set; }
        public string CommentBy { get; set; }
        public string CommentTo { get; set; }
        public Nullable<bool> Isdelete { get; set; }
        public string CommentByName { get; set; }


    }
}