using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShopSenseDemo;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.IO;

public partial class GetProduct : System.Web.UI.Page
{
    //Variables
    private long productId;
    private int position;
    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        bool setupVariables = SetupVariables();

        if (!setupVariables)
        {
            ProductMessage message = new ProductMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
            Response.Write(SerializationHelper.ToJSONString(typeof(ProductMessage), message));
        }

        //Get product
        GetFavoriteProduct();
    }

    private bool SetupVariables()
    {
        // fail if any of these variables are missing
        if (this.Request.QueryString["pid"] == null
                || this.Request.QueryString["pos"] == null)
        {
            return false;
        }

        

        if (this.Request.QueryString["pid"] != null)
        {
            this.productId = long.Parse(this.Request.QueryString["pid"]);
        }

        if (this.Request.QueryString["pos"] != null)
        {
            this.position = int.Parse(this.Request.QueryString["pos"]);
        }

        return true;
    }

    private void GetFavoriteProduct()
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        
        Product product = Product.GetProductById(this.productId, db);

        ProductMessage msg = new ProductMessage();

        string pos = "left";
        switch (position)
        {
            case 1:
                pos = "left";
                break;
            case 2:
                pos = "right";
                break;
        }

        Panel productPanel = WebHelper.GetProductPanel(pos,product);
        msg.ProductId = product.id;
        msg.ProductHtml = WebHelper.RenderHtml(productPanel);
        msg.Panel = position;

        string callbackName = Request.QueryString["callback"];
        Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(ProductMessage), msg) + ");");
        
    }
}