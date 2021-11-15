<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReadPhoto.aspx.cs" Inherits="ReadPhoto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js"></script>
    <script>
        $(document).ready(function(){
                $(document).bind("contextmenu",function(e){
                        return false;
                });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <div>
            <table align="center" style="width: 100%;">
                <tr>
                    <td>
                        <div id="DIV" runat="server" ms_positioning="FlowLayout" style="width: 142px; height: 166px; margin: auto">
                             <img id="foto" runat="server" class="media-object img-thumbnail center" style="width: 142px; height: 166px;" />
                        </div>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
