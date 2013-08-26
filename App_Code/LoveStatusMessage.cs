using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ShopSenseDemo;

/// <summary>
/// Summary description for LoveStatusMessage
/// </summary>
[DataContract]
public class LoveStatusMessage
{
    [DataMember]
    public string ErrorMessage { get; set; }

    [DataMember]
    public Product product { set; get; }

    [DataMember]
    public string FavoritesHtml { set; get; }

    [DataMember]
    public string RedirectUrl { set; get; }
}