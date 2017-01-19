/// <reference path="jquery-1.3.2-vsdoc2.js" />

function deleteConfig(configId, ele) {

    if (!confirm("Confirm delete？")) {
        return;
    }

    $.get(
        "/api/Config/DeleteDataConfig/" + configId,
        function (data, status) {
            alert("Delete success");

            location.reload();
            //$(ele).closest("tr").remove();
        });
}

function cloneConfig(configId, ele) {

    if (!confirm("Confirm duplicate？")) {
        return;
    }

    $.get(
        "/api/Config/CloneDataConfig/" + configId,
        function (data, status) {
            alert("Duplicate success");

            location.reload();
            //$(ele).closest("tr").remove();
        });
}

var _curDataConfigObj = {};

// 编辑区域 控制函数

function addConfig() {
    $.getJSON(
        "/api/Config/AddDataConfig",
        { dataType: "json" },
        function (data) {
            alert("New success");

            location.reload();
        });
}

function saveConfig() {
    _curDataConfigObj.DataName = $("#e_dataname").val();
    _curDataConfigObj.DataBaseName = $("#e_databasename").val();
    _curDataConfigObj.IsAutoRefresh = $("#e_isautorefresh").val();
    _curDataConfigObj.Interval = $("#e_interval").val();
    _curDataConfigObj.Sql = editor_sql.getValue();

    $.ajax({
        dataType: "json",
        data: JSON.stringify(_curDataConfigObj),
        contentType: "application/json; charset=UTF-8",
        type: "post",
        url: "/api/Config/SaveDataConfig",
        success: function (result) {
            if (result == true) {
                alert("Save success");

                location.reload();
            }
            else {
                alert("Save failed");
            }
        }
    });
}

function showEditor(curObj, isScrollTo, tr) {

    $("#maintable tbody tr").removeClass("info");
    $(tr).addClass("info");

    _curDataConfigObj = curObj;

    $("#e_dataid").val(_curDataConfigObj.DataId);
    $("#e_dataname").val(_curDataConfigObj.DataName);
    $("#e_databasename").val(_curDataConfigObj.DataBaseName);
    $("#e_isautorefresh").val(""+_curDataConfigObj.IsAutoRefresh);
    $("#e_interval").val(_curDataConfigObj.Interval);

    editor_sql.setValue(_curDataConfigObj.Sql);

    if (isScrollTo) {
        $("html,body").animate({ scrollTop: $("#editArea").offset().top }, 500);
    }
}

activeli($("#nav_li_data"));