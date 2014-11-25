using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ShopSenseDemo;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

public partial class user : BasePage
{
    private UserProfile reqUser { set; get; }
    private long userId { set; get; }

    public void SetOGTags()
    {
        //Set the OG tags if there is a lid
        Master.Page.Title = reqUser.name + " - " + " FashionKred";
        
        HtmlMeta title = new HtmlMeta();
        title.Attributes.Add("property", "og:title");
        title.Content = reqUser.name;
        Master.FindControl("head").Controls.Add(title);

        HtmlMeta url = new HtmlMeta();
        url.Attributes.Add("property", "og:url");
        url.Content = "http://fashionKred.com/user.aspx?uid=" + reqUser.userId;
        Master.FindControl("head").Controls.Add(url);

        HtmlMeta image = new HtmlMeta();
        image.Attributes.Add("property", "og:image");
        image.Content = reqUser.BigPic();

        Master.FindControl("head").Controls.Add(image);

        HtmlMeta desc = new HtmlMeta();
        desc.Attributes.Add("property", "og:description");
        desc.Content = reqUser.location;
        Master.FindControl("head").Controls.Add(desc);

        HtmlMeta type = new HtmlMeta();
        type.Attributes.Add("property", "og:type");
        type.Content = "profile";
        Master.FindControl("head").Controls.Add(type);

        HtmlMeta profileId = new HtmlMeta();
        profileId.Attributes.Add("property", "fb:profile_id");
        profileId.Content = reqUser.facebookId.ToString();
        Master.FindControl("head").Controls.Add(profileId);

        HtmlMeta appId = new HtmlMeta();
        appId.Attributes.Add("property", "fb:app_id");
        appId.Content = ConfigurationManager.AppSettings["AppId"];
        Master.FindControl("head").Controls.Add(appId);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        bool isOWner = false;
        
        if (this.Session["user"] != null)
        {
            this.user = this.Session["user"] as UserProfile;
            if (Request.QueryString["uid"] != null)
            {
                long reqId = long.Parse(Request.QueryString["uid"].ToString());

                if (this.user.userId == reqId)
                {
                    this.reqUser = this.user;
                }
                else
                {
                    this.reqUser = UserProfile.GetUserProfileById(reqId, db);
                }
            }
            else
            {
                this.reqUser = this.user;
            }
        }
        else
        {
            if (Request.QueryString["uid"] != null)
            {
                long reqId = long.Parse(Request.QueryString["uid"].ToString());
                this.reqUser = UserProfile.GetUserProfileById(reqId, db);
            }
            else
            {
                GetUser(db);
            }
        }
        //set og tages
        SetOGTags();

        //Check if it's the fb Crawler then skip authentication
        if (Request.UserAgent == "facebookexternalhit/1.1 (+http://www.facebook.com/externalhit_uatext.php)")
            return;

        this.CreatorId.Text = reqUser.userId.ToString();
        
        //hide the contest bar
        Panel ContestBar = (Panel)this.Master.FindControl("ContestBar");
        ContestBar.Visible = false;

        System.Web.UI.WebControls.Image userImage = (System.Web.UI.WebControls.Image)this.Master.FindControl("UserImage");
        HyperLink userName = (HyperLink)this.Master.FindControl("UserName");
        
        Label userPoints = (Label)this.Master.FindControl("UserPoints");
        HyperLink loginLink = (HyperLink)this.Master.FindControl("LogInLink");
        Panel dropDown = (Panel)this.Master.FindControl("DropDown");
        if (this.user != null)
        {
            userName.NavigateUrl = "user.aspx?uid=" + user.userId;

            this.userId = user.userId;
        
            if (reqUser == this.user)
                isOWner = true;
            this.UserId.Text = this.user.userId.ToString();

            userImage.ImageUrl = user.pic;
            userName.Text = user.name;
            userPoints.Text = user.points.ToString() + " votes";
        }
        else
        {
            this.user = new UserProfile();
            userImage.Visible = false;
            userName.Visible = false;
            userPoints.Visible = false;
            dropDown.Visible = false;
            loginLink.Visible = true;
            loginLink.NavigateUrl = this.Request.RawUrl.Contains('?') ? this.Request.RawUrl + "&login=1" : this.Request.RawUrl + "?login=1";
        }

        //set up the page
        UserImage.ImageUrl = reqUser.BigPic();
        
        UserName.Text = reqUser.name;

        if (reqUser.location != "Unknown")
        {
            UserLinks.Controls.Add(new Literal{ Text = "<span class=\"link\">" + reqUser.location + "</span>" });

        }

        if (reqUser.facebookId > 0)
        {
            HyperLink fb = new HyperLink();
            fb.Target = "_blank";
            fb.NavigateUrl = "http://facebook.com/profile.php?id=" + reqUser.facebookId;
            System.Web.UI.WebControls.Image fbImg = new System.Web.UI.WebControls.Image();
            fbImg.ImageAlign = ImageAlign.Baseline;
            fbImg.ImageUrl = "images/fb.gif";
            fb.Controls.Add(fbImg);
            UserLinks.Controls.Add(fb);
        }

        //set subscribe button
        if (user == reqUser)
            SubscribePanel.Visible = false;
        else if (UserProfile.IsFollower(reqUser.userId, user.userId, db))
        {
            SubscribePanel.CssClass += " following";
            Subscribe.Text = "Following";
        }

        //set up the top header link
        SetsLink.NavigateUrl = "user.aspx?uid=" + reqUser.userId + "&view=sets";
        LovesLink.NavigateUrl = "user.aspx?uid=" + reqUser.userId + "&view=loves";
        FollowersLink.NavigateUrl = "user.aspx?uid=" + reqUser.userId + "&view=followers";
        FollowingLink.NavigateUrl = "user.aspx?uid=" + reqUser.userId + "&view=following";

        string view = "sets";

        if (Request.QueryString["view"] != null)
        {
            view = Request.QueryString["view"].ToString();
        }

        Dictionary<string, List<object>> results = GetResults(db, view, reqUser.userId, user.userId);
        switch (view)
        {
            case "sets":
                SetsLink.CssClass = "active";
                DisplaySets(results);
                break;
            case "loves":
                LovesLink.CssClass = "active";
                DisplayLoves(results);
                break;
            case "followers":
                FollowersLink.CssClass = "active";
                DisplayFollows(results);
                break;
            case "following":
                FollowingLink.CssClass = "active";
                DisplayFollows(results);
                break;
        }
    }

    public Dictionary<string,List<object>> GetResults(string db, string view, long reqUId, long uId )
    {
        Dictionary<string, List<object>> results = new Dictionary<string, List<object>>();

        string query = "EXEC [stp_SS_GetUserExtended] @id=" + reqUId + ", @view='" + view + "', @uid=" + uId;
        SqlConnection myConnection = new SqlConnection(db);

        try
        {
            myConnection.Open();
            using (SqlDataAdapter adp = new SqlDataAdapter(query, myConnection))
            {
                SqlCommand cmd = adp.SelectCommand;
                cmd.CommandTimeout = 300000;
                System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    SetsLink.Text = dr["SetCount"].ToString() + " Looks";
                }
                dr.NextResult();
                while (dr.Read())
                {
                    LovesLink.Text = dr["LoveCount"].ToString() + " Loves";
                }
                dr.NextResult();
                while (dr.Read())
                {
                    FollowersLink.Text = dr["FollowerCount"].ToString() + " Followers";
                }
                dr.NextResult();
                while (dr.Read())
                {
                    FollowingLink.Text = dr["FollowingCount"].ToString() + " Following";
                }
                dr.NextResult();

                switch (view)
                {
                    case "sets":
                        while (dr.Read())
                        {
                            string contestName = dr["Name"].ToString();
                            Look look = new Look();
                            look.id = long.Parse(dr["Id"].ToString());
                            look.products = new List<Product>();
                            look.isLoved = int.Parse(dr["love"].ToString()) == 1 ? true : false;
                            if (results.ContainsKey(contestName))
                            {
                                results[contestName].Add(look);
                            }
                            else
                            {
                                List<object> looks = new List<object>();
                                looks.Add(look);
                                results.Add(contestName, looks);
                            }
                        }
                        break;
                    case "loves":
                        while (dr.Read())
                        {
                            string retailerName = dr["RetailerName"].ToString();
                            Product pdt = new Product();
                            pdt = Product.GetProductFromSqlDataReader(dr);

                            if (results.ContainsKey(retailerName))
                            {
                                results[retailerName].Add(pdt);
                            }
                            else
                            {
                                List<object> products = new List<object>();
                                products.Add(pdt);
                                results.Add(retailerName, products);
                            }
                        }
                        break;
                    case "followers":
                    case "following":
                         while (dr.Read())
                        {
                            string resultName = "Follows";
                            UserProfile user  = new UserProfile();
                            user = UserProfile.GetUserFromSqlReader(dr);
                            user.IsFollowing = int.Parse(dr["following"].ToString());

                            if (results.ContainsKey(resultName))
                            {
                                results[resultName].Add(user);
                            }
                            else
                            {
                                List<object> users = new List<object>();
                                users.Add(user);
                                results.Add(resultName, users);
                            }
                        }
                        break;
                }
            }
        }
        finally
        {
            myConnection.Close();
        }

        return results;
    }

    public void DisplaySets(Dictionary<string, List<object>> results)
    {
        foreach (KeyValuePair<string, List<object>> contestSet in results)
        {
            Panel ContestHeader = new Panel();
            ContestHeader.CssClass = "LookCaption";
            ContestHeader.Controls.Add(new Literal { Text = contestSet.Key });
            UserProfileContent.Controls.Add(ContestHeader);

            Panel looks = new Panel();

            UserProfileContent.Controls.Add(looks);

            foreach (Look look in contestSet.Value)
            {
                Panel lookPanel = new Panel();
                lookPanel.Style.Add("display", "inline");
                lookPanel.Style.Add("position", "relative");

                Panel lookImage = new Panel();
                lookImage.Style.Add("display", "inline");
                HyperLink lookLink = new HyperLink();

                lookLink.NavigateUrl = "look.aspx?lid=" + look.id;
                System.Web.UI.WebControls.Image lookImg = new System.Web.UI.WebControls.Image();
                string imageFilePath = Path.Combine(Server.MapPath("images/looks"), look.id + ".jpg");

                if (!File.Exists(imageFilePath))
                    continue;

                lookImg.ImageUrl = "images/looks/"+look.id+".jpg" ;
                lookImg.CssClass = "item";

                lookLink.Controls.Add(lookImg);
                lookImage.Controls.Add(lookLink);
                lookPanel.Controls.Add(lookImage);

                Panel sharePanel = new Panel();
                sharePanel.CssClass = "shareBtnWrappr";
                //lookPanel.Controls.Add(new Literal { Text = "<div class=\"share-btn\" style=\"padding-right: 6px;z-index:1;\">Share</div>" });

                Panel lovePanel = new Panel();
                lovePanel.CssClass = "love-button";
               
                lovePanel.ID = "love-" + look.id.ToString();
                lovePanel.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                Label loveLabel = new Label();
                loveLabel.ID = "love-" + look.id.ToString() + "label";
                loveLabel.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                loveLabel.Text = "Love";
                lovePanel.Controls.Add(loveLabel);
                sharePanel.Controls.Add(lovePanel);

                if (look.isLoved)
                {
                    lovePanel.CssClass += " loved";
                    loveLabel.Text = "Loved";
                }

                Panel Pinterest = new Panel();
                Pinterest.Style.Add("display", "inline");   
                
                Pinterest.Controls.Add(new Literal
                {
                    Text = "<a target=\"_blank\"  href=\"create.aspx?lid=" + look.id + "\"><img src=\"images/create-btn.png\" alt=\"Create a look\" /></a>" + 
                        "<a class=\"OutBoundLink\" target=\"_blank\" href=\"//pinterest.com/pin/create/button/?url=" + HttpUtility.UrlEncode("http://fashionkred.com/look.aspx?lid=" + look.id + "&ref=" + user.userId) +
                        "&media=" + HttpUtility.UrlEncode(lookImg.ImageUrl) + "&description=" + HttpUtility.UrlEncode("Found this look at Fashionkred!") + "\"> " +
                        "<img src=\"images/pinterest_pin-it_icon.png\" alt=\"Pin It\" title=\"Pin on Pinterest\" style=\"height:24px;\" /></a>"
                });

                sharePanel.Controls.Add(Pinterest);
                lookPanel.Controls.Add(sharePanel);
                looks.Controls.Add(lookPanel);

            }
        }
    }

    public void DisplayLoves(Dictionary<string, List<object>> results)
    {
        foreach (KeyValuePair<string, List<object>> result in results)
        {
            Panel ContestHeader = new Panel();
            ContestHeader.CssClass = "LookCaption";
            ContestHeader.Controls.Add(new Literal { Text = "<br>" +  result.Key.Replace("''", "'")  + " Loves"});
            UserProfileContent.Controls.Add(ContestHeader);

            Panel products = new Panel();

            UserProfileContent.Controls.Add(products);

            foreach (Product pdt in result.Value)
            {
                Panel pdtPanel = new Panel();
                pdtPanel.CssClass = "box";
                pdtPanel.ToolTip = pdt.name;

                Panel pdtImage = new Panel();
                pdtImage.Style.Add("display", "inline");
                HyperLink pdtLink = new HyperLink();
                pdtLink.Target = "_blank";
                pdtLink.NavigateUrl = pdt.url;

                System.Web.UI.WebControls.Image pdtImg = new System.Web.UI.WebControls.Image();

                pdtImg.ImageUrl = pdt.GetNormalImageUrl();
                //pdtImg.CssClass = "box";

                pdtLink.Controls.Add(pdtImg);
                pdtImage.Controls.Add(pdtLink);
                pdtPanel.Controls.Add(pdtImage);

                
                Panel titlePanel = new Panel();
                titlePanel.CssClass = "pdt-love";
                HyperLink pdtTile = new HyperLink();
                pdtTile.CssClass = "pdtTitle";
                pdtTile.Text = pdt.GetName();
                pdtTile.NavigateUrl = pdt.url;
                titlePanel.Controls.Add(pdtTile);
                titlePanel.Controls.Add(new Literal { Text = "<span style=\"float:right;\">" + string.Format("{0:c}", pdt.price) + "</span>"});
                pdtPanel.Controls.Add(titlePanel);

                Panel sharePanel = new Panel();
                sharePanel.CssClass = "pdtActionLinks";
                //lookPanel.Controls.Add(new Literal { Text = "<div class=\"share-btn\" style=\"padding-right: 6px;z-index:1;\">Share</div>" });

                sharePanel.Controls.Add(new Literal
                {
                    Text = "<a  class=\"OutBoundLink\" target=\"_blank\" href=\"//pinterest.com/pin/create/button/?url=" + HttpUtility.UrlEncode("http://fashionkred.com/product.aspx?pid=" + pdt.id + "&ref=" + user.userId) +
                        "&media=" + HttpUtility.UrlEncode(pdtImg.ImageUrl) + "&description=" + HttpUtility.UrlEncode(pdt.name) + "\"> " +
                        "<img src=\"images/pinterest_pin-it_icon.png\" alt=\"Pin It\" title=\"Pin on Pinterest\" style=\"height:24px;\" /></a>"
                });

                //HyperLink buyLink = new HyperLink();
                //buyLink.Text = "Buy";
                //buyLink.Target = "_blank";
                //buyLink.NavigateUrl = pdt.AffiliateUrl;
                //buyLink.Style.Add("color", "red");
                //sharePanel.Controls.Add(buyLink);
                

                pdtPanel.Controls.Add(sharePanel);
                products.Controls.Add(pdtPanel);

            }
        }
    }

    public void DisplayFollows(Dictionary<string, List<object>> results)
    {
        foreach (KeyValuePair<string, List<object>> result in results)
        {   
            Panel users = new Panel();

            UserProfileContent.Controls.Add(users);

            foreach (UserProfile user in result.Value)
            {
                Panel userPanel = new Panel();
                userPanel.CssClass = "userBox";
                userPanel.ToolTip = user.name;

                Panel userImage = new Panel();
                userImage.Style.Add("display", "inline");
                HyperLink userLink = new HyperLink();
                userLink.NavigateUrl = "user.aspx?uid="  + user.userId;

                System.Web.UI.WebControls.Image userImg = new System.Web.UI.WebControls.Image();

                userImg.ImageUrl = user.BigPic();
                //pdtImg.CssClass = "box";

                userLink.Controls.Add(userImg);
                userImage.Controls.Add(userLink);
                userPanel.Controls.Add(userImage);


                Panel titlePanel = new Panel();
                titlePanel.CssClass = "pdt-love";
                HyperLink userTile = new HyperLink();
                userTile.CssClass = "pdtTitle";
                userTile.Text = user.name;
                userTile.NavigateUrl = "user.aspx?uid=" + user.userId;
                titlePanel.Controls.Add(userTile);
                userPanel.Controls.Add(titlePanel);

                //add subscribe panel
                if (this.userId != user.userId)
                {
                    Panel followPanel = new Panel();
                    followPanel.CssClass = "userActionLinks";

                    HyperLink followLink = new HyperLink();
                    followLink.Target = "_blank";
                    if (user.IsFollowing == 0)
                    {
                        followLink.Text = "Follow";
                        followLink.NavigateUrl = "subscribe.aspx?uid=" + this.userId + "&sid=" + user.userId + "&subscribe=1&redirect=" + "user.aspx?uid=" + user.userId;
                    }
                    if (user.IsFollowing == 1)
                    {
                        followLink.Text = "Unfollow";
                        followLink.Style.Add("color", "#ccc");
                        followLink.NavigateUrl = "subscribe.aspx?uid=" + this.userId + "&sid=" + user.userId + "&subscribe=0&redirect=" + "user.aspx?uid=" + user.userId;
                    }
                    followPanel.Controls.Add(followLink);
                    userPanel.Controls.Add(followPanel);

                }
                users.Controls.Add(userPanel);

            }
        }
    }
}