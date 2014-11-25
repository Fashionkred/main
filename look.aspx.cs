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
using System.Web.Services;
using System.Data;

public partial class Outfit : BasePage
{
    private int upVote { get; set; }
    private int downVote { get; set; }
    private Look look { get; set; }
    private int contestId;
    private long retailerId;


    //New code//

    private string categoryId;
    private string brandId;
    private string colorid;

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



        HtmlMeta desc = new HtmlMeta();
        desc.Attributes.Add("property", "og:description");
        if (look.products.Count == 3)
        {
            desc.Content = "Outfit containing " + look.products[0].name + ", " + look.products[1].name + " and " + look.products[2].name;
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

        hdnDb.Value = "";

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
            look = Look.GetLookById(look.id, this.user.userId, db);

            //clear the session
            this.Session["lid"] = null;
        }

        else if (Request.QueryString["lid"] != null)
        {
            look.id = long.Parse(Request.QueryString["lid"]);
            look = Look.GetLookById(look.id, this.user.userId, db);
        }

        else
        {
            look = Look.GetRandomLook(this.contestId, this.user.userId, db);
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

        if (look.creator.userId == user.userId)
            isCreatorView = true;

        if (look.originalLookId.ToString() == "0")
        {
            lblStyle.Text = "styled by";
        }
        else
        {
            lblStyle.Text = "re-styled by";
        }


        // Redirecting to user profile page when click on username //
        aUserName.HRef = "user.aspx?userid=" + look.creator.userId;
        imgLookUser.ImageUrl = look.creator.pic;
        lblLookUserName.Text = look.creator.name;
        lblLookTitle.Text = look.title;
        lblLoveCount.Text = look.upVote + " Love";
        lblRestyleCount.Text = look.restyleCount + " Restyle";

        //Bind Look//
        dlSingleLook.DataSource = look.products;
        dlSingleLook.DataBind();


        //Bind Tags//
        dlTags.DataSource = look.tags;
        dlTags.DataBind();
    }

    protected void dlSingleLook_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {


            var product = (Product)e.Item.DataItem;

            long id = product.id;

            System.Web.UI.WebControls.Image imageLook = e.Item.FindControl("imgLook") as System.Web.UI.WebControls.Image;
            System.Web.UI.WebControls.Label lblColorName = e.Item.FindControl("lblColorName") as System.Web.UI.WebControls.Label;

            //New Code//

            //categoryId = product.GetCategoryId();

            lblColorName.Text = product.GetColor().ToString();

            imageLook.ImageUrl = product.GetImageUrl();
        }


    }


}