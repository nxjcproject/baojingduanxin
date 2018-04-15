var alarmGroup;
var g_timer;
$(function () {
    alarmGroup = $('#Hiddenfield_PageId').val();
    InitDate();
    loadDataGrid("first");
    LoadSystemAlarmTypeList();
});
//初始化日期框
function InitDate() {
    var nowDate = new Date();
    var beforeDate = new Date();
    beforeDate.setDate(nowDate.getDate() - 1);
    var nowString = nowDate.getFullYear() + '-' + (nowDate.getMonth() + 1) + '-' + nowDate.getDate() + " " + nowDate.getHours() + ":" + nowDate.getMinutes() + ":" + nowDate.getSeconds();
    var beforeString = beforeDate.getFullYear() + '-' + (beforeDate.getMonth() + 1) + '-' + beforeDate.getDate() + " 00:00:00";
    $('#startDate').datetimebox('setValue', beforeString);
    $('#endDate').datetimebox('setValue', nowString);
}
function LoadSystemAlarmTypeList() {
    $.ajax({
        type: "POST",
        url: "AlarmHistoryQuery.aspx/SystemAlarmTypeList",
        data: '{alarmGroup: "' + alarmGroup + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d)['rows'];
            var m_ResultData = [];
            m_ResultData.push({ "AlarmTypeId": "All", "AlarmTypeName": "全部" });
            if (m_MsgData != undefined && m_MsgData != null && m_MsgData.length > 0) {
                for (var i = 0; i < m_MsgData.length; i++) {
                    m_ResultData.push(m_MsgData[i]);
                }
            }

            $('#eventType').combobox('loadData', m_ResultData);
            $('#eventType').combobox("setValue", m_ResultData[0].AlarmTypeId);
        }
    });
}


function loadDataGrid(type, myData) {
    if ("first" == type) {
        $("#Windows_Report").datagrid({
            striped: true,
            rownumbers: true,
            singleSelect: true,
            fit: true,
            remoteSort: false,
            toolbar: '#toolbar_ReportTemplate',
            columns: [[
                { field: 'Name', title: '组织机构', width: 85},
                { field: 'AlarmTypeName', title: '报警类型', width: 100 },
                { field: 'StartTime', title: '开始时间', width: 130 },
                { field: 'EndTime', title: '结束时间', width: 130 },
                { field: 'AlarmText', title: '报警对象', width: 400, sortable: true }
            ]],
        })
    }
    else {
        $("#Windows_Report").datagrid('loadData', myData);
    }
}
function onOrganisationTreeClick(node) {

    // 仅能选中分厂级别
    // 即组织机构ID的层次码 = 5

    //if (node.id.length != 5) {
    //    $.messager.alert('提示', '仅能选择分厂级别。');
    //    return;
    //}

    // 设置组织机构ID
    // organizationId为其它任何函数提供当前选中的组织机构ID

    $('#organizationId').val(node.OrganizationId);
    // 设置组织机构名称
    // 用于呈现，在界面上显示当前的组织机构名称
    $('#productLineName').textbox('setText', node.text);
    clearTimeout(g_timer);
    realtimeAlarm();
}

function query() {
    // 获取组织机构ID
    var organizationId = $('#organizationId').val();

    if (organizationId == '') {
        $.messager.alert('提示', '请先选择需要分析的组织机构。');
        return;
    }

    // 获取起止时间段
    var startTime = $('#startDate').datebox('getValue');
    var endTime = $('#endDate').datebox('getValue');
    var type = $('#eventType').combobox('getValue');
    var m_url = "AlarmHistoryQuery.aspx/GetReportData";
    var m_data = "{organizationId:'" + organizationId + "',startTime:'" + startTime + "',endTime:'" + endTime + "',type:'" + type + "',alarmGroup:'" + alarmGroup + "'}";

    var win = $.messager.progress({
        title: '请稍后',
        msg: '数据载入中...'
    });
    $.ajax({
        type: "POST",
        url: m_url,
        data: m_data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $.messager.progress('close');
            var m_msg = JSON.parse(msg.d);
            if (m_msg.total == 0) {
                //alert("没有查询的数据");
                $('#Windows_Report').datagrid('loadData', []);
                $.messager.alert('提示', '没有相关数据！');
                loadDataGrid("last", []);
            } else {
                loadDataGrid("last", m_msg);
            }
            error: function handleError() {
                $('#Windows_Report').datagrid('loadData', []);
                $.messager.alert('失败', '获取数据失败');
            }
        },
        beforeSend: function (XMLHttpRequest) {
            win;
        }    
    });
  
}
function setTimer() {
    g_timer = setTimeout("realtimeAlarm()", 60000);       //1分钟
}
function realtimeAlarm() {
    if (document.getElementsByName("alarmType")[0].checked == false) {
        clearTimeout(g_timer);
        return;
    }
    else {
        $(".queryDate").hide();
    }
    var win = $.messager.progress({
        title: '请稍后',
        msg: '数据载入中...'
    });
    var organizationId = $('#organizationId').val();
    $.ajax({
        type: "POST",
        url: "AlarmHistoryQuery.aspx/GetRealTimeAlarm",
        data: "{organizationId:'" + organizationId + "',alarmGroup:'" + alarmGroup + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $.messager.progress('close');
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData != null && m_MsgData != undefined) {
                loadDataGrid("last", m_MsgData);
                setTimer();
            }
            else {
                loadDataGrid("last", []);
                setTimer();
            }
        },
        beforeSend: function (XMLHttpRequest) {
            win;
        },
        error: function () {
            $.messager.progress('close');
            setTimer()
        }
    });
}
function setHistory() {
    clearTimeout(g_timer);
    $('#Windows_Report').datagrid("loadData", []);
    $(".queryDate").show();
}
