using CommentProject.DataAccessLayer.Abstract;
using CommentProject.DataAccessLayer.Repositories;
using CommentProject.EntityLayer.Concrete;

namespace CommentProject.DataAccessLayer.EntityFramework
{
    public class EfTitleDal:GenericRepository<Title>, ITitleDal
    {
    }
}
