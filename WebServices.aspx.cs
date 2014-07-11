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
using System.Web.Script.Serialization;


public partial class WebServices : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    // Getting Connection String.
    public static string GetConnectionString()
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        return db;
    }

    //Home page Services

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

    [System.Web.Services.WebMethod]
    public static Array GetFreshLooks(long userId, int offset, int limit)
    {

        string db = GetConnectionString();

        Array looks = Look.GetHPLooks(db, userId, offset, limit).ToArray();
        
        return looks;

    }
    [System.Web.Services.WebMethod]
    public static Array GetPopularLooks(long userId, int offset, int limit)
    {

        string db = GetConnectionString();

        Array looks = Look.GetHPLooks(db, userId, offset, limit, true).ToArray();

        return looks;

    }

    [System.Web.Services.WebMethod]
    public static Array GetRecentLooks(long userId, int offset, int limit)
    {

        return GetFreshLooks(userId, offset, limit);

    }
    [System.Web.Services.WebMethod]

    public static Array GetPopularTags(long userId, int offset, int limit)
    {
        string db = GetConnectionString();

        Array tags;

        tags = Tag.getPopularHashtags(userId, db, offset, limit).ToArray();

        return tags;
    }
    [System.Web.Services.WebMethod]

    public static Array GetTagMetaInfo(long userId, long tagId, int noOfLooks, int noOfItems, int noOfStylists)
    {
        string db = GetConnectionString();

        Array tagmetaInfo;

        tagmetaInfo = Tag.GetTagMetaInfo(userId,tagId, noOfLooks, noOfItems, noOfStylists, db).ToArray();

        return tagmetaInfo;
    }


    [System.Web.Services.WebMethod]
    public static Array GetTaggedRecentLooks(int userId, int tagId,int offset, int limit)
    {

        string db = GetConnectionString();

        Array looks = Look.GetTaggedLooks(db, userId, tagId, offset, limit, false).ToArray();
       
        return looks;

    }

    
    [System.Web.Services.WebMethod]
    public static Array GetTaggedPopularLooks(long userId,long tagId, int offset, int limit)
    {
        string db = GetConnectionString();

        Array looks = Look.GetTaggedLooks(db,userId, tagId, offset, limit, true).ToArray();

        return looks;
    }
    
    [System.Web.Services.WebMethod]
    public static Array GetTaggedPopularItems(long userId, long tagId, int offset, int limit)
    {
        string db = GetConnectionString();

        Array products = Product.GetTaggedPopularProducts(tagId,userId,db,offset,limit).ToArray();

        return products;
    }

    [System.Web.Services.WebMethod]
    public static Array GetTaggedPopularStylists(long userId, long tagId)
    {
        string db = GetConnectionString();

        Array products = UserProfile.GetTaggedPopularStylists(tagId, userId, db).ToArray();

        return products;
    }

    
    [System.Web.Services.WebMethod]

    public static bool UpdateLove(long userId, long lookId, bool isHeart)
    {

        string db = GetConnectionString();

        //Save Vote and get the new look
        bool isSuccess = Look.HeartLook(userId, lookId, isHeart, db);

        //Send notifications asynchronously
        notifications = new NotificationDelegete(SendNotifications);

        string appAccessToken = null;

        notifications.BeginInvoke(lookId, userId, appAccessToken, isHeart, null, null);

        return isSuccess;

    }

    //Look page servies
    [WebMethod]
    public static Array GetLook(long userId, long lookId)
    {

        string db = GetConnectionString();

        Look look = Look.GetLookById(lookId, userId, db);

        List<Look> looks = new List<ShopSenseDemo.Look>();
        looks.Add(look);
        return looks.ToArray();
    }
    [WebMethod]
    public static bool DeleteLook(long userId, long lookId)
    {

        string db = GetConnectionString();

        bool isSuccess = Look.DeleteLook(db, userId, lookId);

        return isSuccess;
    }
    [WebMethod]
    public static bool AddItemtoCloset(long userId, long productId,string colorId, string categoryId, bool isAddToCloset)
    {

        string db = GetConnectionString();

        bool isSuccess = Product.HeartItem(userId, productId, isAddToCloset, colorId, categoryId, db);

        return isSuccess;
    }

    [WebMethod]
    public static Array GetSimilarItems(long userId, long productId, string colorId, string categoryId)
    {

        string db = GetConnectionString();

        Dictionary<string, List<Product>> similarProducts = Product.GetSimilarProducts(categoryId, colorId, productId, userId, db);

        return similarProducts.ToArray();

    }
    [WebMethod]
    public static Array GetColorOptions(long productId)
    {

        string db = GetConnectionString();

        Dictionary<string, ProductColorDetails> colorOptions = Product.GetProductColorOptions(productId, db);

        return colorOptions.ToArray();

    }
    [WebMethod]
    public static string GetCategoryTree()
    {

        string db = GetConnectionString();

        CategoryTree catTree=  CategoryTree.GetCategoryTree(db);

        
        return SerializationHelper.ToJSONString(typeof(CategoryTree), catTree);
        //return trees.ToArray();
    }
    [WebMethod]
    public static Array GetClosetItems(long userId)
    {

        string db = GetConnectionString();

        return UserProfile.GetClosetProducts(userId, db).ToArray();
    }
    [WebMethod]
    public static Array GetClosetItemsByMetaCat(long userId,string metaCat, int offset, int limit)
    {

        string db = GetConnectionString();

        return UserProfile.GetClosetProductsByMetaCat(userId, metaCat, db, offset, limit).ToArray();
    }
    [WebMethod]
    public static Array GetClosetItemsByDate(long userId, int offset, int limit)
    {

        string db = GetConnectionString();

        return UserProfile.GetClosetProductsByDate(userId, db, offset, limit).ToArray();
    }
    [WebMethod]
    public static Array GetPopularProducts(long userId, int offset, int limit, string categoryId=null, string colorId=null, string tags=null)
    {

        string db = GetConnectionString();

        return Product.GetPopularProductsByFilters(userId, db, tags,categoryId,colorId,offset, limit).ToArray();
    }
    [WebMethod]
    public static Array SaveLook(long userId, string productMap, string tagMap, string title,  long originalLookId=0, long editLookId=0)
    {

        string db = GetConnectionString();

        List<Look> looks = new List<ShopSenseDemo.Look>();
        Look look = Look.SaveLook(db, productMap, userId, tagMap, title, originalLookId, editLookId);
        looks.Add(look);
        
        return looks.ToArray();
    }

    [WebMethod]
    public static Array LoginViaFb(long facebookId)
    {
        string db = GetConnectionString();

        List<UserProfile> users = new List<ShopSenseDemo.UserProfile>();
        UserProfile user = UserProfile.LogInViaFb(facebookId, db);
        users.Add(user);

        return users.ToArray();
    }
    [WebMethod]
    public static Array LoginViaEmail(string emailId, string password)
    {
        string db = GetConnectionString();

        List<UserProfile> users = new List<ShopSenseDemo.UserProfile>();
        UserProfile user = UserProfile.LogInUser(emailId, password, db);
        users.Add(user);

        return users.ToArray();
    }
    [WebMethod]
    public static Array RegisterViaFb(long facebookId, string userName, string emailId, string locale, string gender, string pic, string location,  string fbAccessToken)
    {
        string db = GetConnectionString();

        List<UserProfile> users = new List<ShopSenseDemo.UserProfile>();
        
        UserProfile user = new ShopSenseDemo.UserProfile();
        user.accessToken = fbAccessToken;
        user.pic = pic;
        user.facebookId = facebookId;
        user.name = userName;
        user.locale =locale;
        user.sex = (gender == "female" ? Sex.Female : Sex.Male);

        //extended perm
        if (location != null)
            user.location = location;

        if (emailId != null)
            user.email = emailId;

        user.facebookFriends = new List<long>();
        //if (friendsfbId != null)
        //{
        //    foreach (long friendId in friendsfbId)
        //    {
        //        user.facebookFriends.Add(friendId);
        //    }
        //}

        user = UserProfile.SaveOrUpdateUser(user, db);

        users.Add(user);

        return users.ToArray();
    }

    [WebMethod]
    public static bool FollowUser(long userId, long subscriberId,  bool isFollow)
    {
        bool isSuccess = false;
        
        string db = GetConnectionString();

        isSuccess = UserProfile.SubscribeUser(userId, subscriberId, isFollow, db);

        //Send notifications asynchronously
        followNotification = new FollowNotificationDelegete(SendFollowNotifications);

        string appAccessToken = null;

        followNotification.BeginInvoke(subscriberId, userId, appAccessToken, isFollow, null, null);

        return isSuccess;
    }



    //helper function to send notification
    private delegate void NotificationDelegete(long lookId, long userId, string accessToken, bool isHeart);
    private static NotificationDelegete notifications;

    private delegate void FollowNotificationDelegete(long subscriberId, long userId, string accessToken, bool isSubscribe);
    private static FollowNotificationDelegete followNotification;

   
    public static void SendNotifications(long lookId, long userId, string appAccessToken, bool isHeart)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        if (isHeart)
        {

            Look look = Look.GetLookById(lookId, userId, db);

            //send notification to the creator of the outfit unless the creator is a bot (bot is <=0)
            if (look.creator.facebookId > 0)
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
        }

        //send notification to a voter if their friend voted on the same outfit
    }

    public static void SendFollowNotifications(long subscriberId, long userId,string appAccessToken, bool isSubscribe)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        //send notification to the creator of the outfit unless the creator is a bot (bot is <=0)
        if (subscriberId > 0 && isSubscribe)
        {
            UserProfile user = UserProfile.GetUserProfileById(userId, db);
            UserProfile creator = UserProfile.GetUserProfileById(subscriberId, db);

            //Post a fb notification that user has loved a vote

            try
            {
                //Send the notification only if the user is creator's friend
                appAccessToken = FacebookHelper.SendFollowNotification(appAccessToken, creator.facebookId, user);
                //this.Session["app_access_token"] = appAccessToken;

            }
            catch (Facebook.FacebookOAuthException ex)
            {
                //Log the oauth exception.
            }
        }
    }




}


