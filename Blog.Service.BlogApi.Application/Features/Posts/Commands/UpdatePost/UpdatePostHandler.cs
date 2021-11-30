using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Service.BlogApi.Domain.Posts;
using Blog.Service.BlogApi.Domain.Repositories;
using MediatR;

namespace Blog.Service.BlogApi.Application.Features.Posts.Commands.UpdatePost
{
    public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, bool>
    {
        private readonly IBlogUnitOfWork _blogUnitOfWork;

        public UpdatePostHandler(IBlogUnitOfWork blogUnitOfWork)
        {
            _blogUnitOfWork = blogUnitOfWork;
        }

        public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            Post entity = _blogUnitOfWork.PostReadOnlyRepository.Get(request.Id);

            if (entity == null) throw new Exceptions.ItemNotFoundException("No Post is found to update");

            if (!entity.UserId.Equals(request.UserId))
            {
                throw new  Exceptions.ApplicationException("Unauthorized to update"); //need new exeption model class
            }

            entity.Content = request.UpdatePostDto.Content;
            entity.UpdatedAt = DateTime.Now;
            entity.Uploads = request.UpdatePostDto.Uploads == null || request.UpdatePostDto.Uploads.Count == 0 ? entity.Uploads : request.UpdatePostDto.Uploads;

            var isSucceed = _blogUnitOfWork.PostCommandRepository.Update(request.Id, entity);

            if (!isSucceed) throw new Exceptions.ApplicationException("Post is not updated successfully");

            return true;
        }
    }
}
