using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ShopSenseDemo;

/// <summary>
/// Summary description for ColorPickerPanel
/// </summary>
public class ColorPickerPanel: Panel
{
    
	public ColorPickerPanel(long uid, int contestId, int position)
	{

        HtmlGenericControl ul = new HtmlGenericControl("ul");
        this.Controls.Add(ul);
        this.CssClass = "color-filter";

        HtmlGenericControl liTitle = new HtmlGenericControl("li");
        liTitle.Attributes.Add("class", "round5 title");
        liTitle.Controls.Add( new Literal {Text = "Color"});
        ul.Controls.Add(liTitle);

        List<CanonicalColors> colors = new List<CanonicalColors>() {CanonicalColors.Beige, CanonicalColors.Black, CanonicalColors.Blue, CanonicalColors.Brown, CanonicalColors.Gold,
                                               CanonicalColors.Gray, CanonicalColors.Green, CanonicalColors.Orange, CanonicalColors.Pink, CanonicalColors.Purple,
                                               CanonicalColors.Red, CanonicalColors.Silver, CanonicalColors.White, CanonicalColors.Yellow, CanonicalColors.Clear};
        foreach (CanonicalColors color in colors)
        {
            HtmlGenericControl li = new HtmlGenericControl("li");
            li.Attributes.Add("class", "round5");
            ul.Controls.Add(li);

            HyperLink link = new HyperLink();
            link.NavigateUrl = "javascript:GetProductsByColor(" + uid + "," + contestId + "," + position + ",'" + color + "');";
            link.ToolTip = color.ToString();
            link.CssClass = color.ToString();
            if (color == CanonicalColors.Clear)
            {
                link.Text = "Clear";
            }
            
            li.Controls.Add(link);
        }
        
	}
}