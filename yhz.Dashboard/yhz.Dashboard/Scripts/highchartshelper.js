/*
 Highcharts Helper JS v1.0 (2015-07-31)

 author ： yinhz

 (c) 2015-2015 Siemens SISW

 License: Siemens
*/
(function () {
    // 为jquery扩展赛选数据的方法
    (function ($) {
        // 从 datatable 对象中 根据列名 进行 distinct 的到 赛选后的数组
        $.getDisDataFromDTJObj = function (dtJObj, columnName) {
            var datas = [];

            $.each(dtJObj, function (index, value) {
                var val = value[columnName];

                if ($.inArray(val, datas, 0) < 0) {
                    datas.push(val);
                }
            });

            return datas;
        }
    })(jQuery);

    // 辅助类(辅助类中注释是依据给定的数据结构进行说明的）
    this.HighchartsHelper = function() {
        /*  数据结构
            Date        Team    Yield
            5/1/2015	甲班	20
            5/1/2015	乙班	30
            5/2/2015	甲班	15
            5/2/2015	乙班	11      */

        //"xCName", "serieCName", "valueCName"

        /*  数据结构
            Date        Team    Yield
            型号1	    计划	20
            型号1	    实际1	30
            型号1	    实际2	30
            型号2	    计划	15
            型号2	    实际1	11 
            型号2	    实际2	11 */

        /*  根据某列、取得x轴(dtJObj datatable json 对象,xCName X轴列名）
            这里取x轴，则 columnName 为 Date */
        this.getxAxisCategories = function (dtJObj, xCName) {
            // 根据 Date 列 从数据中赛选出所有的日期
            return $.getDisDataFromDTJObj(dtJObj, xCName);
        };

        /*  根据 x 轴获取 series
          这里的 name 为 Team，value 为 Yield s*/
        this.getSeries = function (dtJObj, xAxisCategories, xCName, serieCName, valueCName) {
            var series = [];

            // 取得所有班组
            var serieNames = $.getDisDataFromDTJObj(dtJObj, serieCName);
            // 遍历班组
            for (i in serieNames) {
                var serie = { name: serieNames[i] };

                var data = [];

                // 遍历 日期（即 x 轴）
                for (j in xAxisCategories) {
                    // 遍历 表格、找到 当前日期 当前班组的值
                    $.each(dtJObj, function (index, value) {
                        if (value[xCName] == xAxisCategories[j] && value[serieCName] == serieNames[i]) {
                            data.push(value[valueCName]);
                        }
                    });
                }

                serie.data = data;

                series.push(serie);
            }

            return series;
        }

        this.getPieSeriesData = function (dtJObj, serieCName, valueCName) {
            var data = [];
            $.each(dtJObj, function (index, value) {
                var obj = [];
                obj.push(value[serieCName]);
                obj.push(value[valueCName]);
                data.push(obj);
            }); 
            return data;
        };
    }
}).call(this);
