using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_Work
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Список, куда будут записаны все отфильтрованные заказы (для дальнейшей записи в файл).
        /// </summary>
        public List<Order> orders = new List<Order>();

        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Кнопка для загрузки файла с данными.
        /// </summary>
        private Button myButton;

        /// <summary>
        /// ListBox для вывода отфильтрованных заказов на форму.
        /// </summary>
        public ListBox listBox;

        /// <summary>
        /// Переменная кнопки "Загрузить файл с данными"
        /// </summary>
        private Label label;
        /// <summary>
        /// Вспомогательная переменная, куда будут записаны условия фильтрации (время первого заказа)
        /// </summary>
        private DateTime firstDeliveryDateTime;
        /// <summary>
        /// Вспомогательная переменная, куда будут записаны условия фильтрации (район доставки)
        /// </summary>
        private string cityDistrict;


        static string logFilePath = "default.log"; // Путь по умолчанию

        /// <summary>
        /// Поле для логирования.
        /// </summary>
        Logger logger = new Logger(logFilePath);

        public Form1()
        {
            logger.Log("Приложение запущено.");
            InitializeComponent();
        }

        /// <summary>
        /// Функция обработки закрытия приложения
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            logger.Log("Попытка закрытия приложения.");
            // Здесь вы можете выполнить действия перед закрытием формы
            DialogResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?",
                                                   "Подтверждение",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // Отменяем закрытие формы
                logger.Log("Пользователь отменил закрытие приложения.");
            }
            else
                logger.Log("Приложение завершено.");
        }

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

        // public DateTime getFirstDeliveryDateTime() => firstDeliveryDateTime;

        /// <summary>
        /// Функция для сортировки заказов на основе данных вначале (район и дата первого заказа).
        /// </summary>
        public List<Order> SortOrders(List<Order> orders)
        {
            logger.Log("Сортировка данных...");
            try
            {
                // Фильтрация заказов
                DateTime endTime = firstDeliveryDateTime.AddMinutes(30);
                var sortedOrders = orders.Where(o => o.getOrderDistrict().Equals(cityDistrict, StringComparison.OrdinalIgnoreCase) &&
                                                        o.getOrderDate() >= firstDeliveryDateTime &&
                                                        o.getOrderDate() <= endTime).ToList();
                return sortedOrders;
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show($"Ошибка: Список заказов не может быть null. {ex.Message}");
                logger.Log($"Ошибка: {ex.Message}");
                return new List<Order>(); // Возвращаем пустой список в случае ошибки
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сортировке заказов: {ex.Message}");
                logger.Log($"Ошибка: {ex.Message}");
                return new List<Order>(); // Возвращаем пустой список в случае ошибки
            }
        }

        /// <summary>
        /// Вывод заказов в форму (для удобства).
        /// </summary>
        public void PrintOrders(List<Order> filteredOrders)
        {
            logger.Log("Вывод заказов в форму...");
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
        public void LoadOrdersInFile(List<Order> filteredOrders)
        {
            string filePath = "orders.txt";
            logger.Log("Загрузка отфильтрованных данных в текстовый файл 'orders.txt'...");
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
                logger.Log($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Обработка других возможных исключений
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                logger.Log($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Загрузка данных из файла в ListBox
        /// </summary>
        public void loadFromFile(string filePath)
        {
            logger.Log("Загрузка данных из файла в ListBox...");
            try
            {
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
            catch (Exception ex)
            {
                logger.Log($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Событие нажатия кнопки
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            logger.Log($"Нажата кнопка 'Загрузить файл с данными'.");
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    // Устанавливаем заголовок окна
                    openFileDialog.Title = "Выберите файл";

                    // Устанавливаем фильтр для файлов
                    openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                    DialogResult result = openFileDialog.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;

                        MessageBox.Show($"Выбранный файл: {filePath}");

                        loadFromFile(filePath);

                        // Сортируем заказы
                        orders = SortOrders(orders);
                        // Принтуем в форму (для удобства)
                        PrintOrders(orders);
                        // Загружаем в новый файл "orders.txt" (находится в debug проекта)
                        LoadOrdersInFile(orders);
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        logger.Log("Пользователь отменил выбор файла.");
                    }
                }

            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Файл не найден: {ex.Message}");
                logger.Log($"Ошибка: {ex.Message}");
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка формата данных: {ex.Message}");
                logger.Log($"Ошибка: {ex.Message}");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Ошибка ввода-вывода: {ex.Message}");
                logger.Log($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
                logger.Log($"Ошибка: {ex.Message}");
            }
        }
    }
}