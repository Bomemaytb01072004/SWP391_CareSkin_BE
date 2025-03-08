using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class QuizRepository : IQuizRepository
    {
        private readonly MyDbContext _context;

        public QuizRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuizDTO>> GetAllAsync()
        {
            var quizzes = await _context.Quizs.ToListAsync();
            return quizzes.Select(q => QuizMapper.ToDTO(q));
        }

        public async Task<QuizDTO> GetByIdAsync(int id)
        {
            var quiz = await _context.Quizs.FindAsync(id);
            return quiz != null ? QuizMapper.ToDTO(quiz) : null;
        }

        public async Task<QuizDTO> AddAsync(QuizDTO quizDTO)
        {
            var quiz = QuizMapper.ToEntity(quizDTO);
            _context.Quizs.Add(quiz);
            await _context.SaveChangesAsync();
            return QuizMapper.ToDTO(quiz);
        }

        public async Task<QuizDTO> UpdateAsync(QuizDTO quizDTO)
        {
            var quiz = await _context.Quizs.FindAsync(quizDTO.QuizId);
            if (quiz == null) return null;

            quiz.Title = quizDTO.Title;
            quiz.Description = quizDTO.Description;

            await _context.SaveChangesAsync();
            return QuizMapper.ToDTO(quiz);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var quiz = await _context.Quizs.FindAsync(id);
            if (quiz == null) return false;

            _context.Quizs.Remove(quiz);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
