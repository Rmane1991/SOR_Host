using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Threading;
using BussinessAccessLayer;
using System.Text;

namespace SOR
{
    public partial class SOR : System.Web.UI.MasterPage
    {
        DataSet _dsAutoLoad = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //int timeout1 = Convert.ToInt32("0") * 60;
            //int seconds = timeout1 / 60;

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SessionAlert", "startCountdown(" + timeout1 + ");", true);

            if (Session["UserID"] != null && Session["Username"] != null)
            {
                lblWelcome.Text = "Welcome " + Session["Username"];
                if (!Page.IsPostBack)
                {
                    AutoLoadMenus();
                    AutoLoadMenus2();
                }
            }
            else
            {
                try
                {
                    Response.Redirect(ConfigurationManager.AppSettings["RedirectToLogin"].ToString(), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (ThreadAbortException)
                {
                }
            }
        }
        AppSecurity appSecurity = null;
        LoginEntity _LoginEntity = new LoginEntity();
        string[] _auditParams = new string[3];
        string[] _attemptAction = new string[2];

        #region Logout
        protected void LbLoginStatus_Click(object sender, EventArgs e)
        {
            try
            {

                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                }
                if (Request.Cookies["AuthToken"] != null)
                {
                    Response.Cookies["AuthToken"].Value = string.Empty;
                    Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                }
                try
                {
                    Response.Redirect(ConfigurationManager.AppSettings["RedirectToLogin"].ToString(), false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (ThreadAbortException)
                {
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        protected void btnchange_ServerClick(object sender, EventArgs e)
        {
            //Response.Redirect(ConfigurationManager.AppSettings["RedirectToPassChange"].ToString(), false);
        }

        protected void btnProfile_ServerClick(object sender, EventArgs e)
        {
            // Response.Redirect(ConfigurationManager.AppSettings["RedirectToProfile"].ToString(), false);
        }

        #region Auto Load Menus
        private void AutoLoadMenus()
        {
            try
            {
                string strTags = string.Empty;
                _LoginEntity.RoleID = Convert.ToInt32(Session["UserRoleID"]);
                _LoginEntity.CurrentPage = this.Page.Request.FilePath;
                _LoginEntity.ParentRoleID = Convert.ToInt32(Session["ParentRoleID"]);
                _LoginEntity.UserName = Convert.ToString(Session["Username"]);
                _dsAutoLoad = _LoginEntity.GetMenuListData();
                string _oldMenu = string.Empty;
                bool _isSubMenu = false;
                if (_dsAutoLoad != null && _dsAutoLoad.Tables.Count > 0 && _dsAutoLoad.Tables[0].Rows.Count > 0)
                {
                    //foreach (DataRow drMenus in _dsAutoLoad.Tables[1].Rows)
                    //{
                    //    strTags = drMenus["Home"].ToString().Replace("Pages", "../../Pages");
                    //}
                    foreach (DataRow drMenus in _dsAutoLoad.Tables[0].Rows)
                    {
                        if (!_oldMenu.Equals(Convert.ToString(drMenus[1])) && _isSubMenu) //--< Closing Submenu
                        {
                            strTags += @"</ul> </div> </div> </div>";
                            _isSubMenu = false;
                        }
                        if (!_oldMenu.Equals(Convert.ToString(drMenus[1])) && string.IsNullOrEmpty(Convert.ToString(drMenus[3]))) //---< New main menu
                        {
                            strTags += @" <li><a title=""" + Convert.ToString(drMenus[1]) + @""" href=""" + Convert.ToString(drMenus[2]) + @""" id=""lbtn" + Convert.ToString(drMenus[7]) + @"""><i class=""fa fa-home""></i> " + Convert.ToString(drMenus[1]) + @"</a></li>";
                            _oldMenu = string.Empty;
                            _isSubMenu = false;
                        }
                        else
                        {
                            if (_oldMenu.Equals(Convert.ToString(drMenus[1]))) //--< Appending additional submenu
                            {
                                string url = Convert.ToString(drMenus[4]);
                                strTags += @"<li class=""sub-menu-list"" id=""subm" + Convert.ToString(drMenus[6]) + @"""><a href=""" + Convert.ToString(drMenus[4]) + @"""> <div class=""d-flex align-items-center""> <span class=""icon-list-line""> </span> <span class=""list-line-text"">" + Convert.ToString(drMenus[3]) + @"</span> </div> </a> </li>";
                            }
                            else
                            {
                                string url = Convert.ToString(drMenus[4]);
                                strTags += @"<div class=""accordion-item""><h2 class=""accordion-header"" id=""heading" + Convert.ToString(drMenus[0]) + @"""><button class=""accordion-button collapsed"" id=""BtnSub" + Convert.ToString(drMenus[0]) + @""" type=""button"" data-bs-toggle=""collapse"" data-bs-target=""#subPages" + Convert.ToString(drMenus[0]) + @""" aria-expanded=""true"" aria-controls=""subPages" + Convert.ToString(drMenus[0]) + @"""><div class=""sidebar-headings""> <span class=""" + Convert.ToString(drMenus[8]) + @"""> </span> <h5 class=""sidebar-headings""> " + Convert.ToString(drMenus[1]) + @"</h5></div></button></h2><div id = ""subPages" + Convert.ToString(drMenus[0]) + @""" class=""accordion-collapse collapse"" aria-labelledby=""heading" + Convert.ToString(drMenus[0]) + @""" data-bs-parent=""#accordionExample2""> <div class=""accordion-body""> <ul class=""sub-menu""> <!-- sub-menu-list --><li class=""sub-menu-list"" id=""subm" + Convert.ToString(drMenus[6]) + @""">  <a href = """ + Convert.ToString(drMenus[4]) + @"""> <div class=""d-flex align-items-center""> <span class=""icon-list-line""> </span> <span class=""list-line-text""> " + Convert.ToString(drMenus[3]) + @"</span> </div> </a> </li>";
                                //              strTags += @"<li> <a title=""" + Convert.ToString(drMenus[1]) + @""" href=""#subPages" + Convert.ToString(drMenus[0]) + @""" data-toggle=""collapse"" class=""collapsed"" id=""hrefsubpg" + Convert.ToString(drMenus[0]) + @"""><i class=""""></i> " + Convert.ToString(drMenus[1]) + @" <i class=""icon-submenu fa fa-angle-left pull-right""></i></a> 
                                //<div id=""subPages" + Convert.ToString(drMenus[0]) + @""" class=""collapse ""> <ul class=""nav""><li><a href=""" + Convert.ToString(drMenus[4]) + @""" id=""lbtn" + Convert.ToString(drMenus[7]) + @"""><i class=""fa fa-arrow-right""></i> " + Convert.ToString(drMenus[3]) + @"</a></li>";
                                //          }
                            }
                            _isSubMenu = true;
                        }
                        _oldMenu = Convert.ToString(drMenus[1]);
                    }
                    strTags += @"</ul> </div> </div> </div>";
                    accordionExamplesd.Text = strTags.ToString();
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        #region Auto Load Menus
        private void AutoLoadMenus2()
        {
            try
            {
                string strTagsOpen = string.Empty;   // For the open sidebar
                string strTagsClosed = string.Empty; // For the closed sidebar (only icons)

                _LoginEntity.RoleID = Convert.ToInt32(Session["UserRoleID"]);
                _LoginEntity.CurrentPage = this.Page.Request.FilePath;
                _LoginEntity.ParentRoleID = Convert.ToInt32(Session["ParentRoleID"]);
                _LoginEntity.UserName = Convert.ToString(Session["Username"]);
                _dsAutoLoad = _LoginEntity.GetMenuListData();

                string _oldMenu = string.Empty;
                bool _isSubMenu = false;

                if (_dsAutoLoad != null && _dsAutoLoad.Tables.Count > 0 && _dsAutoLoad.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drMenus in _dsAutoLoad.Tables[0].Rows)
                    {
                        // Handle open sidebar (full menu with submenus)
                        if (!_oldMenu.Equals(Convert.ToString(drMenus[1])) && _isSubMenu)
                        {
                            strTagsOpen += @"</ul> </div> </div> </div>";
                            _isSubMenu = false;
                        }

                        // Handle new main menu for open sidebar
                        if (!_oldMenu.Equals(Convert.ToString(drMenus[1])) && string.IsNullOrEmpty(Convert.ToString(drMenus[3])))
                        {
                            // For open sidebar (full menu with text and icons)
                            strTagsOpen += @" <li><a title=""" + Convert.ToString(drMenus[1]) + @""" href=""" + Convert.ToString(drMenus[2]) + @""" id=""lbtn" + Convert.ToString(drMenus[7]) + @"""><i class=""fa fa-home""></i> " + Convert.ToString(drMenus[1]) + @"</a></li>";
                            _oldMenu = string.Empty;
                            _isSubMenu = false;
                        }
                        else
                        {
                            if (_oldMenu.Equals(Convert.ToString(drMenus[1])))
                            {
                                string url = Convert.ToString(drMenus[4]);
                                strTagsOpen += @"<li class=""sub-menu-list"" id=""subm" + Convert.ToString(drMenus[6]) + @"""><a href=""" + Convert.ToString(drMenus[4]) + @"""> <div class=""d-flex align-items-center""> <span class=""icon-list-line""> </span> <span class=""list-line-text"">" + Convert.ToString(drMenus[3]) + @"</span> </div> </a> </li>";
                            }
                            else
                            {
                                // Opening new accordion menu for open sidebar
                                string url = Convert.ToString(drMenus[4]);
                                strTagsOpen += @"<div class=""accordion-item""><h2 class=""accordion-header"" id=""heading" + Convert.ToString(drMenus[0]) + @"""><button class=""accordion-button collapsed"" id=""BtnSub" + Convert.ToString(drMenus[0]) + @""" type=""button"" data-bs-toggle=""collapse"" data-bs-target=""#subPages" + Convert.ToString(drMenus[0]) + @""" aria-expanded=""true"" aria-controls=""subPages" + Convert.ToString(drMenus[0]) + @"""><div class=""sidebar-headings""> <span class=""" + Convert.ToString(drMenus[8]) + @"""> </span> <h5 class=""sidebar-headings""> " + Convert.ToString(drMenus[1]) + @"</h5></div></button></h2><div id = ""subPages" + Convert.ToString(drMenus[0]) + @""" class=""accordion-collapse collapse"" aria-labelledby=""heading" + Convert.ToString(drMenus[0]) + @""" data-bs-parent=""#accordionExample2""> <div class=""accordion-body""> <ul class=""sub-menu"">";
                            }
                        }

                        // Handle closed sidebar (only main menu items and icons)
                        if (string.IsNullOrEmpty(Convert.ToString(drMenus[3]))) // Main menu item with no submenus
                        {
                            // For the closed sidebar (only icons)
                            strTagsClosed += @"<li><a href=""" + Convert.ToString(drMenus[2]) + @""" id=""lbtn" + Convert.ToString(drMenus[7]) + @"""><i class=""" + Convert.ToString(drMenus[8]) + @"""></i></a></li>";
                        }

                        _isSubMenu = true;
                        _oldMenu = Convert.ToString(drMenus[1]);
                    }

                    // Close any remaining open submenu for the open sidebar
                    strTagsOpen += @"</ul> </div> </div> </div>";

                    // Bind the dynamically generated HTML for the open sidebar (Literal1)
                    Literal1.Text = strTagsOpen.ToString();

                    // Bind the dynamically generated HTML for the closed sidebar (Literal2 - only icons)
                    Literal2.Text = strTagsClosed.ToString();
                }

            }
            catch (Exception Ex)
            {
                // Handle exceptions (optional)
                // Log or handle the error appropriately
                // For example: Logger.LogError(Ex.Message, Ex);
            }

        }

        //private void AutoLoadMenus2()
        //{
        //    try
        //    {
        //        string strTags = string.Empty;
        //        _LoginEntity.RoleID = Convert.ToInt32(Session["UserRoleID"]);
        //        _LoginEntity.CurrentPage = this.Page.Request.FilePath;
        //        _LoginEntity.ParentRoleID = Convert.ToInt32(Session["ParentRoleID"]);
        //        _LoginEntity.UserName = Convert.ToString(Session["Username"]);
        //        _dsAutoLoad = _LoginEntity.GetMenuListData();
        //        string _oldMenu = string.Empty;
        //        bool _isSubMenu = false;

        //        if (_dsAutoLoad != null && _dsAutoLoad.Tables.Count > 0 && _dsAutoLoad.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow drMenus in _dsAutoLoad.Tables[0].Rows)
        //            {
        //                string iconClass = string.IsNullOrEmpty(Convert.ToString(drMenus[8])) ? "fa fa-home" : Convert.ToString(drMenus[8]);

        //                // Closing submenu when switching to a new main menu
        //                if (!_oldMenu.Equals(Convert.ToString(drMenus[1])) && _isSubMenu)
        //                {
        //                    strTags += @"</ul> </div> </div> </div>";
        //                    _isSubMenu = false;
        //                }

        //                // New main menu (no submenus)
        //                if (!_oldMenu.Equals(Convert.ToString(drMenus[1])) && string.IsNullOrEmpty(Convert.ToString(drMenus[3])))
        //                {
        //                    strTags += $@"
        //                <li>
        //                    <a title=""{Convert.ToString(drMenus[1])}"" href=""{Convert.ToString(drMenus[2])}"" id=""lbtn{Convert.ToString(drMenus[7])}"">
        //                        <i class=""{iconClass}""></i> {Convert.ToString(drMenus[1])}
        //                    </a>
        //                </li>
        //            ";
        //                    _oldMenu = string.Empty;
        //                    _isSubMenu = false;
        //                }
        //                else
        //                {
        //                    // Appending submenu items
        //                    if (_oldMenu.Equals(Convert.ToString(drMenus[1])))
        //                    {
        //                        strTags += $@"
        //                    <li class=""sub-menu-list"" id=""submm{Convert.ToString(drMenus[6])}"">
        //                        <a href=""{Convert.ToString(drMenus[4])}"">
        //                            <div class=""d-flex align-items-center"">
        //                                <span class=""{iconClass}""></span> <!-- Dynamic icon -->
        //                                <span class=""list-line-text""> {Convert.ToString(drMenus[3])}</span>
        //                            </div>
        //                        </a>
        //                    </li>
        //                ";
        //                    }
        //                    else
        //                    {
        //                        // Starting a new accordion section for a menu
        //                        strTags += $@"
        //                    <div class=""accordion-item"">
        //                        <h2 class=""accordion-header"" id=""heading{Convert.ToString(drMenus[0])}"">
        //                            <button class=""accordion-button collapsed"" id=""BtnSubb{Convert.ToString(drMenus[0])}"" 
        //                                    type=""button"" data-bs-toggle=""collapse"" 
        //                                    data-bs-target=""#subPagess{Convert.ToString(drMenus[0])}"" 
        //                                    aria-expanded=""true"" aria-controls=""subPagess{Convert.ToString(drMenus[0])}"">
        //                                <div class=""sidebar-headings"">
        //                                    <span class=""{iconClass}""></span> <!-- Dynamic icon -->
        //                                    <h5 class=""sidebar-headings""> {Convert.ToString(drMenus[1])}</h5>
        //                                </div>
        //                            </button>
        //                        </h2>
        //                        <div id=""subPagess{Convert.ToString(drMenus[0])}"" class=""accordion-collapse collapse"" 
        //                             aria-labelledby=""heading{Convert.ToString(drMenus[0])}"" data-bs-parent=""#accordionExample"">
        //                            <div class=""accordion-body"">
        //                                <ul class=""sub-menu"">
        //                ";

        //                        _isSubMenu = true;
        //                    }
        //                }

        //                _oldMenu = Convert.ToString(drMenus[1]);
        //            }

        //            // Closing the last opened submenu
        //            strTags += @"</ul> </div> </div> </div>";

        //            // Output the result to the Literal control
        //            Literal2.Text = strTags.ToString();
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        // Handle any exceptions
        //        // Log or display an error message as needed
        //    }
        //}

        #endregion
    }
    }