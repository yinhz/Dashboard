/// <reference path="../../layer/layer.js" />
/// <reference path="../../jquery-2.1.4.js" />
/// <reference path="jquery-1.3.2-vsdoc2.js" />

function addTerminal() {

    var termianlId = $("#add_i_terminalId").val();

    if (termianlId == undefined || termianlId == "") {
        alert("Must input terminal's identification");
        return;
    }

    if (!confirm("Confirm Add？")) {
        return;
    }

    $.getJSON(
        "/api/Config/AddTerminalInfo/" + termianlId,
        { dataType: "json" },
        function (data) {
            alert("New success");

            location.reload();
        });
}

function deleteConfig(configId, ele) {
    if (!confirm("Confirm delete？")) {
        return;
    }
    $.get(
        "/api/Config/DeleteTerminalInfoConfig/" + configId,
        function (data, status) {
            alert("Delete success");
            $(ele).closest("tr").remove();
        });
}

var _curTerminalInfo = {};

// 编辑区域 控制函数

function saveConfig() {

    _curTerminalInfo.TerminalName = $("#e_terminalname").val();
    //_curTerminalInfo.DashBoardId = $("#e_dashboardname").val();
    _curTerminalInfo.IsForbidden = $("#e_isforbidden").val();
    //_curTerminalInfo.Paras=$.parseJSON($("#e_paras").val());
    //_curTerminalInfo.Paras = $.parseJSON(editor1.getText());
    _curTerminalInfo.Paras = editor1.getText();

    _curTerminalInfo.Theme = $("#e_theme").val();

    $.ajax({
        dataType: "json",
        data: JSON.stringify(_curTerminalInfo),
        contentType: "application/json; charset=UTF-8",
        type: "post",
        url: "/api/Config/SaveTerminalInfo",
        success: function (result) {
            if (result = true) {
                alert("Save success");

                location.reload();
            }
        }
    });
}

function showEditor(txt) {

    _curTerminalInfo = $.parseJSON(txt);

    $("#e_terminalname").val(_curTerminalInfo.TerminalName);
    //$("#e_dashboardname").val(_curTerminalInfo.DashBoardId);
    $("#e_isforbidden").val("" + _curTerminalInfo.IsForbidden);
    //$("#e_paras").val(JSON.stringify(_curTerminalInfoObj.Paras));

    $("#e_theme").val("" + _curTerminalInfo.Theme);
    $("#e_dashboard_theme").val("" + _curTerminalInfo.Theme);

    setDashboardNo();

    GenDashboardTable();

    // 防止 参数为 null
    initEditor("" + _curTerminalInfo.Paras);
    $("#e_dashboard_para").val(_curTerminalInfo.Paras);
    //initEditor(JSON.stringify(_curTerminalInfo.Paras));

    //页面层
    layer.open({
        type: 1,
        title: '',
        skin: 'layui-layer-rim', //加上边框
        area: ['1024px', '500px'], //宽高
        content: $("#editArea")
    });

    //$("html,body").animate({ scrollTop: $("#editArea").offset().top }, 500);
}

function setDashboardNo() {
    if (_curTerminalInfo.Dashboards != null) {
        var no = 1;
        $.each(_curTerminalInfo.Dashboards, function (index, val) {
            val.No = no++;
        });
    }
}

function GenDashboardTable() {
    setDashboardNo();

    if (_curTerminalInfo.Dashboards != null) {
        GenTableByHead(
            "dashboard_table",
            _curTerminalInfo.Dashboards,
            {
                "No": "No",
                "DashBoardName": "Dashboard Name",
                "PlayOrder": "Order",
                "PlayCycle": "Cycle(second)",
                "Theme": "Theme",
                "Paras": "Paras"
            });

        $("#dashboard_table thead tr").append("<th>Operation</th>");
        $("#dashboard_table tbody tr").append(
            "<td><a class=\"btn btn-primary btn-sm active\" onclick=\"MoveUp($($(this).closest('tr').find(\'td\')[0]).html())\">Up</a><a class=\"btn btn-primary btn-sm active\" onclick=\"MoveDown($($(this).closest('tr').find(\'td\')[0]).html())\">Down</a><a class=\"btn btn-primary btn-sm active\" onclick=\"RemoveDashboard($($(this).closest('tr').find(\'td\')[0]).html())\">Remove</a></td>");
    }
    else {
        $("#dashboard_table").html("");
    }
}

// 增加元素到看板
function addDashboard() {
    if (_curTerminalInfo.Dashboards == undefined)
        _curTerminalInfo.Dashboards = [];

    var no = 1;
    if (_curTerminalInfo.Dashboards != null) {
        no = _curTerminalInfo.Dashboards.length + 1;
    }

    _curTerminalInfo.Dashboards.push({
        "No": no,
        "DashBoardId": $("#s_dashboardname").val(),
        "DashBoardName": $("#s_dashboardname option:selected").text(),
        "PlayOrder": $("#e_playorder").val(),
        "PlayCycle": $("#e_playcycle").val(),
        "Theme": $("#e_dashboard_theme").val(),
        "Paras": $("#e_dashboard_para").val()
    });

    // 重新生成元素表格
    GenDashboardTable();
}

function MoveUp(no) {
    if (_curTerminalInfo == null)
        return;

    $.each(_curTerminalInfo.Dashboards, function (index, val) {
        if (val.No == no) {

            if (index == 0)
                return false;

            var temp_order = _curTerminalInfo.Dashboards[index - 1].PlayOrder;
            _curTerminalInfo.Dashboards[index - 1].PlayOrder = val.PlayOrder;
            val.PlayOrder = temp_order;

            // 重新生成元素表格
            GenDashboardTable();
            return false;//相当于break
        }
    });
}

function MoveDown(no) {
    if (_curTerminalInfo == null)
        return;

    $.each(_curTerminalInfo.Dashboards, function (index, val) {
        if (val.No == no) {

            if (index == $(_curTerminalInfo.Dashboards).length - 1)
                return false;

            var temp_order = _curTerminalInfo.Dashboards[index + 1].PlayOrder;
            _curTerminalInfo.Dashboards[index + 1].PlayOrder = val.PlayOrder;
            val.PlayOrder = temp_order;

            // 重新生成元素表格
            GenDashboardTable();
            return false;//相当于break
        }
    });
}

function RemoveDashboard(no) {
    if (_curTerminalInfo == null)
        return;

    $.each(_curTerminalInfo.Dashboards, function (index, val) {
        if (val.No == no) {
            _curTerminalInfo.Dashboards.splice(index, 1);
            // 重新生成元素表格
            GenDashboardTable();
            return false;//相当于break
        }
    });
}

$("#selectAll").bind("click", function () {
    if ($("#selectAll")[0].checked == true) {
        $("[name='rowChkItem']").each(function (index, item) {
            item.checked = true;
        });
    }
    else {
        $("[name='rowChkItem']").each(function (index, item) {
            item.checked = false;
        })
    }
});

function redirectTerminal() {
    var selectedItems = $("[name='rowChkItem']:checked");
    if (selectedItems.length == 0) {
        alert("Select at least one row");
        return;
    }

    //页面层
    layer.open({
        type: 1,
        title: '',
        skin: 'layui-layer-rim', //加上边框
        //area: ['600', '300px'], //宽高
        content: $("#redirectArea")
    });
}

function saveRedirectConfig() {

    var selectedTers = [];

    $("[name='rowChkItem']:checked").each(function (index, item) {
        selectedTers.push(item.value);
    });

    var data = {
        terminals: selectedTers,
        targetTerminal: $("#e_redirectTer").val()
    };

    $.ajax({
        dataType: "json",
        data: JSON.stringify(data),
        contentType: "application/json; charset=UTF-8",
        type: "post",
        url: "/api/Config/RedirectTerminalInfo",
        success: function (result) {
            if (result == true) {
                alert("Save success");
            }
            else {
                alert("Save failed");
            }
            location.reload();
        }
    });
}

activeli($("#nav_li_terminal"));