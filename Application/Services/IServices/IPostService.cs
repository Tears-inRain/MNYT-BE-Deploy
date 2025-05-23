﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels;
using Application.Utils;
using Application.ViewModels.Post;

namespace Application.Services.IServices
{
    public interface IPostService
    {
        Task<ReadPostDTO> CreateBlogPostAsync(int authorId, CreatePostDTO dto);
        Task<ReadPostDTO> CreateForumPostAsync(int authorId, CreatePostDTO dto);
        Task<ReadPostDTO?> UpdatePostAsync(int postId, UpdatePostDTO dto, int requestUserId);
        Task<bool> DeletePostAsync(int postId, int requestUserId);
        Task<bool> ChangePostStatusAsync(int postId, int requestAccountId, string status);
        Task<bool> PublishPostAsync(int postId, int requestUserId);
        Task<ReadPostDTO?> GetPostByIdAsync(int postId);
        Task<List<ReadPostDTO>> GetAllBlogPostsAsync();
        Task<List<ReadPostDTO>> GetAllForumPostsAsync();
        Task<PaginatedList<ReadPostDTO>> GetAllForumPostsPaginatedAsync(QueryParameters queryParameters);
        Task<List<ReadPostDTO>> GetAllBlogByCategoryAsync(string category);
        Task<PaginatedList<ReadPostDTO>> GetAllBlogByCategoryPaginatedAsync(string category, QueryParameters queryParameters);
        Task<List<ReadPostDTO>> GetAllForumByCategoryAsync(string category);
        Task<PaginatedList<ReadPostDTO>> GetAllForumByCategoryPaginatedAsync(string category, QueryParameters queryParameters);
        Task<IList<TopAuthorDTO>> GetTopAuthorsAsync();
    }
}
