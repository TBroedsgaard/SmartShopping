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
    public class CanonicalProduct
    {
        public int Uid;
        public string Name;
        public string Description;
        public string ImageURL;

        public CanonicalProduct(int uid, string name, string description, string imageURL)
        {
            this.Uid = uid;
            this.Name = name;
            this.Description = description;
            this.ImageURL = imageURL;
        }

        public CanonicalProduct(SerializationInfo info, StreamingContext ctxt) 
        {
            this.Uid = (int)info.GetValue("Uid", typeof(int));
            this.Name = (string)info.GetValue("Name", typeof(string));
            this.Description = (string)info.GetValue("Description", typeof(string));
            this.ImageURL = (string)info.GetValue("ImageURL", typeof(string));            
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Uid", this.Uid);
            info.AddValue("Name", this.Name);
            info.AddValue("Description", this.Description);
            info.AddValue("ImageURL", this.ImageURL);
        }

        public override string ToString()
        {
            string printString = this.Uid + " " + this.Name + " " + this.Description + " " + this.ImageURL;
            return printString;
        }
    }
}
