using EMPMGMT.Domain;
using EMPMGMT.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EMPMGMT.Business.Interfaces
{
    public interface ICommentsnUnitBusiness
    {

        List<CommentsModel> GetCommentList(int loginuserid, int userid);

       bool SaveComment(CommentsModel cm);
       bool DeleteComment(CommentsModel cm);
      


    }
}
