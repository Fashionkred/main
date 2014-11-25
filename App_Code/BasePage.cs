using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using ShopSenseDemo;

namespace ShopSenseDemo
{

    public class BasePage : System.Web.UI.Page
    {
        public UserProfile user { get; set; }


        public void GetUser(string db)
        {
            user = new UserProfile();

            //Check if there is a lid in the querystring - then set the look.id
            if (Request.QueryString["lid"] != null)
            {
                this.Session["lid"] = long.Parse(Request.QueryString["lid"]);
            }
            else if (Request.QueryString["pid"] != null)
            {
                long pid = long.Parse(Request.QueryString["pid"]);

                //Find a look with that product id
                Look look = Look.GetLookByProductId(pid, db);
                this.Session["lid"] = look.id;
            }

            string sid = string.Empty;
            if (this.Session["lid"] != null)
            {
                sid = this.Session["lid"].ToString();
            }


            //Get the fb-connected user id
            if (user.userId == 0)
            {
                //Check if it's the fb Crawler then skip authentication
                if (Request.UserAgent == "facebookexternalhit/1.1 (+http://www.facebook.com/externalhit_uatext.php)")
                {
                    user = UserProfile.GetUserProfileById(3, db);
                }

                else if (this.Session["access_token"] == null)
                {
                    if (Request.QueryString["error_code"] != null)
                    {
                        Response.Write(Request.QueryString["error_string"]);
                        return;
                    }

                    if (Request.QueryString["code"] == null)
                    {
                        //means the user is not authorized yet from fb - redirect to login via fb

                        string redirectUrl = FacebookHelper.GetCode(sid);
                        Response.Redirect(redirectUrl);
                    }
                    else
                    {
                        try
                        {
                            //if code exists - check for state for lid
                            if (Request.QueryString["state"] != null)
                            {
                                long state = long.Parse(Request.QueryString["state"]);
                                if (state != 0)
                                    this.Session["lid"] = state;
                            }
                            string userReferral = "Unknown";
                            if (this.Request.Cookies["__userReferralid"] != null)
                            {
                                userReferral = this.Request.Cookies["__userReferralid"].Value;
                            }
                            string ipAddress = WebHelper.GetIpAddress(this.Request);
                            string userAgent = this.Request.UserAgent.ToString();
                            user = FacebookHelper.GetUser(Request.QueryString["code"], userReferral, db);
                            this.Session["access_token"] = user.accessToken;
                            this.Session["user"] = user;
                            
                            // create user cookie
                            HttpCookie userid = new HttpCookie("__userid");
                            userid.Value = user.userId.ToString();
                            userid.Expires = DateTime.UtcNow.AddDays(14);
                            Response.Cookies.Add(userid);

                            // create authentication cookie
                            HttpCookie auth = new HttpCookie("__auth");
                            auth.Value = WebHelper.CreateAuthString(user.userId);
                            auth.Expires = DateTime.UtcNow.AddDays(14);
                            Response.Cookies.Add(auth);

                        }
                        catch (Facebook.FacebookOAuthException ex)
                        {
                            if (ex.ErrorCode >= 100 && ex.ErrorCode < 200) // token expired or app unistalled
                            {
                                //code might have expired - try again
                                string redirectUrl = FacebookHelper.GetCode(sid);
                                Response.Redirect(redirectUrl);
                            }
                        }
                    }
                }
                else
                {
                    user = FacebookHelper.GetUserFromToken(this.Session["access_token"].ToString(), db);
                }
            }
        }

        public void RedirectToErrorPage(ErrorPageType type, bool transfer)
        {
            if (transfer)
            {
                this.Server.Transfer("~/error.aspx?type=" + (int)type);
            }
            else
            {
                this.Response.Redirect("~/error.aspx?type=" + (int)type);
            }
        }

    }
}

