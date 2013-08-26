using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ShopSenseDemo;

/// <summary>
/// Summary description for ProductHyperLink
/// </summary>
public class ProductHyperLink: HyperLink
{
	public ProductHyperLink(Product p)
	{
        this.CssClass = "OutBoundLink";
        this.NavigateUrl = p.url;
        this.Target = "_blank";
	}
}