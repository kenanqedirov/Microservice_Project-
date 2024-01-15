using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeCourse.Web.Models.Basket
{
    public class BasketViewModel
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        private List<BasketItemViewModel> _basketItems { get; set; }
        public List<BasketItemViewModel> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    _basketItems.ForEach(a =>
                    {
                        var discountPrice = a.Price * ((decimal)DiscountRate.Value / 100);
                        a.AppliedDiscount(Math.Round(a.Price - discountPrice, 2));
                    });
                }
                return _basketItems;
            }
            set { _basketItems = value; }
        }

        public decimal TotalPrice
        {
            get => _basketItems.Sum(x => x.GetCurrentPrice);
        }
        public bool HasDiscount
        {
            get=> !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;
        }
        public void CancelApplyDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }
    }
}
