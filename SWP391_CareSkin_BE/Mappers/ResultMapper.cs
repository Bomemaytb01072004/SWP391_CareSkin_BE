using SWP391_CareSkin_BE.DTOS.Requests.Result;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.DTOS.Responses.Result;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class ResultMapper
    {
        public static ResultDTO ToDTO(Result result, bool includeSkinType = false, bool includeUserQuizAttempt = false)
        {
            var resultDTO = new ResultDTO
            {
                ResultId = result.ResultId,
                CustomerId = result.CustomerId,
                QuizId = result.QuizId,
                UserQuizAttemptId = result.UserQuizAttemptId,
                SkinTypeId = result.SkinTypeId,
                TotalScore = result.TotalScore,
                TotalQuestions = result.TotalQuestions,
                LastQuizTime = result.LastQuizTime,
                CreatedAt = result.CreatedAt
            };

            if (includeSkinType && result.SkinType != null)
            {
                resultDTO.SkinType = new SkinTypeDTO    
                {
                    SkinTypeId = result.SkinType.SkinTypeId,
                    TypeName = result.SkinType.TypeName,
                    Description = result.SkinType.Description
                };
            }
            
            if (includeUserQuizAttempt && result.UserQuizAttempt != null)
            {
                resultDTO.UserQuizAttempt = UserQuizAttemptMapper.ToDTO(result.UserQuizAttempt, true);
            }

            return resultDTO;
        }

        public static List<ResultDTO> ToDTOList(IEnumerable<Result> results, bool includeSkinType = false, bool includeUserQuizAttempt = false)
        {
            return results.Select(r => ToDTO(r, includeSkinType, includeUserQuizAttempt)).ToList();
        }

        public static Result ToEntity(CreateResultDTO createResultDTO, int totalScore, int totalQuestions, int skinTypeId)
        {
            return new Result
            {
                CustomerId = createResultDTO.CustomerId,
                QuizId = createResultDTO.QuizId,
                UserQuizAttemptId = createResultDTO.UserQuizAttemptId,
                SkinTypeId = skinTypeId,
                TotalScore = totalScore,
                TotalQuestions = totalQuestions,
                LastQuizTime = DateTime.Now,
                CreatedAt = DateTime.Now
            };
        }
    }
}
