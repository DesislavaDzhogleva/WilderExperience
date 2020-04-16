namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WilderExperience.Data.Models;
    using WilderExperience.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        IEnumerable<T> GetAll<T>(int experienceId);

        Task<int> AddComment(CommentViewModel input);

        Comment GetOriginalById(int id);

        Task DeleteAsync(Comment input);

        T GetById<T>(int id);

        bool Exists(int id);
    }
}
