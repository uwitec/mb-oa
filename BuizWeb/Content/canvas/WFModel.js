/*
陈宏伟 2011.5.13

流程建模工具,数据示例如下:
var sampleData = {
name: '某某流程图',
nodes: [
{ ID: '1', name: '开始节点', type: 'Start', position: { x: 11, y: 11} },
{ ID: '2', name: '人工处理', type: 'Handle', position: { x: 305, y: 450} },
{ ID: '3', name: '人工处理', type: 'Handle', position: { x: 35, y: 670} },
{ ID: '4', name: '条件分支', type: 'XORSplit', position: { x: 311, y: 111} },
{ ID: '5', name: '人工处理', type: 'Handle', position: { x: 600, y: 330} },
{ ID: '6', name: '人工处理', type: 'Handle', position: { x: 110, y: 37} },
{ ID: '7', name: '结束节点', type: 'Finish', position: { x: 66, y: 88} }
],
Lines: [
{ from: '1', to: '2', middlePoint: { x: 11, y: 11} },
{ from: '2', to: '3', middlePoint: { x: 11, y: 11} },
{ from: '2', to: '4', middlePoint: { x: 11, y: 11} },
{ from: '3', to: '4', middlePoint: { x: 11, y: 11} },
{ from: '5', to: '6', middlePoint: { x: 11, y: 11} },
{ from: '5', to: '7', middlePoint: { x: 11, y: 11} },
{ from: '4', to: '5', middlePoint: { x: 11, y: 11} }
]
};        
        
window.onload = function () {
new WFGraph({ id: "myCanvas", data: sampleData });
}

待改进的工作:
1.根据随容器尺寸变化一同变化
2.滚动条或容器位置对节点选择的影响
3.一些固定尺寸要根据图片尺寸自动变,而不是写死值
4.那些在当前可视区域外的节点或连线如何进入可视区域
*/

function WFGraph(config) {


    var transXY = { x: 20, y: 20 }; //转换后的坐标系原点距离容器左下角
    this.data = config.data;
    this.ctx = Ext.fly(config.id).dom.getContext('2d');
    this.height = Ext.fly(config.id).parent().getHeight();
    this.width = Ext.fly(config.id).parent().getWidth();
    this.fullCanvas = function () {
        Ext.fly(this.ctx.canvas).dom.width = this.width;
        Ext.fly(this.ctx.canvas).dom.height = this.height;

        //this.ctx.canvas.width = this.width;
        //this.ctx.canvas.height = this.height;



        // 注册事件方法,注意事件方法要运行在WFGraph对象中,不能运行在window下
        /*this.ctx.canvas.onmousedown = function (scope) {
        return function (event) {
        return scope.canvasMouseDownHandler.call(scope, event);  // 返回一个上下文在scope(this)里的对canvasMouseDownHandler的调用,EXT可能有更好的解决方法
        } 
        } (this);*/

        

        this.redrawAll();
    }

    this.redrawAll = function () {
        this.ctx.setTransform(1, 0, 0, -1, transXY.x, this.height - transXY.y);
        this.ctx.clearRect(-transXY.x, -transXY.y, this.width, this.height);

        this.ctx.drawLineWithArrow({ x: -10, y: 0 }, { x: 45, y: 0 });
        this.ctx.drawLineWithArrow({ x: 0, y: -10}, { x: 0, y: 45 });

        var n;
        for (n in this.data.Lines) {
            this.drawLine(this.data.Lines[n]);
        }

        for (n in this.data.nodes) {
            this.drawNode(this.data.nodes[n]);
        }

        //alert('dddd');
    }

    this.drawLine = function (line) {
        var start = {}, end = {}, n;

        // 找当前连线相关的头尾节点
        for (n in this.data.nodes) {
            if (this.data.nodes[n].ID == line.from) {
                start = this.data.nodes[n].position
            }
            if (this.data.nodes[n].ID == line.to) {
                end = this.data.nodes[n].position
            }
        }

        var dis = this.ctx.distance(start, end);

        // 因为节点图片本身点据一定空间,连线要让开这部分空间,因此需要重新测算起点和终点,具体大小应该根据图片自动设置,以后改进
        this.ctx.drawLineWithArrow(
                    { x: end.x + (start.x - end.x) * (dis - 20) / dis, y: end.y + (start.y - end.y) * (dis - 20) / dis }, //起点让20px
                    {x: start.x + (end.x - start.x) * (dis - 20) / dis, y: start.y + (end.y - start.y) * (dis - 20) / dis} //终点让30px
                );
    }

    this.drawNode = function (n) {
        // 当前使用的图片是24px的,所以让12
        this.ctx.drawImage(WFGraph.nodeImgs[n.type], n.position.x - 12, n.position.y - 12);
        this.drawText(n.name, n.position.x - 12, n.position.y - 12);
    }

    this.drawText = function (text, x, y) {
        this.ctx.save();
        this.ctx.setTransform(1, 0, 0, 1, 0, 0); //如果不恢复坐标系,字显示的是倒的
        this.ctx.translate(transXY.x, this.height - transXY.y);
        this.ctx.fillText(text, x, -y + 12); //同画节点图片,但是文字的长短是可变的,要测量再决定让多少,待改进
        this.ctx.restore();
    }

    //
    this.selected = null;
    this.operateState = null; // moveCanvas

    this.tranp = function (point) {
        return {
            x: point.x - transXY.x - Ext.fly(this.ctx.canvas).getOffsetsTo(document.body)[0] + Ext.fly(document.body).getScroll().left,
            y: this.height - transXY.y - point.y + Ext.fly(this.ctx.canvas).getOffsetsTo(document.body)[1] - Ext.fly(document.body).getScroll().top
        }
    }

    var canvasMove = function (event) {
        Ext.fly(this.ctx.canvas).setStyle('cursor', 'move');
        if (this.operateState) {
            transXY = { x: transXY.x + event.x - this.operateState.x, y: transXY.y - event.y + this.operateState.y };
            this.redrawAll();
            this.operateState = { x: event.x, y: event.y };
        }
    }

    var canvasMouseDownHandler = function (event) {
        //event = event.browserEvent;  //使用了normalized: false,不需要转化
        for (var n in this.data.nodes) {
            if (this.ctx.distance(this.data.nodes[n].position, this.tranp({ x: event.x, y: event.y })) < 16) {
                this.selected = n;

                //this.ctx.canvas.onmousemove = function (scope) { return function (event) { return scope.canvasMouseMoveHandler.call(scope, event); } } (this);
                Ext.fly(this.ctx.canvas).on('mousemove', canvasMouseMoveHandler, this, { normalized: false });  // 上面也可以用

                //this.ctx.canvas.onmouseup = function (scope) { return function (event) { return scope.canvasMouseUpHandler.call(scope, event); } } (this);
                Ext.fly(this.ctx.canvas).on('mouseup', canvasMouseUpHandler, this);  // 上面也可以用

                return;
            }
        }

        this.operateState = { x: event.x, y: event.y }; //
        
        Ext.fly(this.ctx.canvas).on('mousemove', canvasMove, this, { normalized: false });  // 上面也可以用
        Ext.fly(this.ctx.canvas).on('mouseup', function () { this.operateState = null; Ext.fly(this.ctx.canvas).un('mousemove', canvasMove); }, this, { normalized: false });  // 上面也可以用
    }

    var canvasMouseUpHandler = function (event) {
        this.selected = null;
        Ext.fly(this.ctx.canvas).un('mousemove', canvasMouseMoveHandler);
        Ext.fly(this.ctx.canvas).un('mouseup', canvasMouseUpHandler);
        //this.ctx.canvas.onmousemove = this.ctx.canvas.onmouseup = null;
        
    }

    var canvasMouseMoveHandler = function (event) {
        //event = event.browserEvent;
        if (this.selected) {
            this.data.nodes[this.selected].position = this.tranp({ x: event.x, y: event.y });
            this.redrawAll();
        }
    }

    var canvasDblClick = function (event) {
        for (var n in this.data.nodes) {
            if (this.ctx.distance(this.data.nodes[n].position, this.tranp({ x: event.x, y: event.y })) < 16) {
                this.selected = n;
                break;
            }
        }
        alert(this.selected);
        this.selected = null;
    }


    Ext.fly(this.ctx.canvas).on( // 注册mousedown事件响应
        'mousedown',
        canvasMouseDownHandler,
        this,
        { normalized: false }
    );

    Ext.fly(this.ctx.canvas).on( // 注册mousedown事件响应
        'dblclick',
        canvasDblClick,
        this,
        { normalized: false }
    );

    this.fullCanvas();
}


// 扩展方法,计算两点间的距离
CanvasRenderingContext2D.prototype.distance = function (start, end) {
    return Math.sqrt(Math.pow(start.x - end.x, 2) + Math.pow(start.y - end.y, 2));
}

// 扩展方法,画终点是箭头的直线
CanvasRenderingContext2D.prototype.drawLineWithArrow = function (start, end) {
    //画线
    this.beginPath();
    this.moveTo(start.x, start.y);
    this.lineTo(end.x, end.y);
    this.stroke();

    this.save(); //保存状态

    // 将原点移动终点
    this.translate(end.x, end.y);

    // 计算直线角度
    var angle = Math.atan((end.y - start.y) / (end.x - start.x));

    // 将X轴与直线重合,且起点到终点的方法为X轴正方向
    this.rotate(end.x < start.x ? Math.PI + angle : angle);

    // 填充箭头
    this.fillStyle = "rgba(0, 0, 0, 1)";
    this.lineJoin = 'miter'; //'round','bevel','miter'
    this.beginPath();
    this.moveTo(-13, 5);
    this.lineTo(-9, 0);
    this.lineTo(-13, -5);
    this.lineTo(0, 0);
    this.fill();

    this.restore(); //恢复状态
}

WFGraph.nodeImgs = {
    Handle: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/handle.png"),
    XORSplit: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/split.png"),
    Start: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/start.png"),
    Finish: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/stop.png")
};