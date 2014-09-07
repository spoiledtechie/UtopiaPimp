<%@ Page Title="" Language="C#" MasterPageFile="~/admin/UAdmin/UAdmin.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="admin_UAdmin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divContent">
        <div>
            <asp:HyperLink ID="hlUsers" runat="server" NavigateUrl="~/admin/UAdmin/Kingdoms.aspx">Kingdoms</asp:HyperLink></div>
        <ul class="ulList">
            <li>
                <asp:Label ID="lblKingdomCount" runat="server"></asp:Label>
                Total Kingdoms</li>
                <li>
                <asp:Label ID="lblKingdomsUpdated" runat="server"></asp:Label>
                Owner Kindoms updated in 5 days.</li>
        </ul>
    </div>
    <div class="divContent">
        <div>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/admin/UAdmin/Provinces.aspx">Provinces</asp:HyperLink></div>
        <ul class="ulList">
            <li>
                <asp:Label ID="lblUserProvinces" runat="server"></asp:Label>
                Total Attached Provinces</li>
        </ul>
    </div>
    <div class="divContent">
        <div>
          Delete Data older than the age</div>
        <ul class="ulList">
            <li>
                <asp:Button ID="btnDeleteOldData"  runat="server" 
                    Text="Delete CEs" onclick="btnDeleteOldData_Click" />
                </li>
                <li>
                <asp:Button ID="Button1"  runat="server" 
                    Text="Delete Monarch Messages" onclick="Button1_Click" />
                </li>
                
                <li>
                <asp:Button ID="Button2"  runat="server" 
                    Text="Delete kingdom Settings" onclick="Button2_Click" />
                </li>
                <li>
                <asp:Button ID="Button3"  runat="server" 
                    Text="Delete Votes" onclick="Button3_Click" />
                </li>
                <li>
                <asp:Button ID="Button4"  runat="server" 
                    Text="Delete Attacks" onclick="Button4_Click" />
                </li>
                <li>
                <asp:Button ID="Button5"  runat="server" 
                    Text="Delete Sciences" onclick="Button5_Click" />
                </li>
                <li>
                <asp:Button ID="Button6"  runat="server" 
                    Text="Delete Surveys" onclick="Button6_Click" />
                </li>
                <li>
                <asp:Button ID="Button7"  runat="server" 
                    Text="Delete Militarys" onclick="Button7_Click" />
                </li>
                <li>
                <asp:Button ID="Button8"  runat="server" 
                    Text="Delete Memos" onclick="Button8_Click" />
                </li>
                <li>
                <asp:Button ID="Button9"  runat="server" 
                    Text="Delete Notes" onclick="Button9_Click" />
                </li>
                <li>
                <asp:Button ID="Button10"  runat="server" 
                    Text="Delete Ops" onclick="Button10_Click" />
                </li>
                <li>
                <asp:Button ID="Button11"  runat="server" 
                    Text="Delete Target Finder" onclick="Button11_Click" />
                </li>
                  <li>
                <asp:Button ID="Button12"  runat="server" 
                    Text="Delete Non Connected Provinces" onclick="ProvinceData_Click" />
                </li>
                 <li>
                <asp:Button ID="Button13"  runat="server" 
                    Text="Delete CBs" onclick="CBData_Click" />
                </li>
                 <li>
                <asp:Button ID="Button14"  runat="server" 
                    Text="Delete Old Kingdom Data" OnClick="DeleteKingdomData_OnClick" />
                </li>

                <li>
                <asp:Button ID="btnIndexPimp"  runat="server" 
                    Text="Index Pimp DB" onclick="btnIndexPimp_Click"  />
                </li>

                <li>
                <asp:Button ID="IndexBoomersBtn"  runat="server" 
                    Text="Index Boomers DB" onclick="IndexBoomersBtn_Click" />
                </li>
        </ul>
    </div>
</asp:Content>
