using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement
{

    public partial class InventoryForm : Form
    {
        private InventoryManager inventoryManager;
        private TextBox nameTextBox;
        private TextBox quantityTextBox;
        private TextBox priceTextBox;
        private TextBox categoryTextBox;
        private Button addItemButton;
        private Button removeItemButton;
        private Button updateQuantityButton;
        private ListBox itemsListBox;

        public InventoryForm()
        {
            this.Text = "Управление инвентарём";
            this.Width = 500;
            this.Height = 400;

            nameTextBox = new TextBox
            {
                Location = new Point(10, 10),
                Width = 150,
                Text = "Название"
            };

            nameTextBox.Click += nameTextBox_Click;
            nameTextBox.Leave += nameTextBox_Leave;

            quantityTextBox = new TextBox
            {
                Location = new Point(170, 10),
                Width = 80,
                Text = "Количество"
            };

            quantityTextBox.Click += quantityTextBox_Click;
            quantityTextBox.Leave += quantityTextBox_Leave;

            priceTextBox = new TextBox
            {
                Location = new Point(260, 10),
                Width = 100,
                Text = "Цена"
            };

            priceTextBox.Click += priceTextBox_Click;
            priceTextBox.Leave += priceTextBox_Leave;

            categoryTextBox = new TextBox
            {
                Location = new Point(370, 10),
                Width = 100,
                Text = "Категория"
            };

            categoryTextBox.Click += categoryTextBox_Click;
            categoryTextBox.Leave += categoryTextBox_Leave;

            addItemButton = new Button
            {
                Location = new Point(10, 40),
                Text = "Добавить",
                Width = 100
            };
            addItemButton.Click += AddItemButton_Click;

            removeItemButton = new Button
            {
                Location = new Point(120, 40),
                Text = "Удалить",
                Width = 100
            };
            removeItemButton.Click += RemoveItemButton_Click;

            updateQuantityButton = new Button
            {
                Location = new Point(220, 40),
                Text = "Обновить",
                Width = 100
            };
            updateQuantityButton.Click += UpdateQuantityButton_Click;

            itemsListBox = new ListBox
            {
                Location = new Point(10, 70),
                Width = 460,
                Height = 200
            };

            Controls.Add(nameTextBox);
            Controls.Add(quantityTextBox);
            Controls.Add(priceTextBox);
            Controls.Add(categoryTextBox);
            Controls.Add(addItemButton);
            Controls.Add(removeItemButton);
            Controls.Add(updateQuantityButton);
            Controls.Add(itemsListBox);

            inventoryManager = new InventoryManager();
            UpdateItemsList();
        }

        private void nameTextBox_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "Название")
            {
                nameTextBox.Text = string.Empty;
            }
        }
        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                nameTextBox.Text = "Название";
            }
        }


        private void quantityTextBox_Click(object sender, EventArgs e)
        {
            if (quantityTextBox.Text == "Количество")
            {
                quantityTextBox.Text = string.Empty;
            }
        }
        private void quantityTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(quantityTextBox.Text))
            {
                quantityTextBox.Text = "Количество";
            }
        }

        private void priceTextBox_Click(object sender, EventArgs e)
        {
            if (priceTextBox.Text == "Цена")
            {
                priceTextBox.Text = string.Empty;
            }
        }
        private void priceTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(priceTextBox.Text))
            {
                priceTextBox.Text = "Цена";
            }
        }

        private void categoryTextBox_Click(object sender, EventArgs e)
        {
            if (categoryTextBox.Text == "Категория")
            {
                categoryTextBox.Text = string.Empty;
            }
        }
        private void categoryTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(categoryTextBox.Text))
            {
                categoryTextBox.Text = "Категория";
            }
        }





        private void UpdateItemsList()
        {
            itemsListBox.Items.Clear();
            foreach (var item in inventoryManager.Items)
            {
                itemsListBox.Items.Add($"{item.Name} - Количество: {item.Quantity} | Цена: {item.Price} руб. | Категория: {item.Category}");
            }
        }

        private void AddItemButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameTextBox.Text) ||
                string.IsNullOrEmpty(quantityTextBox.Text) ||
                string.IsNullOrEmpty(priceTextBox.Text) ||
                string.IsNullOrEmpty(categoryTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (!int.TryParse(quantityTextBox.Text, out int quantity) ||
                !decimal.TryParse(priceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Неверный формат количества или цены!");
                return;
            }

            InventoryItem newItem = new InventoryItem(nameTextBox.Text, quantity, price, categoryTextBox.Text);
            try
            {
                inventoryManager.AddItem(newItem);
                nameTextBox.Clear();
                quantityTextBox.Clear();
                priceTextBox.Clear();
                categoryTextBox.Clear();
                UpdateItemsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            if (itemsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите товар для удаления!");
                return;
            }

            string selectedItem = itemsListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string name = parts[0].Trim();
                var itemToRemove = inventoryManager.Items.Find(i => i.Name == name);
                if (itemToRemove != null)
                {
                    try
                    {
                        inventoryManager.RemoveItem(itemToRemove);
                        UpdateItemsList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void UpdateQuantityButton_Click(object sender, EventArgs e)
        {
            if (itemsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите товар для обновления!");
                return;
            }

            string selectedItem = itemsListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string name = parts[0].Trim();
                var itemToUpdate = inventoryManager.Items.Find(i => i.Name == name);
                if (itemToUpdate != null)
                {
                    if (string.IsNullOrEmpty(quantityTextBox.Text))
                    {
                        MessageBox.Show("Введите новое количество!");
                        return;
                    }

                    if (!int.TryParse(quantityTextBox.Text, out int newQuantity))
                    {
                        MessageBox.Show("Неверный формат количества!");
                        return;
                    }

                    try
                    {
                        inventoryManager.UpdateItemQuantity(itemToUpdate, newQuantity);
                        UpdateItemsList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
