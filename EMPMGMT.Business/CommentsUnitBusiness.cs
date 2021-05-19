using EMPMGMT.Repository;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;

using EMPMGMT.Repository.Infrastructure.Contract;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business
{

    public class CommentsUnitBusiness : ICommentsnUnitBusiness
    {


        private readonly CommentsRepository CommentsUnitRepository;



        private readonly IUnitOfWork unitOfWork;
        private readonly UserRepository userRepository;
        private readonly ProfileRepository profileRepository;
        private readonly OrganizationUnitRepository organizationUnitRepository;
        public CommentsUnitBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            userRepository = new UserRepository(unitOfWork);
            profileRepository = new ProfileRepository(unitOfWork);
            organizationUnitRepository = new OrganizationUnitRepository(unitOfWork);
            CommentsUnitRepository = new CommentsRepository(unitOfWork);
        }




        public List<CommentsModel> GetCommentList(int loginuserid, int userid)
        {
            Employee user = new Employee();
            List<CommentsModel> usercomments = new List<CommentsModel>();
            List<Comments> listObjComment = CommentsUnitRepository.GetAll(r => r.CommentBy == loginuserid && r.CommentTo == userid && r.Isdelete == false).ToList();

            AutoMapper.Mapper.Map(listObjComment, usercomments);
            foreach (var usercommentsitem in usercomments)
            {
                user=userRepository.SingleOrDefault(x=>x.UserId==loginuserid);
                string fullname=user.FirstName+" "+user.LastName;
                usercommentsitem.CommentByName = fullname;

            }
            return usercomments;
            //usercomments = CommentsUnitRepository.GetAll(r => r.CommentBy == loginuserid && r.CommentTo == userid && r.Isdelete == false).Select(x => new CommentsModel() { Comment = x.Comment, CommentDate = x.CommentDate, CommentId = x.CommentId, CommentBy = x.CommentBy, CommentTo = x.CommentTo, Isdelete = x.Isdelete }).ToList();
            //return usercomments;
        }

        public bool SaveComment(CommentsModel cm)
        {

            Comments comment = new Comments();
            //AutoMapper.Mapper.Map(userModel, user);
            comment.CommentDate = DateTime.UtcNow;
            comment.CommentBy = (int)cm.CommentBy;
            comment.CommentTo = (int)cm.CommentTo;
            comment.Comment = cm.Comment;
            comment.Isdelete = cm.Isdelete;
            if (cm.CommentId == 0)
            {
                CommentsUnitRepository.Insert(comment);
            }
            else
            {
                comment.CommentId = (int)cm.CommentId;
                CommentsUnitRepository.Update(comment);
            }
            return true;

        }

        public bool DeleteComment(CommentsModel cm)
        {




            Comments comment = CommentsUnitRepository.SingleOrDefault(u => u.CommentId == cm.CommentId);
            if (comment != null)
            {
                comment.Isdelete = true;

                CommentsUnitRepository.Update(comment);

            }

            return true;

        }
    }
}
