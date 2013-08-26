using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShopSenseDemo;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.IO;

public partial class createlook : System.Web.UI.Page
{
    //Variables
    private long productId1;
    private long productId2;
    private long productId3;
    private long userId;
    private int contestId;
    private string colormap;

    private delegate void NotificationDelegete(Look look, string accessToken);
    private NotificationDelegete notifications;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["contest"] == null)
        {
            this.contestId = int.Parse(ConfigurationManager.AppSettings["ContestId1"]);
            this.Session["contest"] = this.contestId;
        }
        else
        {
            this.contestId = (int)this.Session["contest"];
        } 

        bool setupVariables = SetupVariables();

        if (!setupVariables)
        {
            LookMessage message = new LookMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
            Response.Write(SerializationHelper.ToJSONString(typeof(LookMessage), message));
        }

        //Check if the userid is same as the session user else return
        if (this.Session["user"] != null)
        {
            UserProfile user = this.Session["user"] as UserProfile;
            if(user.id != this.userId)
            {
                LookMessage message = new LookMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
                Response.Write(SerializationHelper.ToJSONString(typeof(LookMessage), message));
            }
        }
        else
        {
            LookMessage message = new LookMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
            Response.Write(SerializationHelper.ToJSONString(typeof(LookMessage), message));
        }

        //Save Look
        LookMessage msg = SaveLook();

        //set up the image for the ocmbined look
        try
        {
            string imageFilePath = Path.Combine(Server.MapPath("images/looks"), msg.Look.id + ".jpg");
                
            if (msg.Look.products.Count == 3)
            {
                if (!File.Exists(imageFilePath))
                {
                    WebHelper.MergeThreeImages(msg.Look.products[0].GetImageUrl(), msg.Look.products[1].GetNormalImageUrl(), msg.Look.products[2].GetNormalImageUrl(), imageFilePath);
                }
            }
            else
            {
                if (!File.Exists(imageFilePath))
                {
                    WebHelper.MergeTwoImages(msg.Look.products[0].GetImageUrl(), msg.Look.products[1].GetImageUrl(), imageFilePath);
                }
            }
        }
        catch
        {
            //signal to the pinterest pinner that image unavailable?
        }

        string callbackName = Request.QueryString["callback"];
        Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(LookMessage), msg) + ");");

        //Send notifications asynchronously
        notifications = new NotificationDelegete(this.SendNotifications);

        string appAccessToken = null;
        //check if app access token exists
        if (this.Session["app_access_token"] != null)
        {
            appAccessToken = this.Session["app_access_token"].ToString();
        }

        notifications.BeginInvoke(msg.Look, appAccessToken, null, null);
    }

    private bool SetupVariables()
    {
        // fail if any of these variables are missing
        if (this.Request.QueryString["pid1"] == null
                || this.Request.QueryString["pid2"] == null
                || this.Request.QueryString["uid"] == null)
        {
            return false;
        }

        

        if (this.Request.QueryString["pid1"] != null)
        {
            this.productId1 = long.Parse(this.Request.QueryString["pid1"]);
        }

        if (this.Request.QueryString["pid2"] != null)
        {
            this.productId2 = long.Parse(this.Request.QueryString["pid2"]);
        }

        if (this.Request.QueryString["pid3"] != null)
        {
            this.productId3 = long.Parse(this.Request.QueryString["pid3"]);
        }

        if (this.Request.QueryString["uid"] != null)
        {
            this.userId = long.Parse(this.Request.QueryString["uid"]);
        }

        if (this.Request.QueryString["colormap"] != null)
        {
            string[] colors = HttpUtility.UrlDecode(Request.QueryString["colormap"]).Split('|');
            this.colormap = "<ColorList>";

            for (int i = 0; i < colors.Count(); i++ )
            {
                if (string.IsNullOrEmpty(colors[i]) || colors[i] == "Clear")
                    continue;
                else
                {
                    long productId = this.productId1;
                    switch (i)
                    {
                        case 0:
                            productId = this.productId1;
                            break;
                        case 1:
                            productId = this.productId2;
                            break;
                        case 2:
                            productId = this.productId3;
                            break;
                    }

                    colormap += ("<Color pId=\"" + productId + "\" cId=\"" + colors[i] + "\" />");
                }
            }
            this.colormap += "</ColorList>";
        }

        return true;
    }

    private LookMessage SaveLook()
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        string query = "EXEC [stp_SS_SaveLook] @pId1=" + this.productId1 + ", @pId2=" + this.productId2 + ", @uid=" + this.userId + ",@contestId="+this.contestId +",@pId3=" + this.productId3 + ",@color ='" + this.colormap +"'";
        SqlConnection myConnection = new SqlConnection(db);

        LookMessage msg = new LookMessage();

        try
        {
            myConnection.Open();
            using (SqlDataAdapter adp = new SqlDataAdapter(query, myConnection))
            {
                SqlCommand cmd = adp.SelectCommand;
                cmd.CommandTimeout = 300000;
                System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                Look look = new Look();

                look = Look.GetLookFromSqlReader(dr);

                msg.Look = look;
                msg.LookDescription = "Outfit containing ";
                for (int i=0; i < msg.Look.products.Count ; i++)
                {
                    msg.LookDescription += (msg.Look.products[i].name);
                    if (i < msg.Look.products.Count - 1)
                        msg.LookDescription += ", ";
                }
                msg.LookDescription += (" - Check out at http://fashionkred.com/look.aspx?lid=" + msg.Look.id + "&ref=" + this.userId);
            }
        }
        finally
        {
            myConnection.Close();
        }

        return msg;
    }

    public void SendNotifications(Look look, string appAccessToken)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        //send notification to the creator of the outfit unless the creator is a bot (bot is <=0)
        if (look.creator.facebookId > 0)
        {
            //Get look's creator's follower's facebook ids
            List<long> followerIds = UserProfile.GetUserFollowersFacebookIds(look.creator.id, db);

            //Post a fb notification to user's followers that user has posted a look
            foreach (long followerId in followerIds)
            {
                try
                {
                    appAccessToken = FacebookHelper.SendCreateNotification(appAccessToken, look.creator.facebookId, followerId, look);
                    this.Session["app_access_token"] = appAccessToken;
                }
                catch (Facebook.FacebookOAuthException ex)
                {
                    //Log the oauth exception.
                }
            }
        }

        //send notification to a voter if their friend voted on the same outfit
    }

}