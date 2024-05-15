using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Helpers;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models;
using Supermarket.Views;
using System.Windows.Input;
using System.Collections.ObjectModel;


namespace Supermarket.ViewModels
{
    class OfferVM : BaseVM
    {
        private OfferBLL offerBLL;
        private ObservableCollection<Offer> offers;
        public ObservableCollection<Offer> Offers
        {
            get { return offers; }
            private set
            {
                offers = value;
                NotifyPropertyChanged(nameof(Offers));
            }
        }
        public OfferVM()
        {
            offerBLL = new OfferBLL();
            offers = new ObservableCollection<Offer>(offerBLL.GetOffers());
        }

        public void AddOffer(object obj)
        {
            Offer offer = obj as Offer;
            if (offer == null)
            {
                offerBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidOffer(offer))
            {
                offerBLL.ErrorMessage = "Validation failed!";
                return;
            }
            offerBLL.AddOffer(offer);
            if (string.IsNullOrEmpty(offerBLL.ErrorMessage))
            {
                offers.Add(offer);
            }
        }

        private ICommand addOfferCommand;
        public ICommand AddOfferCommand
        {
            get
            {
                return addOfferCommand ?? (addOfferCommand = new RelayCommand(AddOffer));
            }
        }

        public void UpdateOffer(object obj)
        {
            Offer offer = obj as Offer;
            if (offer == null)
            {
                offerBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidOffer(offer))
            {
                offerBLL.ErrorMessage = "Validation failed!";
                return;
            }
            offerBLL.UpdateOffer(offer);
            if (string.IsNullOrEmpty(offerBLL.ErrorMessage))
            {
                // Update the ObservableCollection
                var existingOffer = Offers.FirstOrDefault(p => p.offer_id == offer.offer_id);
                if (existingOffer != null)
                {
                    var index = Offers.IndexOf(existingOffer);
                    Offers[index] = offer; // This only works if the collection is bound to UI with bindings that detect changes.
                }
            }
        }

        private ICommand updateOfferCommand;
        public ICommand UpdateOfferCommand
        {
            get
            {
                return updateOfferCommand ?? (updateOfferCommand = new RelayCommand(UpdateOffer));
            }
        }

        public void DeleteOffer(object obj)
        {
            Offer offer = obj as Offer;
            if (offer == null)
            {
                offerBLL.ErrorMessage = "Invalid input!";
                return;
            }
            offerBLL.DeleteOffer(offer);
            if (string.IsNullOrEmpty(offerBLL.ErrorMessage))
            {
                offers.Remove(offer);
            }
        }

        private ICommand deleteOfferCommand;
        public ICommand DeleteOfferCommand
        {
            get
            {
                return deleteOfferCommand ?? (deleteOfferCommand = new RelayCommand(DeleteOffer));
            }
        }

        private bool IsValidOffer(Offer offer)
        {
            if (string.IsNullOrEmpty(offer.reason))
            {
                return false;
            }
            if(offer.discount < 0 || offer.discount > 100)
            {
                return false;
            }
            if(offer.product_id <= 0)
            {
                return false;
            }
            if(offer.date_start == null || offer.date_end == null)
            {
                return false;
            }
            return true;
        }
    }
}
