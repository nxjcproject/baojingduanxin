
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageHistoryQuery.aspx.cs" Inherits="AlarmMessage.Web.UI_AlarmMessageHistory.MessageHistoryQuery" %>

<%@ Register Src="/UI_WebUserControls/OrganizationSelector/OrganisationTree.ascx" TagName="OrganisationTree" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>短信历史查询</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <!--[if lt IE 8 ]><script type="text/javascript" src="/js/common/json2.min.js"></script><![endif]-->
    <script type="text/javascript" src="/lib/ealib/extend/jquery.PrintArea.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/extend/jquery.jqprint.js" charset="utf-8"></script>

    <script type="text/javascript" src="js/page/MessageHistoryQuery.js" charset="utf-8"></script>
</head>
<body>
 <div class="easyui-layout" data-options="fit:true,border:false">
        <div data-options="region:'west',split:true" style="width: 230px;">
            <uc1:OrganisationTree ID="OrganisationTree_ProductionLine" runat="server" />
        </div>
        <!-- 图表开始 -->
        <div id="toolbar_ReportTemplate" style=" height: 40px; padding-top: 10px">
            <table>
                <tr>
                    <td>                     
                        <table>
                            <tr>
                                <td > 组织机构：</td>
                                <td><input id="productLineName" class="easyui-textbox" style="width: 90px;" readonly="readonly" />
                                    <input id="organizationId" readonly="readonly" style="display: none;" />
                                </td>
                                <td>开始时间：</td>
                                <td>
                                    <input id="startDate" type="text" class="easyui-datetimebox" required="required" style="width: 150px;" />
                                </td>
                                <td>结束时间：</td>
                                <td>
                                    <input id="endDate" type="text" class="easyui-datetimebox" required="required" style="width: 150px;" />
                                </td> 
<%--                                <td>电话号码：</td>     
                                <td>
                                    <input id="phonenumber" type="text" class="easyui-textbox" style="width: 150px;" />
                                </td>--%>

<%--                                <td>发送状态：</td>
                                <td>
                                    <input id="state" class="easyui-combobox" data-options="panelHeight:'auto'" required="required" style="width: 150px;" />
                                </td>--%>
                                <td>发送状态：</td> 
                                <td>
                                    <select id="state" class="easyui-combobox" data-options="panelHeight:'auto'" required="required" style="width: 150px;">
<%--                                        <option value="All">全部</option>--%>
                                        <option value="99">已发送</option>
                                        <option value="0">重试</option>
                                        <option value="1">重试满</option>
                                        <option value="2">超期</option>
                                        <option value="3">超短信条数</option>                     
                                         </select>
                                </td>                                                                                   
                                <td><a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true"
                                    onclick="Query();">查询</a>
                                </td>               
                           </tr>
                       </table>                 
                    </td>
                </tr>
            </table>
        </div>
        <div id="reportTable" class="easyui-panel" data-options="region:'center', border:true, collapsible:true, split:false">
            <table id="Windows_Report"></table>
        </div>
        <!-- 图表结束 -->
     </div>
</body>
</html>
