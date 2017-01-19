/// <reference path="../../jquery-2.1.4.js" />

function addConfig() {

    var eleconfig =
        {
            "ElementName": $("#add_i_ele_name").val(),
            "Descript": $("#add_i_ele_des").val(),
            "DataConfig": { "DataId": $("#add_s_ele_dataid").val() },
            "TypeConfig": { "TypeId": $("#add_s_ele_typeid").val() }
        };

    this._addConfig = function ()
    {
        if (paras.length > 0) {

            var $paratable = $("#preSetParaLayer #paraTable");

            // 这里遍历现有的参数、更新参数的 默认值
            for (i in paras) {
                //根据 paracode 在 弹出框中进行取值
                var val = $paratable.find("#" + paras[i].ParaCode).val();

                paras[i]["Value"] = val;

                // 如果没有填写值、则采用默认值
                if (val != "") {
                    //paras[i]["Value"] = val;
                }
            }

            eleconfig.TypeConfig.ParaConfigs = paras;
        }

        layer.closeAll();

        $.ajax({
            dataType: "json",
            data: JSON.stringify(eleconfig),
            contentType: "application/json; charset=UTF-8",
            type: "post",
            url: "/api/Config/AddElementConfig",
            success: function (result) {
                if (result = true) {
                    alert("New success");

                    location.reload();
                }
            }
        });
    }

    this.setDefaultParaValue = function () {

        if (paras == undefined) {
            return;
        }

        var $paratable = $("#preSetParaLayer #paraTable");

        // 这里遍历现有的参数、更新参数的 默认值
        for (i in paras) {
            //根据 paracode 在 弹出框中进行取值
            $paratable.find("#" + paras[i].ParaCode).val(paras[i]["Value"]);
        }
    }

    var paras;

    $.ajax(
        {
            url: "/api/Config/GetParaConfigs/" + eleconfig.TypeConfig.TypeId,
            type: "get",
            dataType: "json",
            async: false,
            success: function (data) {
                paras = data;
            }
        }
        );

    if (paras.length == 0) {
        this._addConfig();
    }
    else {
        //页面层
        layer.open({
            type: 1,
            title: '',
            skin: 'layui-layer-rim', //加上边框
            area: ['1020px', '500px'], //宽高
            content:
                "<div id=\"preSetParaLayer\" style=\"margin:5px;\" class=\"panel panel-default\">" +
                "    <div class=\"panel-heading\">" +
                "        Parameter set" +
                "        <button style=\"float:right;\" onclick=\"_addConfig();\" class=\"btn btn-danger\"><i class=\"glyphicon glyphicon-plus\"></i> 保存</button>" +
                "    </div>" +
                "    <div class=\"panel-body\">" +
                "        <table id=\"paraTable\" class=\"table table-striped table-hover\">" +
                                GenTableInnerHTMLByHead(
                                    paras,
                                    {
                                        "ParaName": "Parameter Name",
                                        //"ParaCode": "参数编码",
                                        //"Value": "默认值",
                                        "RenderScript": "Input"
                                    }
                                ) +
                "        </table>" +
                "    </div>" +
                "</div>" +
                "<script>this.setDefaultParaValue();</script>"
        });
    }
}


function resetConfig(id) {
    
    this._resetConfig = function (action) {

        if (last_paras.length > 0) {
            var $paratable = $("#preSetParaLayer #paraTable");

            // 这里遍历现有的参数、更新参数的 默认值
            for (i in last_paras) {
                //根据 paracode 在 弹出框中进行取值
                var val = $paratable.find("#" + last_paras[i].ParaCode).val();

                last_paras[i]["Value"] = val;

                // 如果没有填写值、则采用默认值
                if (val != "") {
                    //last_paras[i]["Value"] = val;
                }
            }
        }

        config.TypeConfig.ParaConfigs = last_paras;

        //if (config.TypeConfig.ParaConfigs.length > 0) {

        //    var $paratable = $("#preSetParaLayer #paraTable");

        //    // 这里遍历现有的参数、更新参数的 默认值
        //    for (i in config.TypeConfig.ParaConfigs) {
        //        //根据 paracode 在 弹出框中进行取值
        //        var val = $paratable.find("#" + config.TypeConfig.ParaConfigs[i].ParaCode).val();

        //        // 如果没有填写值、则采用默认值
        //        if (val != "") {
        //            config.TypeConfig.ParaConfigs[i]["Value"] = val;
        //        }
        //    }
        //}

        layer.closeAll();

        $.ajax({
            dataType: "json",
            data: JSON.stringify(config),
            contentType: "application/json; charset=UTF-8",
            type: "post",
            url: "/api/Config/" + action,
            success: function (result) {
                if (result = true) {
                    alert("Reset success");

                    location.reload();
                }
            }
        });
    }

    this.setDefaultParaValue = function () {

        if (config.TypeConfig.ParaConfigs == null || config.TypeConfig.ParaConfigs == undefined) {
            return;
        }

        var $paratable = $("#preSetParaLayer #paraTable");

        // 这里遍历现有的参数、更新参数的 默认值
        for (i in config.TypeConfig.ParaConfigs) {
            //根据 paracode 在 弹出框中进行取值
            var $para = $paratable.find("#" + config.TypeConfig.ParaConfigs[i].ParaCode);
            if ($para != undefined)
                $para.val(config.TypeConfig.ParaConfigs[i]["Value"]);
        }
    }

    var config;

    $.ajax(
        {
            url: "/api/Config/GetElementConfig/" + id,
            type: "get",
            dataType: "json",
            async: false,
            success: function (data) {
                config = data;
            }
        }
        );

    // 获取最新的 paras
    var last_paras;

    $.ajax(
        {
            url: "/api/Config/GetParaConfigs/" + config.TypeConfig.TypeId,
            type: "get",
            dataType: "json",
            async: false,
            success: function (data) {
                last_paras = data;
            }
        }
        );

    //if (config.TypeConfig.ParaConfigs.length == 0) {
    //    if (confirm("Confirm reset？")) {
    //        this._resetConfig();
    //    }
    //}
    //else {
        //页面层
        layer.open({
            type: 1,
            title: '',
            skin: 'layui-layer-rim', //加上边框
            area: ['1024px', '500px'], //宽高
            content:
                "<div id=\"preSetParaLayer\" style=\"margin:5px;\" class=\"panel panel-default\">" +
                "    <div class=\"panel-heading\">" +
                "        Parameter set" +
                "        <button style=\"float:right;\" onclick=\"_resetConfig(\'ResetElementAllConfig\');\" class=\"btn btn-danger\"><i class=\"glyphicon glyphicon-plus\"></i> Reset all</button>" +
                "        <button style=\"float:right;margin-right:5px;\" onclick=\"_resetConfig(\'ResetElementParaConfig\');\" class=\"btn btn-danger\"><i class=\"glyphicon glyphicon-plus\"></i> Reset parameter</button>" +
                "    </div>" +
                "    <div class=\"panel-body\">" +
                "        <table id=\"paraTable\" class=\"table table-striped table-hover\">" +
                                (last_paras.length == 0 ? "No parameter need reset" :
                                GenTableInnerHTMLByHead(
                                    last_paras,
                                    {
                                        "ParaName": "Parameter name",
                                        //"ParaCode": "参数编码",
                                        //"Value": "默认值",
                                        "RenderScript": "Input"
                                    }
                                )) +
                "        </table>" +
                "    </div>" +
                "</div>" +
                "<script>this.setDefaultParaValue();</script>"
        });
    //}
}

function deleteConfig(configId, ele) {
    if (!confirm("Confirm delete？")) {
        return;
    }
    $.get(
        "/api/Config/DeleteElementConfig/" + configId,
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
        "/api/Config/CloneElementConfig/" + configId,
        function (data, status) {
            alert("Duplicate success");
            //$(ele).closest("tr").remove();
            location.reload();
        });
}

var _curEleJsonObj = {};

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

// 编辑区域 控制函数


function saveConfig() {

    _curEleJsonObj.ElementName = $("#edit_i_db_name").val();
    _curEleJsonObj.Descript = $("#edit_i_db_des").val();
    _curEleJsonObj.HtmlTemplate = editor_html.getValue();
    _curEleJsonObj.CssTemplate = editor_css.getValue();
    _curEleJsonObj.JavaScriptTemplate = editor_js.getValue();

    $.ajax({
        dataType: "json",
        data: JSON.stringify(_curEleJsonObj),
        contentType: "application/json; charset=UTF-8",
        type: "post",
        url: "/api/Config/SaveElementConfig",
        success: function (result) {
            if (result = true) {
                alert("Save success");

                location.reload();
            }
        }
    });
}

function showEditor(eleObj, isScrollTo, tr) {

    $("#maintable tbody tr").removeClass("info");
    $(tr).addClass("info");

    _curEleJsonObj = eleObj;

    $("#edit_i_db_name").val(_curEleJsonObj.ElementName);
    $("#edit_i_db_des").val(_curEleJsonObj.Descript);

    editor_html.setValue(_curEleJsonObj.HtmlTemplate);
    editor_css.setValue(_curEleJsonObj.CssTemplate);
    editor_js.setValue(_curEleJsonObj.JavaScriptTemplate);

    if (isScrollTo) {
        $("html,body").animate({ scrollTop: $("#editArea").offset().top }, 500);
    }
}

function preViewConfig() {
    var _preEleTypeJsonObj = {};

    _preEleTypeJsonObj.TypeName = $("#edit_i_db_name").val();
    _preEleTypeJsonObj.Descript = $("#edit_i_db_des").val();
    _preEleTypeJsonObj.HtmlTemplate = editor_html.getValue();
    _preEleTypeJsonObj.CssTemplate = editor_css.getValue();
    _preEleTypeJsonObj.JavaScriptTemplate = editor_js.getValue();

    var jsonConfig = JSON.stringify(_preEleTypeJsonObj);
        
    //jsonConfig = jsonConfig.replace(/\[TerminalId\]/g, $("#para_terminalId").val());

    $.ajaxSetup({
        cache: false //关闭AJAX相应的缓存
    });

    //$("#preViewDiv").load("/DashBoard/EleTypePreView", { "configJson": jsonConfig }, function () {
    //    // do somthing
    //});

    $("#form_frame #input_configjson").val(jsonConfig);
    $("#form_frame #input_paras").val($("#para_Paras").val());
    $("#form_frame #input_dataid").val(_curEleJsonObj.DataConfig.DataId);

    $("#form_frame").submit();

    $('html, body').animate({ scrollTop: $("#pareViewArea").offset().top }, 500);
}

activeli($("#nav_li_element"));