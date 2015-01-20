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
using System.Runtime.Serialization;
using System.Net;


public partial class WebServices : System.Web.UI.Page
{
    public static string userAgent { set; get; }

    protected void Page_Load(object sender, EventArgs e)
    {
        userAgent = Request.UserAgent;
    }


    // Getting Connection String.
    public static string GetConnectionString()
    {
        //if (!HttpContext.Current.Request.UserAgent.Contains("zssss"))
        //    return string.Empty;
           
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        return db;
    }

    //helper function to send notification
    private delegate void NotificationDelegete(long lookId, long userId, string accessToken, bool isHeart);
    private static NotificationDelegete notifications;

    private delegate void ReStyleNotificationDelegete(long lookId, long originalLookId, long userId, string accessToken);
    private static ReStyleNotificationDelegete restyleNotifications;

    private delegate void FollowNotificationDelegete(long subscriberId, long userId, string accessToken, bool isSubscribe);
    private static FollowNotificationDelegete followNotification;

    private delegate void CommentNotificationDelegete(long lookId, long userId, string accessToken, bool addComment);
    private static CommentNotificationDelegete commentNotification;


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
    public static Array GetHomePageLooks(long userId, int offset, int limit)
    {

        string db = GetConnectionString();

        Array looks = Look.GetHomePageLooks(db, userId, offset, limit).ToArray();

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

    public static Array GetFeaturedTags(long userId)
    {
        string db = GetConnectionString();

        Array tags;

        tags = Tag.getFeaturedHashtags(userId, db).ToArray();

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

        if (looks.Length == 0)
            looks = null;

        return looks;

    }

    
    [System.Web.Services.WebMethod]
    public static Array GetTaggedPopularLooks(long userId,long tagId, int offset, int limit)
    {
        string db = GetConnectionString();

        Array looks = Look.GetTaggedLooks(db,userId, tagId, offset, limit, true).ToArray();

        if (looks.Length == 0)
            looks = null;

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
    public static Array GetPopularItemsByUser(long userId, int offset, int limit)
    {
        string db = GetConnectionString();

        Array products = Product.GetPopularProductsByUser(userId, db, offset, limit).ToArray();

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

        if (isSuccess)
        {

            //Send notifications asynchronously
            notifications = new NotificationDelegete(SendNotifications);

            string appAccessToken = null;

            notifications.BeginInvoke(lookId, userId, appAccessToken, isHeart, null, null);
        }

        return isSuccess;

    }
    public static void SendNotifications(long lookId, long userId, string appAccessToken, bool isHeart)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        Look look = Look.GetLookById(lookId, userId, db);

        //no notification for liking your own look
        if (look.creator.userId == userId)
            return;

        //save notification in db
        Notification note = new ShopSenseDemo.Notification(lookId, userId, look.creator.userId, NotificationType.LoveLook);

        if (isHeart)
            Notification.SaveNotification(note, db);
        else
            Notification.DeleteNotification(note, db);

        if (isHeart)
        {
            
            //send notification to the creator of the outfit unless the creator is a bot (bot is <=0)
            if (look.creator.facebookId > 0)
            {
                UserProfile user = UserProfile.GetUserProfileById(userId, db);

                //Post a fb notification that user has loved a vote

                try
                {
                    if (UserProfile.IsFriend(look.creator.userId, user.facebookId))
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


    [WebMethod]
    public static void GenImages(long lookIdStart, long lookIdEnd)
    {
        string db = GetConnectionString();

        int userId = 0;
        for (long lookId = lookIdStart; lookId <= lookIdEnd; lookId++)
        {
            Look look = Look.GetLookById(lookId, userId, db);

            if (look.id != 0)
            {

                //set up the image for the combined look
                try
                {
                    string imageFilePath = Path.Combine(HttpContext.Current.Server.MapPath("images/looks"), look.id + ".jpg");


                    if (!File.Exists(imageFilePath))
                    {
                        WebHelper.CreateLookPanel(look, imageFilePath);
                    }

                }
                catch
                {
                    //signal to the pinterest pinner that image unavailable?
                }
            }
        }
    }

    //Look page servies
    [WebMethod]
    public static Array GetLook(long userId, long lookId)
    {

        string db = GetConnectionString();

        Look look = Look.GetLookById(lookId, userId, db);

        List<Look> looks = new List<ShopSenseDemo.Look>();
        looks.Add(look);
        //set up the image for the combined look
        try
        {
            string imageFilePath = Path.Combine(HttpContext.Current.Server.MapPath("images/looks"), look.id + ".jpg");

            
            if (!File.Exists(imageFilePath))
            {
                WebHelper.CreateLookPanel(look, imageFilePath);
            }
            
        }
        catch
        {
            //signal to the pinterest pinner that image unavailable?
        }

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
    public static string GetMetaCategoryTree()
    {

        string db = GetConnectionString();

        Dictionary<Category, List<Category>> metaCats = Category.GetMetaCategories(db);


        return SerializationHelper.ToJSONString(typeof(Dictionary<Category, List<Category>>), metaCats);

        //return trees.ToArray();
    }

    [WebMethod]
    public static Array GetColors()
    {
        string db = GetConnectionString();

        Dictionary<CanonicalColors, string> colors = new Dictionary<CanonicalColors, string>();
        colors.Add(CanonicalColors.Beige, "Beige");
        colors.Add(CanonicalColors.Black, "Black");
        colors.Add(CanonicalColors.Blue, "Blue");
        colors.Add(CanonicalColors.Brown, "Brown");
        colors.Add(CanonicalColors.Gold, "Gold");
        colors.Add(CanonicalColors.Gray, "Gray");
        colors.Add(CanonicalColors.Green, "Green");
        colors.Add(CanonicalColors.Orange, "Orange");
        colors.Add(CanonicalColors.Pink, "Pink");
        colors.Add(CanonicalColors.Purple, "Purple");
        colors.Add(CanonicalColors.Red, "Red");
        colors.Add(CanonicalColors.Silver, "Silver");
        colors.Add(CanonicalColors.White, "White");
        colors.Add(CanonicalColors.Yellow, "Yellow");

        return colors.ToArray();
    }

    [WebMethod]
    public static Array GetClosetItems(long userId, long viewerId)
    {

        string db = GetConnectionString();

        return UserProfile.GetClosetProducts(userId, db, viewerId).ToArray();
    }
    [WebMethod]
    public static Array GetClosetItemsByMetaCat(long userId, string metaCat, int offset, int limit, long viewerId)
    {

        string db = GetConnectionString();

        return UserProfile.GetClosetProductsByMetaCat(userId, metaCat, db, offset, limit, viewerId).ToArray();
    }
    [WebMethod]
    public static Array GetClosetItemsByDate(long userId, int offset, int limit)
    {

        string db = GetConnectionString();

        return UserProfile.GetClosetProductsByDate(userId, db, offset, limit).ToArray();
    }
    [WebMethod]
    public static Array GetPopularProducts(long userId, int offset, int limit, string categoryId=null, string colorId=null, string tags=null, int brandId=0)
    {

        string db = GetConnectionString();

        return Product.GetPopularProductsByFilters(userId, db, brandId,tags, categoryId, colorId, offset, limit).ToArray();
    }
    [WebMethod]
    public static Array GetPopularProductsv2(long userId, int offset, int limit, string categoryId = null, string colorId = null, string tags = null, int brandId = 0)
    {

        string db = GetConnectionString();

        return Product.GetPopularProductsByFiltersv2(userId, db, brandId, tags, categoryId, colorId, offset, limit).ToArray();
    }

    [WebMethod]
    public static Array SaveLook(long userId, string productMap, string tagMap, string title, long originalLookId = 0, long editLookId = 0)
    {

        string db = GetConnectionString();

        List<Look> looks = new List<ShopSenseDemo.Look>();
        Look look = Look.SaveLook(db, productMap, userId, tagMap, title, originalLookId, editLookId);
        looks.Add(look);

        //set up the image for the combined look
        try
        {
            string imageFilePath = Path.Combine(HttpContext.Current.Server.MapPath("images/looks"), look.id + ".jpg");
            
            if (!File.Exists(imageFilePath))
            {
                WebHelper.CreateLookPanel(look, imageFilePath);
            }
           
        }
        catch
        {
            //signal to the pinterest pinner that image unavailable?
        }


        if (originalLookId != 0)
        {
            //Send notifications asynchronously
            restyleNotifications = new ReStyleNotificationDelegete(reStyleNotifications);

            string appAccessToken = null;

            restyleNotifications.BeginInvoke(look.id, originalLookId, userId, appAccessToken, null, null);
        }

        return looks.ToArray();
    }

    public static void reStyleNotifications(long lookId,long originalLookId, long userId, string appAccessToken)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        Look look = Look.GetLookById(originalLookId, userId, db);

        //no notification for restyling your own look
        if (look.creator.userId == userId)
            return;

        //save notification in db
        Notification note = new ShopSenseDemo.Notification(lookId, userId, look.creator.userId, NotificationType.ReStyle);
        Notification.SaveNotification(note, db);
        

            ////send notification to the creator of the outfit unless the creator is a bot (bot is <=0)
            //if (look.creator.facebookId > 0)
            //{
            //    UserProfile user = UserProfile.GetUserProfileById(userId, db);

            //    //Post a fb notification that user has loved a vote

            //    try
            //    {
            //        if (UserProfile.IsFriend(look.creator.userId, user.facebookId))
            //        {
            //            //Send the notification only if the user is creator's friend
            //            appAccessToken = FacebookHelper.SendNotification(appAccessToken, look.creator.facebookId, user.facebookId, look);
            //            ///////this.Session["app_access_token"] = appAccessToken;
            //        }
            //    }
            //    catch (Facebook.FacebookOAuthException ex)
            //    {
            //        //Log the oauth exception.
            //    }
            //}
      
    }



    [WebMethod]
    public static Array SaveLookByStylists(long userId, string productMap, string tagMap, string title, long originalLookId = 0, long editLookId = 0, bool isFeaturedStylist = false)
    {

        string db = GetConnectionString();

        List<Look> looks = new List<ShopSenseDemo.Look>();
        Look look = Look.SaveLook(db, productMap, userId, tagMap, title, originalLookId, editLookId, isFeaturedStylist);
        looks.Add(look);

        //set up the image for the combined look
        try
        {
            string imageFilePath = Path.Combine(HttpContext.Current.Server.MapPath("images/looks"), look.id + ".jpg");

            if (look.products.Count >= 3)
            {
                if (!File.Exists(imageFilePath))
                {
                    WebHelper.CreateLookPanel(look, imageFilePath);
                }
            }
            else
            {
                if (!File.Exists(imageFilePath))
                {
                    WebHelper.MergeTwoImages(look.products[0].GetImageUrl(), look.products[1].GetImageUrl(), imageFilePath);
                }
            }
        }
        catch
        {
            //signal to the pinterest pinner that image unavailable?
        }

        return looks.ToArray();
    }



    [WebMethod]
    public static Array LoginViaFb(long facebookId)
    {
        string db = GetConnectionString();

        List<UserProfile> users = new List<ShopSenseDemo.UserProfile>();
        UserProfile user = UserProfile.LogInViaFb(facebookId, db);

        WebHelper.SendWelcomeEmail(user.emailId, user.name);
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
        user.gender = (gender == "female" ? Sex.Female : Sex.Male);
        user.userName = null;

        //extended perm
        if (location != null)
            user.location = location;

        if (emailId != null)
            user.emailId = emailId;

        user.facebookFriends = new List<long>();
        //if (friendsfbId != null)
        //{
        //    foreach (long friendId in friendsfbId)
        //    {
        //        user.facebookFriends.Add(friendId);
        //    }
        //}

        user = UserProfile.SaveOrUpdateUser(user, db);

        if (user.IsNew)
        {
            //send welcome email
            try
            {
                WebHelper.SendWelcomeEmail(user.emailId, user.name);
            }
            catch { }
        }
        users.Add(user);

        return users.ToArray();
    }
    
    [WebMethod]
    public static Array RegisterViaEmail(string userName, string emailId, string password)
    {
        string db = GetConnectionString();

        List<UserProfile> users = new List<ShopSenseDemo.UserProfile>();

        UserProfile user = new ShopSenseDemo.UserProfile();
        user.accessToken = null;
        user.pic = null;
        user.facebookId = -1;
        user.userName = userName;
        user.locale = "en-US";
        user.gender = Sex.Female;
        user.password = password;
        user.emailId = emailId;
        
        user.facebookFriends = new List<long>();
        //if (friendsfbId != null)
        //{
        //    foreach (long friendId in friendsfbId)
        //    {
        //        user.facebookFriends.Add(friendId);
        //    }
        //}

        user = UserProfile.SaveOrUpdateUser(user, db);

        if (user.IsNew)
        {
            try
            {
                //send welcome email
                WebHelper.SendWelcomeEmail(user.emailId, user.userName);
            }
            catch { }
        }

        users.Add(user);

        return users.ToArray();
    }

    [WebMethod]
    public static Array UpdateUserInfo(long userId, string userName, string name, string emailId, string gender, string pic, string location, string bio, string url, string fbPage=null,
        string twitterHandle=null, string PinterestHandle=null, string TumblrHandle=null)
    {
        string db = GetConnectionString();

        List<UserProfile> users = new List<ShopSenseDemo.UserProfile>();

        UserProfile user = new ShopSenseDemo.UserProfile();
        user.userId = userId;
        user.pic = pic;
        user.name = name;
        user.gender = (gender == "female" ? Sex.Female : Sex.Male);
        user.userName = userName;
        user.bio = bio;
        user.url = url;

        //extended perm
        if (location != null)
            user.location = location;

        if (emailId != null)
            user.emailId = emailId;

        if (fbPage != null)
            user.fbPage = fbPage;

        if (twitterHandle != null)
            user.twitterHandle = twitterHandle;

        if (PinterestHandle != null)
            user.PinterestHandle = PinterestHandle;

        if (TumblrHandle != null)
            user.TumblrHandle = TumblrHandle;

        user = UserProfile.UpdateUserInfo(user, db);

        users.Add(user);

        return users.ToArray();
    }
    
    [WebMethod]
    public static bool IsUserNameUnique(string userName)
    {
        bool isUnique = false;

        string db = GetConnectionString();

        isUnique = UserProfile.IsUserNameUnique(userName, db);

        return isUnique;
    }

    [WebMethod]
    public static bool IsEmailUnique(string emailId)
    {
        bool isUnique = false;

        string db = GetConnectionString();

        isUnique = UserProfile.IsEmailUnique(emailId, db);

        return isUnique;
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

    public static void SendFollowNotifications(long subscriberId, long userId, string appAccessToken, bool isSubscribe)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        Notification note = new ShopSenseDemo.Notification(0, subscriberId, userId, ShopSenseDemo.NotificationType.FollowUser);

        if (isSubscribe)
            Notification.SaveNotification(note, db);
        else
            Notification.DeleteNotification(note, db);

        //send notification to the creator of the outfit unless the creator is a bot (bot is <=0)
        if (subscriberId > 0 && isSubscribe)
        {
            UserProfile user = UserProfile.GetUserProfileById(userId, db);
            UserProfile creator = UserProfile.GetUserProfileById(subscriberId, db);

            //Post a fb notification that user has loved a vote
            if (creator.facebookId > 0)
            {
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


    [WebMethod]
    public static bool FollowFbFriends(long userId, string  subscriberFbIds, bool isFollow)
    {
        bool isSuccess = false;

        string db = GetConnectionString();

        isSuccess = UserProfile.SubscribeFbUsers(userId, subscriberFbIds, isFollow, db);

        
        return isSuccess;
    }
    [WebMethod]
    public static bool ChangePassword (string userName, string oldPassword, string newPassword)
    {
        bool isSuccess = false;

        string db = GetConnectionString();

        isSuccess = UserProfile.ChangePassword(userName, oldPassword, newPassword, db);

        
        return isSuccess;
    }
    [WebMethod]
    public static bool ForgotPassword(string emailId)
    {
        bool isSuccess = false;

        string db = GetConnectionString();

        string newPassword = string.Empty;
        string userName = string.Empty;

        isSuccess = UserProfile.ForgotPassword(emailId, db, out userName, out newPassword);

        if (isSuccess)
        {
            try
            {
                WebHelper.ForgotPasswordEmail(emailId, userName, newPassword);
            }
            catch { }
        }

        return isSuccess;
    }

    [WebMethod]
    public static bool FollowTags(long userId, string tags, bool isFollow)
    {
        bool isSuccess = false;

        string db = GetConnectionString();

        isSuccess = UserProfile.SubscribeTags(userId, tags, isFollow, db);

        return isSuccess;
    }

    //profile page
    [System.Web.Services.WebMethod]
    public static Array GetUserLooks(long userId, int offset, int limit, long viewerId)
    {
        string db = GetConnectionString();

        Dictionary<string, List<object>> profileInfo = UserProfile.GetUserProfileInfo(userId, "looks", db, offset, limit, viewerId);

        return profileInfo.ToArray();
        
    }

    [System.Web.Services.WebMethod]
    public static Array GetUserProfileInfo(long userId, long viewerId)
    {
        string db = GetConnectionString();

        Dictionary<string, List<object>> profileInfo = UserProfile.GetUserProfileInfo(userId, "profile", db, 1, 1, viewerId);

        return profileInfo.ToArray();

    }
    
    [System.Web.Services.WebMethod]
    public static Array GetUserGarments(long userId, int offset, int limit, long viewerId)
    {
        string db = GetConnectionString();

        Dictionary<string, List<object>> profileInfo = UserProfile.GetUserProfileInfo(userId, "items", db, offset, limit, viewerId);

        return profileInfo.ToArray();// SerializationHelper.ToJSONString(typeof(Dictionary<string, List<object>>), profileInfo);

    }
    
    [System.Web.Services.WebMethod]
    public static Array GetUserHearts(long userId, int offset, int limit, long viewerId)
    {
        string db = GetConnectionString();

        Dictionary<string, List<object>> profileInfo = UserProfile.GetUserProfileInfo(userId, "hearts", db, offset, limit, viewerId);

        return profileInfo.ToArray();

    }
    
    [System.Web.Services.WebMethod]
    public static Array GetUserFollowers(long userId, int offset, int limit, long viewerId)
    {
        string db = GetConnectionString();

        Dictionary<string, List<object>> profileInfo = UserProfile.GetUserProfileInfo(userId, "followers", db, offset, limit, viewerId);

        return profileInfo.ToArray();

    }
    [System.Web.Services.WebMethod]
    public static Array GetUserFollowings(long userId, int offset, int limit, long viewerId)
    {
        string db = GetConnectionString();

        Dictionary<string, List<object>> profileInfo = UserProfile.GetUserProfileInfo(userId, "following", db, offset, limit, viewerId);

        return profileInfo.ToArray();

    }
    
    [WebMethod]
    public string ProfileImagePost(long userId, HttpPostedFile profileImage)
    {
        string[] extensions = { ".jpg", ".jpeg", ".gif", ".bmp", ".png" };
        if (!extensions.Any(x => x.Equals(Path.GetExtension(profileImage.FileName.ToLower()), StringComparison.OrdinalIgnoreCase)))
        {
            return "Invalid file type.";
        }

        string imageFilePath = Path.Combine(HttpContext.Current.Server.MapPath("images/users"), userId + Path.GetExtension(profileImage.FileName.ToLower()));

        profileImage.SaveAs(imageFilePath);

        return imageFilePath;
    }

    [WebMethod]
    public static Array GetTopBrands()
    {
        string db = GetConnectionString();
        List<Brand> topBrands = Brands.GetTopBrands(db);

        return topBrands.ToArray();
    }

    [WebMethod]
    public static Array GetLikers(long lookId, int offset, int limit)
    {
        string db = GetConnectionString();

        List<UserProfile> likers = Look.GetLikers(db, lookId, offset, limit);
        return likers.ToArray();
    }

    [WebMethod]
    public static Array GetReStylers(long lookId, int offset, int limit)
    {
        string db = GetConnectionString();

        List<UserProfile> reStylers = Look.GetReStylers(db, lookId, offset, limit);
        return reStylers.ToArray();
    }

    [WebMethod]
    public static Array GetLikersandRestylers(long lookId, int offset, int limit)
    {
        string db = GetConnectionString();

        List<LightUser> users = Look.GetLikersandRestylers(db, lookId, offset, limit);
        return users.ToArray();
    }

    [WebMethod]
    public static Array GetComments(long lookId, int offset, int limit)
    {
        string db = GetConnectionString();

        List<Comment> comments = Comment.GetComments(db, lookId, offset, limit);
        return comments.ToArray();
    }

    [WebMethod]
    public static bool DeleteComment(long userId, long lookId, long commentId)
    {

        string db = GetConnectionString();

        bool isSuccess = Comment.DeleteComment(db, userId, lookId, commentId);
        if (isSuccess)
        {
            commentNotification = new CommentNotificationDelegete(commentNotifications);

            string appAccessToken = null;

            commentNotification.BeginInvoke(lookId, userId, appAccessToken, false, null, null);
        }
        return isSuccess;
        
    }
    [WebMethod]
    public static bool AddComment(long userId, long lookId, string comment)
    {

        string db = GetConnectionString();

        bool isSuccess = Comment.AddComment(userId, lookId, comment, db) ;

        
        if(isSuccess)
        {
            commentNotification = new CommentNotificationDelegete(commentNotifications);

            string appAccessToken = null;

            commentNotification.BeginInvoke(lookId, userId, appAccessToken, true, null, null);
        }
        return isSuccess;
        
    }

    public static void commentNotifications(long lookId, long userId, string appAccessToken, bool addComment)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        Look look = Look.GetLookById(lookId, userId, db);

        //no notification for restyling your own look
        if (look.creator.userId == userId)
            return;

        //save notification in db
        Notification note = new ShopSenseDemo.Notification(lookId, userId, look.creator.userId, NotificationType.Comment);
        if (addComment)
            Notification.SaveNotification(note, db);
        else
            Notification.DeleteNotification(note, db);

            ////send notification to the creator of the outfit unless the creator is a bot (bot is <=0)
            //if (look.creator.facebookId > 0)
            //{
            //    UserProfile user = UserProfile.GetUserProfileById(userId, db);

            //    //Post a fb notification that user has loved a vote

            //    try
            //    {
            //        if (UserProfile.IsFriend(look.creator.userId, user.facebookId))
            //        {
            //            //Send the notification only if the user is creator's friend
            //            appAccessToken = FacebookHelper.SendNotification(appAccessToken, look.creator.facebookId, user.facebookId, look);
            //            ///////this.Session["app_access_token"] = appAccessToken;
            //        }
            //    }
            //    catch (Facebook.FacebookOAuthException ex)
            //    {
            //        //Log the oauth exception.
            //    }
            //}
      
    }



    [WebMethod]
    public static bool UpdateDeviceToken(long userId, string deviceId, string token)
    {

        string db = GetConnectionString();

        bool isSuccess = UserProfile.SaveDeviceToken(userId, deviceId, token, db);

        return isSuccess;
    }
    [WebMethod]
    public static bool ReportItem(long userId, long  productId)
    {

        string db = GetConnectionString();

        bool isSuccess = Product.ReportItem(userId, productId, db);

        return isSuccess;
    }

    [WebMethod]
    public static Array GetNotifications(long userId, int offset, int limit)
    {
        string db = GetConnectionString();

        List<Notification> notes = Notification.GetLastNotifications(userId, db, offset, limit);
        return notes.ToArray();
    }

}


