using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_Encoder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnEncode_Click(object sender, EventArgs e)
    {
        Label1.Text = Boomers.Utilities.Guids.Encoder.Encode(new Guid( TextBox1.Text));
    }
    protected void btnDecode_Click(object sender, EventArgs e)
    {
        Label1.Text = Boomers.Utilities.Guids.Encoder.Decode(TextBox1.Text).ToString();
    }
}