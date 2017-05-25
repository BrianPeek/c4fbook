<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WHSMailWeb._Default" %>
<%@ Import namespace="System.ComponentModel"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>WHS Email</title>
	<style>
body
{
	margin:0;
	padding:0;
	height:100%;
}
#top
{
	position: absolute;
	left: 16%;
	top: 0px;
	margin: 0px;
	padding: 0px;
    overflow: auto;
    height:54%;
    width:84%;
}

#pager
{
	position: absolute;
	left: 16%;
	top: 54%;
	margin: 0px;
	padding: 0px;
    overflow: auto;
    height: 24px;
    width: 84%;
	text-align: center;
	font-family: Tahoma;
	font-size: Smaller;
}

#bottom
{
	position: absolute;
	left: 16%;
	margin: 0px;
	padding: 0px;
	vertical-align: top;
	top: 56%;
	height: 44%;
	width: 84%;
	overflow: auto;
	border-top: thin solid #EFF3FB;
	word-wrap:break-word;
}

#folders
{
	position: absolute;
	left: 0px;
	margin: 0px;
	padding: 0px;
	vertical-align: top;
	height: 100%;
	width: 15%;
	overflow: auto;
	border-right: thin solid #EFF3FB;
}

#tblHeader
{
	color:#FFFFFF;
	font-family: Tahoma;
	font-size: smaller;
}
	</style>
</head>
<body scroll="no">
    <form id="form1" runat="server">
	<div id="folders">
		<asp:TreeView ID="tvFolders" runat="server" OnSelectedNodeChanged="tvFolders_SelectedNodeChanged" ExpandDepth="0" ImageSet="Inbox" NodeIndent="10">
			<ParentNodeStyle Font-Bold="False" />
			<HoverNodeStyle Font-Underline="True" />
			<SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px" />
			<NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
			NodeSpacing="0px" VerticalPadding="0px" />
		</asp:TreeView>
	</div>			
	<div id="top">
		<asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Horizontal" AutoGenerateColumns="False" 
			CaptionAlign="Top" HorizontalAlign="Center" Width="100%" PageSize="20" Font-Names="Tahoma" Font-Size="Small">
			<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
			<RowStyle BackColor="#EFF3FB" />
			<EditRowStyle BackColor="#2461BF" />
			<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
			<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
			<HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
			<AlternatingRowStyle BackColor="White" />
			<Columns>
				<asp:BoundField DataField="From" HeaderText="From" />
				<asp:TemplateField HeaderText="Subject">
					<ItemTemplate>
						<asp:LinkButton ID="btnLink" OnCommand="btnLink_Command" CommandArgument='<%#Eval("EntryID")%>' runat="server"><%#Eval("Subject")%></asp:LinkButton>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField DataField="Received" HeaderText="Received" />
				<asp:BoundField DataField="Size" HeaderText="Size" />
			</Columns>
		</asp:GridView>  
	</div>
	<div id="pager">
		<asp:LinkButton ID="btnPrev" runat="server" OnClick="btnPrev_Click">&lt;&nbsp;Prev</asp:LinkButton>&nbsp;
		<asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click">Next&nbsp;&gt;</asp:LinkButton>
	</div>
	<div id="bottom" runat="server">
		<table cellpadding="0" cellspacing="0" id="tblHeader" bgcolor="#507CD1" width="100%" runat="server">
			<tr><td><strong>From:</strong></td><td><asp:Label ID="lblFrom" runat="server"/></td></tr>
			<tr><td><strong>Subject:</strong></td><td><asp:Label ID="lblSubject" runat="server"/></td></tr>
			<tr><td><strong>Received:</strong></td><td><asp:Label ID="lblReceived" runat="server"/></td></tr>
		</table>
		<div id="msgContent" runat="server"></div>
	</div>
    </form>
</body>
</html>
