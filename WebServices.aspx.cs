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
using System.Runtime.Serialization.Json;


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

    private delegate void CreateNotificationDelegete(long lookId, long userId, string accessToken);
    private static CreateNotificationDelegete createNotifications;

    private delegate void LookDeleteNotificationDelegete(long lookId, long userId, string accessToken);
    private static LookDeleteNotificationDelegete lookDeleteNotifications;

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
    public static Array GetTopStylists(long userId, int noOfStylist)
    {

        string db = GetConnectionString();

        Array users = UserProfile.GetTopStylists(userId,noOfStylist, db).ToArray();

        return users;

    }

    [System.Web.Services.WebMethod]
    public static Array GetHomePagePromo(long userId)
    {
        string promoFile = @"https://s3-us-west-2.amazonaws.com/fkconfigs/appHomePromo_schedule.json";
        System.Net.ServicePointManager.Expect100Continue = false;
        Promos promos = new Promos();

        using (WebClient client = new WebClient())
        {
            try
            {
                using (Stream stream = client.OpenRead(promoFile))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Promos));
                    //stream.Position = 0;
                    promos = (Promos)ser.ReadObject(stream);
                }
            }
            catch (WebException ex)
            { }

        }

        List<Promo> eligiblePromos = new List<Promo>();
        foreach (Promo promo in promos.promos)
        {
            DateTime startDate = DateTime.Parse(promo.startDate);
            DateTime endDate = DateTime.Parse(promo.endDate);
            if (startDate < DateTime.UtcNow && endDate > DateTime.UtcNow)
            {
                eligiblePromos.Add(promo);
            }
            else
                continue;
        }

        //Promo promo = new Promo("aa", "2/1/2015", "3/1/2015", "aa.jpg", "aa.jpg", 20,40);
        //promos.Add(promo);

        return eligiblePromos.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static Array GetHomePagePromov2(long userId, string userAgent)
    {
        string promoFile = @"https://s3-us-west-2.amazonaws.com/fkconfigs/appHomePromo_schedule.json";
        System.Net.ServicePointManager.Expect100Continue = false;
        Promos promos = new Promos();

        using (WebClient client = new WebClient())
        {
            try
            {
                using (Stream stream = client.OpenRead(promoFile))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Promos));
                    //stream.Position = 0;
                    promos = (Promos)ser.ReadObject(stream);
                }
            }
            catch (WebException ex)
            { }

        }

        List<Promo> eligiblePromos = new List<Promo>();
        foreach (Promo promo in promos.promos)
        {
            DateTime startDate = DateTime.Parse(promo.startDate);
            DateTime endDate = DateTime.Parse(promo.endDate);
            if (startDate < DateTime.UtcNow && endDate > DateTime.UtcNow)
            {
                if (userAgent.Contains("iPad") && promo.iPadHeight > 0)
                {
                    eligiblePromos.Add(promo);
                }
                else if (userAgent.Contains("iPhone") && promo.iPhoneHeight > 0)
                {
                    eligiblePromos.Add(promo);
                }
            }
            else
                continue;
        }

        //Promo promo = new Promo("aa", "2/1/2015", "3/1/2015", "aa.jpg", "aa.jpg", 20,40);
        //promos.Add(promo);

        return eligiblePromos.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static Array GetTagPagePromo(long userId, long tagId)
    {
        string promoFile = @"https://s3-us-west-2.amazonaws.com/fkconfigs/appThemePromo_schedule.json";
        System.Net.ServicePointManager.Expect100Continue = false;
        Promos promos = new Promos();

        using (WebClient client = new WebClient())
        {
            try
            {
                using (Stream stream = client.OpenRead(promoFile))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Promos));
                    //stream.Position = 0;
                    promos = (Promos)ser.ReadObject(stream);
                }
            }
            catch (WebException ex)
            { }

        }

        List<Promo> eligiblePromos = new List<Promo>();
        foreach (Promo promo in promos.promos)
        {
            DateTime startDate = DateTime.Parse(promo.startDate);
            DateTime endDate = DateTime.Parse(promo.endDate);
            if (startDate < DateTime.UtcNow && endDate > DateTime.UtcNow && promo.tagId == tagId)
            {
                eligiblePromos.Add(promo);
                //break;
            }
            else
                continue;
        }

        //Promo promo = new Promo("aa", "2/1/2015", "3/1/2015", "aa.jpg", "aa.jpg", 20,40);
        //promos.Add(promo);

        return eligiblePromos.ToArray();
    }
    
    [System.Web.Services.WebMethod]
    public static Array GetHomePageLooks(long userId, int offset, int limit)
    {

        string db = GetConnectionString();

        Array looks = Look.GetHomePageLooks(db, userId, offset, limit).ToArray();

        return looks;

    }

    [System.Web.Services.WebMethod]
    public static Array GetHomePageLooks_NoFilter(long userId, int offset, int limit)
    {

        string db = GetConnectionString();

        Array looks = Look.GetHomePageLooks(db, userId, offset, limit, false).ToArray();

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

    public static Array GetAllTags(long userId, int offset, int limit)
    {
        string db = GetConnectionString();

        Array tags;

        tags = Tag.getAllHashtags(userId, db, offset, limit).ToArray();

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

    public static Array GetFeaturedBrands(long userId)
    {
        string db = GetConnectionString();

        Array brands;

        brands = Brand.GetFeaturedBrands(userId, db).ToArray();

        return brands;
    }

    [System.Web.Services.WebMethod]

    public static Array GetFeaturedTagsv2(long userId)
    {
        string db = GetConnectionString();

        Array tags;

        tags = Tag.getFeaturedHashtagsv2(userId, db).ToArray();

        return tags;
    }

    [System.Web.Services.WebMethod]

    public static Array GetHPFeaturedTags(long userId)
    {
        string db = GetConnectionString();

        Array tags;

        tags = Tag.getHPFeaturedTags(userId, db).ToArray();

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
    public static Array GetPopularItemsByUserv2(long userId, int offset, int limit)
    {
        string db = GetConnectionString();

        Array products = Product.GetPopularProductsByUserv2(userId, db, offset, limit).ToArray();

        return products;
    }

    [System.Web.Services.WebMethod]
    public static Array GetProduct(long userId,long productId, string categoryId, string colorId)
    {
        string db = GetConnectionString();

        Product product = Product.GetProductById(userId,productId, colorId, categoryId, db);
        List<Product> products = new List<ShopSenseDemo.Product>();
        products.Add(product);

        return products.ToArray();
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
        {
            Notification.SaveNotification(note, db);
            //send #OOTD mail to the winner if this like is from Cult_OOTD
            if (userId == 163)
            {
                UserProfile winner = UserProfile.GetUserProfileById(look.creator.userId, db);
                WebHelper.SendOOTDWinnerEmail(winner, look);
            }
        }
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

        if (isSuccess)
        {

            //Send notifications asynchronously
            lookDeleteNotifications = new LookDeleteNotificationDelegete(DeleteNotifications);

            string appAccessToken = null;

            lookDeleteNotifications.BeginInvoke(lookId, userId, appAccessToken, null, null);
        }

        return isSuccess;
    }
    public static void DeleteNotifications(long lookId, long userId, string appAccessToken)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        
        //delete all notification related to this look in db
        Notification note = new ShopSenseDemo.Notification(lookId, userId, 0, NotificationType.Generic);
        
        Notification.DeleteLookNotification(note, db);
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
    public static Array GetClosetItemsv2(long userId, long viewerId, int items)
    {

        string db = GetConnectionString();

        return UserProfile.GetClosetProductsv2(userId, db, viewerId, items).ToArray();
    }

    [WebMethod]
    public static Array GetClosetItemsByMetaCat(long userId, string metaCat, int offset, int limit, long viewerId)
    {

        string db = GetConnectionString();

        return UserProfile.GetClosetProductsByMetaCat(userId, metaCat, db, offset, limit, viewerId).ToArray();
    }
    [WebMethod]
    public static Array GetClosetItemsByMetaCatv2(long userId, string metaCat, int offset, int limit, long viewerId)
    {

        string db = GetConnectionString();

        return UserProfile.GetClosetProductsByMetaCat(userId, metaCat, db, offset, limit, viewerId, true).ToArray();
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
    public static Array GetPopularProductsv3(long userId, int offset, int limit, string categoryId = null, string colorId = null, string tags = null, int brandId = 0,int items=5, string filter=null)
    {

        string db = GetConnectionString();

        return Product.GetPopularProductsByFiltersv3(userId, db, brandId, tags, categoryId, colorId, offset, limit, items, filter).ToArray();
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
        else if(editLookId == 0)  //send create notification for original look
        {
            createNotifications = new CreateNotificationDelegete(createNotificationsFn);

            string appAccessToken = null;

            createNotifications.BeginInvoke(look.id, userId, appAccessToken, null, null); 
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

    public static void createNotificationsFn(long lookId, long userId, string appAccessToken)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        
        //save notification in db
        Notification note = new ShopSenseDemo.Notification(lookId, 0, userId, NotificationType.CreateLook);
        Notification.SaveCreateNotification(note, db);

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

        //Look look = Look.GetLookById(7339, 4, db);
        //WebHelper.SendOOTDWinnerEmail(user, look);
        //WebHelper.SendWelcomeEmailv2(user.emailId, user, "iPhone", db);
        //WebHelper.SendWelcomeEmail(user.emailId, user.name);
       //WebHelper.ForgotPasswordEmail(user.emailId, user.userName, "abcd");
        users.Add(user);

        return users.ToArray();
    }

    [WebMethod]
    public static Array LoginViaFbv2(long facebookId, string userAgent)
    {
        string db = GetConnectionString();

        List<UserProfile> users = new List<ShopSenseDemo.UserProfile>();
        UserProfile user = UserProfile.LogInViaFb(facebookId, db, userAgent);

        //Look look = Look.GetLookById(7339, 4, db);
        //WebHelper.SendOOTDWinnerEmail(user, look);
        //WebHelper.SendWelcomeEmailv2(user.emailId, user, "iPhone", db);
        //WebHelper.SendWelcomeEmail(user.emailId, user.name);
        //WebHelper.ForgotPasswordEmail(user.emailId, user.userName, "abcd");
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
    public static Array LoginViaEmailv2(string emailId, string password, string userAgent)
    {
        string db = GetConnectionString();

        List<UserProfile> users = new List<ShopSenseDemo.UserProfile>();
        UserProfile user = UserProfile.LogInUser(emailId, password, db, userAgent);
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
        user.userAgent = "iPad";

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
    public static Array RegisterViaFbv2(long facebookId, string userName, string emailId, string locale, string gender, string pic, string location, string fbAccessToken, string userAgent)
    {
        string db = GetConnectionString();

        List<UserProfile> users = new List<ShopSenseDemo.UserProfile>();

        UserProfile user = new ShopSenseDemo.UserProfile();
        user.accessToken = fbAccessToken;
        user.pic = pic;
        user.facebookId = facebookId;
        user.name = userName;
        user.locale = locale;
        user.gender = (gender == "female" ? Sex.Female : Sex.Male);
        user.userName = null;
        user.userAgent = userAgent;

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
                WebHelper.SendWelcomeEmailv2(user.emailId, user, userAgent, db);
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
        user.userAgent = "iPad";
        
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
    public static Array RegisterViaEmailv2(string userName, string emailId, string password, string userAgent)
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
        user.userAgent = userAgent;

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
                WebHelper.SendWelcomeEmailv2(user.emailId, user, userAgent, db);
            }
            catch { }
        }

        users.Add(user);

        return users.ToArray();
    }

    [WebMethod]
    public static Array UpdateUserInfo(long userId, string userName, string name, string emailId, string gender, string pic, string location, string bio, string url, string fbPage=null,
        string twitterHandle=null, string PinterestHandle=null, string TumblrHandle=null, string InstagramHandle = null)
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

        if (InstagramHandle != null)
            user.InstagramHandle = InstagramHandle;

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

        Notification note = new ShopSenseDemo.Notification(0, userId, subscriberId, ShopSenseDemo.NotificationType.FollowUser);

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

    [WebMethod]
    public static bool FollowBrands(long userId, string brands, bool isFollow)
    {
        bool isSuccess = false;

        string db = GetConnectionString();

        isSuccess = UserProfile.SubscribeBrands(userId, brands, isFollow, db);

        return isSuccess;
    }

    [WebMethod]
    public static bool IsFollowedBrand(long userId, long brandId)
    {
        bool isFollowed = false;

        string db = GetConnectionString();

        isFollowed = UserProfile.IsSubscribeBrand(userId, brandId, db);

        return isFollowed;
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
    public static Array GetUserLookBook(long userId, int offset, int limit, long viewerId, int sortType)
    {
        string db = GetConnectionString();

        Dictionary<string, List<object>> profileInfo = UserProfile.GetUserProfileInfo(userId, "lookbook", db, offset, limit, viewerId, sortType);

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

    [System.Web.Services.WebMethod]
    public static Array GetUserFollowedTags(long userId, int offset, int limit, long viewerId)
    {
        string db = GetConnectionString();

       List<Tag> tags = UserProfile.GetUserFollowedTags(userId, db, offset, limit, viewerId);

       return tags.ToArray();

    }

    [System.Web.Services.WebMethod]
    public static Array GetUserFollowedUsers(long userId, int offset, int limit, long viewerId)
    {
        string db = GetConnectionString();

        List<UserProfile> users = UserProfile.GetUserFollowedUsers(userId, db, offset, limit, viewerId);

        return users.ToArray();

    }

    [System.Web.Services.WebMethod]
    public static Array GetUserFollowedBrands(long userId)
    {
        string db = GetConnectionString();

        Dictionary<string, List<object>> profileInfo = UserProfile.GetUserFollowedBrands(userId, db);

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
    public static bool UpdateNotificationSeen(long userId)
    {

        string db = GetConnectionString();

        bool isSuccess = UserProfile.SaveNotificationSeenTime(userId, db);

        return isSuccess;
    }
    [WebMethod]
    public static bool IsNewNotification(long userId)
    {

        string db = GetConnectionString();

        bool isSuccess = Notification.IsNewNotification(userId, db);

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

        List<object> notes = Notification.GetLastNotifications(userId, db, offset, limit);
        return notes.ToArray();
    }

}


