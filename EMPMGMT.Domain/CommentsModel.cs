using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public class CommentsModel
    {
        public int CommentId { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CommentDate { get; set; }
        public Nullable<int> CommentBy { get; set; }
        public int CommentTo { get; set; }
        public Nullable<bool> Isdelete { get; set; }
        public string CommentByName { get; set; }
    }
}
