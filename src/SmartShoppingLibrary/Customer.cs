using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartShoppingLibrary
{
    public enum CustomerState
    { 
        Inactive,
        Shopping,
        Waiting
    }

    [Serializable()]
    public class Customer : ISerializable
    {
        public int Uid;
        public CustomerState State;
        public string Name;
        public Order Order;
        public Shop Shop;
        public List<Order> OrderHistory;

        public Customer(int uid, string name) 
        {
            this.Uid = uid;
            this.State = CustomerState.Inactive;
            this.Name = name;   
            this.OrderHistory = new List<Order>();
        }

        public Customer(SerializationInfo info, StreamingContext ctxt)
        {
            this.Uid = (int)info.GetValue("Uid", typeof(int));
            this.State = (CustomerState)info.GetValue("State", typeof(CustomerState));
            this.Name = (string)info.GetValue("Name", typeof(string));
            this.Order = (Order)info.GetValue("Order", typeof(Order));
            this.Shop = (Shop)info.GetValue("Shop", typeof(Shop));
            this.OrderHistory = (List<Order>)info.GetValue("OrderHistory", typeof(List<Order>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Uid", this.Uid);
            info.AddValue("State", this.State);
            info.AddValue("Name", this.Name);
            info.AddValue("Order", this.Order);
            info.AddValue("Shop", this.Shop);
            info.AddValue("OrderHistory", this.OrderHistory);           
        }

        public void enterShop(Shop shop) 
        {
            this.Shop = shop;
            this.Order = new Order(this, this.Shop);
            this.State = CustomerState.Shopping;
        }

        public override string ToString()
        {
            string printString = this.Uid + " " + this.Name;
            if (this.State.Equals(CustomerState.Shopping))
            {
                printString += " shopping in " + this.Shop.Uid + " " + this.Shop.Name + "\n";
                printString += this.Order.ToString();
            }
            else if (this.State.Equals(CustomerState.Waiting))
            {
                printString += " waiting in " + this.Shop.Uid + " " + this.Shop.Name + "\n";
                printString += this.Order.ToString();
            }
            return printString;
        }

        public void addOrderItem(int canonicalProductUid, int quantity)
        {
            Product product = this.Shop.Products[canonicalProductUid];
            this.Order.AddItem(product, quantity);
        }

        public void PlaceOrder()
        {
            this.State = CustomerState.Waiting;
            this.Order.Place();
        }

        public void ReceiveOrder()
        {
            this.State = CustomerState.Inactive;
            this.Order.Receive();
        }
    }
}
