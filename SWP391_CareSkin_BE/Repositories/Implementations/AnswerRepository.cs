using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly MyDbContext _context;

        public AnswerRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AnswerDTO>> GetAllAsync()
        {
            var answers = await _context.Answers.ToListAsync();
            return answers.Select(a => AnswerMapper.ToDTO(a));
        }

        public async Task<AnswerDTO> GetByIdAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            return answer != null ? AnswerMapper.ToDTO(answer) : null;
        }

        public async Task<AnswerDTO> AddAsync(AnswerDTO answerDTO)
        {
            var answer = AnswerMapper.ToEntity(answerDTO);
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
            return AnswerMapper.ToDTO(answer);
        }

        public async Task<AnswerDTO> UpdateAsync(AnswerDTO answerDTO)
        {
            var answer = await _context.Answers.FindAsync(answerDTO.AnswerId);
            if (answer == null) return null;

            answer.AnswersContext = answerDTO.AnswersContext;
            answer.PointForSkinType = answerDTO.PointForSkinType;
            answer.QuestionId = answerDTO.QuestionId;

            await _context.SaveChangesAsync();
            return AnswerMapper.ToDTO(answer);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null) return false;

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
