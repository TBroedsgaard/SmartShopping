using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SmartShoppingLibrary
{
    public enum OrderState 
    {
        UnderConstruction,
        Placed,
        Packaged,
        Delivered
    }

    [Serializable()]
    public class Order : ISerializable
    {
        public Customer Customer;        
        public OrderState State;
        public Dictionary<OrderState, DateTime> Timestamps;
        public Shop Shop;
        public List<OrderItem> Items;
        public decimal TotalPrice;

        public Order(Customer customer, Shop shop)
        {
            this.Customer = customer;
            this.Shop = shop; 
            this.Items = new List<OrderItem>();
            this.TotalPrice = 0m;
            this.State = OrderState.UnderConstruction;
            this.Timestamps = new Dictionary<OrderState,DateTime>();
            this.Timestamps[this.State] = DateTime.UtcNow;
            this.Shop.AddOrder(this);
        }

        public Order(SerializationInfo info, StreamingContext ctxt)
        {
            this.Customer = (Customer)info.GetValue("Customer", typeof(Customer));
            this.Shop = (Shop)info.GetValue("Shop", typeof(Shop));
            this.Items = (List<OrderItem>)info.GetValue("Items", typeof(List<OrderItem>));
            this.TotalPrice = (decimal)info.GetValue("TotalPrice", typeof(decimal));
            this.State = (OrderState)info.GetValue("State", typeof(OrderState));
            this.Timestamps = (Dictionary<OrderState, DateTime>)info.GetValue("Timestamps", typeof(Dictionary<OrderState, DateTime>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Customer", this.Customer);
            info.AddValue("Shop", this.Shop);
            info.AddValue("Items", this.Items);
            info.AddValue("TotalPrice", this.TotalPrice);
            info.AddValue("State", this.State);
            info.AddValue("Timestamps", this.Timestamps);
        }        

        public void AddItem(Product product, int quantity)
        {
            OrderItem item = new OrderItem(product, quantity);
            this.Items.Add(item);
            this.TotalPrice += product.Price * quantity;
        }

        public void PlaceOrder()
        {
            this.Timestamps[this.State] = DateTime.UtcNow;
            OrderState oldState = this.State;
            this.State = OrderState.Placed;
            this.Shop.UpdateOrder(this);
            this.Customer.State = CustomerState.Waiting;
            this.Customer.OrderHistory.Add(this);
        }


        public override string ToString()
        {
            string printString = "Order (" + this.State + "):\n";
            foreach (OrderItem item in this.Items)
            { 
                printString += item.Product.CanonicalProduct.Name.ToString() + "\n";
            }
            printString += "Total: " + this.TotalPrice.ToString("N2");
            return printString;
        }

        public void Place()
        {
            this.State = OrderState.Placed;
            this.Timestamps[this.State] = DateTime.UtcNow;
            this.Shop.UpdateOrder(this);
        }

        internal void Deliver()
        {
            this.State = OrderState.Packaged;
            this.Timestamps[this.State] = DateTime.UtcNow;
            this.Shop.UpdateOrder(this);
        }

        internal void Receive()
        {
            this.State = OrderState.Delivered;
            this.Timestamps[this.State] = DateTime.UtcNow;
            this.Shop.UpdateOrder(this);
        }

    }
}

    

