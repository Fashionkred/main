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
    private string tagMap;
    private string productMap;
    private string title;
    private long originalLookId;
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
        
        string[] products = {string.Empty};
        if (this.Request.QueryString["productmap"] != null)
        {
            products = HttpUtility.UrlDecode(Request.QueryString["productmap"]).Split('|');
            this.productMap = "<ProductList>";

            for (int i = 0; i < products.Count(); i++)
            {
                string[] productParams = products[i].Split(',');
                if (productParams.Count() != 4)
                    return false;
                //product params are - pid, colorid, categoryid and iscover (bool indicating cover product or not)
                this.productMap += ("<Product pId=\"" + productParams[0] + "\" colorId=\"" + productParams[1]
                                        + "\" catId=\"" + productParams[2] + "\" isCover=\"" + productParams[3] + "\"/>");
            }
            this.productMap += "</ProductList>";

        }

        if (this.Request.QueryString["tagmap"] != null )
        {
            string[] tags = HttpUtility.UrlDecode(Request.QueryString["tagmap"]).Split('|');
            this.tagMap = "<TagList>";

            for (int i = 0; i < tags.Count(); i++)
            {
                if(tags[i] != "undefined")
                    tagMap += ("<Tag tName=\"" + tags[i].TrimStart(' ') + "\"/>");
            }
            this.tagMap += "</TagList>";

        }

        if (this.Request.QueryString["title"] != null && this.Request.QueryString["title"] != "undefined")
        {
            this.title = HttpUtility.UrlDecode(this.Request.QueryString["title"]);
        }

        if (!string.IsNullOrEmpty(this.Request.QueryString["originalLookId"]))
        {
            this.originalLookId = long.Parse(this.Request.QueryString["originalLookId"].ToString());
        }
        else
        {
            this.originalLookId = 0;
        }

        if (this.Request.QueryString["uid"] != null)
        {
            this.userId = long.Parse(this.Request.QueryString["uid"]);
        }

        //if (this.Request.QueryString["colormap"] != null)
        //{
        //    string[] colors = HttpUtility.UrlDecode(Request.QueryString["colormap"]).Split('|');
        //    this.colormap = "<ColorList>";

        //    for (int i = 0; i < colors.Count(); i++ )
        //    {
        //        if (string.IsNullOrEmpty(colors[i]) || colors[i] == "Clear")
        //            continue;
        //        else
        //        {
        //            colormap += ("<Color pId=\"" + products[i] + "\" cId=\"" + colors[i] + "\" />");
        //        }
        //    }
        //    this.colormap += "</ColorList>";
        //}

        return true;
    }

    private LookMessage SaveLook()
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        string query = "EXEC [stp_SS_SaveLook] @product='" + productMap + "', @uid=" + this.userId + ",@tag='" + tagMap + "', @title=N'"+ this.title + "',@color ='" + this.colormap +"'";
        if (originalLookId != 0)
        {
            query += (", @originalLook=" + this.originalLookId);
        }

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
                msg.LookDescription = look.title;

                foreach (Tag tag in msg.Look.tags)
                {
                    if (!tag.name.StartsWith("#"))
                        msg.LookDescription += ("#" + tag.name);
                    else
                        msg.LookDescription += tag.name;

                    msg.LookDescription += " ";
                }

                for (int i=0; i < msg.Look.products.Count ; i++)
                {
                    msg.LookDescription += "#" + (msg.Look.products[i].brandName);
                    if (i < msg.Look.products.Count - 1)
                        msg.LookDescription += " ";
                }

                
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