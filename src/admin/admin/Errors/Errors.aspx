<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true"
    CodeFile="Errors.aspx.cs" Inherits="admin_Errors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="http://codingforcharity.org/libs/jquery/jquery-1.3.2.min.js" />
            <asp:ScriptReference Path="http://codingforcharity.org/libs/jquery/jquery.tableSorter.js" />
        </Scripts>
    </asp:ScriptManagerProxy>

  

    <div> 
        <asp:UpdatePanel ID="upErrorHandling" runat="server">
            <ContentTemplate>
          <asp:Label runat="server" ID="lblErrorCount"> </asp:Label> Total Errors<br />
                Error Handling
                <asp:Button ID="btnReviewedAll" runat="server" Text="All Reviewed" OnClick="btnReviewedAll_Click" />
                <asp:Button ID="btnUsePimp" runat="server" Text="View Pimp Errors" 
                    onclick="btnUsePimp_Click" />
                <asp:Button ID="btnUseShrimp" runat="server" Text="View Shrimp Errors" 
                    onclick="btnUseShrimp_Click" />
                     <asp:Button ID="btnClearCache" runat="server" Text="Clear Errors Cache" onclick="btnClearCache_Click" 
                     />
                <asp:GridView ID="gvErrorHandling" AllowPaging="true" PageSize="20" runat="server" OnRowCommand="gvErrorHandling_RowCommand"
                    AutoGenerateColumns="False" OnPreRender="gv_PreRender" CssClass="GridviewFull"
                    DataKeyNames="uid" >
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                        <asp:ButtonField ButtonType="Button" CommandName="cmdReviewed" Text="Reviewed" />
                        <asp:ButtonField ButtonType="Button" CommandName="cmdDelete" Text="Delete" />
                        <asp:BoundField DataField="uid" HeaderText="uid" InsertVisible="False" ReadOnly="True"
                            SortExpression="uid" Visible="false" />
                        <asp:BoundField DataField="userName" HeaderText="User Name" ReadOnly="True" SortExpression="userName" />
                        <asp:ButtonField ButtonType="Button" CommandName="cmdReviewUsers" Text="Review Users Error" />
                
                        <asp:BoundField DataField="Load_Date" HeaderText="Load Date" ReadOnly="True" SortExpression="Load_Date" />
                        <asp:BoundField DataField="Domain" HeaderText="Domain" ReadOnly="True" SortExpression="Domain" />
                        <asp:BoundField DataField="Error_Url_Path" HeaderText="Url path" ReadOnly="true" SortExpression="Error_Url_Path" />
                        <asp:BoundField DataField="Error_Url_QS" HeaderText="Url QS" ReadOnly="true" SortExpression="Error_Url_QS" />
                        <asp:BoundField DataField="Error_Previous_Url_Path" HeaderText="Prev Url Path" ReadOnly="true" SortExpression="Error_Previous_Url_Path" />
                        <asp:BoundField DataField="Error_Previous_Url_QS" HeaderText="Prev Url QS" ReadOnly="true" SortExpression="Error_Previous_Url_QS" />
                    </Columns>
                    <FooterStyle CssClass="GridviewFooterStyle" />
                    <RowStyle CssClass="GridviewRowStyle" />
                    <EditRowStyle CssClass="GridviewEditRowStyle" />
                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                    <PagerStyle CssClass="GridviewPagerStyle" />
                    <HeaderStyle CssClass="GridviewHeaderStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                </asp:GridView>
                <asp:DataList ID="dlErrorHandling" runat="server" DataKeyField="uid" DataSourceID="SqlErrorHandlingDL">
                    <ItemTemplate>
                        <div class="ErrorHandlingAdminPage">
                            <span class="ErrorHandlingTitle">Error Message:</span>
                            <asp:Label ID="Error_MessageLabel" runat="server" Text='<%# Eval("Error_Message") %>' />
                            <br />
                            <span class="ErrorHandlingTitle">Previous URL:</span>
                            <asp:HyperLink ID="HyperLink2" NavigateUrl='<%# Server.HtmlEncode(Eval("Domain").ToString() + Eval("Error_Previous_Url_Path") + Eval("Error_Previous_Url_QS").ToString()) %>'
                                Text='<%# (Eval("Domain").ToString() + Eval("Error_Previous_Url_Path") + Eval("Error_Previous_Url_QS").ToString()) %>' runat="server"></asp:HyperLink>
                            <br />
                            <span class="ErrorHandlingTitle">OLD Previous URL:</span>
                            <asp:HyperLink ID="hlPreviousURL" NavigateUrl='<%# Server.HtmlEncode(Eval("Error_Previous_URL").ToString()) %>'
                                Text='<%# Eval("Error_Previous_URL") %>' runat="server"></asp:HyperLink>
                            <br />
                            <span class="ErrorHandlingTitle">Error URL:</span>
                            <asp:HyperLink ID="hlErrorURL" runat="server" NavigateUrl='<%# Server.HtmlEncode(Eval("Domain").ToString() + Eval("Error_Url_Path") + Eval("Error_Url_QS").ToString()) %>'
                                Text='<%# (Eval("Domain").ToString() + Eval("Error_Url_Path") + Eval("Error_Url_QS").ToString()) %>'></asp:HyperLink>
                            <br />
                            <span class="ErrorHandlingTitle">OLD Error URL:</span>
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Server.HtmlEncode(Eval("Error_URL").ToString()) %>'
                                Text='<%# Eval("Error_URL") %>'></asp:HyperLink>
                            <br />
                            <span class="ErrorHandlingTitle">Error Target:</span>
                            <asp:Label ID="Error_TargetLabel" runat="server" Text='<%# Eval("Error_Target") %>' />
                            <br />
                            <span class="ErrorHandlingTitle">Error Source:</span>
                            <asp:Label ID="Error_SourceLabel" runat="server" Text='<%# Eval("Error_Source") %>' />
                            <br />
                            <span class="ErrorHandlingTitle">Trace Error:</span>
                            <asp:Label ID="Trace_ErrorLabel" runat="server" Text='<%# Eval("Trace_Error") %>' />
                            <br />
                            <span class="ErrorHandlingTitle">Version:</span>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Version") %>' />
                            <br />
                            <span class="ErrorHandlingTitle">Error Trace:</span><br />
                            <asp:Label ID="Error_TraceLabel" runat="server" Text='<%# Eval("Error_Trace") %>' />
                            
                            <br />
                            <span class="ErrorHandlingTitle">Last Exception:</span><br />
                            <asp:Label ID="Last_ExceptionLabel" runat="server" Text='<%# Eval("Last_Exception") %>' />
                            <br />
                            <span class="ErrorHandlingTitle">Additional information:</span><br />
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("AdditionalInformation") %>' />
                            <br />
                            <span class="ErrorHandlingTitle">Submitted Data:</span><br />
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("Session_Data") %>' />                            
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvErrorHandling" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnReviewedAll" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnUsePimp" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnUseShrimp" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:SqlDataSource ID="SqlErrorHandlingDL" runat="server" ConnectionString="<%$ ConnectionStrings:UPConnectionString %>"
            SelectCommand="SELECT [uid], [Error_Message], [Error_Previous_URL], [Error_URL], [Error_Target], [Error_Source], [Trace_Error], [Error_Trace], [Last_Exception], [Session_Data], [Version], [AdditionalInformation], [Domain], [Error_Url_Path], [Error_Url_QS], [Error_Previous_Url_Path], [Error_Previous_Url_QS] FROM [Global_Errors_Log] WHERE ([uid] = @uid)">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvErrorHandling" Name="uid" PropertyName="SelectedValue"
                    Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <%--<asp:SqlDataSource ID="SqlErrorHandling" runat="server" ConnectionString="<%$ ConnectionStrings:UPConnectionString %>"
        DeleteCommand="DELETE FROM [Global_Errors_Log] WHERE [uid] = @uid" InsertCommand="INSERT INTO [Global_Errors_Log] ([User_ID], [User_Email], [Load_Date], [Error_Message], [Error_Previous_URL], [Error_URL], [Error_Target], [Error_Source], [Trace_Error], [Error_Trace], [Last_Exception], [Session_Data], [Reviewed]) VALUES (@User_ID, @User_Email, @Load_Date, @Error_Message, @Error_Previous_URL, @Error_URL, @Error_Target, @Error_Source, @Trace_Error, @Error_Trace, @Last_Exception, @Session_Data, @Reviewed)"
        SelectCommand="SELECT [uid], [User_ID], [User_Email], [Load_Date], [Error_Message], [Error_Previous_URL], [Error_URL], [Error_Target], [Error_Source], [Trace_Error], [Error_Trace], [Last_Exception], [Session_Data], [Reviewed] FROM [Global_Errors_Log] WHERE ([Reviewed] = @Reviewed) ORDER BY [Load_Date] Desc"
        UpdateCommand="UPDATE [Global_Errors_Log] SET [User_ID] = @User_ID, [User_Email] = @User_Email, [Load_Date] = @Load_Date, [Error_Message] = @Error_Message, [Error_Previous_URL] = @Error_Previous_URL, [Error_URL] = @Error_URL, [Error_Target] = @Error_Target, [Error_Source] = @Error_Source, [Trace_Error] = @Trace_Error, [Error_Trace] = @Error_Trace, [Last_Exception] = @Last_Exception, [Session_Data] = @Session_Data, [Reviewed] = @Reviewed WHERE [uid] = @uid">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="Reviewed" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="uid" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="User_ID" Type="String" />
            <asp:Parameter Name="User_Email" Type="String" />
            <asp:Parameter Name="Load_Date" Type="String" />
            <asp:Parameter Name="Error_Message" Type="String" />
            <asp:Parameter Name="Error_Previous_URL" Type="String" />
            <asp:Parameter Name="Error_URL" Type="String" />
            <asp:Parameter Name="Error_Target" Type="String" />
            <asp:Parameter Name="Error_Source" Type="String" />
            <asp:Parameter Name="Trace_Error" Type="String" />
            <asp:Parameter Name="Error_Trace" Type="String" />
            <asp:Parameter Name="Last_Exception" Type="String" />
            <asp:Parameter Name="Reviewed" Type="Int32" />
            <asp:Parameter Name="uid" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="User_ID" Type="String" />
            <asp:Parameter Name="User_Email" Type="String" />
            <asp:Parameter Name="Load_Date" Type="String" />
            <asp:Parameter Name="Error_Message" Type="String" />
            <asp:Parameter Name="Error_Previous_URL" Type="String" />
            <asp:Parameter Name="Error_URL" Type="String" />
            <asp:Parameter Name="Error_Target" Type="String" />
            <asp:Parameter Name="Error_Source" Type="String" />
            <asp:Parameter Name="Trace_Error" Type="String" />
            <asp:Parameter Name="Error_Trace" Type="String" />
            <asp:Parameter Name="Last_Exception" Type="String" />
            <asp:Parameter Name="Reviewed" Type="Int32" />
        </InsertParameters>
    </asp:SqlDataSource>--%>
    <asp:Literal ID="ltJavascriptInject" runat="server"></asp:Literal>
<br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
</asp:Content>
