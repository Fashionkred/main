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

public partial class love : System.Web.UI.Page
{
    //Variables
    public long userId;
    public long productId;
    public long retailerId;
    public int heart;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.retailerId = int.Parse(ConfigurationManager.AppSettings["Retailer"]);

        if (string.IsNullOrEmpty(this.Request.QueryString["uid"]))
        {
            LoveStatusMessage message = new LoveStatusMessage() { ErrorMessage = "Please <a href=\"/register.aspx\">Register</a> or <a href=\"/login.aspx\">login</a> to confirm your vote!<br/>" };
            Response.Write(SerializationHelper.ToJSONString(typeof(LoveStatusMessage), message));
            return;
        }

        bool setupVariables = SetupVariables();

        //Check if the userid is same as the session user else return
        if (this.Session["user"] != null)
        {
            UserProfile user = this.Session["user"] as UserProfile;
            if (user.userId != this.userId)
            {
                LoveStatusMessage message = new LoveStatusMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
                string callbackName = Request.QueryString["callback"];
                Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(LoveStatusMessage), message) + ");");
            }
        }
        else
        {
            //get redirect url and get the user to log in via facebook
            //check if there's a look id
            string lookid = string.Empty;
            if (Request.QueryString["lid"] != null)
                lookid = Request.QueryString["lid"];

            string redirectUrl = FacebookHelper.GetCode(lookid);
            LoveStatusMessage message = new LoveStatusMessage() { RedirectUrl = redirectUrl };
            string callbackName = Request.QueryString["callback"];
            Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(LoveStatusMessage), message) + ");");
            return; 
        }

        if (!setupVariables)
        {
            LoveStatusMessage message = new LoveStatusMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
            string callbackName = Request.QueryString["callback"];
            Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(LoveStatusMessage), message) + ");");
        }

        //Save Vote and get the new look
        SaveLove();
    }

    private bool SetupVariables()
    {
        // fail if any of these variables are missing
        if (this.Request.QueryString["uid"] == null
            || this.Request.QueryString["pid"] == null)
        {
            return false;
        }

        if (this.Request.QueryString["uid"] != null)
        {
            this.userId = long.Parse(this.Request.QueryString["uid"]);
        }

        if (this.Request.QueryString["pid"] != null)
        {
            this.productId = long.Parse(this.Request.QueryString["pid"]);
        }

        if (this.Request.QueryString["heart"] != null)
        {
            this.heart = int.Parse(this.Request.QueryString["heart"]);
        }
        return true;
    }

    private void SaveLove()
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        string query = "EXEC [stp_SS_SaveLove] @uid=" + this.userId + ", @pid=" + this.productId + ",@love=" + this.heart;
        SqlConnection myConnection = new SqlConnection(db);

        Product p = new Product();
        try
        {
            myConnection.Open();
            using (SqlDataAdapter adp = new SqlDataAdapter(query, myConnection))
            {
                SqlCommand cmd = adp.SelectCommand;
                cmd.CommandTimeout = 300000;
                System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    p = Product.GetProductFromSqlDataReader(dr);
                }
            }

            
            LoveStatusMessage msg = new LoveStatusMessage();
            msg.product = p;

            IList<ShopSenseDemo.Product> loves = UserProfile.GetLovesByUserId(this.userId, p,this.retailerId, db);

            Panel favorites = new Panel();
            foreach (ShopSenseDemo.Product love in loves)
            {
                Panel panel = new Panel();
                panel.CssClass = "fav-image";

                HyperLink link = new HyperLink();
                System.Web.UI.WebControls.Image fav = new System.Web.UI.WebControls.Image();
                link.Controls.Add(fav);

                fav.ImageUrl = love.GetThumbnailUrl();
                fav.AlternateText = love.name;
                link.NavigateUrl = love.url;
                link.CssClass = "OutBoundLink";

                panel.Controls.Add(link);

                favorites.Controls.Add(panel);
            }

            if (loves.Count < 7)
            {
                for (int i = 0; i < 7 - loves.Count; i++)
                {
                    Panel panel = new Panel();
                    panel.CssClass = "fav-image-standart";

                    favorites.Controls.Add(panel);
                }
            }

            StringBuilder content = new StringBuilder();
            StringWriter sWriter = new StringWriter(content);
            HtmlTextWriter htmlWriter = new HtmlTextWriter(sWriter);
            favorites.RenderControl(htmlWriter);

            msg.FavoritesHtml = content.ToString();

            string callbackName = Request.QueryString["callback"];
            Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(LoveStatusMessage), msg) + ");");
        }
        finally
        {
            myConnection.Close();
        }
    }
}