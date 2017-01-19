/// <reference path="jquery-2.1.4.js" />
function a(tableData, rowIndex, row, columnName, cellValue) {

    if (columnName == '箱发库存' && $.isNumeric(cellValue) && cellValue > 25) {
        return "<span style='color:red;'>" + cellValue + "</span>";
    }

    if (columnName == '型号' && rowIndex != tableData.length - 1) {
        var replace_val = '';
        if (cellValue.length > 10) {
            replace_val = cellValue.substring(0, 10) + '...';
        }
        return "<a href='/DashBoard/DashboardPlayer?theme=" + m_theme + "&DashboardId=12482f72747746679942b92408d4dec6&paras={\"Model\":\"" + replace_val + "\"}'>" + cellValue + "</a>";
    }
    return cellValue;
}