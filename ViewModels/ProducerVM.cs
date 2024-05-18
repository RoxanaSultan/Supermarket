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
using Supermarket.Models.Database;
using System.Windows;

namespace Supermarket.ViewModels
{
    class ProducerVM : BaseVM
    {
        private ProducerBLL producerBLL;

        private ObservableCollection<Producer> producers;
        public ObservableCollection<Producer> Producers
        {
            get { return producers; }
            private set
            {
                producers = value;
                NotifyPropertyChanged(nameof(Producers));
            }
        }
        private ObservableCollection<Product> orderedProductsByCategory;

        public ObservableCollection<Product> OrderedProductsByCategory
        {
            get { return orderedProductsByCategory; }
            private set
            {
                orderedProductsByCategory = value;
                NotifyPropertyChanged(nameof(OrderedProductsByCategory));
            }
        }

        private int selectedProducerId;
        public ProducerVM()
        {
            producerBLL = new ProducerBLL();
            producers = new ObservableCollection<Producer>(producerBLL.GetProducers());
            orderedProductsByCategory = new ObservableCollection<Product>();
        }

        public void AddProducer(object obj)
        {
            Producer producer = obj as Producer;
            if (producer == null)
            {
                producerBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidProducer(producer))
            {
                producerBLL.ErrorMessage = "Validation failed!";
                return;
            }
            producerBLL.AddProducer(producer);
            if (string.IsNullOrEmpty(producerBLL.ErrorMessage))
            {
                producers.Add(producer);
            }
        }

        private ICommand addProducerCommand;
        public ICommand AddCommand
        {
            get
            {
                return addProducerCommand ?? (addProducerCommand = new RelayCommand(AddProducer));
            }
        }

        public void UpdateProducer(object obj)
        {
            Producer producer = obj as Producer;
            if (producer == null)
            {
                producerBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidProducer(producer))
            {
                producerBLL.ErrorMessage = "Validation failed!";
                return;
            }
            producerBLL.UpdateProducer(producer);
            if (string.IsNullOrEmpty(producerBLL.ErrorMessage))
            {
                // Update the ObservableCollection
                var existingProducer = Producers.FirstOrDefault(p => p.producer_id == producer.producer_id);
                if (existingProducer != null)
                {
                    var index = Producers.IndexOf(existingProducer);
                    Producers[index] = producer; // This only works if the collection is bound to UI with bindings that detect changes.
                }
            }
        }

        private ICommand updateProducerCommand;
        public ICommand UpdateProducerCommand
        {
            get
            {
                return updateProducerCommand ?? (updateProducerCommand = new RelayCommand(UpdateProducer));
            }
        }

        public void DeleteProducer(object obj)
        {
            Producer producer = obj as Producer;
            if (producer == null)
            {
                producerBLL.ErrorMessage = "Invalid input!";
                return;
            }
            producerBLL.DeleteProducer(producer);
            if (string.IsNullOrEmpty(producerBLL.ErrorMessage))
            {
                producers.Remove(producer);
            }
        }

        private ICommand deleteProducerCommand;
        public ICommand DeleteProducerCommand
        {
            get
            {
                return deleteProducerCommand ?? (deleteProducerCommand = new RelayCommand(DeleteProducer));
            }
        }

        private bool IsValidProducer(Producer producer)
        {
            if (producer == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(producer.name))
            {
                return false;
            }
            if (string.IsNullOrEmpty(producer.country))
            {
                return false;
            }
            return true;
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(SearchProductsByCategory);
            }
        }

        public void SearchProductsByCategory(object obj)
        {
            int producerId = obj as int? ?? 0;
            if (producerId == 0)
            {
                producerBLL.ErrorMessage = "Invalid input!";
                return;
            }
            selectedProducerId = producerId;
            var products = producerBLL.GetProductsFromProducers(producerId);
            orderedProductsByCategory.Clear();
            foreach (var product in products)
            {
                orderedProductsByCategory.Add(product);
            }
            NotifyPropertyChanged(nameof(OrderedProductsByCategory));

        }
    }
}
