﻿using System;
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
    class InventoryVM : BaseVM
    {
        private InventoryBLL inventoryBLL;
        private ObservableCollection<Inventory> inventories;
        public InventoryVM()
        {
            inventoryBLL = new InventoryBLL();
            inventories = new ObservableCollection<Inventory>(inventoryBLL.GetInventories());
        }

        public void AddInventory(object obj)
        {
            Inventory inventory = obj as Inventory;
            if (inventory == null)
            {
                inventoryBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidInventory(inventory))
            {
                inventoryBLL.ErrorMessage = "Validation failed!";
                return;
            }
            inventoryBLL.AddInventory(inventory);
            if (string.IsNullOrEmpty(inventoryBLL.ErrorMessage))
            {
                inventories = new ObservableCollection<Inventory>(inventoryBLL.GetInventories());
            }
        }

        private ICommand addInventoryCommand;
        public ICommand AddInventoryCommand
        {
            get
            {
                return addInventoryCommand ?? (addInventoryCommand = new RelayCommand(AddInventory));
            }
        }

        public void UpdateInventory(object obj)
        {
            Inventory inventory = obj as Inventory;
            if (inventory == null)
            {
                inventoryBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidInventory(inventory))
            {
                inventoryBLL.ErrorMessage = "Validation failed!";
                return;
            }
            inventoryBLL.UpdateInventory(inventory);
            if (string.IsNullOrEmpty(inventoryBLL.ErrorMessage))
            {
                inventories = new ObservableCollection<Inventory>(inventoryBLL.GetInventories());
            }
        }

        private ICommand updateInventoryCommand;
        public ICommand UpdateInventoryCommand
        {
            get
            {
                return updateInventoryCommand ?? (updateInventoryCommand = new RelayCommand(UpdateInventory));
            }
        }

        public void DeleteInventory(object obj)
        {
            Inventory inventory = obj as Inventory;
            if (inventory == null)
            {
                inventoryBLL.ErrorMessage = "Invalid input!";
                return;
            }
            inventoryBLL.DeleteInventory(inventory);
            if (string.IsNullOrEmpty(inventoryBLL.ErrorMessage))
            {
                inventories = new ObservableCollection<Inventory>(inventoryBLL.GetInventories());
            }
        }

        private ICommand deleteInventoryCommand;
        public ICommand DeleteInventoryCommand
        {
            get
            {
                return deleteInventoryCommand ?? (deleteInventoryCommand = new RelayCommand(DeleteInventory));
            }
        }

        private bool IsValidInventory(Inventory inventory)
        {
            if (inventory == null)
            {
                return false;
            }
            if (inventory.product_id < 0 || inventory.quantity < 0)
            {
                return false;
            }
            if (inventory.price_purchase < 0)
            {
                return false;
            }
            if(string.IsNullOrEmpty(inventory.measure))
            {
                return false;
            }
            return true;

        }
    }
}