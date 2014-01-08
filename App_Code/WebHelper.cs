using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using ShopSenseDemo;
using System.Drawing;
using System.Net;
using System.Drawing.Drawing2D;

/// <summary>
/// Summary description for WebHelper
/// </summary>
public class WebHelper
{
    public static int Hash32(string word)
    {
        byte[] buf = Encoding.UTF8.GetBytes(word);
        int h = 0;

        for (int i = 0; i < buf.Length; i++)
        {
            int c = buf[i];
            h ^= ((h << 5) + c + (h >> 2));
        }
        return h;
    }

    public static string CreateAuthString(long userId)
    {
        return WebHelper.Hash32(userId.ToString()).ToString();
    }

    public static bool IsAuthStringValid(long userId, string cookie)
    {
        return (WebHelper.CreateAuthString(userId) == cookie);
    }

    public static string GetIpAddress(HttpRequest request)
    {
        string strIpAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (strIpAddress == null)
            strIpAddress = request.ServerVariables["REMOTE_ADDR"];

        return strIpAddress;
    }

    public static Dictionary<string, string> DecodePayload(string payload)
    {
        //Remove the bad part of signed_request
        //Begin
        string[] sB64String = payload.Split('.');
        payload = payload.Replace((sB64String[0] + "."), string.Empty);
        //End
        var encoding = new UTF8Encoding();
        var decodedJson = payload.Replace("=", string.Empty).Replace('-', '+').Replace('_', '/');
        var base64JsonArray = Convert.FromBase64String(decodedJson.PadRight(decodedJson.Length + (4 - decodedJson.Length % 4) % 4, '='));
        var json = encoding.GetString(base64JsonArray);
        var jObject = JObject.Parse(json);
        var parameters = new Dictionary<string, string>();
        parameters.Add("user_id", (string)jObject["user_id"] ?? "");
        parameters.Add("oauth_token", (string)jObject["oauth_token"] ?? "");
        var expires = ((long?)jObject["expires"] ?? 0);
        parameters.Add("expires", expires > 0 ? expires.ToString() : "");
        parameters.Add("profile_id", (string)jObject["profile_id"] ?? "");
        return parameters;
    }


    public static string RenderHtml(WebControl control)
    {
        StringBuilder content = new StringBuilder();
        StringWriter sWriter = new StringWriter(content);
        HtmlTextWriter htmlWriter = new HtmlTextWriter(sWriter);
        control.RenderControl(htmlWriter);
        return content.ToString();
    }

    public static System.Drawing.Image RoundCorners(System.Drawing.Image StartImage, int CornerRadius)
    {
        CornerRadius *= 2;
        Bitmap RoundedImage = new Bitmap(StartImage.Width, StartImage.Height);
        Graphics g = Graphics.FromImage(RoundedImage);
        g.Clear(System.Drawing.Color.White);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Brush brush = new TextureBrush(StartImage);
        GraphicsPath gp = new GraphicsPath();
        gp.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90);
        gp.AddArc(0 + RoundedImage.Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90);
        gp.AddArc(0 + RoundedImage.Width - CornerRadius, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
        gp.AddArc(0, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
        g.FillPath(brush, gp);
        return RoundedImage;
    }
    
    public static System.Drawing.Image AppendBorder(System.Drawing.Image original, int borderWidth)
    {
        
        var newSize = new System.Drawing.Size(
            original.Width + borderWidth * 2,
            original.Height + borderWidth * 2);

        var img = new Bitmap(newSize.Width, newSize.Height);
        var g = Graphics.FromImage(img);

        System.Drawing.Color borderColor = System.Drawing.Color.FromArgb(198, 192, 186);
        g.Clear(borderColor);
        g.DrawImage(original, new Point(borderWidth, borderWidth));
        g.Dispose();

        return img;
    }
    
    public static void MergeTwoImages(string firstImageUrl, string secondImageUrl, string imageFilePath)
    {
        System.Drawing.Image firstImage, secondImage;
        var request = WebRequest.Create(firstImageUrl);

        using (var response = request.GetResponse())
        using (var stream = response.GetResponseStream())
        {
            firstImage = System.Drawing.Image.FromStream(stream);
            firstImage = RoundCorners(firstImage, 10);

            request = WebRequest.Create(secondImageUrl);
            using (var response2 = request.GetResponse())
            using (var stream2 = response2.GetResponseStream())
            {
                secondImage = System.Drawing.Image.FromStream(stream2);
                secondImage = RoundCorners(secondImage, 10);

                if (firstImage == null)
                {
                    throw new ArgumentNullException("firstImage");
                }

                if (secondImage == null)
                {
                    throw new ArgumentNullException("secondImage");
                }

                Bitmap bitmap = new Bitmap(firstImage.Width + secondImage.Width + 100, Math.Max(firstImage.Height, secondImage.Height));
                //bitmap.MakeTransparent();
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
                    g.DrawImage(firstImage, 0, 0);
                    g.DrawImage(secondImage, firstImage.Width+50, 0);
                }

                bitmap.Save(imageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
        
    }

    public static void MergeThreeImages(string firstImageUrl, string secondImageUrl, string thirdImageUrl, string imageFilePath)
    {
        System.Drawing.Image firstImage, secondImage, thirdImage;
        var request = WebRequest.Create(firstImageUrl);
        int cornerRadius = 30;

        using (var response = request.GetResponse())
        using (var stream = response.GetResponseStream())
        {
            firstImage = System.Drawing.Image.FromStream(stream);
            firstImage = RoundCorners(firstImage, cornerRadius);
            
            request = WebRequest.Create(secondImageUrl);
            using (var response2 = request.GetResponse())
            using (var stream2 = response2.GetResponseStream())
            {
                secondImage = System.Drawing.Image.FromStream(stream2);
                secondImage = RoundCorners(secondImage, cornerRadius);

                request = WebRequest.Create(thirdImageUrl);
                using (var response3 = request.GetResponse())
                using (var stream3 = response3.GetResponseStream())
                {
                    thirdImage = System.Drawing.Image.FromStream(stream3);
                    thirdImage = RoundCorners(thirdImage, cornerRadius);

                    if (firstImage == null)
                    {
                        throw new ArgumentNullException("firstImage");
                    }

                    if (secondImage == null)
                    {
                        throw new ArgumentNullException("secondImage");
                    }
                    if (thirdImage == null)
                    {
                        throw new ArgumentNullException("thirdImage");
                    }

                    Bitmap bitmap = new Bitmap(firstImage.Width + secondImage.Width + 32, Math.Max(firstImage.Height, secondImage.Height + thirdImage.Height) + 20);
                    
                    //bitmap.MakeTransparent();
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
                        bitmap = new Bitmap(AppendBorder(bitmap, 8));
                        //bitmap = new Bitmap(RoundCorners(bitmap, cornerRadius));
                        
                    }
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.DrawImage(firstImage, 16, 18);
                        g.DrawImage(secondImage, firstImage.Width + 24, 16);
                        g.DrawImage(thirdImage, firstImage.Width + 24, secondImage.Height + 24);
                    }

                    bitmap.Save(imageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }

    }

    public static Panel GetProductPanel(string position, Product product,bool isContentPanel = false, bool isLovePanel=false, bool isProductLoved=false, bool isPricePanel = false)
    {
        Panel productPanel = new Panel();
        bool bigImage = true;
        string panelCss = string.Empty, productImageCss ="product-image", lovePanelId = string.Empty, loveLabelId = string.Empty, loveImageId= string.Empty;
        switch (position)
        {
            case "left":
                panelCss = "left-item-wrapper";
                lovePanelId = "MainContent_P1LoveButton";
                loveLabelId = "MainContent_P1Love";
                loveImageId = "MainContent_P1LoveImg";
                break;
            case "left3Item":
                panelCss = "left-item-wrapper-more";
                lovePanelId = "MainContent_P1LoveButton";
                loveLabelId = "MainContent_P1Love";
                loveImageId = "MainContent_P1LoveImg";
                break;
            case "right":
                panelCss = "right-item-wrapper";
                lovePanelId = "MainContent_P2LoveButton";
                loveLabelId = "MainContent_P2Love";
                loveImageId = "MainContent_P2LoveImg";
                break;
            case "rightUpperSmall":
                panelCss = "";
                bigImage = false;
                lovePanelId = "MainContent_P2LoveButton";
                loveLabelId = "MainContent_P2Love";
                loveImageId = "MainContent_P2LoveImg";
                productImageCss = "product-small-image";
                break;
            case "rightLowerSmall":
                panelCss = "";
                bigImage = false;
                lovePanelId = "MainContent_P3LoveButton";
                loveLabelId = "MainContent_P3Love";
                loveImageId = "MainContent_P3LoveImg";
                productImageCss = "product-small-image";
                break;
        }

        productPanel.CssClass = panelCss;

        ProductHyperLink productLink = new ProductHyperLink(product);
        productPanel.Controls.Add(productLink);

        Panel productImage = new Panel();
        productImage.CssClass = productImageCss;
        System.Web.UI.WebControls.Image fav = new System.Web.UI.WebControls.Image();
        productImage.Controls.Add(fav);
        fav.ImageUrl = bigImage == true ? product.GetImageUrl() : product.GetNormalImageUrl();
        
        fav.AlternateText = product.name;
        productLink.Controls.Add(productImage);

        //if lovepanel needs to be included
        if (isLovePanel)
        {
            Panel lovePanel = new Panel();
            lovePanel.ID = lovePanelId;
            Label loveCount = new Label();
            loveCount.ID = loveLabelId;
            System.Web.UI.WebControls.Image loveImg = new System.Web.UI.WebControls.Image();
            loveImg.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
            loveImg.Style.Add(HtmlTextWriterStyle.Width, "24px");
            loveImg.Style.Add(HtmlTextWriterStyle.MarginLeft, "8px");
            loveImg.ID = loveImageId;
            loveImg.ToolTip = "Love It";
            loveImg.AlternateText = "Love It";
            HyperLink buylink = new HyperLink();
            buylink.Text = "Buy";
            buylink.Target = "_blank";
            buylink.NavigateUrl = product.AffiliateUrl;
            buylink.CssClass = "buylink";

            lovePanel.Controls.Add(loveCount);
            lovePanel.Controls.Add(loveImg);
            lovePanel.Controls.Add(buylink);

            productPanel.Controls.Add(lovePanel);

            loveCount.Text = product.loves.ToString();
            if (isProductLoved)
            {
                lovePanel.CssClass = "text-heart-red";
                loveImg.ImageUrl = "images/heart_filled.jpg";
            }
            else
            {
                lovePanel.CssClass = "text-heart";
                loveImg.ImageUrl = "images/heart_empty.jpg";
            }
        }

        if (isContentPanel)
        {
            Panel productContentPanel = new Panel();
            productContentPanel.CssClass = "item-content";

            //ProductHyperLink brandTitle = new ProductHyperLink(product);
            //brandTitle.CssClass += " brandTitle";
            //brandTitle.Text = product.GetBrandName();

            //productContentPanel.Controls.Add(brandTitle);

            Panel productTitlePanel = new Panel();
            ProductHyperLink pdtTitle = new ProductHyperLink(product);
            pdtTitle.CssClass += " pdtTitle";
            pdtTitle.Text = product.GetName();
            productTitlePanel.Controls.Add(pdtTitle);
            productContentPanel.Controls.Add(productTitlePanel);

            productPanel.Controls.Add(productContentPanel);
        }

        if (isPricePanel)
        {
            Panel productPrice = new Panel();
            productPrice.CssClass = "price-content";
            Label price = new Label();
            price.CssClass = "inline";
            price.Text = string.Format("{0:c}", product.price);
            productPrice.Controls.Add(price);
            if (product.salePrice != 0)
            {
                price.CssClass += " strike-through";
                Label salePrice = new Label();
                salePrice.CssClass = "inline";
                salePrice.Text = string.Format("{0:c}", product.salePrice);
                productPrice.Controls.Add(salePrice);
            }
            productPanel.Controls.Add(productPrice);
        }

        return productPanel;
    }

    //Dominant hex color
    public static System.Drawing.Color getDominantColor(Bitmap bmp)
    {
        //Used for tally
        int r = 0;
        int g = 0;
        int b = 0;

        int total = 0;

        for (int x = 0; x < bmp.Width; x++)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                System.Drawing.Color clr = bmp.GetPixel(x, y);

                r += clr.R;
                g += clr.G;
                b += clr.B;

                total++;
            }
        }

        //Calculate average
        r /= total;
        g /= total;
        b /= total;

        return System.Drawing.Color.FromArgb(r, g, b);
    }

    public static void CreateLookPanel(Look look, string imageFilePath)
    {
        List<System.Drawing.Image> lookImages = new List<System.Drawing.Image>();
        List<System.Drawing.Image> swatchImages = new List<System.Drawing.Image>(); 
        //int cornerRadius = 30;
        
        for(int i=0; i< look.products.Count(); i++)
        {
            string imageUrl = look.products[i].isCover ? look.products[i].GetImageUrl() : look.products[i].GetThumbnailUrl();
            var request = WebRequest.Create(imageUrl);
        
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                lookImages.Add(System.Drawing.Image.FromStream(stream));
            }

            if (!string.IsNullOrEmpty(look.products[i].swatchUrl))
            {
                var sRequest = WebRequest.Create(look.products[i].swatchUrl);

                using (var response = sRequest.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    swatchImages.Add(System.Drawing.Image.FromStream(stream));
                }
            }
            else
            {
                //Bitmap bit = new Bitmap(1,1);
                //using (Graphics g = Graphics.FromImage(bit))
                //{
                //    Brush brush = GetColorBrush(look.products[i].GetColor());

                //    g.FillRectangle(brush, 0, 0, bit.Width, bit.Height);
                //}

                //swatchImages.Add(bit);
                var sRequest = WebRequest.Create(look.products[i].GetThumbnailUrl());

                using (var response = sRequest.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    swatchImages.Add(System.Drawing.Image.FromStream(stream));
                }
            }
        }

        Bitmap bitmap = new Bitmap(lookImages[0].Width + lookImages[1].Width + 32, Math.Max(lookImages[0].Height, lookImages[1].Height + lookImages[2].Height) + 20);

        //bitmap.MakeTransparent();
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
            //bitmap = new Bitmap(AppendBorder(bitmap, 8));
            //bitmap = new Bitmap(RoundCorners(bitmap, cornerRadius));

            //Add the color bar
            int colorBarHeight = (bitmap.Height - 5*look.products.Count()) / look.products.Count();
            int colorBarWidth = 12;
            for (int i = 0; i < swatchImages.Count(); i++)
            {
                Bitmap bits = new Bitmap(swatchImages[i]);
                SolidBrush brush = new SolidBrush(getDominantColor(bits));
                g.FillRectangle(brush, 4, 5+ i * (colorBarHeight + 5), colorBarWidth, colorBarHeight);
            }
            

        }
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.DrawImage(lookImages[0], 16, 18);
            g.DrawImage(lookImages[1], lookImages[0].Width + 24, 16);
            g.DrawImage(lookImages[2], lookImages[0].Width + 24, lookImages[1].Height + 24);
        }

        bitmap.Save(imageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);

    }

    public static Brush GetColorBrush(string colorId)
    {
        switch (colorId)
        {
            case "Brown":
                return Brushes.Brown;
            case "Orange":
                return Brushes.Orange;
            case "Yellow":
                return Brushes.Yellow;
            case "Red":
                return Brushes.Red;
            case "Purple":
                return Brushes.Purple;
            case "Blue":
                return Brushes.Blue;
            case "Green":
                return Brushes.Green;
            case "Gray":
                return Brushes.Gray;
            case "White":
                return Brushes.White;
            case "Black":
                return Brushes.Black;
            case "Pink":
                return Brushes.Pink;
            case "Gold":
                return Brushes.Gold;
            case "Silver":
                return Brushes.Silver;
            case "Beige":
                return Brushes.Beige;
            default:
                return Brushes.AntiqueWhite;
        }
    }
}