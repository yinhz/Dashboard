/// <reference path="jquery-2.1.4.js" />

(function ($) {

    // 扩展 jquery 对表格的排序及检索功能
    // 第三个参数 orderBy 接受 "","Asc","Desc"
    // 1、取出所有 td 值
    // 压到数组中
    $.TableOrderBy = function ($tab, columnIndex, orderBy) {
        //if (orderBy == "desc") { order = "desc"; };

        //// 二维数组、存放 行索引 与 当前列的值
        //var tds = [];
        //var indexTd = [];

        //var tds = $tab.find("tbody tr td:nth-child(" + columnIndex + ")");

        //$(tds).each(function (index, element) {
        //    tds
        //});
    }

    $.getJSONObj = function (JSONObjs, columnName, value) {
        var obj;

        $.each(JSONObjs, function (index, val) {
            if (val[columnName] == value) {
                obj = val;
                return false;
            }
        });

        return obj;
    }

    // 为 jquery 扩展获取 url 参数方法
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return "";
    };

    // 为 jquery 扩展获取 当前时间 方法
    $.getYYYYMMDDWEEKDAY = function () {
        var today = new Date();
        var day;
        if (today.getDay() == 0) day = " 星期日"
        if (today.getDay() == 1) day = " 星期一"
        if (today.getDay() == 2) day = " 星期二"
        if (today.getDay() == 3) day = " 星期三"
        if (today.getDay() == 4) day = " 星期四"
        if (today.getDay() == 5) day = " 星期五"
        if (today.getDay() == 6) day = " 星期六"
        return today.getFullYear() + "年" + (today.getMonth() + 1) + "月" + today.getDate() + "日" + day + "";
    }

    // 为 jquery 扩展获取 当前时间 方法
    $.getYYYYMMDD = function () {
        var today = new Date();
        var day;
        if (today.getDay() == 0) day = " 星期日"
        if (today.getDay() == 1) day = " 星期一"
        if (today.getDay() == 2) day = " 星期二"
        if (today.getDay() == 3) day = " 星期三"
        if (today.getDay() == 4) day = " 星期四"
        if (today.getDay() == 5) day = " 星期五"
        if (today.getDay() == 6) day = " 星期六"
        return today.getFullYear() + "年" + (today.getMonth() + 1) + "月" + today.getDate() + "日";
    }

    // 为 jquery 扩展获取 当前时间 方法
    $.getHHMMSS = function () {
        var date = new Date();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var day = date.getDate();
        var month = date.getMonth() + 1;
        var year = date.getFullYear();
        var amOrPm = "AM";
        if (minutes < 10) minutes = "0" + minutes;
        if (seconds < 10) seconds = "0" + seconds;
        if (day < 10) day = "0" + day;
        if (month < 10) month = "0" + month;
        return hours + "点" + minutes + "分" + seconds + "秒";
    }

    // 为 jquery 扩展获取 当前时间 方法
    $.getHHMM = function () {
        var date = new Date();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var day = date.getDate();
        var month = date.getMonth() + 1;
        var year = date.getFullYear();
        var amOrPm = "AM";
        if (minutes < 10) minutes = "0" + minutes;
        if (seconds < 10) seconds = "0" + seconds;
        if (day < 10) day = "0" + day;
        if (month < 10) month = "0" + month;
        return hours + "点" + minutes + "分";
    }

    // 为 jquery 扩展获取 当前时间 方法
    $.getYYYYMMDDHHMMSS = function () {
        var date = new Date();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var day = date.getDate();
        var month = date.getMonth() + 1;
        var year = date.getFullYear();
        if (minutes < 10) minutes = "0" + minutes;
        if (seconds < 10) seconds = "0" + seconds;
        if (day < 10) day = "0" + day;
        if (month < 10) month = "0" + month;
        return year + "年" + month + "月" + day + "日 " + hours + "点" + minutes + "分" + seconds + "秒";
    }
})(jQuery);

// 此为全局变量
// 需要从 url 中去获取 终端ID
// 能够取得的话 则代表是终端在进行访问（那么在进行数据查询的时候，会带上终端信息）
// 如果获取不到 则代表在测试
var m_terminal_id = $.getUrlParam("terminalId");
var m_paras = $.getUrlParam("paras");
var m_theme = $.getUrlParam("theme");

// 获取数据
function GetDashboardData(dataId, success) {

    var dao_url = "/api/Dao/GetData?Id=" + dataId;

    if (m_terminal_id != null && m_terminal_id != "") {
        dao_url = dao_url + "&terminalId=" + m_terminal_id;
    }

    if (m_paras != null && m_paras != "") {
        dao_url = dao_url + "&paras=" + m_paras;
    }

    $.getJSON(
      dao_url,
      function (json) {
          success(json);
      });

    // 这里的终端ID 采用url中取得的终端ID
    //GetTerminalIdDashboardData(dataId, m_terminal_id, success)
}

// 终端获取数据
function GetTerminalIdDashboardData(dataId, terminalId, success) {
    $.getJSON(
      "/api/Dao/GetDashboardData?Id=" + dataId + "&terminalId=" + terminalId,
      function (json) {
          success(json);
      });
}

// 激活li
function activeli(li) {
    li.removeClass();
    li.addClass("active");
    li.siblings().removeClass("active");
}

// headers 结构
// json 。
// key 为 列、value 为 汉化
function GenTableByHead(table_id, tab_data_json, header_json) {
    $("#" + table_id).html(GenTableInnerHTMLByHead(tab_data_json, header_json));
}

function GenTableInnerHTMLByHead(tab_data_json, header_json) {
    var tableInnerHtml = "";

    if ($(tab_data_json).length > 0) {

        //生成 head
        tableInnerHtml += "<thead>";

        var val = tab_data_json[0];

        //生成 head
        for (var head_key in header_json) {
            tableInnerHtml += ("<th>" + header_json[head_key] + "</th>");
        }

        tableInnerHtml += "</tr></thead>";

        //生成内容
        tableInnerHtml += "<tbody>";

        $(tab_data_json).each(function (index) {

            //生成行
            tableInnerHtml += "<tr>";

            var val = tab_data_json[index];

            //生成 head
            for (var head_key in header_json) {
                tableInnerHtml += ("<td>" + val[head_key] + "</td>");
            }

            tableInnerHtml += "</tr>";
        });

        tableInnerHtml += "</tbody>";
    }

    return tableInnerHtml;
}

function GenTable(table_id, tab_data_json) {
    $("#" + table_id).html(GenTableInnerHtml(tab_data_json));
}

// 生成表格 html
function GenTableInnerHtml(tab_data_json, cellValueHandleDelegate) {
    var tableInnerHtml = "";

    if ($(tab_data_json).length > 0) {

        //生成 head
        tableInnerHtml += "<thead><tr>";

        var val = tab_data_json[0];

        //生成 head
        for (var key in val) {
            tableInnerHtml += ("<th>" + key + "</th>");
        }

        tableInnerHtml += "</tr></thead>";

        //生成内容
        tableInnerHtml += "<tbody>";

        $(tab_data_json).each(function (index) {

            //生成行
            tableInnerHtml += "<tr>";

            var val = tab_data_json[index];

            var columnIndex = 0;

            for (var key in val) {

                if (cellValueHandleDelegate != undefined && $.isFunction(cellValueHandleDelegate)) {
                    tableInnerHtml += ("<td>" + cellValueHandleDelegate(tab_data_json, index, val, key, val[key], columnIndex) + "</td>");
                }
                else {
                    tableInnerHtml += ("<td>" + val[key] + "</td>");
                }

                columnIndex++;
            }

            tableInnerHtml += "</tr>";
        });

        tableInnerHtml += "</tbody>";
    }

    return tableInnerHtml;
}

// 显表示格<tr>
function ShowTableTr(table_id, curpage, pagesize) {
    var s = 1; //显示开始索引
    var e = 0; //显示结束索引
    if (curpage == 1) e = pagesize + 1;
    else {
        s = (curpage - 1) * pagesize + 1;
        e = curpage * pagesize + 1;
    }
    var trslen = $(table_id).find("tr").length;
    if (e > trslen) e = trslen;
    for (var i = 1; i < trslen; i++) //保留最前面一行！
    {
        if ((i >= s && i < e) || i < 1) {
            $(table_id).find("tr").eq(i).css("display", "");
        } else {
            $(table_id).find("tr").eq(i).css("display", "none");
        }
    }
}

