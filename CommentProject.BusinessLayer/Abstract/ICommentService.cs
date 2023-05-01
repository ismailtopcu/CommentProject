using CommentProject.EntityLayer.Concrete;

namespace CommentProject.BusinessLayer.Abstract
{
    public interface ICommentService : IGenericService<Comment>
    {
        List<Comment> TGetCommentByTitle(int id);
        List<Comment> TGetCommentsByTitleWithUser(int id);
        public List<Comment> TGetCommentsByUserWithTitle(int id);
    }
}
