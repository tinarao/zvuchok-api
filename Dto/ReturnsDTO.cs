using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services.AuthService;

namespace api.Dto
{
    public interface IReturnsDTO
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class ReturnsDTO : IReturnsDTO
    {
        public required int StatusCode { get; set; }
        public required string Message { get; set; }
    }

    public class ReturnsDTOWithSample : ReturnsDTO
    {
        public required Sample Sample { get; set; }
    }

    public class ReturnsDTOWithUser
    {
        public required int StatusCode { get; set; }
        public required string Message { get; set; }
        public User User { get; set; }
    }
}