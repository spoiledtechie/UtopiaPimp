using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Data
{
    public class Attachments
    {

    //    Public Shared Sub UploadPictures(ByVal Page_ID As String)
    //    Dim cts As New SqlConnection(ConfigurationManager.ConnectionStrings(SQLStatements.ConnectionStringID).ConnectionString)
    //    Dim UserID As String = SQLStatements.UserIDFromPage()

    //    Dim sessionCount As Integer = System.Web.HttpContext.Current.Session.Count
    //    For i As Integer = sessionCount - 1 To 0 Step -1
    //        Dim hif As HtmlInputFile = CType(System.Web.HttpContext.Current.Session("myupload" & i), HtmlInputFile)
    //        If hif.PostedFile.ContentLength <= 2000000 Then
    //            ' Declares the varaibles to be used in thus IF statement
    //            Dim SubmitDate As DateTime = DateTime.UtcNow 'declares the time and date of when the attachment was added
    //            Dim extension As String = Path.GetExtension(hif.PostedFile.FileName).ToLower() 'This code pulls JUST the file extension of the file being uploaded
    //            Dim MIMEType As String = Nothing ' Declares that MIMEType is equal to nothing
    //            Dim AttachmentTitle As String = Path.GetFileNameWithoutExtension(hif.PostedFile.FileName) 'This pulls the file name only without the address of the file and without the extension.
    //            Dim FileSalt As Integer = DateTime.Now.Millisecond 'This gets the millsiseconds used to add salt to the end of the file so no two files have the same name.

    //            'This code decideds what mimetype the file is.
    //            Select Case extension
    //                Case ".gif" 'checks for .gif
    //                    MIMEType = "image/gif" 'If it is .gif, declares the type as image/gif
    //                Case ".jpg", ".jpeg", ".jpe"
    //                    MIMEType = "image/jpeg"
    //                Case ".png"
    //                    MIMEType = "image/png"
    //                Case ".tiff"
    //                    MIMEType = "image/tiff"
    //                Case ".pdf"
    //                    MIMEType = "application/pdf"
    //                Case ".zip"
    //                    MIMEType = "application/zip"
    //                Case ".doc"
    //                    MIMEType = "application/doc"
    //                Case ".xls"
    //                    MIMEType = "application/xls"
    //                Case ".ppt"
    //                    MIMEType = "application/ppt"
    //                Case ".vsd"
    //                    MIMEType = "application/vsd"
    //                Case ".mpp"
    //                    MIMEType = "application/mpp"
    //                Case Else
    //                    Exit Sub
    //            End Select

    //            Const SQLFileUpload As String = "INSERT INTO [IAFD_Pictures] ([Added_By_User_ID], [DateStamp], [Picture_File_Salt], [MIME_Type], [Picture_File_Data], [Picture_File_Data_Thumb], [Picture_Title], [Picture_Page_ID]) VALUES (@Added_By_User_ID, @DateStamp, @Picture_File_Salt, @MIME_Type, @Picture_File_Data, @Picture_File_Data_Thumb ,@Picture_Title, @Picture_Page_ID)"
    //            Dim myCommand1 As New SqlCommand(SQLFileUpload, cts)

    //            'This is for the SQL File upload statement
    //            myCommand1.Parameters.AddWithValue("@Added_By_User_ID", UserID)
    //            myCommand1.Parameters.AddWithValue("@DateStamp", SubmitDate)
    //            myCommand1.Parameters.AddWithValue("@Picture_File_Salt", FileSalt)
    //            myCommand1.Parameters.AddWithValue("@MIME_Type", MIMEType)
    //            myCommand1.Parameters.AddWithValue("@Picture_Title", AttachmentTitle)
    //            myCommand1.Parameters.AddWithValue("@Picture_Page_ID", Page_ID)

    //            Dim VirtualPath As String = System.Web.HttpContext.Current.Request.PhysicalApplicationPath()
    //            VirtualPath = VirtualPath + "Data"
    //            If (Not Directory.Exists(VirtualPath)) Then
    //                Directory.CreateDirectory(VirtualPath)
    //                'Dim dInfo As New DirectoryInfo(VirtualPath)
    //                'Dim dSecurity As DirectorySecurity = dInfo.GetAccessControl()
    //                'dSecurity.AddAccessRule(New FileSystemAccessRule("Everyone", FileSystemRights.Read, AccessControlType.Allow))
    //                'dSecurity.AddAccessRule(New FileSystemAccessRule("Everyone", FileSystemRights.Write, AccessControlType.Allow))
    //                'dSecurity.AddAccessRule(New FileSystemAccessRule("Everyone", FileSystemRights.CreateDirectories, AccessControlType.Allow))
    //                'dSecurity.AddAccessRule(New FileSystemAccessRule("Everyone", FileSystemRights.CreateFiles, AccessControlType.Allow))
    //                'dSecurity.AddAccessRule(New FileSystemAccessRule("Everyone", FileSystemRights.DeleteSubdirectoriesAndFiles, AccessControlType.Allow))
    //                'dInfo.SetAccessControl(dSecurity)
    //            End If

    //            Dim FileName As String = Path.GetFileName(hif.PostedFile.FileName)
    //            Dim FilePath As String = "~\Data\" & AttachmentTitle & FileSalt & extension
    //            Dim FilePathThumb As String = "~\Data\" & AttachmentTitle & "_thumb" & FileSalt & extension
    //            myCommand1.Parameters.AddWithValue("@Picture_File_Data", FilePath)
    //            myCommand1.Parameters.AddWithValue("@Picture_File_Data_Thumb", FilePathThumb)

    //            'hif.PostedFile.SaveAs(VirtualPath & "\" & AttachmentTitle & FileSalt & extension)

    //            'Dim dummyCallBack As System.Drawing.Image.GetThumbnailImageAbort
    //            'dummyCallBack = New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)

    //            'Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromStream(hif.PostedFile.InputStream)


    //            ''Dim maxheight As Double = 135
    //            ''Dim maxwidth As Double = 180
    //            ''Dim imgheight As Double = fullSizeImg.Height
    //            ''Dim imgWidth As Double = fullSizeImg.Width
    //            ''Dim x As Double = 1.1

    //            ''If imgWidth > maxwidth Or imgheight > maxheight Then

    //            'Dim RealImg As System.Drawing.Image = ImageShrink(800, 600, System.Drawing.Image.FromStream(hif.PostedFile.InputStream))
    //            'RealImg.Save(VirtualPath & "\" & AttachmentTitle & FileSalt & extension)
    //            ''End If

    //            Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromStream(hif.PostedFile.InputStream)

    //            Dim imgheight As Double = fullSizeImg.Height
    //            Dim imgWidth As Double = fullSizeImg.Width

    //            If imgWidth > 800 Or imgheight > 600 Then
    //                ImageShrink(600, VirtualPath, AttachmentTitle, FileSalt, extension, System.Drawing.Image.FromStream(hif.PostedFile.InputStream))
    //            Else
    //                hif.PostedFile.SaveAs(VirtualPath & "\" & AttachmentTitle & FileSalt & extension)
    //            End If

    //            ImageShrink(130, VirtualPath, AttachmentTitle & "_thumb", FileSalt, extension, System.Drawing.Image.FromStream(hif.PostedFile.InputStream))
    //            Try
    //                cts.Open()
    //                myCommand1.ExecuteNonQuery()
    //            Finally
    //                cts.Close()

    //            End Try
    //            System.Web.HttpContext.Current.Session.Remove("myupload" & i)
    //        End If
    //    Next
    //    cts.Dispose()

    //End Sub
    //Public Shared Sub UploadNewsPictures(ByVal Page_ID As String, ByVal NewsArticle As String)
    //    Dim cts As New SqlConnection(ConfigurationManager.ConnectionStrings(SQLStatements.ConnectionStringID).ConnectionString)
    //    Dim UserID As String = SQLStatements.UserIDFromPage()

    //    Dim sessionCount As Integer = System.Web.HttpContext.Current.Session.Count
    //    For i As Integer = sessionCount - 1 To 0 Step -1
    //        Dim hif As HtmlInputFile = CType(System.Web.HttpContext.Current.Session("myupload" & i), HtmlInputFile)
    //        If hif.PostedFile.ContentLength <= 2000000 Then
    //            ' Declares the varaibles to be used in thus IF statement
    //            Dim SubmitDate As DateTime = DateTime.UtcNow 'declares the time and date of when the attachment was added
    //            Dim extension As String = Path.GetExtension(hif.PostedFile.FileName).ToLower() 'This code pulls JUST the file extension of the file being uploaded
    //            Dim MIMEType As String = Nothing ' Declares that MIMEType is equal to nothing
    //            Dim AttachmentTitle As String = Path.GetFileNameWithoutExtension(hif.PostedFile.FileName) 'This pulls the file name only without the address of the file and without the extension.
    //            Dim FileSalt As Integer = DateTime.Now.Millisecond 'This gets the millsiseconds used to add salt to the end of the file so no two files have the same name.

    //            'This code decideds what mimetype the file is.
    //            Select Case extension
    //                Case ".gif" 'checks for .gif
    //                    MIMEType = "image/gif" 'If it is .gif, declares the type as image/gif
    //                Case ".jpg", ".jpeg", ".jpe"
    //                    MIMEType = "image/jpeg"
    //                Case ".png"
    //                    MIMEType = "image/png"
    //                Case ".tiff"
    //                    MIMEType = "image/tiff"
    //                Case ".pdf"
    //                    MIMEType = "application/pdf"
    //                Case ".zip"
    //                    MIMEType = "application/zip"
    //                Case ".doc"
    //                    MIMEType = "application/doc"
    //                Case ".xls"
    //                    MIMEType = "application/xls"
    //                Case ".ppt"
    //                    MIMEType = "application/ppt"
    //                Case ".vsd"
    //                    MIMEType = "application/vsd"
    //                Case ".mpp"
    //                    MIMEType = "application/mpp"
    //                Case Else
    //                    Exit Sub
    //            End Select

    //            Const SQLFileUpload As String = "INSERT INTO [IAFD_Pictures] ([Added_By_User_ID], [DateStamp], [Picture_File_Salt], [MIME_Type], [Picture_File_Data], [Picture_File_Data_Thumb], [Picture_Title], [Picture_Page_ID], [Picture_News_ID]) VALUES (@Added_By_User_ID, @DateStamp, @Picture_File_Salt, @MIME_Type, @Picture_File_Data, @Picture_File_Data_Thumb ,@Picture_Title, @Picture_Page_ID, @Picture_News_ID)"
    //            Dim myCommand1 As New SqlCommand(SQLFileUpload, cts)

    //            'This is for the SQL File upload statement
    //            myCommand1.Parameters.AddWithValue("@Added_By_User_ID", UserID)
    //            myCommand1.Parameters.AddWithValue("@DateStamp", SubmitDate)
    //            myCommand1.Parameters.AddWithValue("@Picture_File_Salt", FileSalt)
    //            myCommand1.Parameters.AddWithValue("@MIME_Type", MIMEType)
    //            myCommand1.Parameters.AddWithValue("@Picture_Title", AttachmentTitle)
    //            myCommand1.Parameters.AddWithValue("@Picture_Page_ID", Page_ID)
    //            myCommand1.Parameters.AddWithValue("@Picture_News_ID", NewsArticle)

    //            Dim VirtualPath As String = System.Web.HttpContext.Current.Request.PhysicalApplicationPath()
    //            VirtualPath = VirtualPath + "Data"
    //            If (Not Directory.Exists(VirtualPath)) Then
    //                Directory.CreateDirectory(VirtualPath)
    //                'Dim dInfo As New DirectoryInfo(VirtualPath)
    //                'Dim dSecurity As DirectorySecurity = dInfo.GetAccessControl()
    //                'dSecurity.AddAccessRule(New FileSystemAccessRule("Universal", FileSystemRights.Read, AccessControlType.Allow))
    //                'dSecurity.AddAccessRule(New FileSystemAccessRule("Universal", FileSystemRights.Write, AccessControlType.Allow))
    //                'dSecurity.AddAccessRule(New FileSystemAccessRule("Universal", FileSystemRights.CreateDirectories, AccessControlType.Allow))
    //                'dSecurity.AddAccessRule(New FileSystemAccessRule("Universal", FileSystemRights.CreateFiles, AccessControlType.Allow))
    //                'dSecurity.AddAccessRule(New FileSystemAccessRule("Universal", FileSystemRights.DeleteSubdirectoriesAndFiles, AccessControlType.Allow))
    //                'directory.SetAccessControl(VirtualPath, system.Security.AccessControl.DirectorySecurity
    //            End If

    //            Dim FileName As String = Path.GetFileName(hif.PostedFile.FileName)
    //            Dim FilePath As String = "~\Data\" & AttachmentTitle & FileSalt & extension
    //            Dim FilePathThumb As String = "~\Data\" & AttachmentTitle & "_thumb" & FileSalt & extension
    //            myCommand1.Parameters.AddWithValue("@Picture_File_Data", FilePath)
    //            myCommand1.Parameters.AddWithValue("@Picture_File_Data_Thumb", FilePathThumb)

    //            'Dim dummyCallBack As System.Drawing.Image.GetThumbnailImageAbort
    //            'dummyCallBack = New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)

    //            'Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromStream(hif.PostedFile.InputStream)

    //            'Dim maxheight As Double = 135
    //            'Dim maxwidth As Double = 180
    //            'Dim imgheight As Double = fullSizeImg.Height
    //            'Dim imgWidth As Double = fullSizeImg.Width
    //            'Dim x As Double = 1.1

    //            'If imgWidth > maxwidth Or imgheight > maxheight Then
    //            '    While imgWidth > maxwidth
    //            '        imgheight = imgheight / x
    //            '        imgWidth = imgWidth / x
    //            '        imgheight = Math.Round(imgheight, 2)
    //            '        imgWidth = Math.Round(imgWidth, 2)
    //            '    End While
    //            'End If

    //            'Dim RealImg As System.Drawing.Image = ImageShrink(800, 600, System.Drawing.Image.FromStream(hif.PostedFile.InputStream))
    //            'RealImg.Save(VirtualPath & "\" & AttachmentTitle & FileSalt & extension)

    //            Dim fullSizeImg As System.Drawing.Image = System.Drawing.Image.FromStream(hif.PostedFile.InputStream)

    //            Dim imgheight As Double = fullSizeImg.Height
    //            Dim imgWidth As Double = fullSizeImg.Width

    //            If imgWidth > 800 Or imgheight > 600 Then
    //                ImageShrink(600, VirtualPath, AttachmentTitle, FileSalt, extension, System.Drawing.Image.FromStream(hif.PostedFile.InputStream))
    //            Else
    //                hif.PostedFile.SaveAs(VirtualPath & "\" & AttachmentTitle & FileSalt & extension)
    //            End If

    //            ImageShrink(130, VirtualPath, AttachmentTitle & "_thumb", FileSalt, extension, System.Drawing.Image.FromStream(hif.PostedFile.InputStream))
    //            Try
    //                cts.Open()
    //                myCommand1.ExecuteNonQuery()
    //            Finally
    //                cts.Close()

    //            End Try
    //            System.Web.HttpContext.Current.Session.Remove("myupload" & i)
    //        End If
    //    Next
    //    cts.Dispose()
    //End Sub
    //Public Shared Function ThumbnailCallback() As Boolean
    //    Return False
    //End Function
    //Public Shared Sub ImageShrink(ByVal TargetSize As Integer, ByVal VirtualPath As String, ByVal AttachmentTitle As String, ByVal FileSalt As String, ByVal Extension As String, ByVal FullSizeImg As System.Drawing.Image)

    //    Dim original As System.Drawing.Image = FullSizeImg
    //    Dim TargetH, TargetW As Integer
    //    If original.Height > original.Width Then
    //        TargetH = TargetSize
    //        TargetW = Convert.ToInt64(original.Width * (Convert.ToDouble(TargetH) / (Convert.ToDouble(original.Height))))
    //    Else
    //        TargetW = TargetSize
    //        TargetH = Convert.ToInt64(original.Height * (Convert.ToDouble(TargetW) / (Convert.ToDouble(original.Width))))
    //    End If

    //    Dim imgPhoto As System.Drawing.Image = FullSizeImg
    //    Dim bmPhoto As Bitmap = New Bitmap(TargetW, TargetH, PixelFormat.Format24bppRgb)
    //    bmPhoto.SetResolution(72, 72)
    //    Dim grPhoto As Graphics = Graphics.FromImage(bmPhoto)
    //    grPhoto.SmoothingMode = SmoothingMode.AntiAlias
    //    grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic
    //    grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality
    //    grPhoto.DrawImage(imgPhoto, New Rectangle(0, 0, TargetW, TargetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel)
    //    Dim mm As Stream = File.Create(VirtualPath & "\" & AttachmentTitle & FileSalt & Extension)
    //    Select Case Extension
    //        Case ".gif" 'checks for .gif
    //            bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Gif)
    //        Case ".jpg", ".jpeg", ".jpe"
    //            bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg)
    //        Case ".png"
    //            bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Png)
    //        Case ".tiff"
    //            bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Tiff)
    //    End Select
    //    original.Dispose()
    //    imgPhoto.Dispose()
    //    bmPhoto.Dispose()
    //    grPhoto.Dispose()
    //    mm.Dispose()
    //End Sub

    }
}
