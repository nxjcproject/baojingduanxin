$(function () {
    InitDate();
    loadtreegrid("first");
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

//function onOrganisationTreeClick(node) {
//    $('#productLineName').textbox('setText', node.text);
//    organizationid = node.OrganizationId;
//    $('#organizationId').val(organizationId);

//}

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
}

function Query() {
    var organizationName = $('#productLineName').textbox('getText');
    var organizationId = $('#organizationId').val();
    if (organizationId == "") {
        $.messager.alert('警告', '请选择组织机构');
        return;
    }
    var startTime = $('#startDate').datebox('getValue');
    var endTime = $('#endDate').datebox('getValue');
    //var phoneNumber = $('#phonenumber').textbox('getValue');
    var state1 = $('#state').combobox('getValue');
    var phoneNumber = $('#phoneNumber').numberbox('getText');
    //if (phoneNumber=="") {
    //    phoneNumber = 'Null';
    //}
    if (state1 == "All") {
        var state1=""
    }
    var win = $.messager.progress({
        title: '请稍后',
        msg: '数据载入中...'
    });
    $.ajax({
        type: "POST",
        url: "MessageHistoryQuery.aspx/GetQueryData",
        data: '{organizationId: "' + organizationId + '", organizationName: "' + organizationName + '", startTime: "' + startTime + '", endTime:"' + endTime + '", state1: "' + state1 + '", phoneNumber:"' + phoneNumber + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $.messager.progress('close');
            m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData.total == 0) {
                $('#Windows_Report').treegrid('loadData', []);
                $.messager.alert('提示', '没有相关数据！');
            }
            else {
                loadtreegrid("last", m_MsgData);
            }
        },
        beforeSend: function (XMLHttpRequest) {
            win;
        },
        error: function handleError() {
            $.messager.progress('close');
            $('#Windows_Report').treegrid('loadData', []);
            $.messager.alert('失败', '获取数据失败');
        }
    });
}
//function LoadAlrmType() {
//    $.ajax({
//        type: "POST",
//        url: "MessageHistoryQuery.aspx/etComboboxValue",
//        //data: "{myOrganizationId:'" + m_OrganizationId + "'}",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (msg) {
//            var m_MsgData = jQuery.parseJSON(msg.d);
//            //InitializeEnergyConsumptionGrid(m_GridCommonName, m_MsgData);
//            $('#state').combobox({
//                data: m_MsgData.rows,
//                valueField: 'id',
//                textField: 'StandardName',
//                onSelect: function (param) {
//                    m_StandardId = param.value;                 
//                }
//            });
//            //添加中的combobox
//            $('#txtStandardLevel').combobox({
//                data: m_MsgData.rows,
//                valueField: 'value',
//                textField: 'StandardName'
//            });
//        },
//        error: function () {
//            $.messager.alert('提示', '标准层次加载失败！');
//        }
//    });

function loadtreegrid(type, myData) {
    if (type == "first") {
        $('#Windows_Report').treegrid({
            columns: [[
                  { field: 'OrderSendTime', title: '发送时间', width: 200 },
                   // { field: 'LevelCode', title: '层次码', width: 60 },
                    { field: 'GroupKey2', title: '报警分组', width: 100 },
                    { field: 'AlarmText', title: '报警对象', width: 300 },
                    //{ field: 'StartTime', title: '发送状态', width: 80 },
                    { field: 'TYPE_NAME', title: '发送状态', width: 60 },
                    { field: 'PhoneNumber', title: '手机号', width: 100 },
            ]],
            fit: true,
            toolbar: "#toolbar_ReportTemplate",
            rownumbers: true,
            singleSelect: true,
            striped: true,
            idField: "id",
            treeField: "OrderSendTime",
            initialState: "collapsed",
            data: []
        });
    }
    else {
        $('#Windows_Report').treegrid('loadData', myData);
        // $('#gridMain_ReportTemplate').treegrid("collapseAll");
    }
}