var mSmsItemId = "";
$(function () {
    LoadModifyPasswordDialog('dlg_SmsSetting');
    OnMainLayoutResize('layout_Main', 'dlg_SmsSetting');
    AllowIntegers();
})

function OnMainLayoutResize(myLayoutId, myDialogId) {

    var m_CenterPanelWidth = $('#' + myLayoutId).layout('panel', 'center').panel('options').width;
    var m_CenterPanelHeight = $('#' + myLayoutId).layout('panel', 'center').panel('options').height;
    SetDialogPosization(myDialogId, m_CenterPanelWidth, m_CenterPanelHeight);

    $('#' + myLayoutId).layout('panel', 'center').panel({
        onResize: function (width, height) {
            try {
                SetDialogPosization(myDialogId, width, height);
            }
            catch (e)
            { }
        }
    });
}
function SetDialogPosization(myDialogId, myWidth, myHeight) {
    var m_DialogTop = myHeight > 480 ? (myHeight - 480) / 2 : 0;
    var m_DialogLeft = myWidth > 540 ? (myWidth - 540) / 2 : 0;
    $('#' + myDialogId).dialog('move', {
        left: m_DialogLeft,
        top: m_DialogTop
    });
}
//////////////////////////////加载短信平台设置dialog//////////////////////////
function LoadModifyPasswordDialog(myDialogId) {
    $('#' + myDialogId).dialog({
        title: '短信发送平台设置',
        width: 540,
        height: 440,
        resizable: false,
        draggable: false,
        closable: false,
        closed: false,
        cache: false,
        modal: false,
        buttons: "#buttons"
    });
}

function RefreshFun() {
    $.ajax({
        type: "POST",
        url: "SMSSendingPlatformSetting.aspx/GetSmsConfigInfo",
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData.total == 0) {              
                $.messager.alert('提示', '没有相关数据！');
            }
            else {
                WriteToPage(m_MsgData);
                mSmsItemId = m_MsgData.rows[0]["SmsItemId"];
            }
        },
        error: function handleError() {
            $.messager.alert('失败', '获取数据失败');
        }
    });
}

function WriteToPage(myData) {
    $('#TextBox_SmsName').textbox('setText', myData.rows[0]["SmsName"]);
    $('#TextBox_InterfaceAddress').textbox('setText', myData.rows[0]["InterfaceAddress"]);
    $('#TextBox_InterfacePort').textbox('setText', myData.rows[0]["InterfacePort"]);
    $('#TextBox_UserCode').textbox('setText', myData.rows[0]["UserCode"]);
    $('#TextBox_UserId').textbox('setText', myData.rows[0]["UserId"]);
    $('#TextBox_SmsTemplate').textbox('setText', myData.rows[0]["SmsTemplate"]);
    $('#TextBox_MaxSmsPerNumberOnDay').textbox('setText', myData.rows[0]["MaxSmsPerNumberOnDay"]);
    $('#TextBox_MaxSendTimesPerSms').textbox('setText', myData.rows[0]["MaxSendTimesPerSms"]);
    $('#TextBox_MaxSmsWordLength').textbox('setText', myData.rows[0]["MaxSmsWordLength"]);
    $('#TextBox_InvalidTime').textbox('setText', myData.rows[0]["InvalidTime"]);
    $('#TextBox_Remark').textbox('setText', myData.rows[0]["Remark"]);
    $('#Combo_Enable').combobox('setValue', myData.rows[0]["Enabled"]);
}

function ConfirmSave() {
    $.messager.confirm('提示', '是否确认保存?', function (r) {
        if (r) {
            SaveFun();
        }
    });
}

function SaveFun() {
    var mSmsName = $('#TextBox_SmsName').textbox('getText');
    var mInterfaceAddress = $('#TextBox_InterfaceAddress').textbox('getText');
    var mInterfacePort = $('#TextBox_InterfacePort').textbox('getText');
    var mUserCode = $('#TextBox_UserCode').textbox('getText');
    var mUserId = $('#TextBox_UserId').textbox('getText');
    var mSmsTemplate = $('#TextBox_SmsTemplate').textbox('getText');
    var mMaxSmsPerNumberOnDay = $('#TextBox_MaxSmsPerNumberOnDay').textbox('getText');
    var mMaxSendTimesPerSms = $('#TextBox_MaxSendTimesPerSms').textbox('getText');
    var mMaxSmsWordLength = $('#TextBox_MaxSmsWordLength').textbox('getText');
    var mInvalidTime = $('#TextBox_InvalidTime').textbox('getText');
    var mRemark = $('#TextBox_Remark').textbox('getText');
    var mEnabled = $('#Combo_Enable').combobox('getValue');

    $.ajax({
        type: "POST",
        url: "SMSSendingPlatformSetting.aspx/SaveSmsConfigInfo",
        data: '{mSmsItemId: "' + mSmsItemId + '",mSmsName: "' + mSmsName + '",mInterfaceAddress: "' + mInterfaceAddress + '",mInterfacePort: "' + mInterfacePort + '",mUserCode: "' + mUserCode + '",mUserId: "' + mUserId + '",mSmsTemplate: "' + mSmsTemplate + '",mMaxSmsPerNumberOnDay: "' + mMaxSmsPerNumberOnDay + '",mMaxSendTimesPerSms: "' + mMaxSendTimesPerSms + '",mMaxSmsWordLength: "' + mMaxSmsWordLength + '",mInvalidTime: "' + mInvalidTime + '",mRemark: "' + mRemark + '",mEnabled: "' + mEnabled + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var myData = msg.d;
            if (myData == 1) {
                $.messager.alert('提示', '保存成功！');             
                RefreshFun();
            }
            else {
                $.messager.alert('提示', '保存失败！');
                RefreshFun();
            }
        },
        error: function () {
            $.messager.alert('提示', '保存失败！');
            refresh();
        }
    });
}

function AmendPasswordWindow() {
    $('#AmendPassword').window('open');
}

function CancelPassword() {
    $('#AmendPassword').window('close');
    $('#oldPassword').textbox('clear');
    $('#newPassword').textbox('clear');
}

function ConfirmAmendPassword() {
    $.messager.confirm('提示', '是否确认修改密码?', function (r) {
        if (r) {
            AmendPassword();
        }
    });
}

function AmendPassword() {
    var mOldPwd = $('#oldPassword').textbox('getText');
    var mNewPwd = $('#newPassword').textbox('getText');
    $.ajax({
        type: "POST",
        url: "SMSSendingPlatformSetting.aspx/AmendPasswordInfo",
        data: "{mSmsItemId:'" + mSmsItemId + "',mOldPwd:'" + mOldPwd + "',mNewPwd:'" + mNewPwd + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == "1") {
                $('#oldPassword').attr('value', '');
                $('#newPassword').attr('value', '');
                $.messager.alert('提示', '修改密码成功！');
                CancelPassword();
            }
            //else if (msg.d == "0") {
            //    $.messager.alert('提示', '原密码不正确！');
            //    $('#oldPassword').textbox('clear');
            //    $('#newPassword').textbox('clear');
            //}
            else {
                $.messager.alert('提示', msg.d);
                $('#oldPassword').textbox('clear');
                $('#newPassword').textbox('clear');
            }
        }
    });  
}

function AllowIntegers() {
    $("#TextBox_MaxSmsPerNumberOnDay").textbox('textbox').bind('keyup', function (e) {
        $("#TextBox_MaxSmsPerNumberOnDay").textbox('setValue', $(this).val().replace(/\D/g, ''))
    });
    $("#TextBox_MaxSendTimesPerSms").textbox('textbox').bind('keyup', function (e) {
        $("#TextBox_MaxSendTimesPerSms").textbox('setValue', $(this).val().replace(/\D/g, ''))
    });
    $("#TextBox_MaxSmsWordLength").textbox('textbox').bind('keyup', function (e) {
        $("#TextBox_MaxSmsWordLength").textbox('setValue', $(this).val().replace(/\D/g, ''))
    });
    $("#TextBox_InvalidTime").textbox('textbox').bind('keyup', function (e) {
        $("#TextBox_InvalidTime").textbox('setValue', $(this).val().replace(/\D/g, ''))
    });
}