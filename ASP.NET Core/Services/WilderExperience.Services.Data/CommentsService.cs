namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Comments;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var comments = this.commentsRepository.All()
                .To<T>();

            return comments;
        }

        public async Task<int> AddComment(CommentViewModel input)
        {
            var comment = new Comment()
            {
                ParentId = input.ParentId,
                Content = input.Content,
                UserId = input.UserId,
                CreatedOn = input.CreatedOn,
                ExperienceId = input.ExperienceId,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();

            return comment.Id;
        }
    }
}
