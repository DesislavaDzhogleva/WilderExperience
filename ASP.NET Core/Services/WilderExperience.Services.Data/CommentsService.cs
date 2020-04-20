namespace WilderExperience.Services.Data
{
    using System;
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

        public async Task DeleteAsync(int id)
        {
            var comment = this.commentsRepository.All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (comment == null)
            {
                throw new ArgumentNullException("Comment not found");
            }

            this.commentsRepository.Delete(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int experienceId)
        {
            var comments = this.commentsRepository.All()
                .Where(x => x.ExperienceId == experienceId)
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

        public T GetById<T>(int id)
        {
            var comment = this.commentsRepository.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return comment;
        }

        public bool Exists(int id)
        {
            return this.commentsRepository.All()
                .Where(x => x.Id == id).Count() == 1;
        }

        public bool IsAuthoredBy(int commentId, string id)
        {
            return this.commentsRepository.All()
                .Where(x => x.Id == commentId && x.UserId == id).Count() == 1;
        }
    }
}
