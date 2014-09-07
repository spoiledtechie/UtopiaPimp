<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActiveTopics.ascx.cs"
    Inherits="forum_controls_ActiveTopics" %>
<%@ OutputCache Duration="1800" VaryByParam="none" %>
<ul class="ulList ulListMarPad">
    <asp:DataList ID="dlForumPosts" runat="server" Width="100%">
        <ItemTemplate>
            <li>
                <asp:HyperLink ID="hlPost" runat="server" NavigateUrl='<%# "/forum/Default.aspx?g=posts&t=" + Eval("TopicID") + "&#post" + Eval("LastMessageID") %>'><%#Eval("Topic")%></asp:HyperLink>
                (<%#Eval("NumPosts")%>
                posts)<span style="float: right;"><%#Eval("LastPosted")%></span>
                <div style="margin-left: 10px;">
                    in <span style="font-weight: bold;">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "/forum/Default.aspx?g=topics&f=" + Eval("ForumID")%>'><%#Eval("Forum")%></asp:HyperLink>
            </li>
        </ItemTemplate>
    </asp:DataList>
</ul>
