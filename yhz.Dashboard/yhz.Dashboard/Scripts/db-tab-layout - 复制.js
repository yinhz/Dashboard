/// <reference path="jquery-1.3.2-vsdoc2.js" />

// 利用表格 进行 看板布局用 js 脚本文件

function trace_log(log, ele, eleId) {
    var rowIndex = $(ele).attr("rowIndex");
    var columnIndex = $(ele).attr("columnIndex");
    var elementId = $(ele).attr("_eleid" + eleId);

    var log_text = "当前鼠标位置：rowIndex:" + rowIndex + " columnIndex:" + columnIndex + " elementId:" + elementId;

    $(log).text(log_text);
}

// 取得单元格 行索引
function getRowIndex(td) {
    return parseInt($(td).attr("rowIndex"));
}

// 取得单元格 列索引
function getColumnIndex(td) {
    return parseInt($(td).attr("columnIndex"));
}

// 更新单元格状态（背景色、边框）
function UpdateSelectStatus(table_id, startTd, endTd, curElementId) {
    // 取得所有的 单元格
    var alltd = $("#" + table_id + " tr td");

    for (var i = 0; i < alltd.length; i++) {
        if ($(alltd[i]).attr(curElementId) == undefined || $(alltd[i]).attr(curElementId) == curElementId) {
            $(alltd[i]).removeClass("lay_tab_td_selected");
            //$(alltd[i]).css("background-color", "transparent");

            $(alltd[i]).css("border", "1px dashed black");
        }
    }

    var start_row_index = getRowIndex(startTd);
    var start_column_index = getColumnIndex(startTd);

    var end_row_index = getRowIndex(endTd);
    var end_column_index = getColumnIndex(endTd);

    if (start_row_index == end_row_index
        && start_column_index == end_column_index) {
        $(startTd).addClass("lay_tab_td_selected");
        //$(alltd[i]).css("background-color", "#68ff00");
        $(startTd).css("border", "1px solid red");

        return;
    }

    // 如果 开始行小于结束行 则交换它们
    if (start_row_index > end_row_index) {
        var temp = start_row_index;
        start_row_index = end_row_index;
        end_row_index = temp;
    }
    // 如果 开始列小于结束列 则交换它们
    if (start_column_index > end_column_index) {
        var temp = start_column_index;
        start_column_index = end_column_index;
        end_column_index = temp;
    }

    // 只要在这次所选择的 单元格范围内。则添加颜色
    for (var i = 0; i < alltd.length; i++) {
        var rowIndex = getRowIndex(alltd[i]);
        var columnIndex = getColumnIndex(alltd[i]);

        if (rowIndex >= start_row_index && rowIndex <= end_row_index
            && columnIndex >= start_column_index && columnIndex <= end_column_index) {
            $(alltd[i]).addClass("lay_tab_td_selected");
            //$(alltd[i]).css("background-color", "#68ff00");
            $(alltd[i]).attr(curElementId, curElementId);


            $(alltd[i]).css("border", "1px none black");

            // 画左边框
            if (columnIndex == start_column_index) {
                $(alltd[i]).css("border-left-style", "solid");
                $(alltd[i]).css("border-left-color", "red");
                $(alltd[i]).css("border-left-width", "1px");
            }

            // 画上边框
            if (rowIndex == start_row_index) {
                $(alltd[i]).css("border-top-style", "solid");
                $(alltd[i]).css("border-top-color", "red");
                $(alltd[i]).css("border-top-width", "1px");
            }

            // 画右边框
            if (columnIndex == end_column_index) {
                $(alltd[i]).css("border-right-style", "solid");
                $(alltd[i]).css("border-right-color", "red");
                $(alltd[i]).css("border-right-width", "1px");
            }

            // 画下边框
            if (rowIndex == end_row_index) {
                $(alltd[i]).css("border-bottom-style", "solid");
                $(alltd[i]).css("border-bottom-color", "red");
                $(alltd[i]).css("border-bottom-width", "1px");
            }
        }
    }
}

// 初始化布局表格
function InitLayoutTable(table_id, rows, columns) {
    var l_tab = $("#" + table_id);

    for (var i = 0; i < rows; i++) {

        var tr = "<tr></tr>";

        var jtr = $(tr);

        for (var j = 0; j < columns; j++) {

            var td = "<td rowIndex=" + i + " columnIndex=" + j + "></td>";

            jtr.append(td);

        }

        l_tab.append(jtr);
    }
}

// 开始布局表格（para1:表格id,para2：针对哪个元素）
function BeginLayoutTable(table_id, elementId) {
    var is_td_mouse_down = false;
    var start_td = null;
    var end_td = null;

    var td_eles = $("#" + table_id + " tr td");

    td_eles.unbind();

    td_eles.hover(
        function () { $(this).addClass("lay_tab_td_hover"); },
        function () { $(this).removeClass("lay_tab_td_hover"); });

    td_eles.mousedown(
           function (ele) {
               start_td = ele.currentTarget;

               is_td_mouse_down = true;
           }
       );

    td_eles.mousemove(
            function (ele) {
                //trace_log($("#log"), ele.currentTarget, elementId);

                if (is_td_mouse_down) {
                    end_td = ele.currentTarget;
                    // 这里要遍历表格、把路径之间的格子全部画上颜色
                    UpdateSelectStatus(table_id, start_td, end_td, "_eleid" + elementId);
                }
            }
        );

    td_eles.mouseup(
            function (ele) {
                start_td = null;
                end_td = null;

                is_td_mouse_down = false;
            }
        );
}