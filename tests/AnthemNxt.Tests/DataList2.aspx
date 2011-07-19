<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">
		    
    void Page_Load()
    {
        if (!IsPostBack)
            BindList();
    }

    void BindList()
    {
        dataList.DataSource = TestData;
        dataList.DataBind();
        dataList.UpdateAfterCallBack = true;
    }

    void dataList_EditCommand(object sender, DataListCommandEventArgs e)
    {
        dataList.EditItemIndex = e.Item.ItemIndex;
        BindList();
    }

    void dataList_CancelCommand(object sender, DataListCommandEventArgs e)
    {
        dataList.EditItemIndex = -1;
        BindList();
    }

    void dataList_UpdateCommand(object sender, DataListCommandEventArgs e)
    {
        string name = ((System.Web.UI.WebControls.TextBox)e.Item.FindControl("name")).Text;
        string birthdate = ((System.Web.UI.WebControls.TextBox)e.Item.FindControl("birthdate")).Text;

        TestData.Rows[e.Item.ItemIndex]["name"] = name;
        TestData.Rows[e.Item.ItemIndex]["birthdate"] = DateTime.Parse(birthdate);

        dataList.EditItemIndex = -1;
        BindList();
    }

    DataTable TestData
    {
        get
        {
            DataTable dt = Session["TestData"] as DataTable;
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("birthdate", typeof(DateTime));
                dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };
                dt.Rows.Add(new object[] { 1, "Steven", new DateTime(1959, 5, 22) });
                dt.Rows.Add(new object[] { 2, "Johnny", new DateTime(1963, 10, 31) });
                dt.Rows.Add(new object[] { 3, "Andy", new DateTime(1963, 1, 17) });
                dt.Rows.Add(new object[] { 4, "Mike", new DateTime(1963, 6, 1) });
                Session["TestData"] = dt;
            }
            return dt;
        }
    }

</script>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <h2>
        DataList Example #2</h2>
    <p>
        In this example, the DataList is an Anthem control, and all of the buttons (Edit,
        Update, Cancel) are normal ASP.NET controls. The AnthemNxt.DataList will convert the
        ASP.NET controls to use callbacks. See <a href="DataList.aspx">DataList Example #1</a>
        for an example using Anthem buttons.</p>
    <anthem:DataList ID="dataList" runat="server" OnEditCommand="dataList_EditCommand"
        OnUpdateCommand="dataList_UpdateCommand" OnCancelCommand="dataList_CancelCommand"
        Width="100%">
        <ItemTemplate>
            Name:
            <%# DataBinder.Eval(Container.DataItem, "name") %>
            Age:
            <%# (DateTime.Now - (DateTime)DataBinder.Eval(Container.DataItem, "birthdate")).Days / 365 %>
            <asp:LinkButton ID="edit" runat="server" CommandName="Edit" Text="Edit" />
        </ItemTemplate>
        <EditItemTemplate>
            Name:
            <asp:TextBox ID="name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "name") %>' />
            <br />
            Birthdate:
            <asp:TextBox ID="birthdate" runat="server" Text='<%# ((DateTime)DataBinder.Eval(Container.DataItem, "birthdate")).ToShortDateString() %>' />
            <br />
            <asp:LinkButton ID="update" runat="server" CommandName="Update" Text="Update" />
            <asp:LinkButton ID="cancel" runat="server" CommandName="Cancel" Text="Cancel" />
        </EditItemTemplate>
        <SeparatorTemplate>
            <hr />
        </SeparatorTemplate>
    </anthem:DataList>
</asp:Content>
