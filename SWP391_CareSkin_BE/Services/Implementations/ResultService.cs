using SWP391_CareSkin_BE.DTOS.Requests.Result;
using SWP391_CareSkin_BE.DTOS.Responses.Result;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly IUserQuizAttemptRepository _userQuizAttemptRepository;
        private readonly ISkinTypeRepository _skinTypeRepository;
        private readonly IQuizRepository _quizRepository;

        public ResultService(
            IResultRepository resultRepository,
            IUserQuizAttemptRepository userQuizAttemptRepository,
            ISkinTypeRepository skinTypeRepository,
            IQuizRepository quizRepository)
        {
            _resultRepository = resultRepository;
            _userQuizAttemptRepository = userQuizAttemptRepository;
            _skinTypeRepository = skinTypeRepository;
            _quizRepository = quizRepository;
        }

        public async Task<ResultDTO> CreateResultAsync(CreateResultDTO createResultDTO)
        {
            // Validate inputs
            var userQuizAttempt = await _userQuizAttemptRepository.GetByIdAsync(createResultDTO.UserQuizAttemptId, true);
            if (userQuizAttempt == null)
            {
                throw new ArgumentException($"User quiz attempt with ID {createResultDTO.UserQuizAttemptId} not found");
            }

            var quiz = await _quizRepository.GetByIdAsync(createResultDTO.QuizId);
            if (quiz == null)
            {
                throw new ArgumentException($"Quiz with ID {createResultDTO.QuizId} not found");
            }

            // Calculate total score and questions
            int totalScore = 0;
            int totalQuestions = userQuizAttempt.Histories.Count;

            // Sum up the scores from the answers in the histories
            foreach (var history in userQuizAttempt.Histories)
            {
                totalScore += history.Answer.Score;
            }

            // Determine skin type based on score
            // This is a simplified example - you might have a more complex algorithm
            int skinTypeId = DetermineSkinTypeId(totalScore, totalQuestions);

            // Create result entity using mapper
            var resultEntity = ResultMapper.ToEntity(createResultDTO, totalScore, totalQuestions, skinTypeId);
            
            // Save to database
            var createdResult = await _resultRepository.CreateAsync(resultEntity);
            
            // Mark the attempt as completed
            userQuizAttempt.IsCompleted = true;
            userQuizAttempt.CompletedAt = DateTime.Now;
            await _userQuizAttemptRepository.UpdateAsync(userQuizAttempt);
            
            // Return the DTO with related entities
            return ResultMapper.ToDTO(createdResult, true, true);
        }

        public async Task<ResultDTO> UpdateResultScoreAsync(int resultId, int additionalScore)
        {
            // Get the existing result
            var result = await _resultRepository.GetByIdAsync(resultId);
            if (result == null)
            {
                throw new ArgumentException($"Result with ID {resultId} not found");
            }

            // Update the total score
            result.TotalScore += additionalScore;
            
            // Update the last quiz time
            result.LastQuizTime = DateTime.Now;
            
            // Recalculate skin type if needed based on new score
            result.SkinTypeId = DetermineSkinTypeId(result.TotalScore, result.TotalQuestions);
            
            // Save the updated result
            var updatedResult = await _resultRepository.UpdateAsync(result);
            
            // Return the updated result DTO
            return ResultMapper.ToDTO(updatedResult, true, true);
        }

        public async Task<ResultDTO> GetResultByIdAsync(int resultId)
        {
            var result = await _resultRepository.GetByIdAsync(resultId);
            
            if (result == null)
            {
                throw new ArgumentException($"Result with ID {resultId} not found");
            }
            
            return ResultMapper.ToDTO(result, true, true);
        }

        public async Task<List<ResultDTO>> GetResultsByCustomerIdAsync(int customerId)
        {
            var results = await _resultRepository.GetByCustomerIdAsync(customerId);
            return ResultMapper.ToDTOList(results, true, false);
        }

        public async Task<ResultDTO> GetLatestResultByQuizAndCustomerAsync(int quizId, int customerId)
        {
            var result = await _resultRepository.GetLatestByQuizAndCustomerAsync(quizId, customerId);
            
            if (result == null)
            {
                throw new ArgumentException($"No result found for quiz ID {quizId} and customer ID {customerId}");
            }
            
            return ResultMapper.ToDTO(result, true, true);
        }

        // Helper method to determine skin type based on quiz score
        private int DetermineSkinTypeId(int score, int totalQuestions)
        {
            // This is a simplified example - replace with your actual logic
            double scorePercentage = (double)score / totalQuestions;
            
            // Example logic (replace with your actual skin type determination logic):
            if (scorePercentage >= 0.8) return 1; // e.g., Normal skin
            else if (scorePercentage >= 0.6) return 2; // e.g., Dry skin
            else if (scorePercentage >= 0.4) return 3; // e.g., Oily skin
            else if (scorePercentage >= 0.2) return 4; // e.g., Combination skin
            else return 5; // e.g., Sensitive skin
        }
    }
}
