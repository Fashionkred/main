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

public partial class Vote : BasePage
{
    public enum VoteType
    {
        DownVote=0,
        UpVote,
        Neutral
    }

    //Variables
    public long userId;
    public long lookId;
    public VoteType vote;
    public int pointIncrement;
    private int contestId;
    private long retailerId;
    private bool isLightWeight;

    private delegate void NotificationDelegete(long lookId, long userId, string accessToken, VoteType vote);
    private NotificationDelegete notifications;

    private bool SetupVariables()
    {
        // fail if any of these variables are missing
        if (this.Request.QueryString["uid"] == null
            || this.Request.QueryString["lid"] == null
            || this.Request.QueryString["votetype"] == null
            || this.Request.QueryString["point"] == null)
        {
            return false;
        }

        if (this.Request.QueryString["uid"] != null)
        {
            this.userId = long.Parse(this.Request.QueryString["uid"]);
        }

        if (this.Request.QueryString["lid"] != null)
        {
            this.lookId = long.Parse(this.Request.QueryString["lid"]);
        }

        if (this.Request.QueryString["votetype"] != null)
        {
            switch (this.Request.QueryString["votetype"])
            {
                case "down":
                    this.vote = VoteType.DownVote;
                    break;
                case "up":
                    this.vote = VoteType.UpVote;
                    break;
                
                default:
                    this.vote = VoteType.Neutral;
                    break;
            }
        }

        if (this.Request.QueryString["point"] != null)
        {
            this.pointIncrement = int.Parse(this.Request.QueryString["point"]);
        }

        if (this.Request.QueryString["lightweight"] != null)
        {
            this.isLightWeight = true;
        }

        return true;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        
        if (string.IsNullOrEmpty(this.Request.QueryString["uid"]))
        {
            VoteStatusMessage message = new VoteStatusMessage() { ErrorMessage = "Please <a href=\"/register.aspx\">Register</a> or <a href=\"/login.aspx\">login</a> to confirm your vote!<br/>" };
            Response.Write(SerializationHelper.ToJSONString(typeof(VoteStatusMessage), message));
            return;
        }

        bool setupVariables = SetupVariables();

        //Check if the userid is same as the session user else return
        if (this.Session["user"] != null)
        {
            UserProfile user = this.Session["user"] as UserProfile;
            if (user.id != this.userId)
            {
                VoteStatusMessage message = new VoteStatusMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
                string callbackName = Request.QueryString["callback"];
                Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(VoteStatusMessage), message) + ");"); 
                return;
            }
            else
            {
                //update the user's points
                user.points += this.pointIncrement;
                this.Session["user"] = user;
            }
        }
        else
        {
            //get redirect url and get the user to log in via facebook
            string redirectUrl = FacebookHelper.GetCode(this.lookId.ToString());
            VoteStatusMessage message = new VoteStatusMessage() { RedirectUrl = redirectUrl };
            string callbackName = Request.QueryString["callback"];
            Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(VoteStatusMessage), message) + ");"); 
            return;
        }

        if (!setupVariables)
        {
            VoteStatusMessage message = new VoteStatusMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
            string callbackName = Request.QueryString["callback"];
            Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(VoteStatusMessage), message) + ");");
        }

        if (this.Session["contest"] == null)
        {
            this.contestId = int.Parse(ConfigurationManager.AppSettings["ContestId1"]);
            this.Session["contest"] = this.contestId;
        }
        else
        {
            this.contestId = (int)this.Session["contest"];
        } 
        this.retailerId = long.Parse(ConfigurationManager.AppSettings["Retailer"]);

        //Save Vote and get the new look
        SaveVote();

        //Send notifications asynchronously
        notifications = new NotificationDelegete(this.SendNotifications);

        string appAccessToken = null;
        //check if app access token exists
        if (this.Session["app_access_token"] != null)
        {
            appAccessToken = this.Session["app_access_token"].ToString();
        }

        notifications.BeginInvoke(this.lookId, this.userId,appAccessToken, this.vote, null, null);

    }

    public void SendNotifications(long lookId, long userId, string appAccessToken, VoteType vote)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        //bool isVoted = false;
        Look look = Look.GetLookById(lookId, userId, db);

        //send notification to the creator of the outfit unless the creator is a bot (bot is <=0)
        if (look.creator.facebookId > 0 && vote == VoteType.UpVote)
        {
            UserProfile user = UserProfile.GetUserProfileById(userId, db);

            //Post a fb notification that user has loved a vote
            
            try
            {
                if (UserProfile.IsFriend(look.creator.id, user.facebookId))
                {
                    //Send the notification only if the user is creator's friend
                    appAccessToken = FacebookHelper.SendNotification(appAccessToken, look.creator.facebookId, user.facebookId, look);
                    this.Session["app_access_token"] = appAccessToken;
                }
            }
            catch(Facebook.FacebookOAuthException ex)
            {
                //Log the oauth exception.
            }
        }

        //send notification to a voter if their friend voted on the same outfit
    }

    public void SaveVote()
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        string query = "EXEC [stp_SS_SaveVote] @uid=" + this.userId + ", @lid=" + this.lookId + ", @vote=" + this.vote.GetHashCode() + ", @pointinc=" + this.pointIncrement;
        SqlConnection myConnection = new SqlConnection(db);

        try
        {
            myConnection.Open();
            using (SqlDataAdapter adp = new SqlDataAdapter(query, myConnection))
            {
                SqlCommand cmd = adp.SelectCommand;
                cmd.CommandTimeout = 300000;
                cmd.ExecuteNonQuery();
            }

            if (this.isLightWeight)
                return;

            Look look = Look.GetRandomLook(this.contestId,this.userId, db);
        
            VoteStatusMessage msg = new VoteStatusMessage();
            msg.Look = look;

            bool P1love, P2love, P3love;
            IList<ShopSenseDemo.Product> loves = UserProfile.GetLovesByUserId(this.userId, look, this.retailerId, db, out P1love, out P2love, out P3love);

            //set up th product panel li 
            Panel itemWrapper = new Panel();
            itemWrapper.CssClass = "items-wrapper";

            if (look.products.Count == 3)
            {
                Panel leftItem = WebHelper.GetProductPanel("left3Item", look.products[0], true, true, P1love, false);
                Panel plusItem = new Panel();
                plusItem.CssClass = "plus-content-3items";
                plusItem.Controls.Add(new LiteralControl("+"));
                Panel rightItem = new Panel();
                rightItem.CssClass = "right-content-parts-wrapper";
                rightItem.Controls.Add(new LiteralControl("<center>"));
                Panel rightUpperItem = WebHelper.GetProductPanel("rightUpperSmall", look.products[1], true, true, P2love, false);
                Panel rightLowerItem = WebHelper.GetProductPanel("rightLowerSmall", look.products[2], true, true, P3love, false);
                rightItem.Controls.Add(rightUpperItem);
                rightItem.Controls.Add(rightLowerItem);
                rightItem.Controls.Add(new LiteralControl("</center>"));
                itemWrapper.Controls.Add(leftItem);
                itemWrapper.Controls.Add(plusItem);
                itemWrapper.Controls.Add(rightItem);
            }
            else
            {
                Panel leftItem = WebHelper.GetProductPanel("left", look.products[0], true, true, P1love, false);
                Panel plusItem = new Panel();
                plusItem.CssClass = "plus-content";
                plusItem.Controls.Add(new LiteralControl("+"));
                Panel rightItem = WebHelper.GetProductPanel("right", look.products[1], true, true, P2love, false);
                itemWrapper.Controls.Add(leftItem);
                itemWrapper.Controls.Add(plusItem);
                itemWrapper.Controls.Add(rightItem);
            }

            msg.ProductsHtml = WebHelper.RenderHtml(itemWrapper);

            msg.VoteType = this.vote.ToString();

            Panel favorites = new Panel();
            foreach (ShopSenseDemo.Product love in loves)
            {
                Panel panel = new Panel();
                panel.CssClass = "fav-image";

                ProductHyperLink link = new ProductHyperLink(love);
                System.Web.UI.WebControls.Image fav = new System.Web.UI.WebControls.Image();
                link.Controls.Add(fav);

                fav.ImageUrl = love.GetThumbnailUrl();
                fav.AlternateText = love.name;
                
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
            
            msg.FavoritesHtml = WebHelper.RenderHtml(favorites);

            //set follower bit
            msg.IsFollower = UserProfile.IsFollower(msg.Look.creator.id, this.userId, db) ? 1 : 0;
            
            string callbackName = Request.QueryString["callback"];
            Response.Write( callbackName + "(" + SerializationHelper.ToJSONString(typeof(VoteStatusMessage), msg) + ");");
        }
        finally
        {
            myConnection.Close();
        }
    }

}