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

namespace ShopSenseDemo
{

    public partial class _Default : BasePage
    {
        private long userId;
        private int contestId;

        public void SetOGTags()
        {
            Master.Page.Title = "Fashionkred - online style board for fashionable looks!";

            HtmlMeta title = new HtmlMeta();
            title.Attributes.Add("property", "og:title");
            title.Content = Master.Page.Title;
            Master.FindControl("head").Controls.Add(title);

            HtmlMeta url = new HtmlMeta();
            url.Attributes.Add("property", "og:url");
            url.Content = "http://fashionkred.com/";
            Master.FindControl("head").Controls.Add(url);

            HtmlMeta image = new HtmlMeta();
            image.Attributes.Add("property", "og:image");
            image.Content = "images/nordstromlook.jpeg";

            Master.FindControl("head").Controls.Add(image);

            HtmlMeta desc = new HtmlMeta();
            desc.Attributes.Add("property", "og:description");
            desc.Content = "Fashionkred is an online pinboard of looks created by stylists and fashionistas for different occassions";
            Master.FindControl("head").Controls.Add(desc);

            HtmlMeta type = new HtmlMeta();
            type.Attributes.Add("property", "og:type");
            type.Content = "website";
            Master.FindControl("head").Controls.Add(type);

            HtmlMeta appId = new HtmlMeta();
            appId.Attributes.Add("property", "fb:app_id");
            appId.Content = ConfigurationManager.AppSettings["AppId"];
            Master.FindControl("head").Controls.Add(appId);
        }


        public static long userIdOrg;
        public static long lookId;



        protected void Page_Load(object sender, EventArgs e)
        {
            string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            hdnDb.Value = "";

            if (Session["contest"] == null)
            {
                contestId = int.Parse(ConfigurationManager.AppSettings["ContestId1"]);
                Session["contest"] = contestId;
            }
            else
            {
                contestId = (int)Session["contest"];
            }

            //Request.QueryString["uid"] = "4";
            //Request.QueryString["lid"] = "2648";
            //Request.QueryString["votetype"] = "up";
            //Request.QueryString["point"] = "0";
            //Request.QueryString["lightweight"] = "1";

            //Redirect to signup page unless invite code is there
            if (Request.QueryString["invite"] == null && Request.Cookies["beta"] == null)
            {
                //    Response.Redirect("http://signup.fashionkred.com/");
            }
            else
            {
                HttpCookie betaUser = new HttpCookie("beta");
                betaUser.Value = "1";
                betaUser.Expires = DateTime.UtcNow.AddMonths(1);
                Response.Cookies.Add(betaUser);
            }

            //add the referrer cookie
            if (Request.QueryString["ref"] != null)
            {
                HttpCookie referrer = new HttpCookie("__userReferralid");
                referrer.Value = Request.QueryString["ref"];
                referrer.Expires = DateTime.UtcNow.AddYears(1);
                Response.Cookies.Add(referrer);
            }
            else if (Request.Cookies["__userReferralid"] == null && Request.UrlReferrer != null)
            {
                HttpCookie referrer = new HttpCookie("__userReferralid");
                referrer.Value = this.Request.UrlReferrer != null ? this.Request.UrlReferrer.AbsolutePath : "Unknown";
                referrer.Expires = DateTime.UtcNow.AddYears(1);
                Response.Cookies.Add(referrer);
            }

            //Server.Transfer("look.aspx");
            //set the user
            if (this.Session["user"] != null)
            {
                this.user = this.Session["user"] as UserProfile;
            }
            else if (Request.QueryString["code"] != null || Request.QueryString["login"] == "1")
            {
                this.GetUser(db);
            }
            else
            {
                this.user = new UserProfile();
            }

            //set contest
            //if (Request.QueryString["contestid"] != null)
            //{
            //    this.contestId = int.Parse(Request.QueryString["contestid"].ToString());
            //    this.Session["contest"] = this.contestId;
            //}
            //else
            //{
            //    this.contestId = 0;
            //    this.Session["contest"] = this.contestId;
            //}

            //SetOGTags
            SetOGTags();

            //set top user bar
            userIdOrg = user.id;
            //System.Web.UI.WebControls.Image userImage = (System.Web.UI.WebControls.Image)this.Master.FindControl("UserImage");
            //HyperLink userName = (HyperLink)this.Master.FindControl("UserName");
            //Label userPoints = (Label)this.Master.FindControl("UserPoints");
            //HyperLink loginLink = (HyperLink)this.Master.FindControl("LogInLink");
            //Panel dropDown = (Panel)this.Master.FindControl("DropDown");
            if (userIdOrg > 0)
            {
                UserImage.ImageUrl = user.pic;
                UserName.Text = user.name;
                UserName.NavigateUrl = "user.aspx?uid=" + user.id;
                UserPoints.Text = user.points.ToString() + " votes";
            }
            else
            {
                UserImage.Visible = false;
                UserName.Visible = false;
                UserPoints.Visible = false;
                //dropDown.Visible = false;
                LogInLink.Visible = true;
                LogInLink.NavigateUrl = this.Request.RawUrl.Contains('?') ? this.Request.RawUrl + "&login=1" : this.Request.RawUrl + "?login=1";
            }

            //anonymous view show the dialogue
            if (user.id == 0)
            {
                hdnchkuser.Value = "0";
            }
            else
            {
                 hdnchkuser.Value = user.id.ToString();
            }

            //Let's set the sharing metadata
            UserShare.Text = this.user.IsPrivate ? "0" : "1";
            UserId.Text = this.user.id.ToString();

            //string tagId = Request.QueryString["tagid"];
            ////Let's get the sets and display them
            //if (!string.IsNullOrEmpty(tagId))
            //{
            //    DisplaySets(Look.GetTaggedLooks(db, user.id, int.Parse(tagId)));
            //}
            //else
            //{
            //    DisplaySets(Look.GetHPLooks(db, user.id, contestId));
            //}

        }

        ///Moved to look.cs file///
        //public List<Look> GetLooks(string db,  long uId, long contestId)
        //{
        //    List<Look> looks = new List<Look>();

        //    string query = "EXEC [stp_SS_GetFollowedLooks] @userId=" + uId;
        //    //if (contestId != 0)
        //    //{
        //    //    query += (", @contestId=" + contestId);
        //    //}

        //    SqlConnection myConnection = new SqlConnection(db);

        //    try
        //    {
        //        myConnection.Open();
        //        using (SqlDataAdapter adp = new SqlDataAdapter(query, myConnection))
        //        {
        //            SqlCommand cmd = adp.SelectCommand;
        //            cmd.CommandTimeout = 300000;
        //            System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();

        //            looks = Look.GetLooksFromSqlReader(dr);
        //        }
        //    }
        //    finally
        //    {
        //        myConnection.Close();
        //    }

        //    return looks;
        //}

        // public void DisplaySets(List<Look> results)
        // {
        //    dlLook.DataSource = results;
        ///   dlLook.DataBind();


        ////Code Not being used////

        //foreach (Look look in results)
        //{
        //    Panel lookPanel = new Panel();
        //    lookPanel.Style.Add("display", "inline");
        //    lookPanel.Style.Add("position", "relative");

        //    Panel lookImage = new Panel();
        //    lookImage.Style.Add("display", "inline");
        //    HyperLink lookLink = new HyperLink();

        //    lookLink.NavigateUrl = "look.aspx?lid=" + look.id;
        //    System.Web.UI.WebControls.Image lookImg = new System.Web.UI.WebControls.Image();
        //    string imageFilePath = Path.Combine(Server.MapPath("images/looks"), look.id + ".jpg");

        //    if (!File.Exists(imageFilePath))
        //        continue;

        //    lookImg.ImageUrl = "http://fashionkred.com/images/looks/" + look.id + ".jpg";
        //    lookImg.CssClass = "item";

        //    lookLink.Controls.Add(lookImg);
        //    lookImage.Controls.Add(lookLink);
        //    lookPanel.Controls.Add(lookImage);

        //    Panel sharePanel = new Panel();
        //    sharePanel.CssClass = "shareBtnWrappr";
        //    //lookPanel.Controls.Add(new Literal { Text = "<div class=\"share-btn\" style=\"padding-right: 6px;z-index:1;\">Share</div>" });

        //    Panel lovePanel = new Panel();
        //    lovePanel.CssClass = "love-button";
        //    lovePanel.ID = "love-" + look.id.ToString();
        //    lovePanel.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        //    Label loveLabel = new Label();
        //    loveLabel.ID = "love-" + look.id.ToString() + "label";
        //    loveLabel.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        //    loveLabel.Text = "Love";
        //    lovePanel.Controls.Add(loveLabel);
        //    sharePanel.Controls.Add(lovePanel);

        //    Panel Pinterest = new Panel();
        //    Pinterest.Style.Add("display", "inline");   
        //    Pinterest.Controls.Add(new Literal
        //    {
        //        Text = "<a target=\"_blank\"  href=\"create.aspx?lid=" + look.id + "\"><img src=\"images/create-btn.png\" alt=\"Create a look\" /></a>" +
        //            "<a class=\"OutBoundLink\" target=\"_blank\" href=\"//pinterest.com/pin/create/button/?url=" + HttpUtility.UrlEncode("http://fashionkred.com/look.aspx?lid=" + look.id + "&ref=" + user.id) +
        //            "&media=" + HttpUtility.UrlEncode(lookImg.ImageUrl) + "&description=" + HttpUtility.UrlEncode("Found this look at Fashionkred!") + "\"> " +
        //            "<img src=\"images/pinterest_pin-it_icon.png\" alt=\"Pin It\" title=\"Pin on Pinterest\" style=\"height:24px;\" /></a>"
        //    });

        //    sharePanel.Controls.Add(Pinterest);

        //    lookPanel.Controls.Add(sharePanel);

        //    Panel userPanel = new Panel();
        //    userPanel.CssClass = "userPanel";
        //    HyperLink userLink = new HyperLink();
        //    userLink.NavigateUrl = "user.aspx?uid=" + look.creator.id;
        //    userPanel.Controls.Add(userLink);


        //    System.Web.UI.WebControls.Image userImg = new System.Web.UI.WebControls.Image();
        //    userImg.Style.Add("Height", "30px");
        //    userImg.Style.Add("Width", "30px");

        //    userImg.ImageUrl = look.creator.pic;
        //    userLink.Controls.Add(userImg);

        //    userLink.Controls.Add(new Literal { Text = "<span style=\"color:#717171;\"> Made by: " + look.creator.name + "<br> for " 
        //        + look.TagsFormatted() + " " + look.upVote + " loves, "+ look.restyleCount + " restyles</span>" });

        //    lookPanel.Controls.Add(userPanel);
        //    //LookContent.Controls.Add(lookPanel);

        //}

        // }

        /// <summary>
        /// Bind inner repeater on each itemdatabound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlLook_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Repeater innerLook = (Repeater)e.Item.FindControl("dlInnerLook");
                Repeater tags = (Repeater)e.Item.FindControl("dlTags");
                var look = (Look)e.Item.DataItem;


                // innerLook.DataSource = look.products;
                // innerLook.DataBind();
                tags.DataSource = look.tags;
                tags.DataBind();
            }

        }


    }

}
