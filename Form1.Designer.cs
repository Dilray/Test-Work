﻿using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Reflection.Emit;
using System.Drawing;

namespace Test_Work
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Кнопка для загрузки файла с данными
        /// </summary>
        private Button myButton;

        /// <summary>
        /// ListBox для вывода отфильтрованных заказов на форму
        /// </summary>
        private ListBox listBox;

        System.Windows.Forms.Label label;
        private DateTime firstDeliveryDateTime;
        private string cityDistrict;
        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Функция для сортировки заказов на основе данных вначале (район и дата первого заказа).
        /// </summary>
        private List<Order> SortOrders(ref List<Order> orders)
        {
            // Фильтрация заказов
            DateTime endTime = firstDeliveryDateTime.AddMinutes(30);
            var sortedOrders = orders.Where(o => o.getOrderDistrict().Equals(cityDistrict, StringComparison.OrdinalIgnoreCase) &&
                                                    o.getOrderDate() >= firstDeliveryDateTime &&
                                                    o.getOrderDate() <= endTime).ToList();
            return sortedOrders;
        }

        /// <summary>
        /// Вывод заказов в форму (для удобства).
        /// </summary>
        private void PrintOrders(ref List<Order> filteredOrders)
        {
            listBox.Items.Add("Отфильтрованные заказы:");
            foreach (var order in filteredOrders)
                listBox.Items.Add(
                    $"Заказ номер: {order.getOrderId()},    " +
                    $"Вес: {order.getOrderWeight()},    " +
                    $"Район: '{order.getOrderDistrict()}',    " +
                    $"Время и дата доставки: {order.getOrderDate()}."
                    );
        }

        /// <summary>
        /// Загрузка отфильтрованных данных в текстовый файл 'orders.txt' (находится в debug проекта).
        /// </summary>
        private void LoadOrdersInFile(ref List<Order> filteredOrders)
        {
            string filePath = "orders.txt";

            try
            {
                // Запись отфильтрованных данных в файл
                using (StreamWriter writer = new StreamWriter(filePath, false)) // false для перезаписи
                {
                    foreach (var order in filteredOrders)
                    {
                        writer.WriteLine(
                            $"Заказ номер: {order.getOrderId()},    " +
                            $"Вес: {order.getOrderWeight()},    " +
                            $"Район: '{order.getOrderDistrict()}',    " +
                            $"Время и дата доставки: {order.getOrderDate()}."
                        );
                    }
                }
            }
            catch (IOException ex)
            {
                // Обработка исключений ввода-вывода
                Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Обработка других возможных исключений
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Событие нажатия кнопки
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            List<Order> orders = new List<Order>();

            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    // Устанавливаем заголовок окна
                    openFileDialog.Title = "Выберите файл";

                    // Устанавливаем фильтр для файлов
                    openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;

                        MessageBox.Show($"Выбранный файл: {filePath}");

                        bool b = false;
                        foreach (string line in File.ReadLines(filePath))
                        {
                            string[] items = line.Split();

                            if (b)
                            {
                                orders.Add(new Order(items[0], items[1], items[2], items[3] + ' ' + items[4]));
                            }
                            else
                            {
                                cityDistrict = items[0];
                                firstDeliveryDateTime = DateTime.ParseExact(items[1] + ' ' + items[2], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                b = true;
                            }
                        }
                    }
                }

                // Сортируем заказы
                orders = SortOrders(ref orders);
                // Принтуем в форму (для удобства)
                PrintOrders(ref orders);
                // Загружаем в новый файл "orders.txt" (находится в debug проекта)
                LoadOrdersInFile(ref orders);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Файл не найден: {ex.Message}");
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка формата данных: {ex.Message}");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Ошибка ввода-вывода: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>

        // this.myButton.Click += button1_Click;
        private void InitializeComponent()
        {
            this.label = new System.Windows.Forms.Label();
            this.myButton = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label.Location = new System.Drawing.Point(743, 22);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(288, 19);
            this.label.TabIndex = 2;
            this.label.Text = "Effective Mobile тестовое задание 1";
            this.label.Click += new System.EventHandler(this.label_Click);
            // 
            // myButton
            // 
            this.myButton.Location = new System.Drawing.Point(762, 615);
            this.myButton.Name = "myButton";
            this.myButton.Size = new System.Drawing.Size(236, 38);
            this.myButton.TabIndex = 0;
            this.myButton.Text = "Загрузить файл с данными";
            this.myButton.Click += button1_Click;
            // 
            // listBox
            // 
            this.listBox.Location = new System.Drawing.Point(12, 12);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(620, 641);
            this.listBox.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 684);
            this.Controls.Add(this.myButton);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.label);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

