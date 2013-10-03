using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SmartShoppingLibrary;


namespace SmartShoppingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing SmartShopping classes");
            new TestSmartShoppingClasses();
            Console.ReadLine();
        }

    }
    
    public class TestSmartShoppingClasses
    {
        SmartShoppingData ssd;
        //string filename = @"C:\Users\ubereski\databaseTest.ssd";
        string filename = @"..\..\..\savefiles\databaseTest.ssd";
        Customer customer;
        Shop shop;

        public TestSmartShoppingClasses()
        {
            initSmartShopping();
            interactiveShopping();
            //interactiveManagement();
            //testSerialisation();
            //testLoad();
            //testSave();
            //testLoad();     
            //generateSaveFile();
        }

        private void generateSaveFile()
        {
            SmartShoppingData ssd = new SmartShoppingData();
            ssd.addCanonicalProduct("Coca Cola", "Black sugerwater", "cola.jpg");
            ssd.addCanonicalProduct("Lambi Toilet Paper", "Soft", "lambi.jpg");
            ssd.addCanonicalProduct("Mix of candybars", "Tasty", "nestlebars.jpg");
            ssd.addCanonicalProduct("Carrots", "Goldenrod color", "guleroedder.jpg");

            ssd.addCustomer("John");
            ssd.addShop("Bilka");

            foreach (CanonicalProduct canonicalProduct in ssd.CanonicalProducts.Values)
            {
                Console.Write("Enter price of product " + canonicalProduct.ToString() + ": ");
                decimal price;
                if (decimal.TryParse(Console.ReadLine(), out price))
                {
                    // decimal tryparse does not handle , or . ?!
                    ssd.Shops[0].addProduct(canonicalProduct, price);
                }
                else
                {
                    Console.WriteLine("Invalid price, product not added");
                }
            }

            ssd.Save(this.filename);

        }

        public void initSmartShopping()
        {
            ssd = SmartShoppingData.Load(this.filename);
            SmartShoppingData.stream.Close();
            Console.WriteLine(ssd.ToString());
            Console.ReadLine();
            Console.Clear();
        }
        
        public void testSave()
        {
            //string filename = @"C:\Users\Troels\test2.ssd";          
            Console.WriteLine("Testing save function");
            ssd.Save(filename);
        }

        public void testLoad()
        {
            Console.WriteLine("Testing load function");
            //string filename = @"C:\Users\Troels\test2.ssd";
            SmartShoppingData ssd2;
            ssd2 = SmartShoppingData.Load(filename);
            Console.WriteLine(ssd2.ToString());
            
            /*
            Stream ghostStream = File.Open(filename, FileMode.Open);
            BinaryFormatter ghostbformatter = new BinaryFormatter();
            Console.WriteLine("Reading data from file " + filename);
            CanonicalProduct ghostProd;
            ghostProd = (CanonicalProduct)ghostbformatter.Deserialize(ghostStream);

            Console.WriteLine(ghostProd.ToString() + CanonicalProduct.Uids);
             */
        }

        public void testSerialisation()
        {
            /*
            // test the entire stack
            CanonicalProduct prod = new CanonicalProduct("canprod1", "1st can prod", "url1");
            CanonicalProduct prod2 = new CanonicalProduct("canprod2", "2nd can prod", "url2");
            CanonicalProduct prod3 = new CanonicalProduct("canprod3", "3rd can prod", "url3");
            Console.WriteLine(prod.ToString());
            */
            //string filename = @"C:\Users\Troels\test.ssd";
            /*
            // save
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            Console.WriteLine("Saving Canonical Product to " + filename);
            bformatter.Serialize(stream, prod);
            stream.Close();
            // load
            Stream ghostStream = File.Open(filename, FileMode.Open);
            BinaryFormatter ghostbformatter = new BinaryFormatter();
            Console.WriteLine("Reading data from file " + filename);
            CanonicalProduct ghostProd;
            ghostProd = (CanonicalProduct)ghostbformatter.Deserialize(ghostStream);
            
            Console.WriteLine(ghostProd.ToString());
            */

            /*
            public bool Save(string filename)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            Console.WriteLine("Writing SmartShoppingData to file " + filename);
            bformatter.Serialize(stream, this);
            stream.Close();
            return true;
        }

        public static SmartShoppingData Load(string filename) 
        {
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            Console.WriteLine("Reading SmartShoppingData from file " + filename);
            return (SmartShoppingData)bformatter.Deserialize(stream);
        }
             */
            
        }
        
        private void interactiveManagement()
        {
            throw new NotImplementedException();
        }


        private void interactiveShopping() 
        {
            customer = ssd.Customers[0];
            Console.WriteLine("~~~### Welcome " + customer.Name + " to the interactive SmartShopping experience!!! ###~~~");
            shop = ssd.Shops[0];
            customer.enterShop(shop);
            Console.WriteLine("You have entered " + shop.Name);
            string nextAction = "0";
            while (true)
            {
                if (nextAction.Equals("9"))
                    break;
                Console.WriteLine("What would you like to do? \n 1) Add products to basket, 2) Place order or 3) View statistics?");
                nextAction = Console.ReadLine();
                if (nextAction.Equals("1"))
                {
                    goShop();
                }
                else if (nextAction.Equals("2"))
                {
                    customer.PlaceOrder();
                    break;
                }
                else if (nextAction.Equals("3"))
                {
                    showStats();
                }
                else
                    continue;
            }
            Console.WriteLine("You are now the shop. You have received the following order:");
            Order order = customer.Order;
            Console.WriteLine(order.ToString());
            Console.WriteLine("Press any key when you are done packaging the order");
            Console.ReadLine();
            shop.DeliverOrder(order);
            Console.WriteLine("You are now the costumer again! You have been called to the desk and should pay for your order.");
            Console.WriteLine(order.ToString());
            Console.Write("Please enter the amount to pay: ");
            decimal payment = decimal.Parse(Console.ReadLine());
            if (payment < order.TotalPrice)
                Console.WriteLine("Get your fat ass outta here!");
            else if (payment > order.TotalPrice)
            {
                decimal moneyBack = payment - order.TotalPrice;
                Console.WriteLine("Well, that was a bit too much! Here is your money back: " + moneyBack);
                customer.ReceiveOrder();
            }
            else
            {
                Console.WriteLine("Thank you very much, have a nice day!");
                customer.ReceiveOrder();
            }

            ssd.Save(filename);
            
        }

        private void showStats()
        {
            Console.WriteLine("Here are the stats:\n");
            Console.WriteLine("Average time spent shopping: " + calcAverageShoppingTime());
            Console.WriteLine("Average time spent waiting: " + calcAverageWaitingTime());
        }

        private void goShop()
        {
            Console.WriteLine("You are in " + shop.Name + ". Here you can buy the following products: ");

            foreach (var product in shop.Products.Values)
            {
                Console.WriteLine(product.ToString());
            }
            Console.Write("Pick your product: ");
            int uid = int.Parse(Console.ReadLine());
            Console.Write("How many do you want? ");
            int quantity = int.Parse(Console.ReadLine());
            Product selectedProduct = shop.Products[uid];
            customer.Order.AddItem(selectedProduct, quantity);
            Console.WriteLine("Added " + quantity + " of " + selectedProduct + " to your basket");
        }
        
        public int calcAverageShoppingTime()
        {
            TimeSpan timeTotal = new TimeSpan();
            foreach (Order order in shop.Orders[OrderState.Delivered])
            {
                DateTime shoppingSince = order.Timestamps[OrderState.UnderConstruction];
                DateTime shoppingUntil = order.Timestamps[OrderState.Placed];
                TimeSpan time = shoppingUntil - shoppingSince;
                timeTotal += time;
            }
            int averageShoppingTime = timeTotal.Seconds / shop.Orders[OrderState.Delivered].Count;

            return averageShoppingTime;
        }

        public int calcAverageWaitingTime()
        {
            TimeSpan waitingTimeTotal = new TimeSpan();
            foreach (Order order in shop.Orders[OrderState.Delivered])
            {
                DateTime waitingSince = order.Timestamps[OrderState.Placed];
                DateTime waitingUntil = order.Timestamps[OrderState.Packaged];
                TimeSpan waitingTime = waitingUntil - waitingSince;
                waitingTimeTotal += waitingTime;
                //Console.WriteLine(waitingSince.ToString() + " " + waitingUntil.ToString());

                
            }
            int averageWaitingTime = waitingTimeTotal.Seconds / shop.Orders[OrderState.Delivered].Count;

            return averageWaitingTime;
        
        }
        /*
        public void testProduct()
        {
            product = new Product(0, "Ketchup", 100.0m, "Det er rødt og smager godt");

            Console.WriteLine(product.ToString());
        }*/
        /*
        public void testProductCollection()
        {
            products = new ProductCollection();
            //products.addProduct("Ketchup", 100.0m, "Det er rødt og smager godt");
            //products.addProduct("Coca Cola", 10.0m, "Det er sort og smager sødt");

            foreach (var product in products.AllProducts)
            {
                Console.WriteLine(product.Key + " " + product.Value);            
            }
        }*/
        /*
        public void testShop() 
        { 
            shop = new Shop(0, "SuperBest");
            Console.WriteLine(shop.ToString());            
        }*/

        public void testShopCollection()
        {
            //shops = new ShopCollection();
            //shops.addShop("Brugsen");
            //shops.addShop("Kvickly");
            //shops.ShopDictionary[0].Products.addProduct("Ketchup", 100.0m, "Det er rødt og smager godt");
            //shops.ShopDictionary[0].Products.addProduct("Coca Cola", 10.0m, "Det er sort og smager sødt");
            /*foreach (var shop in shops.ShopDictionary)
            {
                Console.WriteLine(shop.Value.ShopUid);
                foreach (var product in shop.Value.Products.AllProducts)
                {
                    Console.WriteLine(product.Value.ToString());
                }
            } */           
        }
    
        public void testOrderCollection()
        {
            // finish writing test!
            /*
            Order orders = new Order();
            orders.addOrder(product, 2);
             */
        }
    }
}
