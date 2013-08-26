using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShopSenseDemo;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

public partial class getproductsbycolor : System.Web.UI.Page
{
   //Variables
    private long userId;
    private int position;
    private string color;
    private int contestId;
    private string category;
    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        bool setupVariables = SetupVariables();

        if (!setupVariables)
        {
            ProductsMessage message = new ProductsMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
            Response.Write(SerializationHelper.ToJSONString(typeof(ProductsMessage), message));
        }

        //Get product
        GetFavoriteProduct();
    }

    private bool SetupVariables()
    {
        // fail if any of these variables are missing
        if (this.Request.QueryString["uid"] == null
                || this.Request.QueryString["pos"] == null)
        {
            return false;
        }

        if (this.Request.QueryString["uid"] != null)
        {
            this.userId = long.Parse(this.Request.QueryString["uid"]);
        }

        if (this.Request.QueryString["pos"] != null)
        {
            this.position = int.Parse(this.Request.QueryString["pos"]);
        }

        if (this.Request.QueryString["cid"] != null)
        {
            this.contestId = int.Parse(this.Request.QueryString["cid"]);
        }

        if (this.Request.QueryString["color"] != null)
        {
            this.color = this.Request.QueryString["color"];
        }

        if (this.Request.QueryString["category"] != null)
        {
            this.category = this.Request.QueryString["category"];
        }

        return true;
    }

    private void GetFavoriteProduct()
    {
        
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        string query = "EXEC [stp_SS_GetLovesByCategoryWithColor] @uId=" + this.userId + ", @position=" + this.position + ",@color=N'" + this.color + "',@contestId=" + this.contestId+ ",@categoryId=N'" + this.category + "'";
        SqlConnection myConnection = new SqlConnection(db);

        try
        {
            myConnection.Open();
            using (SqlDataAdapter adp = new SqlDataAdapter(query, myConnection))
            {
                SqlCommand cmd = adp.SelectCommand;
                cmd.CommandTimeout = 300000;
                System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();

                
                IList<Product> loves = new List<Product>();

                while (dr.Read())
                {
                    Product p = Product.GetProductFromSqlDataReader(dr);
                    loves.Add(p);
                }

                string itemCssClass = "fav-image", sliderId = "sliderCreate", shapeClass ="create-big-shape" , contentClass="create-content-text";
                string text = "SELECT A DRESS";
                if (position > 1)
                {
                    itemCssClass += ("_" + position);
                    sliderId += position;
                    text = position == 2 ? "SELECT A HANDBAG" : "SELECT A SHOE";
                    shapeClass = "create-small-shape";
                    contentClass = "create-content-small-text";
                }

                Panel favCarousel = new Panel();
                
                Panel slider = new Panel();
                //add the emoty box
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Controls.Add(new LiteralControl("<div class=\"" + shapeClass + "\"><div class=\"" + contentClass + "\"><strong>" + text +
                                 "</strong><br>FROM FAVORITES</div></div>"));

                slider.Controls.Add(li);
                
                int carouselPosition = 1;
                foreach (ShopSenseDemo.Product love in loves)
                {
                    Panel item = new Panel();
                    item.CssClass = itemCssClass;

                    HyperLink link = new HyperLink();
                    System.Web.UI.WebControls.Image fav = new System.Web.UI.WebControls.Image();
                    link.Controls.Add(fav);
                    fav.ImageUrl = love.GetThumbnailUrl();
                    fav.AlternateText = love.name;
                    link.NavigateUrl = "javascript:sliderselect(" + position + "," + carouselPosition + ")";
                    link.ToolTip = love.name;
                    item.Controls.Add(link);

                    favCarousel.Controls.Add(item);

                    //Add the item to the top slider 
                    
                    HtmlGenericControl newLi = new HtmlGenericControl("li");

                    Panel productPanel = new Panel();
                    productPanel.ID = "MainContent_" +  sliderId + "div" + carouselPosition.ToString();
                    productPanel.CssClass = position == 1 ? "product-image" : "product-small-image";
                    ProductHyperLink pdtlink = new ProductHyperLink(love);
                    pdtlink.NavigateUrl = love.AffiliateUrl;
                    System.Web.UI.WebControls.Image pdtImg = new System.Web.UI.WebControls.Image();
                    pdtImg.ImageUrl = position == 1 ? love.GetImageUrl() : love.GetNormalImageUrl();
                    pdtImg.AlternateText = love.name;
                    pdtImg.ToolTip = love.name;
                    pdtImg.ID = "pdt-" + love.id.ToString();
                    pdtlink.Controls.Add(pdtImg);
                    productPanel.Controls.Add(pdtlink);

                    newLi.Controls.Add(productPanel);
                    slider.Controls.Add(newLi);

                    carouselPosition++;
                }

                ProductsMessage msg = new ProductsMessage();
                msg.Position = position;
                msg.FavoriteHtml = WebHelper.RenderHtml(favCarousel);
                msg.SliderHtml = WebHelper.RenderHtml(slider);
                msg.Color = this.color;
                msg.Category = this.category;

                string callbackName = Request.QueryString["callback"];
                Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(ProductsMessage), msg) + ");");
            }
        }
        finally
        {
            myConnection.Close();
        }
    }
 
}