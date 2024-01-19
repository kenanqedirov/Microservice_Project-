﻿using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayment;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;
        public OrderService(IPaymentService paymentService, HttpClient httpClient,
            IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();
            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                CVV = checkoutInfoInput.CVV,
                Expiration = checkoutInfoInput.Expiration,
                TotalPrice = basket.TotalPrice
            };
            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);
            if (!responsePayment)
            {
                return new OrderCreatedViewModel() { Error = "Payment dont finished successfully", isSuccessfull = false };
            }
            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput
                {
                    Province = checkoutInfoInput.Province,
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode
                },
            };
            basket.BasketItems.ForEach(a =>
            {
                var orderItem = new OrderItemCreateInput { ProductId = a.CourseId, Price = a.GetCurrentPrice, PictureUrl = "", ProductName = a.CourseName };
                orderCreateInput.OrderItems.Add(orderItem);
            });
            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);
            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel() { Error = "Payment dont finished successfully", isSuccessfull = false };
            }
            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
            orderCreatedViewModel.Data.isSuccessfull = true;
            await _basketService.Delete();
            return orderCreatedViewModel.Data;
        }
        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
            return response.Data;
        }

        public Task SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            throw new System.NotImplementedException();
        }
    }
}