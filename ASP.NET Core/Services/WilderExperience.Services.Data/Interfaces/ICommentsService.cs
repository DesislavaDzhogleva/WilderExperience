namespace WilderExperience.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WilderExperience.Data.Models;
    using WilderExperience.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        IEnumerable<T> GetAll<T>(int experienceId);

        Task<int> AddComment(CommentViewModel input);

        Task DeleteAsync(int id);

        T GetById<T>(int id);

        bool Exists(int id);
        bool IsAuthoredBy(int commentId, string id);
    }
}
