using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Models;

namespace api.Utils
{
    public class ReturnsFabric
    {
        public static IReturnsDTO Ok()
        {
            return new ReturnsDTO
            {
                StatusCode = 200,
                Message = "Ok"
            };
        }

        public static IReturnsDTO Ok(string message)
        {
            return new ReturnsDTO
            {
                StatusCode = 200,
                Message = message
            };
        }

        public static IReturnsDTO BadRequest()
        {
            return new ReturnsDTO
            {
                StatusCode = 400,
                Message = "Bad request"
            };
        }

        public static IReturnsDTO BadRequest(string message)
        {
            return new ReturnsDTO
            {
                StatusCode = 400,
                Message = message
            };
        }

        public static IReturnsDTO NotFound()
        {
            return new ReturnsDTO
            {
                StatusCode = 404,
                Message = "Bad request"
            };
        }

        public static IReturnsDTO NotFound(string message)
        {
            return new ReturnsDTO
            {
                StatusCode = 404,
                Message = message
            };
        }

        public static IReturnsDTO Created(Sample sample)
        {
            return new ReturnsDTOWithSample
            {
                StatusCode = 404,
                Message = "Bad request",
                Sample = sample,
            };
        }

        public static IReturnsDTO InternalServerError()
        {
            return new ReturnsDTO
            {
                StatusCode = 500,
                Message = "Internal server error"
            };
        }
    }
}