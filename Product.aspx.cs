using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShopSenseDemo;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class ProductPage : BasePage
{
    private long productId { set; get; }

    private ShopSenseDemo.Product product { set; get; }

    public void SetOGTags()
    {
        //Set the OG tags if there is a lid
        Master.Page.Title = product.name + " - FashionKred";

        HtmlMeta title = new HtmlMeta();
        title.Attributes.Add("property", "og:title");
        title.Content = product.name ;
        Master.FindControl("head").Controls.Add(title);

        HtmlMeta url = new HtmlMeta();
        url.Attributes.Add("property", "og:url");
        url.Content = "http://fashionKred.com/product.aspx?pid=" + product.id;
        Master.FindControl("head").Controls.Add(url);

        HtmlMeta image = new HtmlMeta();
        image.Attributes.Add("property", "og:image");
        image.Content = product.GetImageUrl();
        Master.FindControl("head").Controls.Add(image);

        HtmlMeta desc = new HtmlMeta();
        desc.Attributes.Add("property", "og:description");
        desc.Content = product.description;
        Master.FindControl("head").Controls.Add(desc);

        HtmlMeta type = new HtmlMeta();
        type.Attributes.Add("property", "og:type");
        type.Content = "fashionkred:outfit";
        Master.FindControl("head").Controls.Add(type);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["pid"] == null)
            return;

        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        
        this.productId = long.Parse(Request.QueryString["pid"]);

        product = ShopSenseDemo.Product.GetProductById(this.productId, db);

        if (product == null)
        {
            this.RedirectToErrorPage(ErrorPageType.GenericError, true);
            return;
        }

        SetOGTags();

        //set the user
        //this.GetUser(db);
        
        //TEMP: Find a look with that product id and redirect to look page
        Look look = Look.GetLookByProductId(this.productId, db);
        Response.Redirect("look.aspx?lid=" + look.id);
        
        //
        BrandName.Text = product.brandName;
        ProductName.Text = product.name;
        ProductImg.ImageUrl = product.GetImageUrl();
        ProductDesc.Text = product.description;
        Retailer.Text = "Buy at " + product.retailer;
        Colors.Text = "<strong>Available Color: </strong>" + product.colorString;
        Sizes.Text = "<strong>Available Size: </strong>" + product.sizeString;

        Price.Text = string.Format("{0:c}", product.price);
        if (product.salePrice != 0)
        {
            Price.CssClass += "strike-through";
            SalePrice.Text = string.Format("{0:c}", product.salePrice);
            SalePrice.Visible = true;
        }

        BuyLink.NavigateUrl = product.url;

        LoveCount.Text = product.loves.ToString();

        //set the user
        if (this.Session["user"] != null)
        {
            this.user = this.Session["user"] as UserProfile;
            UserProfile user = this.Session["user"] as UserProfile;
            this.UserId.Text = user.id.ToString();
        }
        else
        {
            this.GetUser(db);
        }

        

        //meta data
        PId.Text = this.productId.ToString();
    }
}