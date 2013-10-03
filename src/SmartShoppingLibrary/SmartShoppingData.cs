using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartShoppingLibrary
{
    [Serializable()]
    public class SmartShoppingData : ISerializable
    {
        public Dictionary<int, CanonicalProduct> CanonicalProducts;
        public Dictionary<int, Customer> Customers;
        public Dictionary<int, Shop> Shops;
        public int CanonicalProductUids;
        public int CustomerUids;
        public int ShopUids;
        public static Stream stream;
        

        public SmartShoppingData() 
        {
            this.CanonicalProducts = new Dictionary<int, CanonicalProduct>();
            this.Customers = new Dictionary<int, Customer>();
            this.Shops = new Dictionary<int, Shop>();
        }

        public SmartShoppingData(SerializationInfo info, StreamingContext ctxt) 
        {
            this.CanonicalProducts = (Dictionary<int, CanonicalProduct>)info.GetValue("CanonicalProducts", typeof(Dictionary<int, CanonicalProduct>));
            this.Customers = (Dictionary<int, Customer>)info.GetValue("Customers", typeof(Dictionary<int, Customer>));
            this.Shops = (Dictionary<int, Shop>)info.GetValue("Shops", typeof(Dictionary<int, Shop>));
            this.CanonicalProductUids = (int)info.GetValue("CanonicalProductUids", typeof(int));
            this.CustomerUids = (int)info.GetValue("CustomerUids", typeof(int));
            this.ShopUids = (int)info.GetValue("ShopUids", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) 
        {
            info.AddValue("CanonicalProducts", this.CanonicalProducts);
            info.AddValue("Customers", this.Customers);
            info.AddValue("Shops", this.Shops);
            info.AddValue("CanonicalProductUids", this.CanonicalProductUids);
            info.AddValue("CustomerUids", this.CustomerUids);
            info.AddValue("ShopUids", this.ShopUids);
        }

        public bool Save(string filename)
        {
            stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            Console.WriteLine("Writing SmartShoppingData to file " + filename);
            bformatter.Serialize(stream, this);
            stream.Close();
            return true;
        }

        public static SmartShoppingData Load(string filename) 
        {
            stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            Console.WriteLine("Reading SmartShoppingData from file " + filename);
            return (SmartShoppingData)bformatter.Deserialize(stream);
            
        }

        public void addCanonicalProduct(string name, string description, string imageURL)
        {
            CanonicalProduct canonicalProduct = new CanonicalProduct(this.CanonicalProductUids, name, description, imageURL);
            this.CanonicalProducts.Add(this.CanonicalProductUids, canonicalProduct);
            this.CanonicalProductUids++;
        }

        public void addCustomer(string name)
        {
            Customer customer = new Customer(this.CustomerUids, name);
            this.Customers.Add(this.CustomerUids, customer);
            this.CustomerUids++;
        }

        public void addShop(string name)
        {
            Shop shop = new Shop(this.ShopUids, name);
            this.Shops.Add(this.ShopUids, shop);
            this.ShopUids++;
        }

        public override string ToString()
        {
            string printString = "SmartShopping Data: \n";
            printString += "Canonical Products: \n";
            foreach (var canonicalProduct in CanonicalProducts)
            {
                printString += canonicalProduct.Value.ToString() + "\n";
            }
            printString += "Customers: \n";
            foreach (var customer in Customers)
            {
                printString += customer.Value.ToString() + "\n";
            }
            printString += "Shops: \n";
            foreach (var shop in Shops)
            {
                printString += shop.Value.ToString() + "\n";
            }                       
            return printString;
        }
    }


}