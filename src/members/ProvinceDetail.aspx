<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ProvinceDetail.aspx.cs"
    Inherits="members_ProvinceDetail" %>

<%@ MasterType VirtualPath="~/main.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/controls/HistoriesProvince.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <div id="divCB" class="divProvincePane">
        <center>
            <img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..."
                width="50px" /></center>
    </div>
    <div id="divSOS" class="divProvincePane">
        <center>
            <img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..."
                width="50px" /></center>
    </div>
    <div id="divSurvey" class="divProvincePane">
        <center>
            <img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..."
                width="50px" /></center>
    </div>
    <div id="divSOM" class="divProvincePane">
        <center>
            <img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..."
                width="50px" /></center>
    </div>
    <div id="divHistory" class="divProvincePane">
        <center>
            <img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..."
                width="50px" /></center>
    </div>
    <asp:HiddenField ID="hfProvinceID" runat="server" />
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
    <script type="text/javascript">
        $(document).ready(function () {
            LoadProvinceDetailPage();
        });
    </script>
</asp:Content>
<%--<script runat="server">    
        [System.Web.Services.WebMethod]
        public static string getContent(int index)
        {
            return "Generated on " + DateTime.Now.ToString();
        }
    </script>

    <script type="text/javascript">
        var holder = null;
        function getContent(index) {
            var id = "result" + index;
            holder = $get(id);
            PageMethods.getContent(index, onComplete);
        }
        function onComplete(result) {
            holder.innerHTML = result;
        }    
    </script>--%>
<%--<cc1:Accordion ID="acdProvinceDetail" runat="server" HeaderSelectedCssClass="acc-selected-header"
        CssClass="acc" ContentCssClass="acc-content" HeaderCssClass="acc-header">
        <Panes>
            <cc1:AccordionPane ID="AccordionPane1" Enabled="true" runat="server">
                <Header>
                    <a href="javascript:getContent(1);">CB</a>
                    <asp:Label ID="lblCBLast" runat="server"></asp:Label>
                </Header>
                <Content>
                    <asp:Literal ID="ltCB" runat="server"></asp:Literal>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                    <a href="javascript:getContent(2);">SOS</a>
                    <asp:Label ID="lblSOSUpdate" runat="server"></asp:Label>
                </Header>
                <Content>
                    <asp:Literal ID="ltSOS" runat="server"></asp:Literal>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane3" runat="server">
                <Header>
                    <a href="javascript:getContent(3);">Survey</a>
                    <asp:Label ID="lblSurveyUpdate" runat="server"></asp:Label>
                </Header>
                <Content>
                    <asp:Literal ID="ltSurvey" runat="server"></asp:Literal>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane4" runat="server">
                <Header>
                    <a href="javascript:getContent(4);">SOM</a>
                    <asp:Label ID="lblSOMUpdate" runat="server"></asp:Label>
                </Header>
                <Content>
                    <asp:Literal ID="ltSOM" runat="server"></asp:Literal>
                </Content>
            </cc1:AccordionPane>
            <%-- <cc1:AccordionPane ID="AccordionPane5" runat="server">
                <Header>
                    <a href="javascript:getContent(5);">SOM+SOM</a>
                    <asp:Label ID="lblSOMSOMUpdate" runat="server"></asp:Label>
                </Header>
                <Content>
                    <asp:Literal ID="ltSOMSOM" runat="server"></asp:Literal>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="AccordionPane6" runat="server">
                <Header>
                    <a href="javascript:getContent(6);">History</a>
                </Header>
                <Content>
                    <asp:Literal ID="ltHistory" runat="server"></asp:Literal>
                </Content>
            </cc1:AccordionPane>
        </Panes>
    </cc1:Accordion>--%>