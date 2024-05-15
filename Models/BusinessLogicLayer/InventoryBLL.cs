using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            oldInventory.product_id = inventory.product_id;
            oldInventory.quantity = inventory.quantity;
            oldInventory.measure = inventory.measure;
            oldInventory.date_supply = inventory.date_supply;
            oldInventory.date_expiration = inventory.date_expiration;
            oldInventory.price_purchase = inventory.price_purchase;

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
            return context.Inventories.ToList();
        }

        public Inventory GetInventory(int id)
        {
            return context.Inventories.FirstOrDefault(i => i.inventory_id == id);
        }

        public List<Inventory> GetInventoriesByProduct(int product_id)
        {
            return context.Inventories.Where(i => i.product_id == product_id).ToList();
        }
    }
}
