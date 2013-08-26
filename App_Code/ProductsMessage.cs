using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for ProductsMessage
/// </summary>
[DataContract]
public class ProductsMessage
{

    [DataMember]
    public string ErrorMessage { get; set; }

    [DataMember]
    public string RedirectUrl { set; get; }

    [DataMember]
    public int Position { set; get; }

    [DataMember]
    public string FavoriteHtml { set; get; }

    [DataMember]
    public string SliderHtml { set; get; }

    [DataMember]
    public string Color { set; get; }

    [DataMember]
    public string Category { set; get; }
}