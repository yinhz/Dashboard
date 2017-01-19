/// <reference path="../../jquery-2.1.4.js" />

function deleteConfig(configId, ele) {
    if (!confirm("Comfirm delete？")) {
        return;
    }
    $.get(
        "/api/Config/DeleteElementTypeConfig/" + configId,
        function (data, status) {
            alert("Delete success");

            location.reload();
            //$(ele).closest("tr").remove();
        });
}

function cloneConfig(configId, ele) {
    if (!confirm("Comfirm duplicate？")) {
        return;
    }
    $.get(
        "/api/Config/CloneElementTypeConfig/" + configId,
        function (data, status) {
            alert("Duplicate success");

            location.reload();
            //$(ele).closest("tr").remove();
        });
}

var curEleTypeJsonObj = {};

var editor_html = CodeMirror.fromTextArea(document.getElementById("edit_area_html"), {
    mode: "text/html",
    lineNumbers: true,
    styleActiveLine: true,
    lineWrapping: true,
    extraKeys: {
        "F11": function (cm) {
            cm.setOption("fullScreen", !cm.getOption("fullScreen"));
        },
        "Esc": function (cm) {
            if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
        }
    }
});

var editor_css = CodeMirror.fromTextArea(document.getElementById("edit_area_css"), {
    mode: "text/css",
    extraKeys: {
        "Ctrl-Space": "autocomplete",
        "F11": function (cm) {
            cm.setOption("fullScreen", !cm.getOption("fullScreen"));
        },
        "Esc": function (cm) {
            if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
        }
    },
});

var editor_js = CodeMirror.fromTextArea(document.getElementById("edit_area_javascript"), {
    mode: "text/javascript",
    lineNumbers: true,
    matchBrackets: true,
    continueComments: "Enter",
    extraKeys: {
        "Ctrl-Q": "toggleComment",
        "F11": function (cm) {
            cm.setOption("fullScreen", !cm.getOption("fullScreen"));
        },
        "Esc": function (cm) {
            if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
        }
    }
});

var mixedMode = {
    name: "htmlmixed",
    scriptTypes: [{
        matches: /\/x-handlebars-template|\/x-mustache/i,
        mode: null
    },
                  {
                      matches: /(text|application)\/(x-)?vb(a|script)/i,
                      mode: "vbscript"
                  }]
};

var editor_para = CodeMirror.fromTextArea(document.getElementById("edit_para_html"), {
    mode: mixedMode,
    lineNumbers: true,
    styleActiveLine: true,
    lineWrapping: true,
    extraKeys: {
        "F11": function (cm) {
            cm.setOption("fullScreen", !cm.getOption("fullScreen"));
        },
        "Esc": function (cm) {
            if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
        }
    }
});


// 编辑区域 控制函数

function addConfig() {
    $.getJSON(
        "/api/Config/AddElementTypeConfig?widthData=" + $("#s_widthData").val(),
        { dataType: "json" },
        function (data) {
            if (result = true) {
                alert("New success");

                location.reload();
            }
        });
}

function saveConfig() {

    curEleTypeJsonObj.TypeName = $("#edit_i_db_name").val();
    curEleTypeJsonObj.Descript = $("#edit_i_db_des").val();
    curEleTypeJsonObj.HtmlTemplate = editor_html.getValue();
    curEleTypeJsonObj.CssTemplate = editor_css.getValue();
    curEleTypeJsonObj.JavaScriptTemplate = editor_js.getValue();

    $.ajax({
        dataType: "json",
        data: JSON.stringify(curEleTypeJsonObj),
        contentType: "application/json; charset=UTF-8",
        type: "post",
        url: "/api/Config/SaveElementTypeConfig",
        success: function (result) {
            if (result = true) {
                alert("Save success");

                location.reload();
            }
        }
    });
}

function showEditor(eleTypeObj, isScrollTo, tr) {

    $("#maintable tbody tr").removeClass("info");
    $(tr).addClass("info");

    curEleTypeJsonObj = eleTypeObj;

    $("#edit_i_db_name").val(curEleTypeJsonObj.TypeName);
    $("#edit_i_db_des").val(curEleTypeJsonObj.Descript);

    editor_html.setValue(curEleTypeJsonObj.HtmlTemplate);
    editor_css.setValue(curEleTypeJsonObj.CssTemplate);
    editor_js.setValue(curEleTypeJsonObj.JavaScriptTemplate);

    ////页面层
    //layer.open({
    //    type: 1,
    //    skin: 'layui-layer-rim', //加上边框
    //    area: ['900px', '600px'], //宽高
    //    content: $("#editArea")
    //});

    GenParaConfigTable();
        
    if (isScrollTo) {
        $("html,body").animate({ scrollTop: $("#editArea").offset().top }, 500);
    }
}

var rowno = 1;

function GenParaConfigTable()
{
    rowno = 1;

    // 为元素生成个唯一的 id
    $.each(curEleTypeJsonObj.ParaConfigs, function (index, val) {
        val.Id = rowno;
        rowno++;
    });

    GenTableByHead(
        "para_table",
        curEleTypeJsonObj.ParaConfigs,
        {
            "Id": "No",
            "ParaName": "Name",
            "ParaCode": "Code",
            "Value": "Default"
        });

    $("#para_table thead tr").append("<th>Edit</th>");
    $("#para_table tbody tr").append("<td><a class=\"btn btn-primary btn-sm active\" onclick=\"editPara($($(this).closest(\'tr\').find(\'td\')))\">Edit</a> <a class=\"btn btn-primary btn-sm active\" onclick=\"RemoveParaConfig($($(this).closest(\'tr\').find(\'td\')[0]).html())\">Remove</a></td>");
}

// 给元素类型增加参数
function saveParaConfig() {

    var pararowid = $("#pararowid").val();

    if (pararowid == "") {
        alert("Must select the parameter");
        return;
    }

    var paraobj = $.getJSONObj(curEleTypeJsonObj.ParaConfigs, "Id", pararowid);

    if (paraobj == undefined)
    {
        alert("Editing object may have been deleted");
        return;
    }

    paraobj.ParaName = $("#i_para_name").val();
    paraobj.ParaCode = $("#i_para_code").val();
    paraobj.Value = $("#i_para_value").val();
    paraobj.RenderScript = editor_para.getValue();

    // 重新生成参数表格
    GenParaConfigTable();

    alert("Save success");
}

function editPara(tds)
{
    $("#para_table tbody tr").removeClass("info");
    $(tds.parent()).addClass("info");

    $("#pararowid").val(tds[0].innerHTML);

    var paraobj = $.getJSONObj(curEleTypeJsonObj.ParaConfigs, "Id", tds[0].innerHTML);

    $("#i_para_name").val(paraobj.ParaName);
    $("#i_para_code").val(paraobj.ParaCode);
    $("#i_para_value").val(paraobj.Value);
    editor_para.setValue(paraobj.RenderScript);
}

function RemoveParaConfig(id) {
    // 画完、立即重新赋值、并重绘
    if (curEleTypeJsonObj == null)
        return;

    $.each(curEleTypeJsonObj.ParaConfigs, function (index, val) {
        if (val.Id == id) {
            curEleTypeJsonObj.ParaConfigs.splice(index, 1);
            // 重新生成元素表格
            GenParaConfigTable();
            return false;//相当于break
        }
    });
}

// 给元素类型增加参数
function addParaConfig() {
    if (curEleTypeJsonObj == undefined) {
        alert("Must select Element type");
        return;
    }

    curEleTypeJsonObj.ParaConfigs.push({
        "Id": rowno++,
        "ParaName": $("#i_para_name").val(),
        "ParaCode": $("#i_para_code").val(),
        "RenderScript": editor_para.getValue(),
        "Value": $("#i_para_value").val(),
    });

    // 重新生成参数表格
    GenParaConfigTable();
}

function preViewConfig() {

    // clone current eletype
    var _preEleTypeJsonObj = $.parseJSON(JSON.stringify(curEleTypeJsonObj));

    _preEleTypeJsonObj.TypeName = $("#edit_i_db_name").val();
    _preEleTypeJsonObj.Descript = $("#edit_i_db_des").val();
    _preEleTypeJsonObj.HtmlTemplate = editor_html.getValue();
    _preEleTypeJsonObj.CssTemplate = editor_css.getValue();
    _preEleTypeJsonObj.JavaScriptTemplate = editor_js.getValue();

    var jsonConfig = JSON.stringify(_preEleTypeJsonObj);

    //jsonConfig = jsonConfig.replace(/\[ElementId\]/g, $("#para_ElementId").val());
    //jsonConfig = jsonConfig.replace(/\[DataId\]/g, $("#para_DataId").val());
    //jsonConfig = jsonConfig.replace(/\[IsAutoRefresh\]/g, $("#para_IsAutoRefresh").val());
    //jsonConfig = jsonConfig.replace(/\[Interval\]/g, $("#para_Interval").val());

    //jsonConfig = jsonConfig.replace(/\[ParasJson\]/g, $("#para_ParasJson").val());
    //jsonConfig = jsonConfig.replace(/\[TerminalId\]/g, $("#para_terminalId").val());

    $.ajaxSetup({
        cache: false //关闭AJAX相应的缓存
    });

    //$("#postToIframe").load("/DashBoard/EleTypePreView", { "configJson": jsonConfig }, function () {
    //    // do somthing
    //});

    //$("#preViewDiv").load("/DashBoard/EleTypePreView", { "configJson": jsonConfig }, function () {
    //    // do somthing
    //});

    $("#form_frame #input_configjson").val(jsonConfig);
    $("#form_frame #input_paras").val($("#para_Paras").val());
    $("#form_frame #input_dataid").val($("#para_DataId").val());

    $("#form_frame").submit();

    $('html, body').animate({ scrollTop: $("#pareViewArea").offset().top }, 500);
}

activeli($("#nav_li_elementType"));

$(document).ready(function () {

    $("#btn_para_clear").click(function (event) {
        editor_para.setValue("");
    });

    $("#btn_para_input").click(function (event) {

        var para_name = $("#i_para_name").val();
        var para_code = $("#i_para_code").val();
        var para_value = $("#i_para_value").val();

        var script = "<div class=\"input-group\">\n  <span class=\"input-group-addon\" id=\"basic-addon1\">" +
            para_name
            + "</span>\n  <input id=\"" + para_code + "\" value=\"" + para_value + "\" class=\"form-control\" type=\"text\" placeholder=\"" + para_name + "\" />\n</div>";

        editor_para.setValue(script);
    });

    $("#btn_para_select").click(function (event) {

        var para_name = $("#i_para_name").val();
        var para_code = $("#i_para_code").val();
        var para_value = $("#i_para_value").val();
                
        var script = "<div class=\"input-group\">\n  <span class=\"input-group-addon\" id=\"basic-addon1\">" +
            para_name
            + "</span>\n  <select id=\"" + para_code + "\" class=\"form-control\">\n    <option value=\"" + para_value + "\">" + para_value + "</option>\n  </select>\n</div>";

        editor_para.setValue(script);

    });

    $("#btn_para_area").click(function (event) {

        var para_name = $("#i_para_name").val();
        var para_code = $("#i_para_code").val();
        var para_value = $("#i_para_value").val();

        var script = "<div class=\"input-group\">\n  <span class=\"input-group-addon\" id=\"basic-addon1\">" +
            para_name
            + "</span>\n  <textarea id=\"" + para_code + "\" value=\"" + para_value + "\" class=\"form-control\" type=\"text\" placeholder=\"" + para_name + "\" />\n</div>";

        editor_para.setValue(script);

    });
});
    