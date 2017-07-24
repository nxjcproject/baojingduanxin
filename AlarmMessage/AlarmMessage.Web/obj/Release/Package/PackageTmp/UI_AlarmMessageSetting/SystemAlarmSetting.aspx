<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemAlarmSetting.aspx.cs" Inherits="AlarmMessage.Web.UI_AlarmMessageSetting.SystemAlarmSetting" %>

<%@ Register Src="/UI_WebUserControls/OrganizationSelector/OrganisationTree.ascx" TagName="OrganisationTree" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <!--[if lt IE 8 ]><script type="text/javascript" src="/js/common/json2.min.js"></script><![endif]-->
    <script type="text/javascript" src="/lib/ealib/extend/jquery.PrintArea.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/extend/jquery.jqprint.js" charset="utf-8"></script>

    <script type="text/javascript" src="js/page/SystemAlarmSetting.js" charset="utf-8"></script>
</head>
<body>
 <div class="easyui-layout" data-options="fit:true,border:false">
        <div data-options="region:'west',split:true" style="width: 150px;">
            <uc1:OrganisationTree ID="OrganisationTree_ProductionLine" runat="server" />
        </div>
        <!-- 图表开始 -->
        <div id="toolbar_ReportTemplate" style="display: none;">
            <table>
                <tr>
                    <td>                     
                        <table>
                            <tr>
                                <td style="width:80px"> 组织机构：</td>
                                <td><input id="productLineName" class="easyui-textbox" style="width: 90px;" readonly="readonly" />
                                    <input id="organizationId" readonly="readonly" style="display: none;" />
                                </td>                                                                                           
                                <td><a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true"
                                    onclick="QuerySystemAlarmContrastFun();">查询</a>
                                </td>               
                           </tr>
                       </table>                 
                    </td>

                </tr>
                <tr>
                    <td>
                        <table>  
                                 <tr>                                                              
                                    <td><a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true"
                                        onclick="RefreshFun();">刷新</a>
                                    </td>
                                    <td>
                                        <a id="id_add" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="addFun()">添加</a>
                                    </td>
                             
<%--                                 <td>
                                        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="saveFun();">保存</a>
                                    </td>--%>
                         <%--            <td>
                                        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-undo',plain:true" onclick="reject()">撤销</a>
                                     </td>--%>
                                     <td>
                                        <a id="id_deleteAll" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel',plain:true" onclick="cancelFun();">删除</a>
                                    </td>                                  
                                    </tr>                          
                         </table>
                    </td>
                </tr>
            </table>
        </div>

        <div id="reportTable" class="easyui-panel" data-options="region:'center', border:true, collapsible:true, split:false">
            <table id="gridMain_SystemAlarmContrast"></table>
        </div>
        <!-- 图表结束 -->


         <!-- 编辑窗口开始 -->
            <div id="AddandEditor" class="easyui-window" title="短信接收" data-options="modal:true,closed:true,iconCls:'icon-edit',minimizable:false,maximizable:false,collapsible:false,resizable:false" style="width:400px;height:auto;padding:10px 60px 20px 60px">
	    	    <table>
                     <tr>
	    			    <td>报警类型：</td>
	    			    <td>
                          <input class="easyui-combobox" id="alarmType"  style="width:100px" data-options="panelHeight: 'auto'"/>           
	    			    </td>
	    		    </tr>
                    <tr>
	    			    <td>姓名：</td> 
	    			    <td><input class="easyui-textbox" type="text" id="name" name="name" data-options="required:true,buttonText:'查询',buttonIcon:'icon-search',prompt:'Search...'"style="width:160px" /></td>
	    		    </tr>
	    		    <tr>
	    			    <td>工号：</td>
	    			    <td><input class="easyui-textbox" type="text" id="id" name="id"  readonly="readonly" style="width:100px" />（非填项）</td>
	    		    </tr>
	    		   <%--  <tr>
	    			    <td>手机号码：</td>
	    			    <td><input class="easyui-textbox" id="phoneNumber" name="phoneNumber" readonly="readonly" style="width:100px" />（非填项）</td>
	    		    </tr>--%>
                     <tr>
	    			    <td>发送时间：</td>
	    			    <td>
                            <input id="beginTime" class="easyui-timespinner"  style="width:80px;" value="00:00:00" required="required" data-options="min:'00:00',showSeconds:true">-
                            <input id="endTime" class="easyui-timespinner"  style="width:80px;"  value="23:59:59" required="required" data-options="min:'00:00',showSeconds:true">
	    			    </td>
	    		    </tr>
	    		    <tr>
	    			    <td>延时：</td>
	    			    <td>
                          <%--<input class="easyui-textbox" id="delay" name="delay" style="width:100px" />分钟--%>
                             <select class="easyui-combobox" id="delay" name="delay" style="width:100px" data-options="panelHeight: 'auto'">
                                <option>0</option>
                                <option>10</option>
                                <option>20</option>
                                <option>30</option>
                                <option>40</option>
                                <option>60</option>
                            </select>分钟
	    			    </td>
	    		    </tr>
                        <tr>
	    			    <td>是否可用：</td>
	    			    <td>
                            <select class="easyui-combobox" id="enabled" name="enabled" style="width:100px" data-options="panelHeight: 'auto'">
                                <option value="True">是</option>
                                <option value="False">否</option>
                            </select>
	    			    </td>
	    		    </tr>
	    	    </table>
	            <div style="text-align:center;padding:5px;margin-left:-18px;">
	    	        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" onclick="save()">保存</a>
	    	        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="$('#AddandEditor').window('close');">取消</a>
	            </div>
            </div>
            <!-- 编辑窗口开始 -->
             <!-- 编辑窗口开始 -->
            <div id="stafftable" class="easyui-window" title="短信接收" data-options="modal:true,closed:true,iconCls:'icon-edit',minimizable:false,maximizable:false,collapsible:false,resizable:false" style="width:420px;height:400px;padding:2px 10px 2px 10px">
	    	    <table id="grid_staffinfo"></table>   
            </div>
            <!-- 编辑窗口开始 -->


    </div>
    <form id="form_EnergyConsumptionPlan" runat="server">
        <div>
            <asp:HiddenField ID="Hiddenfield_PageId" runat="server" />
        </div>
    </form>
</body>
</html>
  