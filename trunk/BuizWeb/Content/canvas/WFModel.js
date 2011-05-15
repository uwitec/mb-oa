﻿/*
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
lines: [
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
        for (n in this.data.lines) {
            this.drawLine(this.data.lines[n]);
        }

        for (n in this.data.nodes) {
            this.drawNode(this.data.nodes[n]);
        }

        //alert('dddd');
    }

    this.getLine = function (line) {
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
        var lineBegin = { x: end.x + (start.x - end.x) * (dis - 20) / dis, y: end.y + (start.y - end.y) * (dis - 20) / dis }; //起点让20px
        var lineEnd = { x: start.x + (end.x - start.x) * (dis - 20) / dis, y: start.y + (end.y - start.y) * (dis - 20) / dis }; //终点让30px

        return { begin: lineBegin, end: lineEnd };
    }

    this.drawLine = function (line) {
        var l = this.getLine(line);

        // 因为节点图片本身点据一定空间,连线要让开这部分空间,因此需要重新测算起点和终点,具体大小应该根据图片自动设置,以后改进
        this.ctx.drawLineWithArrow(l.begin, l.end);
        this.drawText(line.name, (l.begin.x + l.end.x) / 2, (l.begin.y + l.end.y) / 2);
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

    this.tranp = function (point) {
        return {
            x: point.x - transXY.x - Ext.fly(this.ctx.canvas).getOffsetsTo(document.body)[0] + Ext.fly(document.body).getScroll().left,
            y: this.height - transXY.y - point.y + Ext.fly(this.ctx.canvas).getOffsetsTo(document.body)[1] - Ext.fly(document.body).getScroll().top
        }
    }

    this.captureNode = function (event) {
        var node = null;
        for (var n in this.data.nodes) {
            // 如果选中节点,注册移动事件处理为节点移动
            if (this.ctx.distance(this.data.nodes[n].position, this.tranp({ x: event.x, y: event.y })) < 16) {
                node = this.data.nodes[n]; // 选中的节点,处理后直接返回
                break;
            }
        }
        return node;
    }

    this.captureLine = function (event) {
        var line = null;
        for (var l in this.data.lines) {
            // 如果选中节点,注册移动事件处理为节点移动
            if (this.ctx.minDistance(this.tranp({ x: event.x, y: event.y }), this.getLine(this.data.lines[l])) < 6) {
                line = this.data.lines[l];
                break;
            }
        }
        return line;
    }

    // select 操作模式封装,处理节点移动,视口移动,双击节点事件
    var selectOperateState = {
        mode: 'select',
        listeners: {
            mousedown: function (event) {
                //event = event.browserEvent;  //使用了normalized: false,不需要转化
                var n = this.captureNode(event);
                if (n) {
                    this.operateState.params = { selected: n };
                    Ext.fly(this.ctx.canvas).on('mousemove',
                            function (event) {
                                //event = event.browserEvent;
                                if (this.operateState.params && this.operateState.params.selected) {
                                    this.operateState.params.selected.position = this.tranp({ x: event.x, y: event.y });
                                    this.redrawAll();
                                }
                            }, // function (event) 
                            this,
                            { normalized: false }
                        ); //Ext.fly(this.ctx.canvas).on
                }
            },
            mousemove: null,
            mouseup: function (event) {
                this.resetOperateState();
            },
            dblclick: function (event) {
                var n = this.captureNode(event) || this.captureLine(event);
                if (n)
                    alert(n.name);
            }
        },
        params: null
    };

    var moveOperateState = {
        mode: 'move',
        listeners: {
            mousedown: function (event) {
                //event = event.browserEvent;  //使用了normalized: false,不需要转化
                this.operateState.params = { x: event.x, y: event.y }; //
                Ext.fly(this.ctx.canvas).on('mousemove',
                    function (event) { //////////////////////////////
                        if (this.operateState.params && this.operateState.params.x) {
                            transXY = { x: transXY.x + event.x - this.operateState.params.x, y: transXY.y - event.y + this.operateState.params.y };
                            this.redrawAll();
                            this.operateState.params = { x: event.x, y: event.y };
                        }
                    }, ///////////////////////////////////////////////////////
                    this,
                    { normalized: false }
                );
            },
            mousemove: null,
            mouseup: function (event) {
                this.operateState.params = null;
            },
            dblclick: null
        },
        params: null
    };

    var newHandleOperateState = {
        mode: 'newHandle',
        listeners: {
            mousedown: function (event) {
                this.data.nodes.push({ ID: GUID(), name: 'new', type: 'Handle', position: this.tranp(event) });
                this.redrawAll();
            },
            mousemove: null,
            mouseup: null,
            dblclick: null
        },
        params: null
    };

    var newEventOperateState = {
        mode: 'newEvent',
        listeners: {
            mousedown: function (event) {
                this.data.nodes.push({ ID: GUID(), name: 'new', type: 'Event', position: this.tranp(event) });
                this.redrawAll();
            },
            mousemove: null,
            mouseup: null,
            dblclick: null
        },
        params: null
    };

    var newSplitOperateState = {
        mode: 'newSplit',
        listeners: {
            mousedown: function (event) {
                this.data.nodes.push({ ID: GUID(), name: 'new', type: 'XORSplit', position: this.tranp(event) });
                this.redrawAll();
            },
            mousemove: null,
            mouseup: null,
            dblclick: null
        },
        params: null
    };

    var newTransOperateState = {
        mode: 'newTrans',
        listeners: {
            click: function (event) {
                var n = this.captureNode(event);
                if (this.operateState.params && this.operateState.params.preNode) {
                    if (n) {
                        if (n.ID != this.operateState.params.preNode.ID) { //还有一种情况,如果连接已经存在
                            this.data.lines.push({ ID: GUID(), from: this.operateState.params.preNode.ID, to: n.ID, name: '未命名' });
                            this.operateState.params = { preNode: null };
                            this.redrawAll();
                        }
                        else {
                            alert('两端的节点必须是不同的!');
                        }
                    }
                }
                else {
                    this.operateState.params = { preNode: n };
                }
            },
            mousemove: null,
            mouseup: null,
            dblclick: null
        },
        params: null
    };

    var deleteOperateState = {
        mode: 'delete',
        listeners: {
            click: function (event) {
                var n = this.captureNode(event);
                if (n && confirm("确认删除该节点及其所有连接吗?")) {
                    for (var line in this.data.lines) {
                        if (line.from == n.ID || line.to == n.ID) {
                            var b = this.data.lines.indexOf(line);
                            if (b >= 0) {
                                this.data.lines.splice(b, 1);
                            }
                        }  // end of if
                    }
                    var a = this.data.nodes.indexOf(n);
                    if (a >= 0) {
                        this.data.nodes.splice(a, 1);
                    }
                    this.redrawAll();
                }
                else {
                    n = this.captureLine(event);
                    if (n && confirm("确认删除该连接吗?")) {
                        var a = this.data.lines.indexOf(n);
                        if (a >= 0) {
                            this.data.lines.splice(a, 1);
                        }
                        this.redrawAll();
                    }
                }
            }
        },
        params: null
    };

    // 可用的操作状态列表
    var operateStates = {
        select: selectOperateState,
        move: moveOperateState,
        newHandle: newHandleOperateState,
        newSplit: newSplitOperateState,
        newTrans: newTransOperateState,
        'delete': deleteOperateState,
        newEvent: newEventOperateState
    };

    // 操作状态的模式及参数
    this.operateState = { };

    // 重置操作状态为初始,初始为select
    this.resetOperateState = function () {
        this.setOperateState("select");
    }

    // 设置操作状态
    this.setOperateState = function (model) {
        Ext.fly(this.ctx.canvas).clearListeners(); //清除所有事件监听

        this.operateState = operateStates[model];
        for (var eventName in this.operateState.listeners) {
            if (this.operateState.listeners[eventName]) {// 注册mousedown事件响应
                Ext.fly(this.ctx.canvas).on(eventName, this.operateState.listeners[eventName], this, { normalized: false });
            }
        }
    }

    this.resetOperateState();

    this.fullCanvas();
}



// 扩展方法,计算两点间的距离
CanvasRenderingContext2D.prototype.distance = function (start, end) {
    return Math.sqrt(Math.pow(start.x - end.x, 2) + Math.pow(start.y - end.y, 2));
}

// 扩展方法,计算点到线段的距离,如果垂足在线段外,则取点到线段端点距离的最小值
// point: {x:10,y:10} , segment: {begin: {x:10,y:10},end: {x:10,y:10}}
// 假设线外点为P,线段起点为B,线段终点为E
CanvasRenderingContext2D.prototype.minDistance = function (point, segment) {
    var PB = this.distance(point, segment.begin);
    var PE = this.distance(point, segment.end);
    var BE = this.distance(segment.begin, segment.end);

    if (BE * BE + PB * PB <= PE * PE)  //PBE是钝角
        return PB;
    if (BE * BE + PE * PE <= PB * PB)
        return PE;

    var p = (PB + PE + BE) / 2;
    return Math.sqrt(p * (p - PB) * (p - PE) * (p - BE)) * 2 / BE; //海伦公式
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
    Event: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/event24.png"),
    Finish: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/stop.png")
};