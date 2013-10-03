using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SmartShoppingLibrary
{
    [Serializable()]
    public class OrderItem : ISerializable
    {
        public Product Product;
        public int Quantity;

        public OrderItem(Product product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
        }

        public OrderItem(SerializationInfo info, StreamingContext ctxt) 
        {
            this.Product = (Product)info.GetValue("Product", typeof(Product));
            this.Quantity = (int)info.GetValue("Quantity", typeof(int));            
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Product", this.Product);
            info.AddValue("Quantity", this.Quantity);
        }

        public override string ToString()
        {
            string printString = this.Quantity + " stk a " + this.Product.ToString();
            return printString;
        }
    }
}
