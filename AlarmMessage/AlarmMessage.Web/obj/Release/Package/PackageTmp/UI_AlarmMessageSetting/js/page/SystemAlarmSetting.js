$(function () {
   // InitTime();
    LoadSystemAlarmTypeList();
    loadDataGrid("first");
    OpenStafftable("first");
    $('#name').textbox({
        onClickButton: function () {
            $('#stafftable').window('open');
            LoasStaffTableData();
        }
    });
});
//初始化日期框
function InitTime() {
    $('#beginTime').timespinner({ min: '00:00', required: true, showSeconds: true });
    $('#endTime').timespinner({ min: '00:00', required: true, showSeconds: true });
}
var mContrastItemId = "";
function loadDataGrid(type, myData) {
    if (type == "first") {
        $('#gridMain_SystemAlarmContrast').datagrid({
            columns: [[
                    { field: 'StaffInfoID', title: '员工编号', width: 60 },
                    { field: 'Name', title: '姓名', width: 60 },
                    { field: 'StartTime', title: '发送时间', width: 80 },
                    { field: 'EndTime', title: '结束时间', width: 80 },
                    { field: 'SendDelay', title: '延时', width: 75 },
                    { field: 'PhoneNumber', title: '手机号', width: 80 },
                    { field: 'AlarmText', title: '报警类别', width: 80 },                   
                    { field: 'MessageType', title: '信息类型', width: 80 },
                    { field: 'Organization', title: '组织机构', width: 80 },
                    {
                        field: 'Enabled', title: '是否可用', width: 60, formatter: function (value, row, index) {
                            if (value=="True") {
                                return value = "是";
                            } else if (value == "False") {
                                return value="否";
                            }
                        }
                    },
                    {
                        field: 'OperateColumn', title: '编辑', width: 60, formatter: function (value,row,index) {
                            return '<a href="#" onclick="editStaffInfo(false, \'' + row.ContrastItemId + '\');">编辑</a>';
                        }
                    }
            ]],
            fit: true,
            toolbar: "#toolbar_ReportTemplate",
            rownumbers: true,
            singleSelect: true,
            striped: true,
            idField: 'ContrastItemId',
            data: [],
            onClickRow: function (index, row) {
                mContrastItemId=row.ContrastItemId;
            }
        });
    }
    else {
        $('#gridMain_SystemAlarmContrast').datagrid('loadData', myData);
        // $('#gridMain_ReportTemplate').datagrid("collapseAll");
    }
}
var mOrganizationID = "";
var mAlarmType = "AllValue";
var mAlarmTypeName = "全部";
function onOrganisationTreeClick(node) {
    $('#productLineName').textbox('setText', node.text);
    mOrganizationID = node.OrganizationId;
    $('#organizationId').val(mOrganizationID);
   
}
var alarmType = "AllValue";
var alarmTypeName = "全部";
function LoadSystemAlarmTypeList() {
    $.ajax({
        type: "POST",
        url: "SystemAlarmSetting.aspx/SystemAlarmTypeList",
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            m_MsgData = jQuery.parseJSON(msg.d);
            $('#SystemAlarmType').combobox({
                data: m_MsgData.rows,
                valueField: 'AlarmTypeId',
                textField: 'AlarmTypeName',
                onSelect: function (param) {
                    mAlarmType = param.AlarmTypeId;
                    mAlarmTypeName = param.AlarmTypeName;
                }
            });        
            $('#alarmType').combobox({
                data: m_MsgData.rows,
                valueField: 'AlarmTypeId',
                textField: 'AlarmTypeName',
                onSelect: function (param) {
                    alarmType = param.AlarmTypeId;
                    alarmTypeName = param.AlarmTypeName;
                }
            });
        }
    });
}
function QuerySystemAlarmContrastFun() {
    if (mOrganizationID == "") {
        $.messager.alert('警告', '请选择组织机构');
        return;
    }
    $.ajax({
        type: "POST",
        url: "SystemAlarmSetting.aspx/GetSystemAlarmContrast",
        data: '{organizationID: "' + mOrganizationID + '", AlarmType: "' + mAlarmType + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData.total == 0) {
                $('#gridMain_SystemAlarmContrast').datagrid('loadData', []);
                $.messager.alert('提示', '没有相关数据！');
            }
            else {
                loadDataGrid("last", m_MsgData);
            }
        },
        error: function handleError() {
            $('#gridMain_SystemAlarmContrast').datagrid('loadData', []);
            $.messager.alert('失败', '获取数据失败');
        }
    });
}


function addFun()
{
    editStaffInfo(true);

}
var mName = "";
var mStaffInfoID = "";
var mPhoneNumber = "";
var organizationId = "";
var mStaffInfoItemId = "";


function OpenStafftable(type,myData)
{  
    if (type == "first") {
        $('#grid_staffinfo').datagrid({
            columns: [[
                       { field: 'StaffInfoID', title: '员工编号', width: 60 },
                       { field: 'Name', title: '姓名', width: 60 },
                       { field: 'PhoneNumber', title: '手机号', width: 80 },
                    //   { field: 'Sex', title: '性别', width: 40 },
                       { field: 'Organization', title: '组织机构', width: 80 },
                       { field: 'WorkingTeamName', title: '工作组', width: 60 }                
            ]],
            fit: true,
       //     rownumbers: true,
            singleSelect: true,
            striped: true,
            data: [],
            onDblClickRow: function (index, row) {
                mName=row.Name;
                mStaffInfoID=row.StaffInfoID;
                mPhoneNumber = row.PhoneNumber;
                organizationId = row.OrganizationID;
                mStaffInfoItemId = row.StaffInfoItemId;

                $('#name').textbox('setText', mName);
                $('#id').textbox('setText', mStaffInfoID);
                $('#phoneNumber').textbox('setText', mPhoneNumber);
                $('#stafftable').window('close');
                //if (row.PhoneNumber == "")
                //    $.messager.alert('提示','请在职工信息表填写该职工手机号！');
            }
        });
    }
    else{
        $('#grid_staffinfo').datagrid('loadData',myData);
    }
}
function LoasStaffTableData() {
    if (mOrganizationID == "") {
        $.messager.alert('警告', '请选择组织机构');
        return;
    }
    $.ajax({
        type: "POST",
        url: "SystemAlarmSetting.aspx/GetStaffTableData",
        data: '{organizationID: "' + mOrganizationID + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData.total == 0) {
                OpenStafftable("last",[]);
                $.messager.alert('提示', '没有相关数据！');
            }
            else {
                OpenStafftable("last", m_MsgData);
            }
        },
        error: function handleError() {
            OpenStafftable("last", []);
            $.messager.alert('失败', '获取数据失败');
        }
    });
}
var isStaffInfoInsert = false;
function editStaffInfo(isInsert, contrastId) {
    //if (authArray[2] == '0') {
    //    $.messager.alert("提示", "该用户没有编辑权限！");
    //    return;
    //}
    if (isInsert) {
        isStaffInfoInsert = true;
        //初始化
        $('#alarmType').combobox('setValue', "AllValue");
        $('#name').textbox('setText',"");
        $('#id').textbox('setText', "");
        $('#phoneNumber').textbox('setText', "");
        $('#beginTime').timespinner('setValue',"00:00:00");
        $('#endTime').timespinner('setValue', "23:59:59");
        $('#delay').combobox('setValue',"0");
        $('#enabled').combobox('setValue', "是");
    }
    else {
        isStaffInfoInsert = false;    
        $('#gridMain_SystemAlarmContrast').datagrid('selectRecord', contrastId);
        var data = $('#gridMain_SystemAlarmContrast').datagrid('getSelected');
        alarmType = data.AlarmTypeId;
        alarmTypeName = data.AlarmTypeName;
        mName = data.Name;
        mStaffInfoID = data.StaffInfoID;
        mPhoneNumber = data.PhoneNumber;
        organizationId = data.OrganizationID;
        mStaffInfoItemId = data.StaffInfoItemId;
        $('#alarmType').combobox('select', data.AlarmTypeId);
        $('#name').textbox('setText', data.Name);
        $('#id').textbox('setText', data.StaffInfoID);
        $('#phoneNumber').textbox('setText', data.PhoneNumber);  
        $('#beginTime').timespinner('setValue', data.StartTime);
        $('#endTime').timespinner('setValue', data.EndTime);
        $('#delay').combobox('setValue',data.SendDelay);
        $('#enabled').combobox('setValue', data.Enabled);
    }
    $('#AddandEditor').window('open');
}
function save()
{
    //var mName = "";
    //var mStaffInfoID = "";
    //var mPhoneNumber = "";
    //var organizationId = "";
    //var mStaffInfoItemId = "";

    if (mName == "") {
        $.messager.alert('提示', '请选择需要发送报警短信的用户！');
    }
    var mBeginTime = $('#beginTime').val();
    var mEndTime = $('#endTime').val();
    //if (mBeginTime >= mEndTime)
    //    $.messager.alert('提示', '请选择正确的短信发送时间段！');
    var mdelay = $('#delay').combobox('getValue');
    var mEnabled = $('#enabled').combobox('getValue');


   
    $.ajax({
        type: "POST",
        url: "SystemAlarmSetting.aspx/AddSystemAlarmStaffInfo",
        data: '{isStaffInfoInsert:"' + isStaffInfoInsert + '",contrastItemId:"' + mContrastItemId + '",organizationID: "' + organizationId + '",alarmType:"' + alarmType + '",alarmTypeName:"' + alarmTypeName + '",phoneNumber:"' + mPhoneNumber + '",staffInfoItemId:"' + mStaffInfoItemId + '",beginTime:"' + mBeginTime + '",endTime:"' + mEndTime + '",delay:"' + mdelay + '",enabled:"' + mEnabled + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#AddandEditor').window('close');
            m_MsgData = msg.d;
            if (isStaffInfoInsert) {            
                $.messager.alert('提示', m_MsgData + '条记录添加成功！');        
            }
            else {
                $.messager.alert('提示', m_MsgData + '条记录编辑成功！');
            }
            QuerySystemAlarmContrastFun();        
        },
        error: function handleError() {
            $('#AddandEditor').window('close');
            $.messager.alert('失败', '添加记录失败！');
            QuerySystemAlarmContrastFun();
        }
    });
}
function RefreshFun() {
    QuerySystemAlarmContrastFun();
}
function cancelFun()
{
    //mContrastItemId
    if (mContrastItemId != "") {
        $.ajax({
            type: "POST",
            url: "SystemAlarmSetting.aspx/CancelSystemAlarmStaffInfo",
            data: '{contrastItemId: "' + mContrastItemId +'"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                m_MsgData = msg.d;
                $.messager.alert('提示', m_MsgData + '条记录删除成功！');
                QuerySystemAlarmContrastFun();

            },
            error: function handleError() {          
                $.messager.alert('失败', '该记录删除失败！');
                QuerySystemAlarmContrastFun();
            }
        });
    } else {
        $.messager.alert('提示','请选择要删除的记录！');
    }
}