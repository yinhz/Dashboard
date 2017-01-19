/// <reference path="jquery-1.3.2-vsdoc2.js" />

function deleteConfig(configId, ele) {

    if (!confirm("Confirm delete？")) {
        return;
    }

    $.get(
        "/api/Config/DeleteDashBoardConfig/" + configId,
        function (data, status) {
            alert("Delete success");
            //$(ele).closest("tr").remove();
            location.reload();
        });

}

function cloneConfig(configId, ele) {

    if (!confirm("Confirm duplicate？")) {
        return;
    }

    $.get(
        "/api/Config/CloneDashBoardConfig/" + configId,
        function (data, status) {
            alert("Duplicate success");
            //$(ele).closest("tr").remove();
            location.reload();
        });
}

// 编辑区域 控制函数
var curDashBoardObj = {};

var tab = null;

var rowno = 1;

// 增加元素到看板
function addElementConfig() {
    if (curDashBoardObj.ElementConfigs == undefined) {
        alert("Must select Dashboard");
        return;
    }

    curDashBoardObj.ElementConfigs.push({
        "Id": rowno++,
        "ElementId": $("#add_s_db_ele").val(),
        "ElementName": $("#add_s_db_ele option:selected").text(),
        "RowIndex": 0,
        "ColumnIndex": 0,
        "RowSpan": 1,
        "ColumnSpan": 1
    });

    // 重新生成元素表格
    GenElementTable();
}

// 新增看板
function addConfig() {
    $.getJSON(
        "/api/Config/AddDashBoardConfig",
        { dataType: "json" },
        function (data) {
            if (result = true) {
                alert("New success");

                location.reload();
            }
        });
}

// 保存看板
function saveConfig() {

    curDashBoardObj.DashBoardName = $("#edit_db_name").val();
    curDashBoardObj.Descript = $("#edit_db_descript").val();
    curDashBoardObj.RowNums = $("#edit_db_rows").val();
    curDashBoardObj.ColumnNums = $("#edit_db_columns").val();
    curDashBoardObj.BackgroundImagePath = $("#edit_db_bgimg").val();

    $.ajax({
        dataType: "json",
        data: JSON.stringify(curDashBoardObj),
        contentType: "application/json; charset=UTF-8",
        type: "post",
        url: "/api/Config/SaveDashBoardConfig",
        success: function (result) {
            if (result = true) {
                alert("Save success");

                location.reload();
                //showEditor(txt)

                //showByCurDashBoardObj();
            }
        }
    });
}

// 开始编辑
function showEditor(obj, isScrollTo, tr) {

    $("#maintable tbody tr").removeClass("info");
    $(tr).addClass("info");

    curDashBoardObj = obj;

    showByCurDashBoardObj();

    if (isScrollTo) {
        $("html,body").animate({ scrollTop: $("#editArea").offset().top }, 500);
    }
}

function showByCurDashBoardObj() {
    rowno = 1;

    // 为元素生成个唯一的 id
    $.each(curDashBoardObj.ElementConfigs, function (index, val) {
        val.Id = rowno;
        rowno++;
    });

    // 为当前编辑的看板编辑区域 赋值
    $("#edit_db_id").val(curDashBoardObj.DashBoardId);
    $("#edit_db_name").val(curDashBoardObj.DashBoardName);
    $("#edit_db_descript").val(curDashBoardObj.Descript);
    $("#edit_db_rows").val(curDashBoardObj.RowNums);
    $("#edit_db_columns").val(curDashBoardObj.ColumnNums);
    $("#edit_db_bgimg").val(curDashBoardObj.BackgroundImagePath);

    // 生成看板的 元素的表格、及布局表格
    GenElementTable();
}

function GenElementTable() {
    GenTableByHead(
        "element_table",
        curDashBoardObj.ElementConfigs,
        {
            "Id": "No",
            "ElementId": "Element Id",
            "ElementName": "Name",
            "RowIndex": "Row index",
            "ColumnIndex": "Column index",
            "RowSpan": "Row span",
            "ColumnSpan": "Column span",
            "ZIndex": "ZIndex"
        });

    $("#element_table thead tr").append("<th>Edit</th>");
    $("#element_table tbody tr").append("<td><a class=\"btn btn-primary btn-sm active\" onclick=\"EditDashboardElement(this)\">Edit</a><a class=\"btn btn-primary btn-sm active\" onclick=\"RemoveEle($($(this).parent().parent().find(\'td\')[0]).html())\">Remove</a></td>");

    if (curDashBoardObj.BackgroundImagePath != "") {
        $("#lay_tab").css(
            "background-image",
            "url(\"" + curDashBoardObj.BackgroundImagePath + "\")");
        $("#lay_tab").css(
            "background-size",
            "100% 100%");
    }
    else {
        $("#lay_tab").css(
            "background-image",
            "");
    }

    // 为 元素进行布局展示
    tab = new LayoutTable("lay_tab", curDashBoardObj);
    tab.init();
}

function EditDashboardElement(ele)
{
    var id = $($(ele).parent().parent().find("td")[0]).html();

    $.each(curDashBoardObj.ElementConfigs, function (index, val) {
        if (val.Id == id) {

            $("#elementZIndex").val(curDashBoardObj.ElementConfigs[index].ZIndex);

            $("#elementZIndex").unbind();
            $("#elementZIndex").change(function (event) {

                curDashBoardObj.ElementConfigs[index].ZIndex = $("#elementZIndex").val();

                $($(ele).closest("tr").find("td")[7]).html(curDashBoardObj.ElementConfigs[index].ZIndex);
            });
        }
    });

    BeginLayoutEle(id)
}

function RemoveEle(id) {
    // 画完、立即重新赋值、并重绘
    if (curDashBoardObj == null)
        return;

    $.each(curDashBoardObj.ElementConfigs, function (index, val) {
        if (val.Id == id) {
            curDashBoardObj.ElementConfigs.splice(index, 1);
            // 重新生成元素表格
            GenElementTable();
            return false;//相当于break
        }
    });
}

// 开始某个元素的布局
function BeginLayoutEle(id) {

    //var id = $($(ele).parent().parent().find("td")[0]).html();

    var callback = function (rowindex, columnindex, rowspan, columnspan) {

        // 画完、立即重新赋值、并重绘
        if (curDashBoardObj == null)
            return;

        $.each(curDashBoardObj.ElementConfigs, function (index, val) {
            if (val.Id == id) {
                curDashBoardObj.ElementConfigs[index].RowIndex = rowindex;
                curDashBoardObj.ElementConfigs[index].ColumnIndex = columnindex;
                curDashBoardObj.ElementConfigs[index].RowSpan = rowspan;
                curDashBoardObj.ElementConfigs[index].ColumnSpan = columnspan;
            }
        });

        alert(rowindex + " " + columnindex + " " + rowspan + " " + columnspan);

        // 每次编辑完成、重新生成 元素表格、及元素布局表格
        GenElementTable();

        // 重置表格后、继续上一表格的编辑
        // tab.setcurele(id, callback);
    }

    tab.setcurele(id, callback);
}

function preViewDashboard() {
    if (curDashBoardObj != null) {
        var url = encodeURI(
            "/DashBoard/DashboardPlayer?DashboardId="
            + curDashBoardObj.DashBoardId
            + "&paras="
            + $("#para_Paras").val()
            + "&theme="
            + $("#para_Theme").val()
            );

        window.open(url);
    }
}

activeli($("#nav_li_dashBoard"));