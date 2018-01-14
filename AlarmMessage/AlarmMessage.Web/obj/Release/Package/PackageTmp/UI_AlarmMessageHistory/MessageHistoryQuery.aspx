
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
        <div data-options="region:'west',split:true" style="width: 150px;">
            <uc1:OrganisationTree ID="OrganisationTree_ProductionLine" runat="server" />
        </div>
        <!-- 图表开始 -->
        <div id="toolbar_ReportTemplate" style=" height: 60px; padding-top: 10px">                    
                        <table>
                            <tr>
                                <td style ="width:60px; text-align:right;"> 组织机构</td>
                                <td style ="width:115px;"><input id="productLineName" class="easyui-textbox" style="width: 110px;" readonly="readonly" />
                                    <input id="organizationId" readonly="readonly" style="display: none;" />
                                </td>
                                <td style ="width:60px; text-align:right;">开始时间</td>
                                <td style ="width:155px;">
                                    <input id="startDate" type="text" class="easyui-datetimebox" required="required" style="width: 150px;" />
                                </td>
                                 <td style ="width:60px; text-align:right;">手机号</td> 
                                <td style ="width:105px;">
                                    <input id="phoneNumber" class="easyui-numberbox" type="text" style="width:100px;" />
                                </td> 
                                <td style ="width:80px;">(选填)</td>
                                </tr>
                            <tr>
                                <td style ="width:60px; text-align:right;">发送状态</td> 
                                <td>
                                    <select id="state" class="easyui-combobox" data-options="panelHeight:'auto'" required="required" style ="width:110px">
                                        <option value="All" selected ="selected">全部</option>
                                        <option value="99">已发送</option>
                                        <option value="0">正在发送</option>
                                        <option value="80">发送前报警解除</option>
                                        <option value="1">重试满</option>
                                        <option value="2">超期</option>
                                        <option value="3">超条数</option>  
                                        <option value="4">电话号码不合法</option>                     
                                    </select>
                                </td> 
                                <td style ="width:60px; text-align:right;">结束时间</td>
                                <td>
                                    <input id="endDate" type="text" class="easyui-datetimebox" required="required" style="width: 150px;" />
                                </td>
                                <td style ="width:60px; text-align:right;">统计方式</td>
                                <td>
                                    <select id="StaticsMethod" class="easyui-combobox" data-options="panelHeight:'auto'" required="required" style ="width:100px">
                                        <option value="SmsMessage" selected ="selected">按短信</option>
                                        <option value="PhoneNumber">按手机号</option>                     
                                    </select>
                                </td>                                                                      
                                <td><a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-search'"
                                    onclick="Query();">查询</a>
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
