using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.DTOs.Requests.FAQ;

namespace SWP391_CareSkin_BE.Mappers
{
    public class FAQMapper
    {
        // Chuyển đổi từ CreateFAQDTO sang Model FAQ
        public static FAQ ToFAQ(CreateFAQDTO dto)
        {
            return new FAQ
            {
                Question = dto.Question,
                Answer = dto.Answer
            };
        }

        // Chuyển đổi từ Model FAQ sang DTO ShowFAQDTO
        public static ShowFAQDTO ToShowFAQDTO(FAQ faq)
        {
            return new ShowFAQDTO
            {
                FAQId = faq.FAQId,
                Question = faq.Question,
                Answer = faq.Answer
            };
        }

        // Chuyển đổi danh sách FAQ sang danh sách ShowFAQDTO
        public static List<ShowFAQDTO> ToShowFAQDTOList(List<FAQ> faqs)
        {
            return faqs.Select(ToShowFAQDTO).ToList();
        }

        // Cập nhật dữ liệu từ DTO vào model FAQ
        public static void UpdateFAQ(FAQ faq, UpdateFAQDTO dto)
        {
            if (!string.IsNullOrEmpty(dto.Question))
            {
                faq.Question = dto.Question;
            }
            if (!string.IsNullOrEmpty(dto.Answer))
            {
                faq.Answer = dto.Answer;
            }
        }
    }
}
