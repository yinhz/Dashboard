/// <reference path="jquery-1.3.2-vsdoc2.js" />

function log(txt)
{
    $("#pp").html(txt);
}

(function () {

    // 创建看板布局类
    // 需要 宽、高、看板json、上下文 来初始化它
    this.DbLayout = function (_width, _height, _db_config, _contenxt) {

        // 看板配置
        this.Config = _db_config;

        // 宽、高
        this.Width = _width;
        this.Height = _height;

        // 行数、列数
        this.RowNums = this.Config.RowNums;
        this.ColumnNums = this.Config.ColumnNums;

        // 行高、列宽
        this.RowHeight = this.Height / this.RowNums;
        this.ColumnWidth = this.Width / this.ColumnNums;

        // 画布2d上下文
        this.CanvasContext = _contenxt;

        // 看板元素配置集合
        this.ElementConfigs = this.Config.ElementConfigs;

        // 元素集合
        this.Elements = [];

        var canvas = _contenxt.canvas;

        var $canvas = $(canvas);

        $canvas.mousemove(
                function (ele) {

                    log("X：" + ele.offsetX + " Y：" + ele.offsetY);

                    //trace_log($("#log"), ele.currentTarget, elementId);

                    //if (is_td_mouse_down) {
                    //    end_td = ele.currentTarget;
                    //    // 这里要遍历表格、把路径之间的格子全部画上颜色
                    //    UpdateSelectStatus(table_id, start_td, end_td, "_eleid" + elementId);
                    //}
                }
            );

        /************ 方法 ************/
        // 绘制看板
        this.Render = function () {

            // 先清空
            this.Clear();

            this.CanvasContext.fillStyle = "#FFF00FF";

            for (p in this.ElementConfigs) {
                var element = new DbElement(
                    this.RowHeight,
                    this.ColumnWidth,
                    this.ElementConfigs[p],
                    this.CanvasContext);

                element.Render();

                this.Elements.push(element);
            };

            //Elements[0].Clear();
        };

        // 擦除看板
        this.Clear = function () {
            this.Elements = [];
            canvas.width = canvas.width;
        };

        this.GetElement = function (_id) {
            if (this.Elements.length == 0)
                return null;

            for (ele in this.Elements)
            {
                if (ele.ElementConfig.Id == _id)
                {
                    return ele;
                }
            }

            return null;
        };
    }

    // 看板元素
    this.DbElement = function (_row_height, _column_width, _ele_config, _contenxt) {

        this.AddDbElement = function (_row_index,_column_index,_row_span,_column_span) {
            var cfg =
                {
                    RowIndex: _row_index,
                    ColumnIndex: _column_index,
                    RowSpan: _row_span,
                    ColumnSpan: _column_span
                };

            var ele = DbElement(_row_height, _column_width, cfg, _contenxt);

            ele.Render();
        };

        this.ElementConfig = _ele_config;
        this.CanvasContext = _contenxt;

        this.RowIndex = _ele_config.RowIndex;
        this.ColumnIndex = _ele_config.ColumnIndex;

        this.RowSpan = _ele_config.RowSpan;
        this.ColumnSpan = _ele_config.ColumnSpan;

        this.X = this.ColumnIndex * _column_width;
        this.Y = this.RowIndex * _row_height;

        this.Width = this.ColumnSpan * _column_width;
        this.Height = this.RowSpan * _row_height;

        var ObligateSpace = 5;
        var BorderObligateSpace = 4;

        var CanvasObligateSpace = 50;

        // 以后可能需要改为随机，所以写成函数
        this.GetBackgroundColor = function () { return "yellow"; }
        // 以后可能需要改为随机，所以写成函数
        this.GetBorderColor = function () { return "black"; };

        // 绘制元素
        this.Render = function () {
            this.RenderWColor(null);
        };

        this.RenderWColor = function (_color) {
            if (_color == null) {
                this.CanvasContext.fillStyle = this.GetBackgroundColor();
                this.CanvasContext.fillRect(
                    CanvasObligateSpace + this.X + ObligateSpace,
                    CanvasObligateSpace + this.Y + ObligateSpace,
                    this.Width - ObligateSpace * 2 - CanvasObligateSpace * 2,
                    this.Height - ObligateSpace * 2 - CanvasObligateSpace * 2);

                this.CanvasContext.strokeStyle = this.GetBorderColor();
                this.CanvasContext.strokeRect(
                    CanvasObligateSpace + this.X + ObligateSpace - BorderObligateSpace,
                    CanvasObligateSpace + this.Y + ObligateSpace - BorderObligateSpace,
                    this.Width - ObligateSpace * 2 + BorderObligateSpace * 2 - CanvasObligateSpace * 2,
                    this.Height - ObligateSpace * 2 + BorderObligateSpace * 2 - CanvasObligateSpace * 2);
            }
            else {

            }
        };

        // 擦除元素
        this.Clear = function () {
            this.CanvasContext.clearRect(this.X, this.Y, this.Width, this.Height);
        };
    }

}).call(this);

