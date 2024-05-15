﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models.BusinessLogicLayer
{
    public class ProducerBLL
    {
        private supermarketEntities1 context = new supermarketEntities1();
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
            oldProducer.name = producer.name;
            oldProducer.country = producer.country;
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
    }
}
