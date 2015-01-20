using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook;
using Newtonsoft.Json.Linq;
using ShopSenseDemo;
using System.Configuration;

/// <summary>
/// Summary description for FacebookHelper
/// </summary>
namespace ShopSenseDemo
{
    public class FacebookHelper
    {
        public static string appSecret = ConfigurationManager.AppSettings["AppSecret"];
        public static string appId = ConfigurationManager.AppSettings["AppId"];
        public static string fbUrl = "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope=email,user_location,publish_actions";
        public static string canvasUrl = ConfigurationManager.AppSettings["CanvasUrl"];

        public static string GetCode(string sid)
        {
            string url = string.Format(fbUrl, appId, canvasUrl);

            if (!string.IsNullOrEmpty(sid))
            {
                url += ("&state=" + sid);
            }
            else
                url += "&state=0";

            return url;
        }

        public static UserProfile GetUser(string code, string referral, string db)
        {
            var client = new FacebookClient();
            client.AppId = appId;
            client.AppSecret = appSecret;

            dynamic response = client.Get("oauth/access_token",
            new
            {
                client_id = appId,
                redirect_uri = canvasUrl,
                client_secret = appSecret,
                code = code
            });

            UserProfile user = new UserProfile();
            if (response.access_token != null)
            {
                client.AccessToken = user.accessToken = response.access_token;
                dynamic result = client.Get("me", new { fields = "name,id,locale,friends,location,gender,email" });

                //default perm
                user.facebookId = long.Parse(result.id);
                user.name = result.name;
                user.locale = result.locale;
                user.gender = (result.gender == "female" ? Sex.Female : Sex.Male);
                user.Referral = referral;
                user.pic = "https://graph.facebook.com/" + user.facebookId + "/picture?width=50&height=50";

                //extended perm
                if (result.location != null)
                    user.location = result.location.name;
                    
                if (result.email != null)
                    user.emailId = result.email;

                user.facebookFriends = new List<long>();
                if (result.friends != null)
                {
                    foreach (var friend in result.friends.data)
                    {
                        user.facebookFriends.Add(long.Parse(friend.id));
                    }
                }

                //for new users - suggest users to follow
                
                user = UserProfile.SaveOrUpdateUser(user, db);
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication. - default to the admin user

                user = UserProfile.GetUserProfileById(3, db);

            }

            return user;
        }

        public static bool UpdateExistingUser(string access_token, string db)
        {
            var client = new FacebookClient();
            client.AppId = appId;
            client.AppSecret = appSecret;
            client.AccessToken = access_token;

            UserProfile user = new UserProfile();
            try{
                dynamic result = client.Get("me", new { fields = "name,id,locale,location,gender,email" });

                //default perm
                user.facebookId = long.Parse(result.id);
                user.name = result.name;
                user.locale = result.locale;
                user.gender = (result.gender == "female" ? Sex.Female : Sex.Male);

                //extended perm
                if (result.location != null)
                    user.location = result.location.name;
                
                if (result.email != null)
                    user.emailId = result.email;

                user.facebookFriends = new List<long>();
                if (result.friends != null)
                {
                    foreach (var friend in result.friends.data)
                    {
                        user.facebookFriends.Add(long.Parse(friend.id));
                    }
                }
                user.accessToken = access_token;
                user = UserProfile.SaveOrUpdateUser(user, db);
                
            }
            catch(FacebookOAuthException ex)
            {
                user = null;
                return false;
            }

            return true;
        }

        public static UserProfile GetUserFromToken(string accessToken, string db)
        {
            var client = new FacebookClient();
            client.AppId = appId;
            client.AppSecret = appSecret;
            client.AccessToken = accessToken;

            UserProfile user = new UserProfile();

            dynamic result = client.Get("me", new { fields = "name,id,locale,friends,location,gender,email,picture" });

            //default perm
            user.facebookId = long.Parse(result.id);
            user.name = result.name;
            user.locale = result.locale;
            user.gender = (result.gender == "female" ? Sex.Female : Sex.Male);

            //extended perm
            if (result.location != null)
                user.location = result.location.name;
            if (result.picture != null)
                user.pic = result.picture.data.url;
            if (result.email != null)
                user.emailId = result.email;
            if (result.friends != null)
            {
                user.facebookFriends = new List<long>();
                foreach (var friend in result.friends.data)
                {
                    user.facebookFriends.Add(long.Parse(friend.id));
                }
            }

            user = UserProfile.SaveOrUpdateUser(user, db);

            return user;
        }


        public static string SendNotification(string appAccessToken, long creatorId, long userId, Look look)
        {
            var client = new FacebookClient();
            client.AppId = appId;
            client.AppSecret = appSecret;

            if (string.IsNullOrEmpty(appAccessToken))
            {
                dynamic response = client.Get("oauth/access_token",
                new
                {
                    client_id = appId,
                    client_secret = appSecret,
                    grant_type = "client_credentials"
                });
                appAccessToken = response.access_token;
            }

            string path = "/" + creatorId + "/notifications?access_token=" + appAccessToken;
            string templateString = "@[" + userId + "] loved your look in FashionKred!";

            var parameters = new Dictionary<string, object>
        {
             { "href" ,  "/look.aspx?lid=" + look.id },
             { "template" ,  templateString },
             { "ref" ,  "votenotifications" }
        };

            client.Post(path, parameters);

            return appAccessToken;

        }

        public static string SendRestyleNotification(string appAccessToken, long creatorId, long userId, Look look)
        {
            var client = new FacebookClient();
            client.AppId = appId;
            client.AppSecret = appSecret;

            if (string.IsNullOrEmpty(appAccessToken))
            {
                dynamic response = client.Get("oauth/access_token",
                new
                {
                    client_id = appId,
                    client_secret = appSecret,
                    grant_type = "client_credentials"
                });
                appAccessToken = response.access_token;
            }

            string path = "/" + creatorId + "/notifications?access_token=" + appAccessToken;
            string templateString = "@[" + userId + "] restyled your look in FashionKred!";

            var parameters = new Dictionary<string, object>
        {
             { "href" ,  "/look.aspx?lid=" + look.id },
             { "template" ,  templateString },
             { "ref" ,  "votenotifications" }
        };

            client.Post(path, parameters);

            return appAccessToken;

        }

        public static string SendFollowNotification(string appAccessToken, long creatorId, UserProfile user)
        {
            var client = new FacebookClient();
            client.AppId = appId;
            client.AppSecret = appSecret;

            if (string.IsNullOrEmpty(appAccessToken))
            {
                dynamic response = client.Get("oauth/access_token",
                new
                {
                    client_id = appId,
                    client_secret = appSecret,
                    grant_type = "client_credentials"
                });
                appAccessToken = response.access_token;
            }

            string path = "/" + creatorId + "/notifications?access_token=" + appAccessToken;
            string templateString = "@[" + user.facebookId + "] followed you in FashionKred!";

            var parameters = new Dictionary<string, object>
        {
             { "href" ,  "/user.aspx?uid=" + user.userId },
             { "template" ,  templateString },
             { "ref" ,  "follownotifications" }
        };

            client.Post(path, parameters);

            return appAccessToken;

        }

        public static string SendCreateNotification(string appAccessToken, long creatorId, long userId, Look look)
        {
            var client = new FacebookClient();
            client.AppId = appId;
            client.AppSecret = appSecret;

            if (string.IsNullOrEmpty(appAccessToken))
            {
                dynamic response = client.Get("oauth/access_token",
                new
                {
                    client_id = appId,
                    client_secret = appSecret,
                    grant_type = "client_credentials"
                });
                appAccessToken = response.access_token;
            }

            string path = "/" + userId + "/notifications?access_token=" + appAccessToken;
            string templateString = "@[" + creatorId + "] created a new look in FashionKred!";

            var parameters = new Dictionary<string, object>
        {
             { "href" ,  "/look.aspx?lid=" + look.id },
             { "template" ,  templateString },
             { "ref" ,  "createnotifications" }
        };

            client.Post(path, parameters);

            return appAccessToken;

        }

        public static string SendLookNotification(string appAccessToken, long creatorId, long userId, Look look)
        {
            var client = new FacebookClient();
            client.AppId = appId;
            client.AppSecret = appSecret;

            if (string.IsNullOrEmpty(appAccessToken))
            {
                dynamic response = client.Get("oauth/access_token",
                new
                {
                    client_id = appId,
                    client_secret = appSecret,
                    grant_type = "client_credentials"
                });
                appAccessToken = response.access_token;
            }

            string path = "/" + userId + "/notifications?access_token=" + appAccessToken;
            string templateString = "#LookOfTheWeek: Check out the top voted look by @[" + creatorId + "] in FashionKred!";

            var parameters = new Dictionary<string, object>
        {
             { "href" ,  "/look.aspx?lid=" + look.id },
             { "template" ,  templateString },
             { "ref" ,  "looknotifications" }
        };

            client.Post(path, parameters);

            return appAccessToken;

        }
    }
}