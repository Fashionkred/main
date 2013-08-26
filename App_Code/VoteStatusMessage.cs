using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ShopSenseDemo;
using System.Web.UI.WebControls;

[DataContract]
public class VoteStatusMessage
{
    [DataMember]
    public string ErrorMessage { get; set; }

    [DataMember]
    public Look Look { set; get; }

    [DataMember]
    public string ProductsHtml { set; get; }

    [DataMember]
    public string VoteType { set; get; }

    [DataMember]
    public string FavoritesHtml { set; get; }

    [DataMember]
    public string RedirectUrl { set; get; }

    [DataMember]
    public int IsFollower { set; get; }

}
