using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SmartShoppingLibrary
{
    [Serializable()]
    public class Shop : ISerializable
    {
        public int Uid;
        public string Name;
        /*
         * Dict fordi vi skal kunne gå fra en Uid sat specificeret i et stackpanels / billedes navn 
         * til det egentlige produkt. Er det ikke nødvendigt kan man bruge en liste i stedet
         */
        public Dictionary<int, Product> Products; 
        //public List<Product> Products;
        public Dictionary<OrderState, HashSet<Order>> Orders;
        //public List<Order> Orders;
        public Shop(int uid, string name) 
        {
            this.Uid = uid;
            this.Name = name;
            this.Products = new Dictionary<int, Product>();
            this.Orders = new Dictionary<OrderState, HashSet<Order>>
            {
                {OrderState.UnderConstruction, new HashSet<Order>()},
                {OrderState.Placed, new HashSet<Order>()},
                {OrderState.Packaged, new HashSet<Order>()},
                {OrderState.Delivered, new HashSet<Order>()}
            };
            //this.Orders[OrderState.UnderConstruction] = new HashSet<Order>();
        }

        public Shop(SerializationInfo info, StreamingContext ctxt)
        {
            this.Uid = (int)info.GetValue("Uid", typeof(int));
            this.Name = (string)info.GetValue("Name", typeof(string));
            this.Products = (Dictionary<int, Product>)info.GetValue("Products", typeof(Dictionary<int, Product>));
            //this.Products = (List<Product>)info.GetValue("Products", typeof(List<Product>));
            this.Orders = (Dictionary<OrderState, HashSet<Order>>)info.GetValue("Orders", typeof(Dictionary<OrderState, HashSet<Order>>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Uid", this.Uid);
            info.AddValue("Name", this.Name);
            info.AddValue("Products", this.Products);
            info.AddValue("Orders", this.Orders);
        }

        public override string ToString()
        {
            string printString = this.Uid + " " + this.Name;
            /*foreach (var product in this.Products)
            {
                printString += product.Value.ToString() + "\n";
            }*/

            return printString;
        }

        public void addProduct(CanonicalProduct canonicalProduct, decimal price)
        {
            Product product = new Product(canonicalProduct, price);
            this.Products.Add(canonicalProduct.Uid, product);
        }

        public void AddOrder(Order order)
        {
            this.Orders[OrderState.UnderConstruction].Add(order);
        }

        public void UpdateOrder(Order order)
        {
            this.Orders[order.State - 1].Remove(order);
            this.Orders[order.State].Add(order); 
        }

        public void DeliverOrder(Order order)
        {
            order.Deliver();
            this.UpdateOrder(order);
        }
        /*
        public void AddOrderToPackagingQueue(Order order)
        {
            // this.Orders has to know previous state
            //this.Orders[order.State].Add(order, order);
            //this.Orders.Add(order);
            //this.Orders.Add(OrderState.Placed, PlacedOrders
            this.Orders[OrderState.Placed].Add(order);
        }

        public void RemoveOrderFromPackagingQueue(Order order)
        {
            foreach (Order anOrder in this.Orders)
            { 
                if (order.Equals(anOrder))
                    this.Orders.Remove(order);
            }
        }
         */
    }
}
