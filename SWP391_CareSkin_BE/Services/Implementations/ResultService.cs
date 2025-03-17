using SWP391_CareSkin_BE.DTOS.Requests.Result;
using SWP391_CareSkin_BE.DTOS.Responses.Result;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
            int skinTypeId = await DetermineSkinTypeId(totalScore);

            // Create result entity using mapper
            var resultEntity = ResultMapper.ToEntity(createResultDTO, totalScore, totalQuestions, skinTypeId);
            
            // Save to database
            var createdResult = await _resultRepository.CreateAsync(resultEntity);
            
            // Mark the attempt as completed
            userQuizAttempt.IsCompleted = true;
            userQuizAttempt.CompletedAt = DateTime.Now;
            await _userQuizAttemptRepository.UpdateAsync(userQuizAttempt);
            
            // Return the DTO with related entities
            return ResultMapper.ToDTO(createdResult);
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
            result.SkinTypeId = await DetermineSkinTypeId(result.TotalScore);
            
            // Save the updated result
            var updatedResult = await _resultRepository.UpdateAsync(result);
            
            // Return the updated result DTO
            return ResultMapper.ToDTO(updatedResult);
        }

        public async Task<ResultDTO> GetResultByIdAsync(int resultId)
        {
            var result = await _resultRepository.GetByIdAsync(resultId);
            
            if (result == null)
            {
                throw new ArgumentException($"Result with ID {resultId} not found");
            }
            
            return ResultMapper.ToDTO(result);
        }

        public async Task<List<ResultDTO>> GetResultsByCustomerIdAsync(int customerId)
        {
            var results = await _resultRepository.GetByCustomerIdAsync(customerId);
            return ResultMapper.ToDTOList(results);
        }

        // Helper method to determine skin type based on quiz score
        private async Task<int> DetermineSkinTypeId(int score)
        {
            // Get all skin types from the database
            var skinTypes = await _skinTypeRepository.GetAllAsync();
            
            // Find the skin type where the score falls between MinScore and MaxScore
            var matchingSkinType = skinTypes.FirstOrDefault(st => score >= st.MinScore && score <= st.MaxScore);
            
            // If no matching skin type is found, return the one with the closest score range
            if (matchingSkinType == null)
            {
                // Find the skin type with the closest min/max range to the score
                matchingSkinType = skinTypes
                    .OrderBy(st => Math.Min(Math.Abs(st.MinScore - score), Math.Abs(st.MaxScore - score)))
                    .FirstOrDefault();
            }
            
            return matchingSkinType?.SkinTypeId ?? 1; // Default to ID 1 if no skin type found
        }
    }
}
