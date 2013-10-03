using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SmartShoppingLibrary
{
    [Serializable()]
    public class Product : ISerializable
    {
        public CanonicalProduct CanonicalProduct;
        public decimal Price;
        
        public Product(CanonicalProduct canonicalProduct, decimal price)
        {
            this.CanonicalProduct = canonicalProduct;
            this.Price = price;
        }

        public Product(SerializationInfo info, StreamingContext ctxt) 
        {
            this.CanonicalProduct = (CanonicalProduct)info.GetValue("CanonicalProduct", typeof(CanonicalProduct));
            this.Price = (decimal)info.GetValue("Price", typeof(decimal));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("CanonicalProduct", this.CanonicalProduct);
            info.AddValue("Price", this.Price);
        }

        public override string ToString()
        {
            string printString = this.Price.ToString("C") + " " + CanonicalProduct.ToString();
            return printString;
            
        }
    }
}
