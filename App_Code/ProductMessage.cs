using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ShopSenseDemo;

/// <summary>
/// Summary description for GetProductMessage
/// </summary>
[DataContract(Name = "ProductMessage")]
[KnownType(typeof(ProductMessage))]
public class ProductMessage
{
    [DataMember]
    public long ProductId { get; set; }

    [DataMember]
    public string ErrorMessage { get; set; }

    [DataMember]
    public string ProductHtml { set; get; }

    //[DataMember]
    //public string ProductContentHtml { set; get; }

    [DataMember]
    public int Panel { set; get; }

}