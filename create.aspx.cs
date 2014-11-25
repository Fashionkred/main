using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShopSenseDemo;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Threading;

public partial class create : BasePage
{
    private long userId { get; set; }
    private int contestId;
    private Look initialLook { get; set; }

    private void CreateSliderAndFav(Product pdt, int position, string itemCssClass, int carouselPosition, ref Panel favCarousel, ref HtmlGenericControl slider, string sliderId)
    {
        Panel item = new Panel();
        item.CssClass = itemCssClass;

        HyperLink link = new HyperLink();
        System.Web.UI.WebControls.Image fav = new System.Web.UI.WebControls.Image();
        link.Controls.Add(fav);
        fav.ImageUrl = pdt.GetThumbnailUrl();
        fav.AlternateText = pdt.name;
        link.NavigateUrl = "javascript:sliderselect(" + position + "," + carouselPosition + ")";
        link.ToolTip = pdt.name;
        item.Controls.Add(link);
        favCarousel.Controls.Add(item);

        HtmlGenericControl newLi = new HtmlGenericControl("li");
        
        Panel productPanel = new Panel();
        productPanel.ID = sliderId + "div" + carouselPosition.ToString();
        productPanel.CssClass = position == 1 ? "product-image" : "product-small-image";
        ProductHyperLink pdtlink = new ProductHyperLink(pdt);
        pdtlink.NavigateUrl = pdt.AffiliateUrl;
        System.Web.UI.WebControls.Image pdtImg = new System.Web.UI.WebControls.Image();
        pdtImg.ImageUrl = position == 1 ? pdt.GetImageUrl() : pdt.GetNormalImageUrl();
        pdtImg.AlternateText = pdt.name;
        pdtImg.ToolTip = pdt.name;
        pdtImg.ID = "pdt-" + pdt.id.ToString();
        pdtlink.Controls.Add(pdtImg);
        pdtImg.Attributes.Add("color", pdt.GetColor()); 
        productPanel.Controls.Add(pdtlink);

        newLi.Controls.Add(productPanel);
        slider.Controls.Add(newLi);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        
        //set the user
        if (this.Session["user"] != null)
        {
            this.user = this.Session["user"] as UserProfile;
        }
        else
        {
            this.GetUser(db);
        }

        if (Request.QueryString["lid"] != null)
        {
            long lookId = long.Parse(Request.QueryString["lid"]);
            initialLook = Look.GetLookById(lookId, this.user.userId, db);
            this.contestId = initialLook.contestId;
            this.Session["contest"] = this.contestId;
            this.OriginalLook.Text = lookId.ToString();
        }

        if (Request.QueryString["contestid"] != null)
        {
            this.contestId = int.Parse(Request.QueryString["contestid"]);
            this.Session["contest"] = this.contestId;
        }
        else if (this.Session["contest"] == null || (int)this.Session["contest"] == 0)
        {
            this.contestId = int.Parse(ConfigurationManager.AppSettings["ContestId1"]);
            this.Session["contest"] = this.contestId;
        }
        else
        {
            this.contestId = (int)this.Session["contest"];
        }
        
        Master.Page.Title = "FashionKred - Create outfit from your favorites";

        //set the subtext
        //Label subText = (Label)this.Master.FindControl("Subtext");
        //subText.Text = "CREATE AN OUTFIT TO WIN $50 MACY'S GIFT CARD";
        System.Web.UI.WebControls.Image userImage = (System.Web.UI.WebControls.Image)this.Master.FindControl("UserImage");
        userImage.ImageUrl = user.pic;
        HyperLink userName = (HyperLink)this.Master.FindControl("UserName");
        userName.Text = user.name;
        userName.NavigateUrl = "user.aspx?uid=" + user.userId;

        Label userPoints = (Label)this.Master.FindControl("UserPoints");
        userPoints.Text = user.points.ToString() + " votes";

        this.userId = user.userId;
        UserId.Text = this.userId.ToString();
        UserShare.Text = this.user.IsPrivate ? "0" : "1";

        //Set the cover product - TOO - right now the first product is always cover product
        CoverPdt.Text = "0";

        //Get favorites for the specified categories
        Dictionary<string, IList<Product>> favorites = UserProfile.GetLovesByContest(userId, contestId, db);
        int position = 1;
        
        foreach (KeyValuePair<string, IList<Product>> favorite in favorites)
        {
            Product pdt = new Product();

            if(initialLook != null)
                pdt = initialLook.products[position - 1];
                
            string carouselClass = "carousel-favorites", prevCarouselClass = "prev-carousel", nextCarouselClass = "next-carousel", itemCssClass = "fav-image", sliderId = "sliderCreate";
            if (position > 1)
            {
                carouselClass += ("_" + position);
                prevCarouselClass += ("_" + position);
                nextCarouselClass += ("_" + position);
                itemCssClass += ("_" + position);
                sliderId +=  position;
            }

            Panel favCaptionCarousel = new Panel();
            favCaptionCarousel.CssClass = "caption-carousel";

            string[] captions = favorite.Key.Split(',');
            switch (position)
            {
                case 1:
                    P1Cat.Text = captions[0];
                    P1Id.Text = pdt.id.ToString();
                    break;
                case 2:
                    P2Cat.Text = captions[0];
                    P2Id.Text = pdt.id.ToString();
                    break;
                case 3:
                    P3Cat.Text = captions[0];
                    P3Id.Text = pdt.id.ToString();
                    break;
            }

            foreach (string caption in captions)
            {
                HyperLink cat = new HyperLink();
                cat.CssClass = "caption-link";
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                cat.Text = textInfo.ToTitleCase(caption);
                cat.NavigateUrl = "javascript:GetProductsByCategory(" + this.userId + "," + contestId + "," + position + ",'" + caption + "');";
                favCaptionCarousel.Controls.Add(cat);
            }
            Favorites.Controls.Add(favCaptionCarousel);
            Panel colorPicker = new ColorPickerPanel(user.userId, this.contestId, position);
            Favorites.Controls.Add(colorPicker);
            

            Panel favPanel = new Panel();
            favPanel.CssClass = "carousel";
            favPanel.Controls.Add(new Literal() { Text = "<div class=\""+ prevCarouselClass +"\"><img src=\"images/carousel_left.png\" alt=\"Previous\"></div>"+
		                                                 "<div class=\""+ nextCarouselClass +"\"><img src=\"images/carousel_right.png\" alt=\"Next\"></div>" });

            Panel favCarousel = new Panel();
            favCarousel.CssClass = carouselClass;
            favPanel.Controls.Add(favCarousel);

            int carouselPosition = 1;
            //Add the item to the top slider 
            HtmlGenericControl slider = (HtmlGenericControl)Page.Master.FindControl("MainContent").FindControl(sliderId);

            if (pdt.id != 0)
            {
                CreateSliderAndFav(pdt, position, itemCssClass, carouselPosition, ref favCarousel, ref slider, sliderId);
                slider.Style.Add("left", position == 1 ? "-336px" : "-175px");
                carouselPosition++;
            }
                
            foreach (ShopSenseDemo.Product love in favorite.Value)
            {
                if (pdt !=null && pdt.id == love.id)
                    continue;

                CreateSliderAndFav(love, position, itemCssClass, carouselPosition, ref favCarousel, ref slider, sliderId);
                
                carouselPosition++;
            }
            
            position++;

            Favorites.Controls.Add(favPanel);
        }
        Favorites.Controls.Add(new Literal() { Text = "<br/><br/><hr/>" });

    }
}