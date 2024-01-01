﻿using Dapper;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> DeleteById(int Id)
        {
            var status = await _dbConnection.ExecuteAsync($"delete from discount where id={Id}");
            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount not found",404);
        }

        public async Task<Response<List<Models.Discount>>> GetAll()
        {
            var discount = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");
            return Response<List<Models.Discount>>.Success(discount.ToList(), 200);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where userId=@UserId and code=@Code", new
            {
                UserId = userId,
                Code = code
            });
            var hasDiscount = discount.FirstOrDefault();
            return hasDiscount == null ? Response<Models.Discount>.Fail("Not Found",404) : Response<Models.Discount>.Success(hasDiscount,200);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("select *from discount where id =@id", new { Id = id })).SingleOrDefault();
            if (discount == null)
            {
                return Response<Models.Discount>.Success(discount, 404);
            }
            return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(Models.Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("Insert into discount (userId,rate,code) values(@UserId,@Rate ,@Code)",discount);
            if (saveStatus>0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("an error while adding",500);
        }

        public async Task<Response<NoContent>> Update(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("Update discount set userId=@UserId,code=@Code ,rate =@Rate where id=@Id", new
            {
                id=discount.Id,
                userId = discount.UserId,
                code=discount.Code,
                rate=discount.Rate,
            });
            if (status>0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("discount not found", 404);
        }
    }
}
