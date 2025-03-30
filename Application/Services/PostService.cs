using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.ViewModels;
using Application.Services.IServices;
using System.Net;
using Application.ViewModels.Media;
using Application.ViewModels.Post;
using Application.Common.Utils;


namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PostService> _logger;

        public PostService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PostService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReadPostDTO> CreateBlogPostAsync(int authorId, CreatePostDTO dto)
        {
            _logger.LogInformation("Creating a blog post for AuthorId: {AuthorId}", authorId);

            var post = _mapper.Map<BlogPost>(dto);
            post.AuthorId = authorId;
            post.Status = "Draft";
            post.TypeEnum = Domain.Enums.PostType.Blog;

            await _unitOfWork.PostRepo.AddAsync(post);
            await _unitOfWork.SaveChangesAsync();

            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var imageDto in dto.Images)
                {
                    var media = _mapper.Map<Media>(imageDto);
                    media.EntityType = "Post";
                    media.EntityId = post.Id;
                    media.Type = "image";

                    await _unitOfWork.MediaRepo.AddAsync(media);
                }

                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Successfully created BlogPost Id: {PostId}", post.Id);

            return await AttachMediaAndMapSingleAsync(post);
        }

        public async Task<ReadPostDTO> CreateForumPostAsync(int authorId, CreatePostDTO dto)
        {
            _logger.LogInformation("Creating a forum post for AuthorId: {AuthorId}", authorId);

            var post = _mapper.Map<BlogPost>(dto);
            post.AuthorId = authorId;
            post.Status = "Draft";

            await _unitOfWork.PostRepo.AddAsync(post);
            await _unitOfWork.SaveChangesAsync();

            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var imageDto in dto.Images)
                {
                    var media = _mapper.Map<Media>(imageDto);
                    media.EntityType = "Post";
                    media.EntityId = post.Id;
                    media.Type = "image";

                    await _unitOfWork.MediaRepo.AddAsync(media);
                }

                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Successfully created ForumPost Id: {PostId}", post.Id);

            return await AttachMediaAndMapSingleAsync(post);
        }

        public async Task<ReadPostDTO?> UpdatePostAsync(int postId, UpdatePostDTO dto, int requestAccountId)
        {
            _logger.LogInformation("Updating postId: {PostId} by accountId: {AccountId}", postId, requestAccountId);

            var post = await _unitOfWork.PostRepo.GetByIdAsync(postId);
            if (post == null)
            {
                _logger.LogWarning("Post Id: {PostId} not found or is deleted", postId);
                return null;
            }

            if (post.AuthorId != requestAccountId)
            {
                _logger.LogWarning("Account Id: {AccountId} is not the author of postId: {PostId}", requestAccountId, postId);
                return null;
            }
            if (!string.IsNullOrWhiteSpace(dto.Category))
                post.Category = dto.Category;
            if (!string.IsNullOrWhiteSpace(dto.Title))
                post.Title = dto.Title;
            if (!string.IsNullOrWhiteSpace(dto.Description))
                post.Description = dto.Description;
            if (dto.Period.HasValue)
                post.Period = dto.Period.Value;
            //if (dto.TypeEnum.HasValue) post.TypeEnum = dto.TypeEnum.Value;

            _unitOfWork.PostRepo.Update(post);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully updated Post Id: {PostId}", post.Id);

            return await AttachMediaAndMapSingleAsync(post);
        }

        public async Task<bool> DeletePostAsync(int postId, int requestAccountId)
        {
            _logger.LogInformation("Deleting postId: {PostId} by accountId: {AccountId}", postId, requestAccountId);

            var post = await _unitOfWork.PostRepo.GetByIdAsync(postId);
            if (post == null)
            {
                _logger.LogWarning("Post Id: {PostId} not found or is deleted", postId);
                return false;
            }

            if (post.AuthorId != requestAccountId)
            {
                _logger.LogWarning("Account Id: {AccountId} not allowed to delete postId: {PostId}", requestAccountId, postId);
                return false;
            }

            _unitOfWork.PostRepo.SoftDelete(post);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully soft-deleted Post Id: {PostId}", post.Id);
            return true;
        }

        public async Task<bool> ChangePostStatusAsync(int postId, int requestAccountId, string status)
        {
            _logger.LogInformation("Change postId: {PostId} status by accountId: {AccountId}", postId, requestAccountId);

            var post = await _unitOfWork.PostRepo.GetByIdAsync(postId);
            if (post == null)
            {
                _logger.LogWarning("Post Id: {PostId} not found", postId);
                return false;
            }

            post.Status = status;

            _unitOfWork.PostRepo.Update(post);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully change Post Id: {PostId} status", post.Id);
            return true;
        }

        public async Task<bool> PublishPostAsync(int postId, int requestAccountId)
        {
            _logger.LogInformation("Publishing postId: {PostId} by accountId: {AccountId}", postId, requestAccountId);

            var post = await _unitOfWork.PostRepo.GetByIdAsync(postId);
            if (post == null)
            {
                _logger.LogWarning("Post Id: {PostId} not found", postId);
                return false;
            }

            if (post.AuthorId != requestAccountId)
            {
                _logger.LogWarning("Account Id: {AccountId} not allowed to publish postId: {PostId}", requestAccountId, postId);
                return false;
            }

            post.Status = "Published";
            post.PublishedDay = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.PostRepo.Update(post);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully published Post Id: {PostId}", post.Id);
            return true;
        }

        public async Task<ReadPostDTO?> GetPostByIdAsync(int postId)
        {
            var post = await _unitOfWork.PostRepo
                .GetAllQueryable("Author,BlogLikes,BlogBookmarks,Comments")
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                _logger.LogWarning("Post Id: {PostId} not found or is deleted", postId);
                return null;
            }

            return await AttachMediaAndMapSingleAsync(post);
        }

        public async Task<List<ReadPostDTO>> GetAllForumPostsAsync()
        {
            var forumPosts = await _unitOfWork.PostRepo
                .GetAllQueryable("Author,BlogLikes,BlogBookmarks,Comments")
                .Where(post => post.TypeEnum == Domain.Enums.PostType.Forum)
                .ToListAsync();

            return await AttachMediaAndMapAsync(forumPosts);
        }

        public async Task<List<ReadPostDTO>> GetAllBlogPostsAsync()
        {
            var blogPosts = await _unitOfWork.PostRepo
                .GetAllQueryable("Author,BlogLikes,BlogBookmarks,Comments")
                .Where(post => post.TypeEnum == Domain.Enums.PostType.Blog)
                .ToListAsync();

            return await AttachMediaAndMapAsync(blogPosts);
        }

        public async Task<PaginatedList<ReadPostDTO>> GetAllForumPostsPaginatedAsync(QueryParameters queryParameters)
        {
            var query = _unitOfWork.PostRepo.GetAllQueryable("Author,BlogLikes,BlogBookmarks,Comments")
                .Where(post => post.TypeEnum == Domain.Enums.PostType.Forum);

            var pagedEntities = await PaginatedList<BlogPost>.CreateAsync(
                query.OrderByDescending(p => p.CreateDate),
                queryParameters.PageNumber,
                queryParameters.PageSize
            );

            var dtosWithImages = await AttachMediaAndMapAsync(pagedEntities.Items);

            return new PaginatedList<ReadPostDTO>(
                dtosWithImages,
                pagedEntities.TotalCount,
                pagedEntities.PageIndex,
                pagedEntities.Items.Count
            );
        }

        public async Task<List<ReadPostDTO>> GetAllForumByCategoryAsync(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new Exceptions.ApplicationException(HttpStatusCode.BadRequest, "Category cannot be null or empty.");
            }

            var forumPosts = await _unitOfWork.PostRepo
                .GetAllQueryable("Author,BlogLikes,BlogBookmarks,Comments")
                .Where(p => p.Category == category
                            && p.TypeEnum == Domain.Enums.PostType.Forum)
                .ToListAsync();

            return await AttachMediaAndMapAsync(forumPosts);
        }

        public async Task<PaginatedList<ReadPostDTO>> GetAllForumByCategoryPaginatedAsync(string category, QueryParameters queryParameters)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new Exceptions.ApplicationException(HttpStatusCode.BadRequest, "Category cannot be null or empty.");
            }

            var query = _unitOfWork.PostRepo
                .GetAllQueryable("Author,BlogLikes,BlogBookmarks,Comments")
                .Where(p => p.Category == category
                            && p.TypeEnum == Domain.Enums.PostType.Forum);

            var pagedEntities = await PaginatedList<BlogPost>.CreateAsync(
                query.OrderByDescending(p => p.CreateDate),
                queryParameters.PageNumber,
                queryParameters.PageSize
            );

            var dtosWithImages = await AttachMediaAndMapAsync(pagedEntities.Items);

            return new PaginatedList<ReadPostDTO>(
                dtosWithImages,
                pagedEntities.TotalCount,
                pagedEntities.PageIndex,
                pagedEntities.Items.Count
            );
        }


        public async Task<List<ReadPostDTO>> GetAllBlogByCategoryAsync(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new Exceptions.ApplicationException(HttpStatusCode.BadRequest, "Category cant not be null or empty.");
            }

            var blogPosts = await _unitOfWork.PostRepo
                .GetAllQueryable("Author,BlogLikes,BlogBookmarks,Comments")
                .Where(p => p.Category == category && p.TypeEnum == Domain.Enums.PostType.Blog)
                .ToListAsync();

            return await AttachMediaAndMapAsync(blogPosts);
        }

        public async Task<PaginatedList<ReadPostDTO>> GetAllBlogByCategoryPaginatedAsync(string category, QueryParameters queryParameters)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new Exceptions.ApplicationException(HttpStatusCode.BadRequest, "Category cannot be null or empty.");
            }

            var query = _unitOfWork.PostRepo
                .GetAllQueryable("Author,BlogLikes,BlogBookmarks,Comments")
                .Where(p => p.Category == category
                            && p.TypeEnum == Domain.Enums.PostType.Blog);

            var pagedEntities = await PaginatedList<BlogPost>.CreateAsync(
                query.OrderByDescending(p => p.CreateDate),
                queryParameters.PageNumber,
                queryParameters.PageSize
            );

            var dtosWithImages = await AttachMediaAndMapAsync(pagedEntities.Items);

            return new PaginatedList<ReadPostDTO>(
                dtosWithImages,
                pagedEntities.TotalCount,
                pagedEntities.PageIndex,
                pagedEntities.Items.Count
            );
        }


        public async Task<IList<TopAuthorDTO>> GetTopAuthorsAsync()
        {
            return await _unitOfWork.PostRepo.GetTopAuthorAsync(3);
        }

        private async Task<ReadPostDTO?> AttachMediaAndMapSingleAsync(BlogPost post)
        {
            var images = await _unitOfWork.MediaRepo.GetAllQueryable()
                .Where(m => m.EntityType == "Post" && m.EntityId == post.Id)
                .ToListAsync();

            var postDto = _mapper.Map<ReadPostDTO>(post);
            postDto.Images = _mapper.Map<List<ReadMediaDTO>>(images);

            return postDto;
        }

        private async Task<List<ReadPostDTO>> AttachMediaAndMapAsync(List<BlogPost> posts)
        {
            if (!posts.Any())
                return new List<ReadPostDTO>();

            var postIds = posts.Select(p => p.Id).ToList();

            var mediaList = await _unitOfWork.MediaRepo.GetAllQueryable()
                .Where(m => m.EntityType == "Post" && m.EntityId.HasValue && postIds.Contains(m.EntityId.Value))
                .ToListAsync();

            var mediaGrouped = mediaList
                .GroupBy(m => m.EntityId)
                .ToDictionary(g => g.Key!.Value, g => g.ToList());

            var mappedDtos = _mapper.Map<List<ReadPostDTO>>(posts);

            foreach (var postDto in mappedDtos)
            {
                if (mediaGrouped.ContainsKey(postDto.Id))
                {
                    var relatedMedia = mediaGrouped[postDto.Id];
                    postDto.Images = _mapper.Map<List<ReadMediaDTO>>(relatedMedia);
                }
                else
                {
                    postDto.Images = new List<ReadMediaDTO>();
                }
            }

            return mappedDtos;
        }
    }
}