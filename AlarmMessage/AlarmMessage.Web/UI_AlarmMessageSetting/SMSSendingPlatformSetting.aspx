<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSSendingPlatformSetting.aspx.cs" Inherits="AlarmMessage.Web.UI_AlarmMessageSetting.SMSSendingPlatformSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>短信发送平台设置</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <!--[if lt IE 8 ]><script type="text/javascript" src="/js/common/json2.min.js"></script><![endif]-->
    <script type="text/javascript" src="/lib/ealib/extend/jquery.PrintArea.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/extend/jquery.jqprint.js" charset="utf-8"></script>

    <script type="text/javascript" src="js/page/SMSSendingPlatformSetting.js" charset="utf-8"></script>
</head>
<body>
      <div id="layout_Main" class="easyui-layout" data-options="fit:true,border:false">
        <div data-options="region:'center',fit:true,border:false"></div>
      </div>
      <!--创建短信平台设置dialog-->
        <div id="dlg_SmsSetting" style="padding-left:8px;">
            <%--<fieldset>--%>
                <%--<legend>短信发送平台设置</legend>--%>
                <table style="width: 100%;border-collapse:separate; border-spacing:0px 1px;">
                    <tr>
                        <th style="height: 27px;">名称</th>
                        <td>
                            <input id="TextBox_SmsName" class="easyui-textbox" style="width: 180px;" />
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">接口地址</th>
                        <td>
                            <input id="TextBox_InterfaceAddress" class="easyui-textbox" style="width: 340px;" />
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">接口端口</th>
                        <td>
                            <input id="TextBox_InterfacePort" class="easyui-textbox" style="width: 180px;" />
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">用户代码</th>
                        <td>
                            <input id="TextBox_UserCode" class="easyui-textbox" style="width: 180px;" />
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">用户ID</th>
                        <td>
                            <input id="TextBox_UserId" class="easyui-textbox" style="width: 180px;" />
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">短信模版</th>
                        <td>
                            <input id="TextBox_SmsTemplate" class="easyui-textbox" style="width: 180px;" />
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">每天发送的最大数量</th>
                        <td>
                            <input id="TextBox_MaxSmsPerNumberOnDay" class="easyui-textbox" style="width: 180px;" /><span>（同一号码）</span>
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">每条短信重试次数</th>
                        <td>
                            <input id="TextBox_MaxSendTimesPerSms" class="easyui-textbox" style="width: 180px;" />
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">短信内容最大长度</th>
                        <td>
                            <input id="TextBox_MaxSmsWordLength" class="easyui-textbox" style="width: 180px;" />
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">短信失效时间</th>
                        <td>
                            <input id="TextBox_InvalidTime" class="easyui-textbox" style="width: 180px;" /><span>（分钟）</span>
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">备注</th>
                        <td>
                            <input id="TextBox_Remark" class="easyui-textbox" style="width: 180px;" />
                        </td>
                    </tr>
                    <tr>
                        <th style="height: 27px;">是否开启</th>
                        <td>
                            <select class="easyui-combobox" id="Combo_Enable" style="width:100px" data-options="panelHeight: 'auto'">
                                    <option value="True">启用</option>
                                    <option value="False">停用</option>
                            </select>
                        </td>
                    </tr>                 
                </table>
            <%--</fieldset>--%>
        </div>
       <!-- 创建button -->
        <div id="buttons">
            <table cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td style="text-align: left">
                        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="AmendPasswordWindow();">修改密码</a>
                    </td>                             
                    <td style="text-align: center">
                        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true" onclick="RefreshFun();">刷新</a>
                    </td>                              
                    <td style="text-align: right">
                        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="ConfirmSave();">保存</a>
                    </td>
                </tr>
            </table>
        </div>

      <div id="AmendPassword" class="easyui-window" title="修改密码" data-options="modal:true,closed:true,iconCls:'icon-edit',minimizable:false,maximizable:false,collapsible:false,resizable:false,plain:true" style="width:300px;height:180px;padding:8px 25px 8px 25px"> 
       <div class="easyui-layout" data-options="fit:true,border:false" >    
         <div id="toorBarWindows" data-options="region:'center'" style="padding:10px;">          
                <table>
                    <tr>            
                        <td>原密码：</td>
                        <td>
                             <input id="oldPassword" class="easyui-textbox" type="password" style="width:150px;"/>
                        </td>                                                                                                                      
                    </tr>
                    <tr>            
                        <td>新密码：</td>
                        <td>
                             <input id="newPassword" class="easyui-textbox" type="password" style="width:150px;"/>
                        </td>                                                                                                                      
                    </tr>
                 </table>                                                         
         </div>
         <div data-options="region:'south'" style="text-align:center;padding:5px;">
	        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" onclick="ConfirmAmendPassword();">确定</a>
	        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="CancelPassword();">取消</a>
         </div>                          
     </div>
   </div>
   <form id="Form1" runat="server"></form>    
</body>
</html>
