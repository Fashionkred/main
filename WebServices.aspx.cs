using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShopSenseDemo;
using System.Configuration;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Web.Services;
using ShopSenseDemo;


public partial class WebServices : System.Web.UI.Page
{



    private delegate void NotificationDelegete(long lookId, long userId, string accessToken, VoteType vote);
    private static NotificationDelegete notifications;

    public enum VoteType
    {
        DownVote = 0,
        UpVote,
        Neutral
    }


    public static long userIdOrg;
    public static long lookId;
    public static VoteType vote;
    public static int pointIncrement;
    private static int contestId;
    private static long retailerId;
    private static bool isLightWeight;


    private static bool SetupVariables(long UserIdMain, string lookIdMain, string voteTypeMain, string pointMain, Boolean lightweightMain, string db)
    {


        // fail if any of these variables are missing
        if (UserIdMain.ToString() == null
            || lookIdMain == null
            || voteTypeMain == null
            || pointMain == null)
        {
            return false;
        }

        if (UserIdMain != null)
        {
            userIdOrg = long.Parse(UserIdMain.ToString());
        }

        if (lookIdMain != null)
        {
            lookId = long.Parse(lookIdMain);
        }

        if (voteTypeMain != null)
        {
            switch (voteTypeMain)
            {
                case "down":
                    vote = VoteType.DownVote;
                    break;
                case "up":
                    vote = VoteType.UpVote;
                    break;

                default:
                    vote = VoteType.Neutral;
                    break;
            }
        }

        if (pointMain != null)
        {
            pointIncrement = int.Parse(pointMain);
        }

        if (lightweightMain != null)
        {
            isLightWeight = true;
        }

        return true;
    }






    protected void Page_Load(object sender, EventArgs e)
    {

    }


    // Getting Connection String.
    public static string GetConnectionString()
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        return db;
    }



    [WebMethod]
    public static Array BindSimilarItems(string categoryId, string colorId, long productId, long userid, string db)
    {

        db = GetConnectionString();

        ShopSenseDemo.Product product = new ShopSenseDemo.Product();


        Dictionary<string, List<Product>> similarProducts = Product.GetSimilarProducts(categoryId, colorId, productId, userid, db);

        return similarProducts.ToArray();

    }




    [System.Web.Services.WebMethod]

    public static string UpdateLove(long UserIdMain, string lookIdMain, string voteTypeMain, int pointMain, Boolean lightweightMain, string db)
    {

        db = GetConnectionString();

        if (string.IsNullOrEmpty(UserIdMain.ToString()))
        {
            //VoteStatusMessage message = new VoteStatusMessage() { ErrorMessage = "Please <a href=\"/register.aspx\">Register</a> or <a href=\"/login.aspx\">login</a> to confirm your vote!<br/>" };
            //  Response.Write(SerializationHelper.ToJSONString(typeof(VoteStatusMessage), message));
            // return;
        }

        bool setupVariables = SetupVariables(UserIdMain, lookIdMain, voteTypeMain, pointMain.ToString(), lightweightMain, db);

        string userfortest = "";

        //Check if the userid is same as the session user else return
        /////   if (Session["user"] != null)
        if (userfortest != null)
        {
            //        //UserProfile user = this.Session["user"] as UserProfile;

            //UserProfile user = Session["user"] as UserProfile;
            // if (user.id != UserIdMain)
            // {
            VoteStatusMessage message = new VoteStatusMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
            ////               //string callbackName = Request.QueryString["callback"];
            //Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(VoteStatusMessage), message) + ");");
            //return;
            // }
            //  else
            ///{
            //update the user's points
            // user.points += pointIncrement;
            /////        this.Session["user"] = user;
            //   }
        }
        else
        {
            //get redirect url and get the user to log in via facebook
            string redirectUrl = FacebookHelper.GetCode(lookId.ToString());
            VoteStatusMessage message = new VoteStatusMessage() { RedirectUrl = redirectUrl };
            //string callbackName = Request.QueryString["callback"];
            //  Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(VoteStatusMessage), message) + ");"); 
            //  return;
        }

        //if (!setupVariables)
        //{
        //    VoteStatusMessage message = new VoteStatusMessage() { ErrorMessage = "Sorry, we\'ve encountered an unknown error.<br />Please try again." };
        //    string callbackName = Request.QueryString["callback"];
        //    Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(VoteStatusMessage), message) + ");");
        //}



        retailerId = long.Parse(ConfigurationManager.AppSettings["Retailer"]);

        //Save Vote and get the new look
        SaveVote();

        //Send notifications asynchronously
        notifications = new NotificationDelegete(SendNotifications);

        string appAccessToken = null;
        ////      //check if app access token exists
        //if (this.Session["app_access_token"] != null)
        //{
        //   appAccessToken = this.Session["app_access_token"].ToString();
        //}

        notifications.BeginInvoke(lookId, UserIdMain, appAccessToken, vote, null, null);



        /////////////////////////////////////////////////////////////////////


        return "";




    }


    [System.Web.Services.WebMethod]

    public static Array BindLooks(string tagId, string userId, string pageId, string db)
    {

        db = GetConnectionString();

        Array looks;

        //Let's get the sets and display them
        if (!string.IsNullOrEmpty(tagId) && tagId != "undefined")
        {
            looks = Look.GetTaggedLooks(db, Convert.ToInt64(userId), int.Parse(tagId), Convert.ToInt32(pageId)).ToArray();
        }
        else
        {
            looks = Look.GetHPLooks(db, Convert.ToInt64(userId), Convert.ToInt32(pageId)).ToArray();
        }



        return looks;

    }



    public static void SendNotifications(long lookId, long userId, string appAccessToken, VoteType vote)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        bool isVoted = false;
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
                    ///////this.Session["app_access_token"] = appAccessToken;
                }
            }
            catch (Facebook.FacebookOAuthException ex)
            {
                //Log the oauth exception.
            }
        }

        //send notification to a voter if their friend voted on the same outfit
    }



    public static void SaveVote()
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        string query = "EXEC [stp_SS_SaveVote] @uid=" + userIdOrg + ", @lid=" + lookId + ", @vote=" + vote.GetHashCode() + ", @pointinc=" + pointIncrement;
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

            if (isLightWeight)
                return;

            Look look = Look.GetRandomLook(contestId, userIdOrg, db);

            VoteStatusMessage msg = new VoteStatusMessage();
            msg.Look = look;

            bool P1love, P2love, P3love;
            IList<ShopSenseDemo.Product> loves = UserProfile.GetLovesByUserId(userIdOrg, look, retailerId, db, out P1love, out P2love, out P3love);

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

            msg.VoteType = vote.ToString();

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
            msg.IsFollower = UserProfile.IsFollower(msg.Look.creator.id, userIdOrg, db) ? 1 : 0;

            ////string callbackName = Request.QueryString["callback"];
            //// Response.Write(callbackName + "(" + SerializationHelper.ToJSONString(typeof(VoteStatusMessage), msg) + ");");
        }
        finally
        {
            myConnection.Close();
        }
    }
}


