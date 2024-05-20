using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Supermarket.Models.Database;

namespace Supermarket.Models.BusinessLogicLayer
{
    public class InventoryBLL
    {
        private supermarketEntities context = new supermarketEntities();
        public string ErrorMessage { get; set; }
        public void AddInventory(object obj)
        {
            Inventory inventory = obj as Inventory;
            if (inventory == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            var existingProduct = context.Products.FirstOrDefault(p => p.product_id == inventory.product_id);
            if (existingProduct.active == false)
            {
                ErrorMessage = "Product is not active!";
                return;
            }
            inventory.Product = existingProduct;
            context.Inventories.Add(inventory);
            context.SaveChanges();
        }

        public void UpdateInventory(object obj)
        {
            Inventory inventory = obj as Inventory;
            if (inventory == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            Inventory oldInventory = context.Inventories.FirstOrDefault(i => i.inventory_id == inventory.inventory_id);
            if (oldInventory == null)
            {
                ErrorMessage = "Inventory not found!";
                return;
            }
            if (inventory.price_purchase != oldInventory.price_purchase)
            {
                ErrorMessage = "You cant update the purchase price!";
                MessageBox.Show("You cant update the purchase price!");
                return;
            }
            oldInventory.product_id = inventory.product_id;
            oldInventory.quantity = inventory.quantity;
            oldInventory.measure = inventory.measure.Substring(0,10);
            oldInventory.date_supply = inventory.date_supply;
            oldInventory.date_expiration = inventory.date_expiration;
            oldInventory.price_purchase = inventory.price_purchase;
            if (inventory.price_selling > inventory.price_purchase)
            {
                oldInventory.price_selling = inventory.price_selling;
            }
            else
            {
                ErrorMessage = "Selling price must be greater than purchase price!";
                MessageBox.Show("Selling price must be greater than purchase price!");
                return;
            }

            context.SaveChanges();
        }

        public void DeleteInventory(object obj)
        {
            Inventory inventory = obj as Inventory;
            if (inventory == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            Inventory oldInventory = context.Inventories.FirstOrDefault(i => i.inventory_id == inventory.inventory_id);
            if (oldInventory == null)
            {
                ErrorMessage = "Inventory not found!";
                return;
            }
            context.Inventories.Remove(oldInventory);
            context.SaveChanges();
        }

        public List<Inventory> GetInventories()
        {
            DateTime currentDate = DateTime.Now;
            var expiredStocks = context.Inventories.Where(i => i.active && i.date_expiration < currentDate).ToList();
            foreach(var inventory in expiredStocks)
            {
                inventory.active = false;
            }
            context.SaveChanges();
            return context.Inventories.Where(i => i.active).ToList();
        }


        public Inventory GetInventory(int id)
        {
            return context.Inventories.FirstOrDefault(i => i.inventory_id == id);
        }

        public List<Inventory> GetInventoriesByProduct(int product_id)
        {
            return context.Inventories.Where(i => i.product_id == product_id).ToList();
        }

        public double GetPriceForProduct(int product_id)
        {
            return context.Inventories.Where(i => i.product_id == product_id).OrderByDescending(i => i.date_supply).FirstOrDefault().price_purchase;
        }

        public double GetPurchasePrice(int product_id)
        {
            return context.Inventories.Where(i => i.product_id == product_id).OrderByDescending(i => i.date_supply).FirstOrDefault().price_purchase;
        }
    }
}
