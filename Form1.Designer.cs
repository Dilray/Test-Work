using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

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

            // Запись отфильтрованных данных в файл
            using (StreamWriter writer = new StreamWriter(filePath, false)) // false для перезаписи
            {
                foreach (var order in filteredOrders)
                    writer.WriteLine(
                    $"Заказ номер: {order.getOrderId()},    " +
                    $"Вес: {order.getOrderWeight()},    " +
                    $"Район: '{order.getOrderDistrict()}',    " +
                    $"Время и дата доставки: {order.getOrderDate()}."
                    );
            }
        }

        /// <summary>
        /// Событие нажатия кнопки
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            List<Order> orders = new List<Order>();

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
                            orders.Add(new Order(items[0], items[1], items[2], items[3] + ' ' + items[4]));
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

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>

        // this.myButton.Click += button1_Click;
        private void InitializeComponent()
        {
            this.myButton = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // myButton
            // 
            this.myButton.Location = new System.Drawing.Point(737, 553);
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
            this.ClientSize = new System.Drawing.Size(1111, 666);
            this.Controls.Add(this.myButton);
            this.Controls.Add(this.listBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
        }
    }
}

