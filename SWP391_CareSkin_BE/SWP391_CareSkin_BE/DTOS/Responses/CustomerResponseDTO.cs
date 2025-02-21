﻿namespace SWP391_CareSkin_BE.DTOS.Responses
{
    public class CustomerResponseDTO
    {
        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateOnly? Dob { get; set; }
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }
        public string Address { get; set; }
    }
}
