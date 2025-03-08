using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly MyDbContext _context;

        public QuestionRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuestionDTO>> GetAllAsync()
        {
            var questions = await _context.Questions.ToListAsync();
            return questions.Select(q => QuestionMapper.ToDTO(q));
        }

        public async Task<QuestionDTO> GetByIdAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            return question != null ? QuestionMapper.ToDTO(question) : null;
        }

        public async Task<QuestionDTO> AddAsync(QuestionDTO questionDTO)
        {
            var question = QuestionMapper.ToEntity(questionDTO);
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return QuestionMapper.ToDTO(question);
        }

        public async Task<QuestionDTO> UpdateAsync(QuestionDTO questionDTO)
        {
            var question = await _context.Questions.FindAsync(questionDTO.QuestionsId);
            if (question == null) return null;

            question.QuestionContext = questionDTO.QuestionContext;
            question.QuizId = questionDTO.QuizId;

            await _context.SaveChangesAsync();
            return QuestionMapper.ToDTO(question);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return false;

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
