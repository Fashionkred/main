using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopSenseDemo;

/// <summary>
/// Summary description for Contest
/// </summary>
public class Contest
{
    public int Retailer {set; get;}

    public int CombinationSet {set; get;}

    public List<string> Categories { set; get; }

    public long CreatorId { set; get; }

	public Contest()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}