using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Test_Work
{
    /// <summary>
    /// Класс объекта "заказ"
    /// </summary>
    internal class Order
    {
        /// <summary>
        /// Поле айдишника заказа
        /// </summary>
        protected int OrderId {  get; set; }
        /// <summary>
        /// Поле веса заказа (в килограммах, но на всякий случай сделал double).
        /// </summary>
        protected double OrderWeight {  get; set; }
        /// <summary>
        /// Поле района заказа.
        /// </summary>
        protected string OrderDistrict {  get; set; }
        /// <summary>
        /// Поле даты заказа.
        /// </summary>
        protected DateTime OrderDate { get; set; }

        public int getOrderId() => OrderId;
        public double getOrderWeight() => OrderWeight;
        public string getOrderDistrict() => OrderDistrict;
        public DateTime getOrderDate() => OrderDate;

        /// <summary>
        /// Конструктор, создающий объект типа "заказ"
        /// </summary>
        public Order(string orderId, string orderWeight, string orderDistrict, string orderDate = "1970-00-00 12:00:00")
        {
            OrderId = int.Parse(orderId);
            OrderWeight = double.Parse(orderWeight);
            OrderDistrict = orderDistrict;
            OrderDate = DateTime.ParseExact(orderDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
