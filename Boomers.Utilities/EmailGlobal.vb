Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Collections.ObjectModel
Imports System.Net.Mail ' For the Email Server to be sent once Submit has been clicked.

Public Class EmailGlobal
    Public Shared Function IPAddressFunction() As String
        'Dim IPAddress As String = "localhost" 'Development Box
        Dim IPAddress As String = "relay-hosting.secureserver.net" ' Production Machine
        Return IPAddress
    End Function

    Public Shared Function EmailAddressFunction() As String
        Dim Email As String = "admin@indialanticfire.com" 'Development Machine
        Return Email
    End Function

    Public Shared Function MailBodyString(ByVal EmailBody As String, ByVal ActionToTake As String) As String
        Dim Servername As String = System.Web.HttpContext.Current.Request.ServerVariables("SERVER_NAME")
        Dim DateTime As Date = Date.UtcNow

        Dim MailBody As New StringBuilder
        'starts frist line of code
        MailBody.Append("<html><body><table border='1' cellpadding='0' cellspacing='0'><tr><td colspan='2' align='center' style='background-color:#9f1c1e; color:White;'>IAFD</td></tr>")
        MailBody.Append("<tr><td align='left' style='background-color:#9f1c1e; color:White;'>Server: </td><td align='left'>" + "<a style='color:Black;' href=" + "http://" + Servername + "/Default.aspx" + ">" + "http://" + Servername + "/Default.aspx" + "</a>" + "</td></tr>")
        MailBody.Append("<tr><td align='left' style='background-color:#9f1c1e; color:White;'>Date: </td><td align='left'>" + DateTime + "</td></tr>")
        MailBody.Append("<tr><td align='left' style='background-color:#9f1c1e; color:White;'>Information: </td><td align='left'>" + EmailBody + "</td><tr>")
        MailBody.Append("<tr><td align='left' style='background-color:#9f1c1e; color:White;'>Action to Take: </td><td align='left'>" + ActionToTake + "</td></tr></table></body></html>")
        Return MailBody.ToString
    End Function

    Public Shared Sub EmailSettings(ByVal Users As String, ByVal EmailBody As String, ByVal EmailSubject As String, ByVal ActionToTake As String)
        'This is the General Rules for the Mailings below.  Which is called at the end of each mailing.
        Dim mu As MembershipUser = Membership.GetUser(Users)
        Dim SubmitDate As DateTime = Now
        Dim Servername As String = System.Web.HttpContext.Current.Request.ServerVariables("SERVER_NAME")
        If mu.IsApproved = True Then
            Dim ctsUser As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("IAFD") 'this is the connection string that talks to web.config connection
            Dim SQLdatasource As New SqlDataSource(ctsUser.ConnectionString, "SELECT [Email] FROM [IAFD_User_Info] WHERE ([User_Name] = '" + Users + "')")
            Dim idsUser As IEnumerable = SQLdatasource.Select(DataSourceSelectArguments.Empty)
            Dim Email As String = -1
            For Each row As DataRowView In idsUser
                Email = row("Email").ToString
                Dim msgFrom As New MailAddress(EmailAddressFunction)
                Dim msgTo As New MailAddress(Email)
                Dim mailClient As New SmtpClient(IPAddressFunction)
                Dim msgMail As New MailMessage(msgFrom, msgTo)
                msgMail.IsBodyHtml = True
                msgMail.Subject = EmailSubject
                msgMail.Body = MailBodyString(EmailBody, ActionToTake)
                mailClient.UseDefaultCredentials = True
                ' Send delivers the message to the mail server
                mailClient.Send(msgMail)
            Next
        End If
    End Sub

    Public Shared Sub EmailSettingsNewUser(ByVal Users As String, ByVal EmailBody As String, ByVal EmailSubject As String, ByVal ActionToTake As String)
        'This is the General Rules for the Mailings below.  Which is called at the end of each mailing.
        Dim MU As MembershipUser = Membership.GetUser(Users)
        Dim SubmitDate As DateTime = Now
        Dim Servername As String = System.Web.HttpContext.Current.Request.ServerVariables("SERVER_NAME")
        Dim msgFrom As New MailAddress(EmailAddressFunction)
        Dim msgTo As New MailAddress(MU.Email)
        Dim mailClient As New SmtpClient(IPAddressFunction)
        Dim msgMail As New MailMessage(msgFrom, msgTo)
        msgMail.IsBodyHtml = True
        msgMail.Subject = EmailSubject

        msgMail.Body = MailBodyString(EmailBody, ActionToTake)

        mailClient.UseDefaultCredentials = True
        ' Send delivers the message to the mail server
        mailClient.Send(msgMail)

    End Sub

    Public Shared Sub IsApprovedAdmin(ByVal IsApproved As Boolean, ByVal UserName As String, ByVal email As String)
        Dim cts As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("TAFConnectionString") 'this is the connection string that talks to web.config connection

        Dim Servername As String = System.Web.HttpContext.Current.Request.ServerVariables("SERVER_NAME")
        'Is notice that the user has opted not to recieve Email notifications. If 1 then sends mail, if 0 then dont send mail.
        Dim datetime As Date = Now
        'sends a message to the email say that they have just been approved.
        Dim msgFrom As New MailAddress(EmailAddressFunction)
        Dim msgTo As New MailAddress(email)
        Dim mailClient As New SmtpClient(IPAddressFunction)
        Dim msgMail As New MailMessage(msgFrom, msgTo)
        'checks what type of email to send for isapproved or not approved.
        Dim Subject As String = -1
        Dim EmailBody As String = -1
        Dim ActionToTake As String = -1
        If IsApproved = True Then
            Subject = "A Admin has just Approved you."
            EmailBody = UserName + ", You have just been Approved for the http://indialanticfire.com Website"
            ActionToTake = "Please Go Here to Logon:" + "<a href=" + "http://" + Servername + "/Default.aspx" + ">" + "http://" + Servername + "/Default.aspx" + "</a>"
        ElseIf IsApproved = False Then
            Subject = "A Admin has just UN-Approved you."
            EmailBody = "<html><body>" + UserName + ", You have just been UN-Approved from the http://indialanticfire.com Website" + "<br /><br />" + "If you think this is error, please contact the Admin"
            ActionToTake = "None"
        End If

        msgMail.Body = MailBodyString(EmailBody, ActionToTake)
        msgMail.Subject = Subject

        msgMail.IsBodyHtml = True
        mailClient.UseDefaultCredentials = True
        ' Send delivers the message to the mail server
        mailClient.Send(msgMail)
        'Disables the Roles Gridview so that it is updated.

    End Sub

    Public Shared Sub NewUser(ByVal FirstName As String, ByVal LastName As String, ByVal UserName As String)
        'This will be the mail sender of the application.
        'Then It Gets the email of the User and then sends out the email to the User
        Dim Role As IEnumerable = Roles.GetUsersInRole("Admin")
        Dim Users As String
        Dim user As String
        For Each user In Role
            Users = user
            Dim Servername As String = System.Web.HttpContext.Current.Request.ServerVariables("SERVER_NAME")
            Dim Subject As String = "A User just registered and needs to be approved."
            Dim Body As String = "The User " + FirstName + " " + LastName + " with the User name of " + UserName + " needs to be approved."
            Dim ActionToTake As String = "Please Go Here:" + "<a href=" + "http://" + Servername + "/admin/Admin.aspx" + ">" + "http://" + Servername + "/admin/Default.aspx" + "</a>"
            EmailSettings(Users, Body, Subject, ActionToTake)
        Next
    End Sub
    Public Shared Sub ErrorEmail(ByVal Subject As String, ByVal Body As String, ByVal MoreBody As String)
        Dim Role As IEnumerable = Roles.GetUsersInRole("admin")
        Dim Users As String
        Dim user As String
        For Each user In Role
            Users = user
            EmailSettings(Users, Body, Subject, MoreBody)
        Next
    End Sub
End Class
