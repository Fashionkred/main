using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShopSenseDemo;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    public UserProfile user { set; get; }

    public System.Web.UI.WebControls.Image UserImageControl
    {
        get { return UserImage; }
    }

    

    public Label UserPointsLabel
    {
        get { return UserPoints; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        ContestString.Text = ConfigurationManager.AppSettings["ContestString"];
        ContestLogo.ImageUrl = ConfigurationManager.AppSettings["ContestImage"];
        //ContestSubString.Text = ConfigurationManager.AppSettings["ContestSubString"];
        if (this.Session["contest"] == null)
        {
            this.Session["contest"] = int.Parse(ConfigurationManager.AppSettings["ContestId1"]);
        }

        int totalContests = int.Parse(ConfigurationManager.AppSettings["TotalContest"]);
        for (int i = 1; i <= totalContests; i++)
        {
            HyperLink contestlink = new HyperLink();
            int contestId = int.Parse(ConfigurationManager.AppSettings["ContestId" + i]);
            
            if (this.Session["contest"] != null && (int)this.Session["contest"] == contestId)
            {
                contestlink.CssClass = "contest-link-selected";
            }
            else
            {
                contestlink.CssClass = "contest-link";
            }

            contestlink.Text = ConfigurationManager.AppSettings["ContestName" + i];
            var urlString = this.Request.Url.ToString();
            int startIndex = urlString.LastIndexOf('/') + 1;
            int endIndex = urlString.LastIndexOf('?') != -1 ? urlString.LastIndexOf('?')-1 : urlString.Length - 1;

            if (Request.QueryString["contestid"] != null)
            {
                contestlink.NavigateUrl = urlString.Replace("contestid=" + Request.QueryString["contestid"], "contestid=" + contestId);
            }
            else
            {
                if (startIndex < endIndex)
                {
                    string pageName = urlString.Substring(startIndex, endIndex - startIndex + 1);
                    contestlink.NavigateUrl = pageName + "?contestid=" + contestId;
                }
                else
                {
                    contestlink.NavigateUrl = "look.aspx" + "?contestid=" + contestId;
                }
            }

            ContestBar.Controls.Add(contestlink);
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
    }
}
