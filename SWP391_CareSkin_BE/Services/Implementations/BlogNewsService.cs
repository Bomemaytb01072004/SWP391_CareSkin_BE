﻿using SWP391_CareSkin_BE.DTOs.Requests.BlogNews;
using SWP391_CareSkin_BE.DTOs.Responses.BlogNews;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Implementations;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class BlogNewsService : IBlogNewsService
    {
        private readonly IBlogNewsRepository _newsRepository;

        public BlogNewsService(IBlogNewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<List<BlogNewsDTO>> GetAllNewsAsync()
        {
            var blog = await _newsRepository.GetAllNewsAsync();
            return blog.Select(BlogNewsMapper.ToDTO).ToList();
        }

        public async Task<BlogNewsDTO> GetNewsByIdAsync(int blogId)
        {
            var blog = await _newsRepository.GetNewsByIdAsync(blogId);
            return BlogNewsMapper.ToDTO(blog);
        }

        public async Task<BlogNewsDTO> GetNewsByNameAsync(string title)
        {
            var blog = await _newsRepository.GetNewsByNameAsync(title);
            return BlogNewsMapper.ToDTO(blog);
        }

        public async Task<BlogNewsDTO> AddNewsAsync(BlogNewsCreateRequest request, string pictureUrl)
        {
            var newsEntity = BlogNewsMapper.ToEntity(request, pictureUrl);
            if(newsEntity == null)
        {
          throw new Exception("Failed to create entity from request"); 
        }
            await _newsRepository.AddNewsAsync(newsEntity);

            var createdNews = _newsRepository.GetNewsByIdAsync(newsEntity.BlogId);
            return BlogNewsMapper.ToDTO(newsEntity);
        }

        public async Task<BlogNewsDTO> UpdateNewsAsync(int blogId, BlogNewsUpdateRequest request)
        {
            var existedNews = await _newsRepository.GetNewsByIdAsync(blogId);
            if (existedNews == null) return null;

            BlogNewsMapper.UpdateEntity(existedNews, request);
            await _newsRepository.UpdateNewsAsync(existedNews);

            var updateNews = await _newsRepository.GetNewsByIdAsync(blogId);
            return BlogNewsMapper.ToDTO(updateNews);

        }

        public async Task<bool> DeleteNewsAsync(int blogId)
        {
            await _newsRepository.DeleteNewsAsync(blogId);
            return true;
        }


    }
}
