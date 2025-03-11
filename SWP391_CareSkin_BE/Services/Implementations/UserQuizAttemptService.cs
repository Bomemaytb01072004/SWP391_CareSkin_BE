using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.DTOS.Requests.UserQuizAttempt;
using SWP391_CareSkin_BE.DTOS.Responses.UserQuizAttempt;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class UserQuizAttemptService : IUserQuizAttemptService
    {
        private readonly IUserQuizAttemptRepository _userQuizAttemptRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly ICustomerRepository _customerRepository;

        public UserQuizAttemptService(
            IUserQuizAttemptRepository userQuizAttemptRepository,
            IQuizRepository quizRepository,
            ICustomerRepository customerRepository)
        {
            _userQuizAttemptRepository = userQuizAttemptRepository;
            _quizRepository = quizRepository;
            _customerRepository = customerRepository;
        }

        public async Task<UserQuizAttemptDTO> CreateUserQuizAttemptAsync(CreateUserQuizAttemptDTO createUserQuizAttemptDTO)
        {
            // Validate quiz and customer exist
            var quizExists = await _quizRepository.ExistsAsync(createUserQuizAttemptDTO.QuizId);
            if (!quizExists)
            {
                throw new ArgumentException($"Quiz with ID {createUserQuizAttemptDTO.QuizId} not found");
            }

            var customerExists = await _customerRepository.GetCustomerByIdAsync(createUserQuizAttemptDTO.CustomerId);
            if (customerExists == null)
            {
                throw new ArgumentException($"Customer with ID {createUserQuizAttemptDTO.CustomerId} not found");
            }

            // Get the attempt number for this quiz and customer
            var attemptNumber = await _userQuizAttemptRepository.GetAttemptNumberAsync(
                createUserQuizAttemptDTO.QuizId, 
                createUserQuizAttemptDTO.CustomerId);

            // Create the attempt entity
            var attemptEntity = UserQuizAttemptMapper.ToEntity(createUserQuizAttemptDTO, attemptNumber);
            
            // Save to database
            var createdAttempt = await _userQuizAttemptRepository.CreateAsync(attemptEntity);
            
            // Return the DTO
            return UserQuizAttemptMapper.ToDTO(createdAttempt);
        }

        public async Task<UserQuizAttemptDTO> GetUserQuizAttemptByIdAsync(int attemptId, bool includeHistories = false)
        {
            var attempt = await _userQuizAttemptRepository.GetByIdAsync(attemptId, includeHistories);
            
            if (attempt == null)
            {
                throw new ArgumentException($"User quiz attempt with ID {attemptId} not found");
            }
            
            return UserQuizAttemptMapper.ToDTO(attempt, includeHistories);
        }

        public async Task<List<UserQuizAttemptDTO>> GetUserQuizAttemptsByCustomerIdAsync(int customerId, bool includeHistories = false)
        {
            var customerExists = await _userQuizAttemptRepository.ExistsAsync(customerId);
            if (!customerExists)
            {
                throw new ArgumentException($"Customer with ID {customerId} not found");
            }
            
            var attempts = await _userQuizAttemptRepository.GetByCustomerIdAsync(customerId, includeHistories);
            return UserQuizAttemptMapper.ToDTOList(attempts, includeHistories);
        }

        public async Task<List<UserQuizAttemptDTO>> GetUserQuizAttemptsByQuizAndCustomerAsync(int quizId, int customerId, bool includeHistories = false)
        {
            var quizExists = await _quizRepository.ExistsAsync(quizId);
            if (!quizExists)
            {
                throw new ArgumentException($"Quiz with ID {quizId} not found");
            }

            var customerExists = await _userQuizAttemptRepository.ExistsAsync(customerId);
            if (!customerExists)
            {
                throw new ArgumentException($"Customer with ID {customerId} not found");
            }
            
            var attempts = await _userQuizAttemptRepository.GetByQuizAndCustomerAsync(quizId, customerId, includeHistories);
            return UserQuizAttemptMapper.ToDTOList(attempts, includeHistories);
        }

        public async Task<UserQuizAttemptDTO> CompleteUserQuizAttemptAsync(int attemptId)
        {
            var attempt = await _userQuizAttemptRepository.GetByIdAsync(attemptId);
            
            if (attempt == null)
            {
                throw new ArgumentException($"User quiz attempt with ID {attemptId} not found");
            }
            
            if (attempt.IsCompleted)
            {
                throw new InvalidOperationException($"User quiz attempt with ID {attemptId} is already completed");
            }
            
            // Mark the attempt as completed
            var updatedAttempt = UserQuizAttemptMapper.CompleteAttempt(attempt);
            
            // Save to database
            await _userQuizAttemptRepository.UpdateAsync(updatedAttempt);
            
            // Return the updated DTO
            return UserQuizAttemptMapper.ToDTO(updatedAttempt);
        }
    }
}
