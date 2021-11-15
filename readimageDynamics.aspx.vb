Imports System.Drawing.Imaging.ImageFormat
Imports System.IO
Imports System.IO.MemoryStream
Imports System.Drawing
Imports System.Data.OracleClient

Partial Class readimageDynamics
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cn As New OracleConnection(ConfigurationManager.ConnectionStrings("Galileo").ConnectionString)
        Dim strImageID As String = Request.QueryString("id")
        Dim sql As String
        Dim myCommand As New OracleCommand("", cn)
        Dim cod As String = ""

        cn.Open()
      


        sql = "SELECT foto FROM CACARNETBITB where correlativo=:id"
        ' & strImageID & "'"
        '  Dim cn As New OracleConnection(ConfigurationManager.ConnectionStrings("galileo").ConnectionString)
        myCommand.CommandText = sql
        myCommand.Parameters.AddWithValue(":id", strImageID)

        'strImageID = "i081188"
        Try
            'myConnection.Open()

            '            Dim myDataReader As OleDb.OleDbDataReader
            Dim myDataReader As OracleDataReader
            'myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            myDataReader = myCommand.ExecuteReader
            Dim b() As Byte
            Do While (myDataReader.Read())



                Dim image As Image
                b = CType(myDataReader.Item("foto"), Byte())

                Dim bmp As New Bitmap(New System.IO.MemoryStream(b))
                image = bmp

                Response.ContentType = "image/Jpeg"
                Dim ms As MemoryStream = New MemoryStream
                image.Save(ms, Imaging.ImageFormat.Jpeg)
                Dim bytImage(ms.Length) As Byte
                bytImage = ms.ToArray()
                ms.Close()

                ms = New MemoryStream(bytImage)
                Response.Clear()

                Dim chunkSize As Integer = 1024

                Dim i As Integer
                For i = 0 To bytImage.Length Step chunkSize
                    ' Everytime check to see if the browser is still connected
                    If (Not Response.IsClientConnected) Then
                        Exit For
                    End If

                    Dim size As Integer = chunkSize
                    If (i + chunkSize >= bytImage.Length) Then
                        size = (bytImage.Length - i)
                    End If

                    Dim chunk(size - 1) As Byte
                    ms.Read(chunk, 0, size)

                    Response.BinaryWrite(chunk)
                    Response.Flush()


                Next
                ms.Close()

            Loop
            cn.Close()
            Response.Write("No hay foto")
        Catch SQLexc As Exception
            Response.Write("Read Failed : " & SQLexc.ToString())
        End Try

    End Sub
End Class
