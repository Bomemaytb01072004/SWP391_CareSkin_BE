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
        public static ResultDTO ToDTO(Result result)
        {
            var resultDTO = new ResultDTO
            {
                ResultId = result.ResultId,
                CustomerId = result.CustomerId,
                UserQuizAttemptId = result.UserQuizAttemptId,
                SkinTypeId = result.SkinTypeId,
                TotalScore = result.TotalScore,
                TotalQuestions = result.TotalQuestions,
                LastQuizTime = result.LastQuizTime,
                CreatedAt = result.CreatedAt
            };
                resultDTO.SkinType = new SkinTypeDTO    
                {
                    SkinTypeId = result.SkinType.SkinTypeId,
                    TypeName = result.SkinType.TypeName,
                    MinScore = result.SkinType.MinScore,
                    MaxScore = result.SkinType.MaxScore,
                    Description = result.SkinType.Description,
                    IsActive = result.SkinType.IsActive
                };

            return resultDTO;
        }

        public static List<ResultDTO> ToDTOList(IEnumerable<Result> results)
        {
            return results.Select(r => ToDTO(r)).ToList();
        }

        public static Result ToEntity(CreateResultDTO createResultDTO, int totalScore, int totalQuestions, int skinTypeId)
        {
            return new Result
            {
                CustomerId = createResultDTO.CustomerId,
                UserQuizAttemptId = createResultDTO.UserQuizAttemptId,
                SkinTypeId = skinTypeId,
                TotalScore = totalScore,
                TotalQuestions = totalQuestions,
                LastQuizTime = DateOnly.FromDateTime(DateTime.Now),
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };
        }
    }
}
