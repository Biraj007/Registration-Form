<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Registration_Form.Registration" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration Form</title>
    <style>
        body { text-align: center; margin-top: 2rem; font-family: Arial; background: #f0f2f5; }
        .container { display: flex; flex-direction: column; align-items: center; }
        .box { width: 500px; padding: 25px; border: 2px solid #333; border-radius: 10px; background: #fff; box-shadow: 2px 2px 10px rgba(0,0,0,0.2); margin-bottom: 2rem; }
        .box h2 { text-align: center; margin-bottom: 20px; }
        .form-row { margin-bottom: 15px; display: flex; align-items: center; justify-content: space-between; }
        .form-row label { flex: 1; text-align: right; margin-right: 15px; }
        .form-row input { flex: 2; padding: 7px; border: 1px solid #ccc; border-radius: 4px; }
        .form-button { display: inline-block; margin: 5px; padding: 6px 12px; background: #4CAF50; color: white; border: none; border-radius: 4px; cursor: pointer; }
        .form-button:hover { background: #45a049; }
        .grid-container { width: 1000px; text-align: center; border: 2px solid #333; border-radius: 10px; padding: 15px; background: #fff; box-shadow: 2px 2px 10px rgba(0,0,0,0.2); }
        .grid-container h3 { text-align: center; }
        .gridview-style { border-collapse: collapse; width: 100%; table-layout: fixed; }
        .gridview-style th, .gridview-style td { border: 1px solid #333; padding: 10px; text-align: center; word-wrap: break-word; max-width: 300px; }
        .gridview-style th { background: #333; color: white; }
        .gridview-style tr:nth-child(even) { background: #e9e9e9; }
        .gridview-style a { display: inline-block; margin: 3px; padding: 6px 12px; background: #4CAF50; color: white !important; text-decoration: none; border-radius: 4px; }
        .gridview-style a:hover { background: #45a049; }
        .dob-box { text-align: center; }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="container">

        <div class="box">
            <h2>Registration Form</h2>

            <div class="form-row">
                <label>Name:</label>
                <asp:TextBox ID="Name" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valName" runat="server" ControlToValidate="Name" ErrorMessage="*" ForeColor="Red" Display="Dynamic" ValidationGroup="regGroup" />
            </div>

            <div class="form-row">
                <label>Surname:</label>
                <asp:TextBox ID="Surname" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valSurname" runat="server" ControlToValidate="Surname" ErrorMessage="*" ForeColor="Red" Display="Dynamic" ValidationGroup="regGroup" />
            </div>

            <div class="form-row">
                <label>Date of Birth:</label>
                <asp:TextBox ID="DOB" runat="server" TextMode="Date" CssClass="dob-box"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valDOB" runat="server" ControlToValidate="DOB" ErrorMessage="*" ForeColor="Red" Display="Dynamic" ValidationGroup="regGroup" />
            </div>

            <div class="form-row">
                <label>Email:</label>
                <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valEmail" runat="server" ControlToValidate="Email" ErrorMessage="*" ForeColor="Red" Display="Dynamic" ValidationGroup="regGroup" />
            </div>

            <div class="form-row">
                <label>Password:</label>
                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valPassword" runat="server" ControlToValidate="Password" ErrorMessage="*" ForeColor="Red" Display="Dynamic" ValidationGroup="regGroup" />
            </div>

            <div class="form-row" id="confirmRow" runat="server">
                <label>Confirm Password:</label>
                <asp:TextBox ID="ConPas" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valConPas" runat="server" ControlToValidate="ConPas" ErrorMessage="*" ForeColor="Red" Display="Dynamic" ValidationGroup="regGroup" />
            </div>

            <div style="text-align: center;">
                <asp:Button ID="Register" runat="server" CssClass="form-button" Text="Register" OnClick="Register_Click" ValidationGroup="regGroup" />
                <asp:Button ID="UpdateBtn" runat="server" CssClass="form-button" Text="Update" Visible="false" OnClick="UpdateBtn_Click" />
                <asp:Button ID="Cancel" runat="server" CssClass="form-button" Text="Cancel" OnClick="Cancel_Click" CausesValidation="false" />
            </div>

            <asp:HiddenField ID="HiddenUserID" runat="server" />

        </div>

        <div class="grid-container">
            <h3>Registered Users:</h3>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridview-style" DataKeyNames="id" OnRowCommand="GridView1_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Surname" HeaderText="Surname" />
                    <asp:TemplateField HeaderText="DOB">
                        <ItemTemplate>
                            <%# Eval("DOB", "{0:dd-MM-yyyy}") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="EditRow" CommandArgument='<%# Eval("id") %>' CssClass="form-button">Edit</asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="DeleteRow" CommandArgument='<%# Eval("id") %>' CssClass="form-button" OnClientClick="return confirm('Are you sure you want to delete this record?');">Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </div>
</form>
</body>
</html>
