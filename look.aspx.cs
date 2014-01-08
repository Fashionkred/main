using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ShopSenseDemo;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;

public partial class Outfit : BasePage
{
    private int upVote { get; set; }
    private int downVote { get; set; }
    private Look look { get; set; }
    private int contestId;
    private long retailerId;
    
    private long userId { get; set; }

    public void SetOGTags()
    {
        //Set the OG tags if there is a lid
        Master.Page.Title = "FashionKred - Find your best outfit!";

        HtmlMeta title = new HtmlMeta();
        title.Attributes.Add("property", "og:title");
        if (look.products.Count == 3)
        {
            title.Content = look.products[0].name + ", " + look.products[1].name + " and " + look.products[2].name;
        }
        else
        {
            title.Content = look.products[0].name + " and " + look.products[1].name;
        }
        Master.FindControl("head").Controls.Add(title);

        HtmlMeta url = new HtmlMeta();
        url.Attributes.Add("property", "og:url");
        url.Content = "http://fashionKred.com/look.aspx?lid=" + look.id;
        Master.FindControl("head").Controls.Add(url);

        HtmlMeta type = new HtmlMeta();
        type.Attributes.Add("property", "og:type");
        type.Content = "fashionkred:outfit";
        Master.FindControl("head").Controls.Add(type);

        HtmlMeta image = new HtmlMeta();
        image.Attributes.Add("property", "og:image");

        try
        {
            string imageFilePath = Path.Combine(Server.MapPath("images/looks"), look.id + ".jpg");
                
            if (look.products.Count >= 3)
            {
                if (!File.Exists(imageFilePath))
                {
                    WebHelper.CreateLookPanel(look, imageFilePath);
                    //WebHelper.MergeThreeImages(look.products[0].GetImageUrl(), look.products[1].GetNormalImageUrl(), look.products[2].GetNormalImageUrl(), imageFilePath);
                }

                image.Content = "http://fashionkred.com/images/looks/" + look.id + ".jpg";
                Master.FindControl("head").Controls.Add(image);
            }
            else
            {
                if (!File.Exists(imageFilePath))
                {
                    WebHelper.MergeTwoImages(look.products[0].GetImageUrl(), look.products[1].GetImageUrl(), imageFilePath);
                }

                image.Content = "http://fashionkred.com/images/looks/" + look.id + ".jpg";
                Master.FindControl("head").Controls.Add(image);
            }
        }
        catch
        {
            image.Content = look.products[0].GetImageUrl();
            Master.FindControl("head").Controls.Add(image);
        }

        //HtmlMeta image2 = new HtmlMeta();
        //image2.Attributes.Add("property", "og:image");
        //image2.Content = look.products[1].GetImageUrl();
        //Master.FindControl("head").Controls.Add(image2);

        HtmlMeta desc = new HtmlMeta();
        desc.Attributes.Add("property", "og:description");
        if (look.products.Count == 3)
        {
            desc.Content = "Outfit containing " + look.products[0].name + ", " + look.products[1].name + " and " +look.products[2].name;
        }
        else
        {
            desc.Content = "Outfit containing " + look.products[0].name + " and " + look.products[1].name;
        }
        Master.FindControl("head").Controls.Add(desc);

        HtmlMeta appId = new HtmlMeta();
        appId.Attributes.Add("property", "fb:app_id");
        appId.Content = ConfigurationManager.AppSettings["AppId"];
        Master.FindControl("head").Controls.Add(appId);
    }

    
    protected void Page_Load(object sender, EventArgs e)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        bool isVoted = false;

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

        if (Request.QueryString["contestid"] != null)
        {
            this.contestId = int.Parse(Request.QueryString["contestid"]);
            this.Session["contest"] = this.contestId;
        }
        else if (this.Session["contest"] == null)
        {
            this.contestId = int.Parse(ConfigurationManager.AppSettings["ContestId1"]);
            this.Session["contest"] = this.contestId;
        }
        else
        {
            this.contestId = (int)this.Session["contest"];
        }
        this.retailerId = long.Parse(ConfigurationManager.AppSettings["Retailer"]);
       
        look = new Look();
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

        //Check if look is set or not
        if (this.Session["lid"] != null)
        {
            look.id = long.Parse(this.Session["lid"].ToString());
            look = Look.GetLookById(look.id, this.user.id, out isVoted, db);

            //clear the session
            this.Session["lid"] = null;
        }

        else if (Request.QueryString["lid"] != null)
        {
            look.id = long.Parse(Request.QueryString["lid"]);
            look = Look.GetLookById(look.id,this.user.id, out isVoted, db);
        }

        else
        {
            look = Look.GetRandomLook(this.contestId,this.user.id, db);
        }

        //Make sure we have a look or at least 2 products in the look
        if (look.id == 0 || (look.products != null && look.products.Count < 2))
        {
            //redirect to error page
            return;
        }

        //SetOGTags
        SetOGTags();

        //deprecating contests -if the look's contest is different than session contest - reset session contest
        if (this.Session["contest"] != null)
        {
            if (look.contestId != (int)this.Session["contest"])
                this.Session["contest"] = look.contestId;
        }
        else
        {
            this.Session["contest"] = look.contestId;
        }

        //Check if the look's creator is the current logged in user
        bool isCreatorView = false;

        if (look.creator.id == user.id)
            isCreatorView = true;

        if (isCreatorView || isVoted)
        {
            UserImage.Visible = false;
            SubscribePanel.Visible = false;
            BackButton.Visible = true;
            //CreateLook.Visible = false;
            //ShareButton.Visible = true;
            LeftButton.Visible = false;
            RightButton.Visible = false;
            LeftButtonDisabled.Visible = true;
            RightButtonDisabled.Visible = true;
            VotePanel.Visible = true;
            VoteMsg.Text = (look.upVote + look.downVote).ToString() + " people voted so far";

            double upVote, downVote;
            Label leftVote = new Label();
            LeftTextButtonDisabled.Controls.Add(leftVote);
            Label rightVote = new Label();
            RightTextButtonDisabled.Controls.Add(rightVote);

            if (look.upVote + look.downVote == 0)
            {
                leftVote.Text = "0% VOTED MATCH";
                rightVote.Text = "0% VOTED NO MATCH";
            }
            else
            {
                upVote = Math.Ceiling((double)(look.upVote * 100 / (look.upVote + look.downVote)));
                downVote = Math.Ceiling((double)(look.downVote * 100 / (look.upVote + look.downVote)));
                leftVote.Text = upVote.ToString() + "% VOTED MATCH";
                rightVote.Text = downVote.ToString() + "% VOTED NO MATCH";
            }
            //Set the creator name to back to voting
            CreatorName.Text = "Back to voting";
        }
        else
        {
            //Set creator tags
            CreatorImage.ImageUrl = look.creator.pic;
            CreatorName.Text = "Made by: " + look.creator.name;
            CreatorName.NavigateUrl = "user.aspx?uid=" + look.creator.id;
        }
        
        //anonymous view show the dialogue
        if (user.id == 0)
        {
            Unsigned.Text = "1";
        }
        //Set the product
        P1Image.ImageUrl = look.products[0].GetImageUrl();

        P1Brand.Text = look.products[0].GetBrandName();
        P1Label.Text = look.products[0].GetName();
        if (string.IsNullOrEmpty(look.products[0].AffiliateUrl))
        {
            P1Title.NavigateUrl = P1Label.NavigateUrl = look.products[0].url;
        }
        else
        {
            P1Title.NavigateUrl = P1Label.NavigateUrl = look.products[0].AffiliateUrl + "&u1=" + look.creator.id;
        }

        P1Love.Text = look.products[0].loves.ToString();
        P1BuyButton.NavigateUrl = look.products[0].AffiliateUrl + "&u1=" + look.creator.id ;

        //Cat, color, retailer
        P1Cat.Text = look.products[0].GetCategory();
        P1Color.Text = look.products[0].GetColor();
        P1Retailer.Text = look.products[0].retailer;
        P1Retailer.Text += look.products[0].isCover ? " Cover" : "";
        //suppress price
        //P1Price.Text = string.Format("{0:c}", look.products[0].price);
        //if (look.products[0].salePrice != 0)
        //{
        //    P1Price.CssClass += " strike-through";
        //    P1SalePrice.Text = string.Format("{0:c}", look.products[0].salePrice);
        //    P1SalePrice.CssClass += " show";
        //}

        P2Image.ImageUrl = look.products[1].GetImageUrl();
        P2Brand.Text = look.products[1].GetBrandName();
        P2Label.Text = look.products[1].GetName();
        if (string.IsNullOrEmpty(look.products[1].AffiliateUrl))
        {
            P2Title.NavigateUrl = P2Label.NavigateUrl = look.products[1].url;
        }
        else
        {
            P2Title.NavigateUrl = P2Label.NavigateUrl = look.products[1].AffiliateUrl + "&u1=" + look.creator.id;
        }

        P2Love.Text = look.products[1].loves.ToString();
        P2BuyButton.NavigateUrl = look.products[1].AffiliateUrl + "&u1=" + look.creator.id;

        //Cat, color, retailer
        P2Cat.Text = look.products[1].GetCategory();
        P2Color.Text = look.products[1].GetColor();
        P2Retailer.Text = look.products[1].retailer;
        P2Retailer.Text += look.products[1].isCover ? " Cover" : "";
        //P2Price.Text = string.Format("{0:c}", look.products[1].price);
        //if (look.products[1].salePrice != 0)
        //{
        //    P2Price.CssClass += " strike-through";
        //    P2SalePrice.Text = string.Format("{0:c}", look.products[1].salePrice);
        //    P2SalePrice.CssClass += " show";
        //}
        if (look.products.Count == 3)
        {
            //Change the classes
            Left.CssClass = "left-item-wrapper-more";
            gallery.CssClass = "gallery-3-items";
            RightPanel.CssClass = "right-content-parts-wrapper";
            RightUpper.CssClass = "";
            RightLower.Visible = true;
            P2ImageDiv.CssClass = "product-small-image";
            Plus.CssClass = "plus-content-3items";
            P2Image.ImageUrl = look.products[1].GetNormalImageUrl();

            //set P3
            P3Image.ImageUrl = look.products[2].GetNormalImageUrl();
            P3Brand.Text = look.products[2].GetBrandName();
            P3Label.Text = look.products[2].GetName();
            if (string.IsNullOrEmpty(look.products[2].AffiliateUrl))
            {
                P3Title.NavigateUrl = P3Label.NavigateUrl = look.products[2].url;
            }
            else
            {
                P3Title.NavigateUrl = look.products[2].AffiliateUrl + "&u1=" + look.creator.id;
            }
            P3Love.Text = look.products[2].loves.ToString();
            P3Id.Text = look.products[2].id.ToString();
            P3BuyButton.NavigateUrl = look.products[2].AffiliateUrl + "&u1=" + look.creator.id;

            //Cat, color, retailer
            P3Cat.Text = look.products[2].GetCategory();
            P3Color.Text = look.products[2].GetColor();
            P3Retailer.Text = look.products[2].retailer;
            P3Retailer.Text += look.products[2].isCover ? " Cover" : "";
        }
        this.userId = user.id;
        System.Web.UI.WebControls.Image userImage = (System.Web.UI.WebControls.Image)this.Master.FindControl("UserImage");
        HyperLink  userName = (HyperLink)this.Master.FindControl("UserName");
        Label userPoints = (Label)this.Master.FindControl("UserPoints");
        HyperLink loginLink = (HyperLink)this.Master.FindControl("LogInLink");
        Panel dropDown = (Panel)this.Master.FindControl("DropDown"); 
        if (this.userId > 0)
        {
            userImage.ImageUrl = user.pic;
            userName.Text = user.name;
            userName.NavigateUrl = "user.aspx?uid=" + user.id;
            userPoints.Text = user.points.ToString() + " votes";
        }
        else
        {
            userImage.Visible = false;
            userName.Visible = false;
            userPoints.Visible = false;
            dropDown.Visible = false;
            loginLink.Visible = true;
            loginLink.NavigateUrl = this.Request.RawUrl.Contains('?') ? this.Request.RawUrl + "&login=1" : this.Request.RawUrl + "?login=1";
        }
        
        //Set the metadata
        LookId.Text = look.id.ToString();
        P1Id.Text = look.products[0].id.ToString();
        P2Id.Text = look.products[1].id.ToString();
        UpVote.Text = look.upVote.ToString();
        DownVote.Text = look.downVote.ToString();
        UserId.Text = this.userId.ToString();
        UserShare.Text = this.user.IsPrivate ? "0" : "1";
        CreatorId.Text = look.creator.id.ToString();
        LookTitle.Text = look.title;
        LookTags.Text = look.TagsFormatted();
        LovesCount.Text = look.upVote.ToString() + "loves";
        StyleCount.Text = look.restyleCount.ToString() + "restyles";
        ViewCount.Text = look.viewCount.ToString() + "views";

        if (look.originalLookId != 0)
        {
            ReStyled.Text = "Restyled from this look";
            ReStyled.NavigateUrl = "look.aspx?lid=" + look.originalLookId;
        }

        //Set the following class
        if (UserProfile.IsFollower(look.creator.id, this.userId, db))
        {
            SubscribePanel.CssClass += " following";
            Subscribe.Text = "Following";
        }
       
        //if user have more than one look - pull favorites
        if (user.points > 0)
        {
            bool P1love, P2love, P3love;
            IList<ShopSenseDemo.Product> loves = UserProfile.GetLovesByUserId(user.id, look, this.retailerId, db, out P1love, out P2love, out P3love);

            if (P1love)
            {
                P1LoveButton.CssClass += "-red";
                P1LoveImg.ImageUrl = "images/heart_filled.jpg";
            }

            if (P2love)
            {
                P2LoveButton.CssClass += "-red";
                P2LoveImg.ImageUrl = "images/heart_filled.jpg";
            }

            if (P3love)
            {
                P3LoveButton.CssClass += "-red";
                P3LoveImg.ImageUrl = "images/heart_filled.jpg";
            }

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

                Favorites.Controls.Add(panel);
            }

            if (loves.Count < 7)
            {
                for (int i = 0; i < 7 - loves.Count; i++)
                {
                    Panel panel = new Panel();
                    panel.CssClass = "fav-image-standart";

                    Favorites.Controls.Add(panel);
                }
            }
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
                Panel panel = new Panel();
                panel.CssClass = "fav-image-standart";

                Favorites.Controls.Add(panel);
            }
        }
    }

    protected void LoveIt_Click(object sender, EventArgs e)
    {
        //Save the look

        //Bring another look if <10 otherwise prompt to invite friend
    }

    protected void Meh_Click(object sender, EventArgs e)
    {
        //Save the look

        //Bring another look if <10 otherwise prompt to invite friend
    }
}