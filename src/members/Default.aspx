<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="members_Default" %>

<%@ MasterType VirtualPath="~/main.master" %>
<%@ Register Src="../forum/controls/TopPosts.ascx" TagName="TopPosts" TagPrefix="uc1" %>
<%@ Register Src="../forum/controls/LatestPosts.ascx" TagName="LatestPosts" TagPrefix="uc2" %>
<%@ Register Src="../forum/controls/ActiveTopics.ascx" TagName="ActiveTopics" TagPrefix="uc3" %>
<%@ Register Src="../controls/reusable/Tumblr.ascx" TagName="Tumblr" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="watchRightAd">
        <div>
            <asp:Label runat="server" ID="lblWarning"></asp:Label>
        </div>
        <div class="pnlContent">
            <div class="pnls">
                <div class="pnldivHeaders">
                    Kingdom Summary
                </div>
                <div class="pnldivContent">
                    <div id="kingdomSum">
                        <script type="text/javascript">
                            $(document).ready(function () {
                                LoadKingdomSummary();
                            });
                        </script>
                        <center>
                            <img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..."
                                width="50px" /></center>
                    </div>
                </div>
            </div>
            <div class="pnls" id="divPnls" runat="server">
                <div class="pnldivHeaders">
                    Province Codes
                </div>
                <div class="pnldivContent">
                    <asp:Literal ID="ltProvinceCodes" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="pnls" id="divContactsNotSigned" runat="server">
                <div class="pnldivHeaders">
                    Provinces Who Haven't Posted Their Contact Info
                </div>
                <div class="pnldivContent">
                    <asp:Literal ID="ltContacts" runat="server"></asp:Literal>
                </div>
            </div>
           
        </div>
        <div class="pnlContent">
            <div class="pnls" id="pnlExtras" runat="server">
                <div class="pnldivHeaders">
                    Extras
                </div>
                <ul class="ulList ulListExtraPadding">
                    <li><span runat="server" id="chat" class="deleteButton"></span></li>
                    <li><span runat="server" id="spnChatKWide" class="deleteButton"></span></li>
                    <li>
                        <asp:Literal runat="server" ID="spnIRCChat"></asp:Literal></li>
                    <li>
                        <asp:HyperLink ID="HyperLink16" runat="server" NavigateUrl="~/Other/TargetFinder.aspx"
                            Target="_blank">Target Finder</asp:HyperLink>
                        <span runat="server" id="spTargetCounts"></span></li>
                    
                        <li><span class="st_twitter_hcount" st_url="http://utopia-game.com" displaytext="Tweet" st_title="Utopia - The Best FREE Online Strategy Warfare Game Ever! #games"></span><span st_url="http://utopia-game.com" class="st_facebook_hcount"  st_title="Utopia - The Best FREE Online Strategy Warfare Game Ever!"
                        displaytext="Share"></span> - Spread the word about <b>Utopia</b> EASILY!</li>
                        <li><span class="st_twitter_hcount" st_url="http://utopiapimp.com" displaytext="Tweet" st_title="An Awesome Tool for Utopia-game.com! #games"></span><span  st_title="An Awesome Tool for Utopia-game.com!" st_url="http://utopiapimp.com" class="st_facebook_hcount"
                        displaytext="Share"></span> - Spread the word about <b>UtopiaPimp</b> EASILY!</li>
                </ul>
            </div>
            <div class="pnls">
                <div class="pnldivHeaders">
                    <a href="http://blog.utopiapimp.com">UtopiaPimp Blog</a>
                    <asp:Literal ID="ltHeader" runat="server"></asp:Literal>
                </div>
                <div class="rss">
                    <a href="http://blog.utopiapimp.com/rss">
                        <img src="http://codingforcharity.org/utopiapimp/img/rss.png" />
                    </a>
                </div>
                <uc4:Tumblr ID="Tumblr1" runat="server" />
            </div>
            <div class="pnls">
                <div class="pnldivHeaders">
                    <a href="http://getsatisfaction.com/utopiapimp">FeedBack Updates</a>
                </div>
                <div id='gsfn_list_widget'>
                    <div id='gsfn_content'>
                        Loading...</div>
                </div>
            </div>
            <script src="http://getsatisfaction.com/utopiapimp/widgets/javascripts/5cc59d5e3d/widgets.js"
                type="text/javascript"></script>
            <script src="http://getsatisfaction.com/utopiapimp/topics.widget?callback=gsfnTopicsCallback&amp;limit=10&amp;sort=last_active_at"
                type="text/javascript"></script>
        </div>
    </div>
    <div id="dialog" title="Uniques">
    </div>
    <div class="divAdRight" id="divAdRight">
     
         <script type="text/javascript"><!--
             google_ad_client = "ca-pub-6494646249414123";
             /* PostSecretSkyScraper */
             google_ad_slot = "6985393558";
             google_ad_width = 160;
             google_ad_height = 600;
//-->
</script>
<script type="text/javascript"
src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
</script>

    </div>
    <script type="text/javascript" src="http://w.sharethis.com/button/buttons.js"></script>
    <script type="text/javascript">        stLight.options({ publisher: '44150a77-2fa9-4b5f-86c0-0248e4a27d5e' });</script>
</asp:Content>
