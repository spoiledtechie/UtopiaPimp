using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        TextBox1.Text = TestingCache().yoyo.ToString();
        var item = TestingCache().yoyo + 10;
        TextBox1.Text = TextBox1.Text + ":" + TestingCache().yoyo.ToString();
                HttpContext.Current.Cache.Add("testing", item, null, DateTime.Now.AddDays(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
    }

    private static testing TestingCache()
    {
        var test = HttpContext.Current.Cache["testing"] as testing;
        if (test == null)
        {
            test = new testing();
                  test.yoyo = 0;
            HttpContext.Current.Cache.Add("testing", test, null, DateTime.Now.AddDays(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
        }
        else
        {
            test.yoyo += + 1;
        }
        return test;
    }
    protected void Unnamed1_Click(object sender, EventArgs e)
    {

        //TextBox1.Text = UtopiaParser.GetOwnedProvinceIDFromProvinceName(TextBox1.Text, 5, 10);
    }
}
public class testing
{
    public int yoyo;
}