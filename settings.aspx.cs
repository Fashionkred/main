using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShopSenseDemo;
using System.Configuration;

public partial class settings : BasePage
{
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

        System.Web.UI.WebControls.Image userImage = (System.Web.UI.WebControls.Image)this.Master.FindControl("UserImage");
        userImage.ImageUrl = user.pic;
        HyperLink userName = (HyperLink)this.Master.FindControl("UserName");
        userName.Text = user.name;
        userName.NavigateUrl = "user.aspx?uid=" + user.id;

        Label userPoints = (Label)this.Master.FindControl("UserPoints");
        userPoints.Text = user.points.ToString() + " votes";

        Panel ContestBar = (Panel)this.Master.FindControl("ContestBar");
        ContestBar.Visible = false;

        if (!Page.IsPostBack)
        {
            Private.Checked = this.user.IsPrivate;
        }

    }

    protected void Update_Click(object sender, EventArgs e)
    {
        bool isPrivate = this.user.IsPrivate ? true : false;
        
        //check if current option different from user's settings
        if (isPrivate ^ Private.Checked)
        {
            //toggle settings and save the user's setting in session
            if (user.IsPrivate)
            {
                user.userFlags &= ~UserFlags.PrivateSharing;
            }
            else
            {
                user.userFlags |= UserFlags.PrivateSharing;
            }

            this.Session["user"] = user;

            string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            UserProfile.SaveOrUpdateUser(user, db);
        }

    }
}