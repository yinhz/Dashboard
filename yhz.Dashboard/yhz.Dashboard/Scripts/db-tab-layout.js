/// <reference path="jquery-1.3.2-vsdoc2.js" />

// 利用表格 进行 看板布局用 js 脚本文件
(function (window, undefined) {
    function trace_log(log, ele, id) {
        var rowIndex = $(ele).attr("rowIndex");
        var columnIndex = $(ele).attr("columnIndex");
        var elementId = $(ele).attr("_eleid" + id);

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
        var alltd = $("#" + table_id + " td");

        for (var i = 0; i < alltd.length; i++) {
            //状态还原（只能还原正在编辑的元素）不能影响其他元素
            if ($(alltd[i]).attr(curElementId) == curElementId) {
                $(alltd[i]).removeAttr(curElementId);
                $(alltd[i]).removeClass("db_lay_tab_td_selected");
                $(alltd[i]).css("border", "2px dashed white");
            }
        }

        var start_row_index = getRowIndex(startTd);
        var start_column_index = getColumnIndex(startTd);

        var end_row_index = getRowIndex(endTd);
        var end_column_index = getColumnIndex(endTd);

        // 如果只占一个单元格、则直接 选中
        if (start_row_index == end_row_index
            && start_column_index == end_column_index) {
            $(startTd).addClass("db_lay_tab_td_selected");
            $(startTd).attr(curElementId, curElementId);
            $(startTd).css("border", "2px solid red");

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
                $(alltd[i]).addClass("db_lay_tab_td_selected");
                //$(alltd[i]).css("background-color", "#68ff00");
                $(alltd[i]).attr(curElementId, curElementId);

                //$(alltd[i]).css("border", "1px none black");

                // 画左边框
                if (columnIndex == start_column_index) {
                    $(alltd[i]).css("border-left-style", "solid");
                    $(alltd[i]).css("border-left-color", "red");
                    $(alltd[i]).css("border-left-width", "2px");
                }

                // 画上边框
                if (rowIndex == start_row_index) {
                    $(alltd[i]).css("border-top-style", "solid");
                    $(alltd[i]).css("border-top-color", "red");
                    $(alltd[i]).css("border-top-width", "2px");
                }

                // 画右边框
                if (columnIndex == end_column_index) {
                    $(alltd[i]).css("border-right-style", "solid");
                    $(alltd[i]).css("border-right-color", "red");
                    $(alltd[i]).css("border-right-width", "2px");
                }

                // 画下边框
                if (rowIndex == end_row_index) {
                    $(alltd[i]).css("border-bottom-style", "solid");
                    $(alltd[i]).css("border-bottom-color", "red");
                    $(alltd[i]).css("border-bottom-width", "2px");
                }
            }
        }
    }

    // 初始化布局表格
    function LayoutTable(table_id, dashBoardConfig) {
        this.init = f_init;
        this.setcurele = f_setcurele;
        this.tab = $("#" + table_id);

        this.size = f_size;
        this.width = f_width;
        this.height = f_height;

        function f_init() {
            var l_tab = $("#" + table_id);

            //l_tab.find("tr").remove();

            var trs = "";

            for (var i = 0; i < dashBoardConfig.RowNums; i++) {

                trs += "<tr>";

                for (var j = 0; j < dashBoardConfig.ColumnNums; j++) {

                    trs += "<td rowIndex=" + i + " columnIndex=" + j + "></td>";

                }

                trs += "</tr>"
            }

            l_tab.html(trs);

            if (dashBoardConfig.ElementConfigs != undefined && dashBoardConfig.ElementConfigs.length > 0) {
                for (var i = 0; i < dashBoardConfig.ElementConfigs.length; i++) {
                    var ele = dashBoardConfig.ElementConfigs[i];

                    var startTd = this.tab.find("tr:eq(" + ele.RowIndex + ") td:nth-child(" + (ele.ColumnIndex + 1) + ")");

                    var endTd = this.tab.find("tr:eq(" + (ele.RowIndex + ele.RowSpan - 1) + ") td:nth-child(" + (ele.ColumnIndex + ele.ColumnSpan) + ")");

                    startTd.html(
                        "<button style='margin-left:5px;' class='btn btn-default btn-sm' " +
                        " onclick=\"BeginLayoutEle('" + ele.Id + "');" +
                        "\">" +
                        "<i class='glyphicon glyphicon-edit'></i> <span style='font-weight:bold;'>" + ele.ElementName + "</span>" +
                        "</button>"
                        );

                    //startTd[0].onclick = function (evt) {
                    //    BeginLayoutEle("" + ele.Id);
                    //}

                    //startTd.find("button").bind("click", function (e) {
                    //    //BeginLayoutEle('" + ele.Id + "');
                    //    //e.stopPropagation();
                    //});

                    UpdateSelectStatus(table_id, startTd, endTd, "_eleid" + ele.Id);
                }
            }
        }

        function f_setcurele(elementId, callback) {
            var is_td_mouse_down = false;
            var start_td = null;
            var end_td = null;

            var td_eles = $("#" + table_id + " tr td");

            td_eles.unbind();

            // 这里给 当前的 elementid 高亮
            for (var i = 0; i < td_eles.length; i++) {
                //状态还原（只能还原正在编辑的元素）不能影响其他元素
                if ($(td_eles[i]).attr("_eleid" + elementId) == ("_eleid" + elementId)) {
                    $(td_eles[i]).addClass("db_lay_tab_td_highligth");
                }
                else {
                    $(td_eles[i]).removeClass("db_lay_tab_td_highligth");
                }
            }

            td_eles.hover(
                function () { $(this).addClass("db_lay_tab_td_hover"); },
                function () { $(this).removeClass("db_lay_tab_td_hover"); });

            td_eles.mousedown(
                   function (ele) {
                       start_td = ele.currentTarget;

                       //// 如果按下的单元格下 确实有 按钮
                       //if ($(start_td).find("button").length > 0) {

                       //    // 是否是 当前编辑项

                       //    // 是否是 其他需要编辑的项目

                       //    // 是否确实就要画在这个格子

                       //}
                       //// 如果没有按钮 目前不做任何处理
                       //else { }

                       is_td_mouse_down = true;
                   }
               );

            td_eles.mousemove(
                    function (ele) {
                        //trace_log($("#log"), ele.currentTarget, elementId);

                        if (is_td_mouse_down) {
                            end_td = ele.currentTarget;
                            // 这里要遍历表格、把路径之间的格子全部画上颜色 鼠标
                            UpdateSelectStatus(table_id, start_td, end_td, "_eleid" + elementId);
                        }
                    }
                );

            td_eles.mouseup(
                    function (ele) {
                        end_td = ele.currentTarget;

                        var start_row_index = getRowIndex(start_td);
                        var start_column_index = getColumnIndex(start_td);

                        var end_row_index = getRowIndex(end_td);
                        var end_column_index = getColumnIndex(end_td);

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

                        // 回调、把画的结果给传回来
                        callback(start_row_index, start_column_index, (end_row_index - start_row_index + 1), (end_column_index - start_column_index + 1));

                        is_td_mouse_down = false;
                    }
                );
        }

        function f_size(width, height) {
            f_width(width);
            f_height(height);
        }
        function f_width(width)
        { this.tab.width(width); }
        function f_height(height)
        { this.tab.height(height); }
    }

    window.LayoutTable = LayoutTable;

    _LayoutTable = window.LayoutTable;

})(window, undefined);