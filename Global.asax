<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin","*");
    }

    protected void Application_AcquireRequestState(object sender, EventArgs e)
    {
        string db = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        // create userprofile
        if (HttpContext.Current.Session!= null && HttpContext.Current.Session["user"] == null
            && this.Request.Cookies["__userid"] != null && this.Request.Cookies["__auth"] != null)
        {
            long userId = long.Parse(this.Request.Cookies["__userid"].Value);

            if (WebHelper.IsAuthStringValid(userId, this.Request.Cookies["__auth"].Value))
            {
                ShopSenseDemo.UserProfile userProfile = null;

                userProfile = ShopSenseDemo.UserProfile.GetUserProfileById(userId, db);

                if (userProfile != null)
                {
                    if (!string.IsNullOrEmpty(userProfile.accessToken))
                    {
                        bool accessTokenValid = ShopSenseDemo.FacebookHelper.UpdateExistingUser(userProfile.accessToken, db);
                        //bool accessTokenValid = true;
                        
                        if (accessTokenValid)
                        {
                            HttpContext.Current.Session["access_token"] = userProfile.accessToken;
                            HttpContext.Current.Session["user"] = userProfile;
                        }
                    }
                }
            }
            else
            {
                // expire cookies
                if (this.Request.Cookies["__userid"] != null)
                {
                    this.Response.Cookies["__userid"].Expires = DateTime.Now.AddDays(-1);
                }

                if (this.Request.Cookies["__auth"] != null)
                {
                    this.Response.Cookies["__auth"].Expires = DateTime.Now.AddDays(-1);
                }
            }
        }
    }

    
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
