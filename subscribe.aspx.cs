using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShopSenseDemo;
using System.Configuration;
using System.Data.SqlClient;

public partial class subscribe : System.Web.UI.Page
{
    public long userId;
    public long subscriberId;
    public int isSubscribe;

    private delegate void NotificationDelegete(string accessToken);
    private NotificationDelegete notifications;

    private bool SetupVariables()
    {
        // fail if any of these variables are missing
        if (this.Request.QueryString["uid"] == null
            || this.Request.QueryString["sid"] == null)
        {
            return false;
        }

        if (this.Request.QueryString["uid"] != null)
        {
            this.userId = long.Parse(this.Request.QueryString["uid"]);
        }

        if (this.Request.QueryString["sid"] != null)
        {
            this.subscriberId = long.Parse(this.Request.QueryString["sid"]);
        }

        if (this.Request.QueryString["subscribe"] != null)
        {
            this.isSubscribe = int.Parse(this.Request.QueryString["subscribe"]);
        }
        return true;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        bool setupVariables = SetupVariables();

        //Check if the userid is same as the session user else return
        if (this.Session["user"] != null)
        {
            UserProfile user = this.Session["user"] as UserProfile;
            if (user.id != this.userId)
            {
                SubscribeStatusMessage message = new SubscribeStatusMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
                string callbackName = Request.QueryString["callback"];
                Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(SubscribeStatusMessage), message) + ");");
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
            SubscribeStatusMessage message = new SubscribeStatusMessage() { RedirectUrl = redirectUrl };
            string callbackName = Request.QueryString["callback"];
            Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(SubscribeStatusMessage), message) + ");");
            return;
        }

        if (!setupVariables)
        {
            SubscribeStatusMessage message = new SubscribeStatusMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
            string callbackName = Request.QueryString["callback"];
            Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(SubscribeStatusMessage), message) + ");");
        }

        //Save Vote and get the new look
        Subscribe();

        //Send notifications asynchronously
        notifications = new NotificationDelegete(this.SendNotifications);

        string appAccessToken = null;
        //check if app access token exists
        if (this.Session["app_access_token"] != null)
        {
            appAccessToken = this.Session["app_access_token"].ToString();
        }

        notifications.BeginInvoke( appAccessToken, null, null);

        if(Request.QueryString["redirect"] != null)
        {
            Response.Redirect(Request.QueryString["redirect"]);
        }
    }

    private void Subscribe()
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        string query = "EXEC [stp_SS_Subscribe] @uid=" + this.userId + ", @sid=" + this.subscriberId + ",@subscribe=" + this.isSubscribe;
        SqlConnection myConnection = new SqlConnection(db);

        Product p = new Product();
        try
        {
            myConnection.Open();
            using (SqlDataAdapter adp = new SqlDataAdapter(query, myConnection))
            {
                SqlCommand cmd = adp.SelectCommand;
                cmd.CommandTimeout = 300000;
                cmd.ExecuteNonQuery();
            }

            SubscribeStatusMessage msg = new SubscribeStatusMessage();

            string callbackName = Request.QueryString["callback"];
            Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(SubscribeStatusMessage), msg) + ");");
        }
        finally
        {
            myConnection.Close();
        }
    }
    public void SendNotifications(string appAccessToken)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        //send notification to the creator of the outfit unless the creator is a bot (bot is <=0)
        if (this.subscriberId > 0 && this.isSubscribe != 0)
        {
            UserProfile user = UserProfile.GetUserProfileById(userId, db);
            UserProfile creator = UserProfile.GetUserProfileById(subscriberId, db);

            //Post a fb notification that user has loved a vote

            try
            {
                //Send the notification only if the user is creator's friend
                appAccessToken = FacebookHelper.SendFollowNotification(appAccessToken, creator.facebookId, user);
                this.Session["app_access_token"] = appAccessToken;
                
            }
            catch (Facebook.FacebookOAuthException ex)
            {
                //Log the oauth exception.
            }
        }
    }
}