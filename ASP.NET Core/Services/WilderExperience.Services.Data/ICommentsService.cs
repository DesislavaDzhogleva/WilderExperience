namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WilderExperience.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        IEnumerable<T> GetAll<T>();

        Task<int> AddComment(CommentViewModel input);
    }
}
