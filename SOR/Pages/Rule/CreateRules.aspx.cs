using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOR.Pages.Rule
{
    public partial class CreateRules : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Simulating data source (you can replace it with your data)
                var dataSource = new List<dynamic>
        {
            new { ItemName = "Item 1", IsChecked = false },
            new { ItemName = "Item 2", IsChecked = true }
        };

                // Bind the data to the Repeater
                //Repeater1.DataSource = dataSource;
                //Repeater1.DataBind();
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Get the checkbox control that triggered the event
            CheckBox checkBox = sender as CheckBox;

            // Get the RepeaterItem that contains the checkbox
            RepeaterItem item = (RepeaterItem)checkBox.NamingContainer;

            // Optionally, you can find other controls in the same row
            Label label = item.FindControl("Label1") as Label;

            // Logic based on checkbox state
            if (checkBox.Checked)
            {
                // Do something when checked
                label.Text += " - Checked!";
            }
            else
            {
                // Do something when unchecked
                label.Text += " - Unchecked!";
            }
        }
    }
}