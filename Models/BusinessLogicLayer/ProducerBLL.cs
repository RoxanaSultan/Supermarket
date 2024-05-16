using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Models.Database;

namespace Supermarket.Models.BusinessLogicLayer
{
    public class ProducerBLL
    {
        private supermarketEntities context = new supermarketEntities();
        public string ErrorMessage { get; set; }
        public void AddProducer(object obj)
        {
            Producer producer = obj as Producer;
            if (producer == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            context.Producers.Add(producer);
            context.SaveChanges();
        }

        public void UpdateProducer(object obj)
        {
            Producer producer = obj as Producer;
            if (producer == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            Producer oldProducer = context.Producers.FirstOrDefault(p => p.producer_id == producer.producer_id);
            if (oldProducer == null)
            {
                ErrorMessage = "Producer not found!";
                return;
            }
            oldProducer.name = producer.name.Substring(0,15);
            oldProducer.country = producer.country.Substring(0,15);
            context.SaveChanges();
        }

        public void DeleteProducer(object obj)
        {
            Producer producer = obj as Producer;
            if (producer == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            Producer oldProducer = context.Producers.FirstOrDefault(p => p.producer_id == producer.producer_id);
            if (oldProducer == null)
            {
                ErrorMessage = "Producer not found!";
                return;
            }
            context.Producers.Remove(oldProducer);
            context.SaveChanges();
        }

        public List<Producer> GetProducers()
        {
            return context.Producers.ToList();
        }

        public Producer GetProducer(int id)
        {
            return context.Producers.FirstOrDefault(p => p.producer_id == id);
        }
        public Producer GetProducer(string name)
        {
            return context.Producers.FirstOrDefault(p => p.name == name);
        }
        public List<GetProductsFromProducers_Result> GetProductsFromProducers(int id)
        {
            return context.GetProductsFromProducers(id).ToList();
        }
    }
}
