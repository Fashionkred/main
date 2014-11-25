using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace ShopSenseDemo
{
    public class SerializationHelper
    {
        public static string ToJSONString(Type type, object msg)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                //List <object> knownTypes = new List<object>{Product, UserProfile, Look}();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(type, new Type[] {typeof(Product), typeof(UserProfile), typeof(Tag), typeof(CategoryTree)});
                ser.WriteObject(stream, msg);

                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }
}
