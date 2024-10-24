using backend.Helpers;
using backend.Models;

namespace backend.Dtos
{
    public class AttendeeRegisterDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }


    public class AttendeeBuyTicketDto
    {
        public string? Email { get; set; }
    }

    public class FlutterwaveResponseDto
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public Data? Data { get; set; }

    }

    public class Data
    {
        public Customer? Customer { get; set; }
    }

    public class Customer
    {
        public string? Name { get; set; }
        public string? Phone_number { get; set; }
        public string? Email { get; set; }
        public DateTime Created_at { get; set; }

    }

    public class ReBuyTicketDto
    {
        public string? Email { get; set; }
        public string? TransactionId { get; set; }
    }











}