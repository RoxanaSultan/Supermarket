using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Models.Database;

namespace Supermarket.Models.BusinessLogicLayer
{
    public class OfferBLL
    {
        private supermarketEntities2 context = new supermarketEntities2();
        public string ErrorMessage { get; set; }
        public void AddOffer(object obj)
        {
            Offer offer = obj as Offer;
            if (offer == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            context.Offers.Add(offer);
            context.SaveChanges();
        }

        public void UpdateOffer(object obj)
        {
            Offer offer = obj as Offer;
            if (offer == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            Offer oldOffer = context.Offers.FirstOrDefault(o => o.offer_id == offer.offer_id);
            if (oldOffer == null)
            {
                ErrorMessage = "Offer not found!";
                return;
            }
            oldOffer.product_id = offer.product_id;
            oldOffer.discount = offer.discount;
            oldOffer.date_start = offer.date_start;
            oldOffer.date_end = offer.date_end;
            oldOffer.reason = offer.reason;
            offer.Product = context.Products.FirstOrDefault(p => p.product_id == offer.product_id);
            context.SaveChanges();
        }

        public void DeleteOffer(object obj)
        {
            Offer offer = obj as Offer;
            if (offer == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            Offer oldOffer = context.Offers.FirstOrDefault(o => o.offer_id == offer.offer_id);
            if (oldOffer == null)
            {
                ErrorMessage = "Offer not found!";
                return;
            }
            context.Offers.Remove(oldOffer);
            context.SaveChanges();
        }

        public List<Offer> GetOffers()
        {
            return context.Offers.ToList();
        }

        public List<Offer> GetOffersByProduct(int product_id)
        {
            return context.Offers.Where(o => o.product_id == product_id).ToList();
        }
        public List<Offer> GetOffersByDate(DateTime date)
        {
            return context.Offers.Where(o => o.date_start <= date && o.date_end >= date).ToList();
        }
    }
}
