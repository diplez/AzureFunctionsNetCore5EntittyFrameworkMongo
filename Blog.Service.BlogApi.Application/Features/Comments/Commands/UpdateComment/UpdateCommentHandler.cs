﻿using Blog.Service.BlogApi.Domain.Comments;
using Blog.Service.BlogApi.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Service.BlogApi.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, bool>
    {
        private readonly IBlogUnitOfWork _blogUnitOfWork;

        public UpdateCommentHandler(IBlogUnitOfWork blogUnitOfWork)
        {
            _blogUnitOfWork = blogUnitOfWork;
        }

        public async Task<bool> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            Comment entity = _blogUnitOfWork.CommentReadOnlyRepository.Get(request.PostId, request.Id);

            if (entity == null) throw new Exceptions.ApplicationException("No comment is found to update");

            if (!entity.UserId.Equals(request.UserId))
            {
                throw new Exceptions.ApplicationException("Unauthorized to update comment"); //need new exeption model class
            }

            entity.Content = request.UpdateCommentDto.Content;
            entity.UpdatedAt = DateTime.Now;

            var isSucceed = _blogUnitOfWork.CommentCommandRepository.Update(request.PostId, request.Id, entity);

            if (!isSucceed) throw new Exceptions.ApplicationException("Failed to update comment");

            return true;
        }
    }
}
