using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for Promo
/// </summary>
[DataContract]
public class Promo
{
    [DataMember]
    public string deeplink { get; set; }

    [DataMember]
    public string startDate { get; set; }

    [DataMember]
    public string endDate { get; set; }

    [DataMember]
    public string iPhoneAsset { get; set; }

    [DataMember]
    public string iPadAsset { get; set; }
    [DataMember]
    public int iPhoneHeight { get; set; }
    [DataMember]
    public int iPadHeight { get; set; }
    [DataMember]
    public int tagId { get; set; }

	public Promo(string link, string sDate, string eDate, string iphAsset, string ipdAsset, int iphWidth, int ipdWidth)
	{
        this.deeplink = link;
        this.startDate = sDate;
        this.endDate = eDate;
        this.iPhoneAsset = iphAsset;
        this.iPadAsset = ipdAsset;
        this.iPhoneHeight = iphWidth;
        this.iPadHeight = ipdWidth;
        this.tagId = 0;
	}
}

[DataContract]
public class Promos
{
    [DataMember]
    public List<Promo> promos { get; set; }
}