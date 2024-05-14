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
    class ProducerVM : BaseVM
    {
        private ProducerBLL producerBLL;
        private ObservableCollection<Producer> producers;
        public ProducerVM()
        {
            producerBLL = new ProducerBLL();
            producers = new ObservableCollection<Producer>(producerBLL.GetProducers());
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
                producers = new ObservableCollection<Producer>(producerBLL.GetProducers());
            }
        }

        private ICommand addProducerCommand;
        public ICommand AddProducerCommand
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
                producers = new ObservableCollection<Producer>(producerBLL.GetProducers());
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
                producers = new ObservableCollection<Producer>(producerBLL.GetProducers());
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
    }
}
