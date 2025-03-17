using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOS.Requests.History;
using SWP391_CareSkin_BE.DTOS.Responses.History;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class HistoryService : IHistoryService
    {
        private readonly MyDbContext _context;
        private readonly IHistoryRepository _historyRepository;

        public HistoryService(MyDbContext context, IHistoryRepository historyRepository)
        {
            _context = context;
            _historyRepository = historyRepository;
        }

        public async Task<List<HistoryDTO>> CreateHistoriesAsync(int attemptId, List<CreateHistoryDTO> createHistoryDTOs)
        {
            // Validate that the attempt exists
            var attempt = await _context.UserQuizAttempts.FindAsync(attemptId);
            if (attempt == null)
            {
                throw new Exception($"Attempt with ID {attemptId} not found");
            }

            // Validate all questions and answers exist
            foreach (var dto in createHistoryDTOs)
            {
                // Validate that the question exists
                var question = await _context.Questions.FindAsync(dto.QuestionId);
                if (question == null)
                {
                    throw new Exception($"Question with ID {dto.QuestionId} not found");
                }

                // Validate that the answer exists
                var answer = await _context.Answers.FindAsync(dto.AnswerId);
                if (answer == null)
                {
                    throw new Exception($"Answer with ID {dto.AnswerId} not found");
                }
            }

            // Create list to store created histories
            var createdHistories = new List<History>();

            // Process each history entry
            foreach (var dto in createHistoryDTOs)
            {
                // Check if a history record already exists for this attempt and question
                var existingHistory = await _historyRepository.GetHistoryByAttemptAndQuestion(attemptId, dto.QuestionId);

                if (existingHistory != null)
                {
                    // Update the existing history record
                    existingHistory.AnswerId = dto.AnswerId;
                    
                    // Use repository to update history
                    var updatedHistory = await _historyRepository.UpdateHistory(existingHistory);
                    createdHistories.Add(updatedHistory);
                }
                else
                {
                    // Create a new history record
                    var newHistory = HistoryMapper.ToEntity(attemptId, dto);
                    
                    // Use repository to create history
                    var createdHistory = await _historyRepository.CreateHistory(newHistory);
                    createdHistories.Add(createdHistory);
                }
            }

            // Return the created histories as DTOs using mapper
            return createdHistories.Select(h => HistoryMapper.ToDTO(h, true)).ToList();
        }

        public async Task<HistoryDTO> CreateOrUpdateHistoryAsync(int attemptId, CreateHistoryDTO historyDTO)
        {
            // Validate that the attempt exists
            var attempt = await _context.UserQuizAttempts.FindAsync(attemptId);
            if (attempt == null)
            {
                throw new Exception($"Attempt with ID {attemptId} not found");
            }

            // Validate that the question exists
            var question = await _context.Questions.FindAsync(historyDTO.QuestionId);
            if (question == null)
            {
                throw new Exception($"Question with ID {historyDTO.QuestionId} not found");
            }

            // Validate that the answer exists
            var answer = await _context.Answers.FindAsync(historyDTO.AnswerId);
            if (answer == null)
            {
                throw new Exception($"Answer with ID {historyDTO.AnswerId} not found");
            }

            // Check if a history record already exists for this attempt and question
            var existingHistory = await _historyRepository.GetHistoryByAttemptAndQuestion(attemptId, historyDTO.QuestionId);

            if (existingHistory != null)
            {
                // Update the existing history record
                existingHistory.AnswerId = historyDTO.AnswerId;
                
                // Use repository to update history
                var updatedHistory = await _historyRepository.UpdateHistory(existingHistory);
                
                return HistoryMapper.ToDTO(updatedHistory);
            }
            else
            {
                // Create a new history record
                var newHistory = HistoryMapper.ToEntity(attemptId, historyDTO);
                
                // Use repository to create history
                var createdHistory = await _historyRepository.CreateHistory(newHistory);
                
                return HistoryMapper.ToDTO(createdHistory);
            }
        }

        public async Task<List<HistoryDTO>> GetHistoriesByAttemptIdAsync(int attemptId, bool includeDetails = false)
        {
            // Use repository to get histories by attempt ID
            var histories = await _historyRepository.GetHistoriesByAttemptId(attemptId);
            
            // Use mapper to convert to DTOs
            return HistoryMapper.ToDTOList(histories, includeDetails);
        }

        public async Task<HistoryDTO> GetHistoryByIdAsync(int historyId, bool includeDetails = false)
        {
            // Use repository to get history by ID
            var history = await _historyRepository.GetHistoryById(historyId);

            if (history == null)
            {
                throw new Exception($"History with ID {historyId} not found");
            }

            // Use mapper to convert to DTO
            return HistoryMapper.ToDTO(history, includeDetails);
        }
    }
}
