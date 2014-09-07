<%@ Page Language="C#" MasterPageFile="~/LoggedOut.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" Title="Utopia Pimp 2.1" %>

<%@ Register Src="~/forum/controls/TopPosts.ascx" TagName="TopPosts" TagPrefix="uc1" %>
<%@ Register Src="~/forum/controls/LatestPosts.ascx" TagName="LatestPosts" TagPrefix="uc2" %>
<%@ Register Src="~/forum/controls/ActiveTopics.ascx" TagName="ActiveTopics" TagPrefix="uc3" %>
<%@ Register Src="~/controls/reusable/Tumblr.ascx" TagName="Tumblr" TagPrefix="uc4" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <div>
        <asp:Label runat="server" ID="lblWarning"></asp:Label>
    </div>
    
    <div class="pnlContent">
        <div class="pnls">
            <div class="pnldivHeaders">
                Welcome to UtopiaPimp.com
            </div>
            The Best Dang Tool for the Best Online Game in the WORLD <a href="http://utopia-game.com">Utopia</a>!
            <p>
                The tool that puts the fun and amazing attributes of Utopia into your hands to bring
                it to a new level of game play! Not only that, it is a wonderful way to unite your
                Kingdom and make wars much more fun by giving you powerful technology to organize
                and view combat data and statistics no other application can do. If you go to war
                with a Kingdom that doesn’t use Utopiapimp.com, it is like fighting an army armed
                with swords and catapults with your Mechanized 21st century divisions.
                <br />
                <br />
                What you will find in UtopiaPimp is the ability to see who’s army is out and what
                opponents and teammates have ops on them. This stunning tool can tell your whole
                Kingdom at a click who has gold, runes or food for stealing. It tracks total attacks
                made by your kingdom or how many acres your team and the opponent have swapped allowing
                everyone in your kingdom to make wise and informed battle choices.
                <br />
                <br />
                Monarchs will be especially appreciative of the ability UtopiaPimp will give them
                to see who needs help in the kingdom and to track the status of all their Provinces.
                Monarchs that use this tool will always insure that when they press the button for
                war, their kingdom is ready to win!
                <br />
                <br />
                <b>From a Users point of View:</b>
                <br />
                <br />
                Utopiapimp.com is a site that formats all war information into a one page overview.
                It allows the KD to paste all intel, ops, spells and attacks on one page. So if
                I cast a spell (eg storms) and paste it into pimp, you and your kingdom mates will
                see the storms and how long it will last. There is an icon for every offensive spell.
                If I do three gc steals and paste them into pimp, it will total them and list them
                under recent ops. If you do a SoM when their troops are out, it will display that
                with an icon. It also color codes how recent every CB, survey, and science spy is.
                It also allows you to sort the enemy, for example by modified defense. So you can
                find a nice target to break. It also groups all the intel on a specific enemy province
                into a summary page.
                <br />
                <br />
                This allows us to not have cluttered forum threads during war, nor search through
                CBs and intel looking for the one you want or the most recent one. It also will
                let you see what was done already to reduce duplication of spell ops, or say casting
                vermin on an undead (the race and personalities of all the enemy are listed on the
                main page).
            </p>
        </div>
        
    </div>
    <div class="pnlContent"><ul class="ulList"><li><span class="st_twitter_hcount" st_url="http://utopia-game.com" displaytext="Tweet" st_title="Utopia - The Best FREE Online Strategy Warfare Game Ever! #games"></span><span st_url="http://utopia-game.com" class="st_facebook_hcount"  st_title="Utopia - The Best FREE Online Strategy Warfare Game Ever!"
                        displaytext="Share"></span> - Spread the word about <b>Utopia</b> EASILY!</li>
                        <li><span class="st_twitter_hcount" st_url="http://utopiapimp.com" displaytext="Tweet" st_title="An Awesome Tool for Utopia-game.com! #games"></span><span  st_title="An Awesome Tool for Utopia-game.com!" st_url="http://utopiapimp.com" class="st_facebook_hcount"
                        displaytext="Share"></span> - Spread the word about <b>UtopiaPimp</b> EASILY!</li></ul>
        <div class="pnls">
            <div class="pnldivHeaders">
                <asp:Literal ID="ltHeader" runat="server"></asp:Literal>
            </div>
            <div class="rss">
                <a href="http://blog.utopiapimp.com/rss">
                    <img src="http://codingforcharity.org/utopiapimp/img/rss.png" />
                </a>
            </div>
            <uc4:Tumblr ID="Tumblr1" runat="server" />
        </div>
    </div>
     <script type="text/javascript" src="http://w.sharethis.com/button/buttons.js"></script>
    <script type="text/javascript">        stLight.options({ publisher: '44150a77-2fa9-4b5f-86c0-0248e4a27d5e' });</script>
</asp:Content>
