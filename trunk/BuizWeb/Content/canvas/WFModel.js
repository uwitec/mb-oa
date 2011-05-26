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

        if (this.data) {
            transXY = { x: this.data.OffsetX, y: this.data.OffsetY };
            this.redrawAll();
        }
    }

    this.redrawAll = function () {
        this.ctx.setTransform(1, 0, 0, -1, transXY.x, this.height - transXY.y);
        this.ctx.clearRect(-transXY.x, -transXY.y, this.width, this.height);

        var oldStrokeStyle = this.ctx.strokeStyle;
        this.ctx.strokeStyle = '#999';
        var oldLineWidth = this.ctx.lineWidth;
        this.ctx.lineWidth = 1;
        var oldfillStyle = this.ctx.fillStyle;
        this.ctx.fillStyle = '#999';
        this.ctx.drawLineWithArrow({ x: -10, y: 0 }, { x: 45, y: 0 });
        this.ctx.drawLineWithArrow({ x: 0, y: -10 }, { x: 0, y: 45 });
        this.ctx.fillStyle = oldfillStyle;
        this.ctx.lineWidth = oldLineWidth;
        this.ctx.strokeStyle = oldStrokeStyle;
        /*
        var n;
        for (n in this.data.lines) {
        this.drawLine(this.data.lines[n]);
        }*/
        /*
        var i, length;
        for (i = 0, length = this.data.lines.length; i < length; i++) {
        this.drawLine(this.data.lines[i]);
        }*/

        Ext.Array.forEach(this.data.lines, function (line) { this.drawLine(line) }, this);

        /*
        for (n in this.data.nodes) {
        this.drawNode(this.data.nodes[n]);
        }*/
        /*
        for (i = 0, length = this.data.nodes.length; i < length; i++) {
        this.drawNode(this.data.nodes[i]);
        }*/
        Ext.Array.forEach(this.data.nodes, function (node) { this.drawNode(node) }, this);

        //alert('dddd');
    }

    this.getLine = function (line) {
        var start = {}, end = {}; //, n, length = this.data.nodes.length;

        // 找当前连线相关的头尾节点
        /*
        for (n = 0; n < length; n++) {
        if (this.data.nodes[n].ID == line.from) {
        start = this.data.nodes[n].position
        }
        if (this.data.nodes[n].ID == line.to) {
        end = this.data.nodes[n].position
        }
        }*/

        start = this.data.nodes[Ext.Array.pluck(this.data.nodes, "ID").indexOf(line.from)].position;
        end = this.data.nodes[Ext.Array.pluck(this.data.nodes, "ID").indexOf(line.to)].position;

        
        var dis, lineBegin, lineEnd;
        if (line.middlePoint) {
            dis = this.ctx.distance(start, line.middlePoint);
            lineBegin = { x: line.middlePoint.x + (start.x - line.middlePoint.x) * (dis - 20) / dis, y: line.middlePoint.y + (start.y - line.middlePoint.y) * (dis - 20) / dis }; //起点让20px
            dis = this.ctx.distance(line.middlePoint,end);
            lineEnd = { x: line.middlePoint.x + (end.x - line.middlePoint.x) * (dis - 20) / dis, y: line.middlePoint.y + (end.y - line.middlePoint.y) * (dis - 20) / dis }; //终点让30px
        }
        else {
            dis = this.ctx.distance(start, end);
            lineBegin = { x: end.x + (start.x - end.x) * (dis - 20) / dis, y: end.y + (start.y - end.y) * (dis - 20) / dis }; //起点让20px
            lineEnd = { x: start.x + (end.x - start.x) * (dis - 20) / dis, y: start.y + (end.y - start.y) * (dis - 20) / dis }; //终点让30px
        }

        return { begin: lineBegin, end: lineEnd, middlePoint: line.middlePoint };
    }

    this.drawLine = function (line) {
        var l = this.getLine(line);
        var old = this.ctx.strokeStyle;
        this.ctx.strokeStyle = '#666';
        var oldfillStyle = this.ctx.fillStyle;
        this.ctx.fillStyle = '#666';
        // 因为节点图片本身点据一定空间,连线要让开这部分空间,因此需要重新测算起点和终点,具体大小应该根据图片自动设置,以后改进
        this.ctx.drawLineWithArrow(l.begin, l.end, l.middlePoint); // 画一条折线,middlePoint为中间经过的点
        if (l.middlePoint) {
            this.drawText(line.name, (2 * l.begin.x + l.middlePoint.x) / 3, (2 * l.begin.y + l.middlePoint.y) / 3); //将文本写在起点和中间点之间,如果中间点不存在,则用终点替代
        }
        else {
            this.drawText(line.name, (4 * l.begin.x + l.end.x) / 5, (4 * l.begin.y + l.end.y) / 5);
        }
        this.ctx.fillStyle = oldfillStyle;
        this.ctx.strokeStyle = old;
    }

    this.drawLineBold = function (line) {
        var oldLineWidth = this.ctx.lineWidth;
        this.ctx.lineWidth = 2;
        var oldStrokeStyle = this.ctx.strokeStyle;
        this.ctx.strokeStyle = '#111';
        var oldfillStyle = this.ctx.fillStyle;
        this.ctx.fillStyle = '#111';
        this.drawLine(line);
        this.ctx.fillStyle = oldfillStyle;
        this.ctx.strokeStyle = oldStrokeStyle;
        this.ctx.lineWidth = oldLineWidth;
    }

    this.drawNode = function (n) {
        // 当前使用的图片是24px的,所以让12
        this.ctx.drawImage(WFGraph.nodeImgs[n.type], n.position.x - 12, n.position.y - 12);
        this.drawText(n.name, n.position.x - 12, n.position.y - 12);
    }

    this.drawNodeRec = function (n) {
        // 当前使用的图片是24px的,所以让12
        this.ctx.strokeRect(n.position.x - 14, n.position.y - 14, 28, 28);
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
        var node = null, n, length = this.data.nodes.length;
        for (n = 0; n < length; n++) {
            if (this.ctx.distance(this.data.nodes[n].position, this.tranp({ x: event.x, y: event.y })) < 16) {
                node = this.data.nodes[n]; // 选中的节点,处理后直接返回
                break;
            }
        }
        return node;
    }

    this.captureLine = function (event) {
        var line = null, i, length = this.data.lines.length, BE;
        for (i = 0; i < length; i++) {
            // 如果选中节点,注册移动事件处理为节点移动
            BE = this.getLine(this.data.lines[i])
            if (BE.middlePoint) {
                var minDis = Math.min(
                    this.ctx.minDistance(this.tranp({ x: event.x, y: event.y }), { begin: BE.begin, end: BE.middlePoint }),
                    this.ctx.minDistance(this.tranp({ x: event.x, y: event.y }), { begin: BE.middlePoint, end: BE.end })
                    );
                if (minDis < 6) {
                    line = this.data.lines[i];
                    break;
                }
            }
            else if (this.ctx.minDistance(this.tranp({ x: event.x, y: event.y }), BE) < 6) {
                line = this.data.lines[i];
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
                var n = null;
                if (n = this.captureNode(event)) {
                    this.operateState.params = { selected: n };
                    Ext.fly(this.ctx.canvas).on('mousemove',
                            function (event) {
                                //event = event.browserEvent;
                                if (this.operateState.params && this.operateState.params.selected) {
                                    this.operateState.params.selected.position = this.tranp({ x: event.getPageX(), y: event.getPageY() });
                                    this.redrawAll();
                                }
                            }, // function (event) 
                            this,
                            { normalized: true }
                        ); //Ext.fly(this.ctx.canvas).on
                    Ext.fly(this.ctx.canvas).on('mouseup',
                            function (event) {
                                if (this.operateState.params && this.operateState.params.selected) {
                                    Ext.Ajax.request({
                                        url: '/workflow/Manage/modifyTemplate',
                                        params: {
                                            templateId: this.data.ID,
                                            mode: 'select',
                                            nodeID: this.operateState.params.selected.ID,
                                            x: this.operateState.params.selected.position.x,
                                            y: this.operateState.params.selected.position.y
                                        },
                                        success: function (response) {
                                            //alert('保存成功!');
                                            //wfg.redrawAll();
                                            Ext.getCmp('graphStatus').setText('节点移动...'); //这句不应该回这里
                                        },
                                        failure: function (response) {
                                            alert(response.responseText);
                                        }
                                    });
                                }
                                this.resetOperateState();
                            }, // function (event) 
                            this,
                            { normalized: false }
                    )
                }
                else {
                    if (n = this.captureLine(event)) { // 如果选中连线,则视为调整连线中间点
                        this.operateState.params = { selected: n };
                        Ext.fly(this.ctx.canvas).on('mousemove',
                            function (event) {
                                //event = event.browserEvent;
                                if (this.operateState.params && this.operateState.params.selected) {
                                    var pos = this.tranp({ x: event.getPageX(), y: event.getPageY() });
                                    //debugger;
                                    if (this.ctx.minDistance(pos, this.getLine(n)) > 6) {
                                        this.operateState.params.selected.middlePoint = pos;
                                    }
                                    else {
                                        this.operateState.params.selected.middlePoint = null; //接近节点直连线,则消除中间点
                                    }
                                    this.redrawAll();
                                }
                            }, // function (event) 
                            this,
                            { normalized: true }
                        ); //Ext.fly(this.ctx.canvas).on
                        Ext.fly(this.ctx.canvas).on('mouseup',
                            function (event) {
                                if (this.operateState.params && this.operateState.params.selected) {
                                    Ext.Ajax.request({
                                        url: '/workflow/Manage/modifyTemplate',
                                        params: {
                                            templateId: this.data.ID,
                                            mode: 'select',
                                            lineID: this.operateState.params.selected.ID,
                                            x: this.operateState.params.selected.middlePoint ? this.operateState.params.selected.middlePoint.x : null,
                                            y: this.operateState.params.selected.middlePoint ? this.operateState.params.selected.middlePoint.y : null
                                        },
                                        success: function (response) {
                                            //alert('保存成功!');
                                            //wfg.redrawAll();
                                            Ext.getCmp('graphStatus').setText('中间点...'); //这句不应该回这里
                                        },
                                        failure: function (response) {
                                            alert(response.responseText);
                                        }
                                    });
                                }
                                this.resetOperateState();
                            }, // function (event) 
                            this,
                            { normalized: false }
                    )
                    }
                }
            },
            mousemove: function (event) {
                this.redrawAll(); //用于消除原选择框,笨!!!
                var pos = this.tranp({ x: event.x, y: event.y })
                Ext.getCmp('graphStatus').setText('[' + pos.x + ',' + pos.y + ']'); //这句不应该回这里
                var overEl;
                if (overEl = this.captureNode(event)) {
                    this.drawNodeRec(overEl);
                }
                else {
                    if (overEl = this.captureLine(event)) {
                        this.drawLineBold(overEl);
                    }
                }
            },
            mouseup: null,
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
                Ext.Ajax.request({
                    url: '/workflow/Manage/modifyTemplate',
                    params: {
                        templateId: this.data.ID,
                        mode: 'move',
                        x: transXY.x,
                        y: transXY.y
                    },
                    success: function (response) {
                        //alert('保存成功!');
                        //wfg.redrawAll();
                        Ext.getCmp('graphStatus').setText('画布移动...'); //这句不应该回这里
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
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
                var nodeID = GUID();
                // 打开表单编辑，提交再生成后
                new Ext.window.Window({
                    title: '流程处理节点信息',
                    modal: true,
                    closable: true,
                    id: 'fwin',
                    //animateTarget: this,
                    width: 600,
                    //height: 500,
                    layout: 'fit',
                    bodyPadding: 0,
                    items: [
                        new MB.form.WFNodeHandle({
                            templateId: this.data.ID,
                            //id:'',
                            x: this.tranp(event).x,
                            y: this.tranp(event).y,
                            close: function () { Ext.getCmp('fwin').close(); },
                            submitSccess: function (form, action) {
                                alert("submitSccess");
                                wfg.data.nodes.push({ ID: form.getValues()["ID"], name: form.getValues()["Name"], type: 'WFNodeHandle', position: { x: parseInt(form.getValues()['x']), y: parseInt(form.getValues()['y'])} });
                                wfg.redrawAll();
                                this.close(); 
                            },
                            submitFailure: function (form, action) { alert("submitFailure"); }
                        })
                    ]
                }).show();
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
                this.data.nodes.push({ ID: GUID(), name: 'new', type: 'WFNodeEvent', position: this.tranp(event) });
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
                var nodeID = GUID();
                // 打开表单编辑，提交再生成后
                new Ext.window.Window({
                    title: '分支节点信息',
                    id: 'fwin',
                    modal: true,
                    closable: true,
                    //animateTarget: this,
                    width: 600,
                    //height: 500,
                    layout: 'fit',
                    bodyPadding: 0,
                    items: [
                        new MB.form.WFNodeXORSplit({
                            templateId: this.data.ID,
                            //id:'',
                            x: this.tranp(event).x,
                            y: this.tranp(event).y,
                            close: function () { Ext.getCmp('fwin').close(); },
                            submitSccess: function (form, action) {
                                alert("submitSccess");
                                wfg.data.nodes.push({ ID: form.getValues()["ID"], name: form.getValues()["Name"], type: 'WFNodeXORSplit', position: { x: parseInt(form.getValues()['x']), y: parseInt(form.getValues()['y'])} });
                                wfg.redrawAll();
                                this.close();
                            },
                            submitFailure: function (form, action) { alert("submitFailure"); }
                        })
                    ]
                }).show();
            },
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
                        if (n.ID != this.operateState.params.preNode.ID) {
                            var findFlag = false;
                            for (var l in this.data.lines) {//判断连接已经存在
                                if (this.data.lines[l].from == this.operateState.params.preNode.ID && this.data.lines[l].to == n.ID) {
                                    findFlag = true;
                                    break;
                                }
                            }
                            if (findFlag) {
                                alert('连接已经存在!!');
                            }
                            else { // 所有检查通过
                                switch (this.operateState.params.preNode.type) {
                                    case 'WFNodeHandle':
                                        new Ext.window.Window({
                                            title: '节点连接信息',
                                            id: 'fwin',
                                            modal: true,
                                            closable: true,
                                            width: 600,
                                            layout: 'fit',
                                            bodyPadding: 0,
                                            items: [
                                            new MB.form.WFNodeAction({
                                                templateId: this.data.ID, // 一定要传,取实体上下文
                                                from: this.operateState.params.preNode.ID,
                                                to: n.ID,
                                                //id:'',
                                                close: function () { Ext.getCmp('fwin').close(); },
                                                submitSccess: function (form, action) {
                                                    alert("submitSccess");
                                                    wfg.data.lines.push({ ID: form.getValues()["ID"], name: form.getValues()["Name"], from: form.getValues()["from"], to: form.getValues()["to"] });
                                                    wfg.operateState.params = { preNode: null };
                                                    wfg.redrawAll();
                                                    this.close();
                                                },
                                                submitFailure: function (form, action) { alert("submitFailure"); }
                                            })
                                        ]
                                        }).show();
                                        break;
                                    case 'WFNodeXORSplit':
                                        new Ext.window.Window({
                                            title: '节点连接信息',
                                            id: 'fwin',
                                            modal: true,
                                            closable: true,
                                            width: 600,
                                            layout: 'fit',
                                            bodyPadding: 0,
                                            items: [
                                            new MB.form.WFNodeExpression({
                                                templateId: this.data.ID, // 一定要传,取实体上下文
                                                from: this.operateState.params.preNode.ID,
                                                to: n.ID,
                                                //id:'',
                                                close: function () { Ext.getCmp('fwin').close(); },
                                                submitSccess: function (form, action) {
                                                    alert("submitSccess");
                                                    wfg.data.lines.push({ ID: form.getValues()["ID"], name: '[' + form.getValues()["OrderNO"] + ']' + form.getValues()["Expression"], from: form.getValues()["from"], to: form.getValues()["to"] });
                                                    wfg.operateState.params = { preNode: null };
                                                    wfg.redrawAll();
                                                    this.close();
                                                },
                                                submitFailure: function (form, action) { alert("submitFailure"); }
                                            })
                                        ]
                                        }).show();
                                        break;
                                    case 'WFNodeStart':
                                        Ext.Ajax.request({
                                            url: '/workflow/Manage/setStartNodeNext',
                                            params: {
                                                templateId: this.data.ID,
                                                from: this.operateState.params.preNode.ID,
                                                to: n.ID
                                            },
                                            scope: this,
                                            success: function (response, opts) {
                                                alert("submitSccess");
                                                wfg.data.lines.push({ ID: this.operateState.params.preNode.ID, name: '', from: this.operateState.params.preNode.ID, to: n.ID });
                                                wfg.operateState.params = { preNode: null };
                                                wfg.redrawAll();
                                            },
                                            failure: function (response) {
                                                alert(response.responseText);
                                            }
                                        });
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else {
                            this.operateState.params = null;
                            this.redrawAll();
                        }
                    }
                }
                else {
                    this.operateState.params = { preNode: n };
                    this.drawNodeRec(n);
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
                if (n && confirm("确认删除该节点吗?")) { //如果删除的是节点,该节点必须没有被使用到
                    if (n.type == 'WFNodeStart' || n.type == 'WFNodeFinish') {
                        alert('开始节点和结束节点不能删除!');
                        return;
                    }

                    /*var hasLine = false;
                    
                    for (var line in this.data.lines) {
                    if (this.data.lines[line].from == n.ID || this.data.lines[line].to == n.ID) {
                    hasLine = true;
                    break;
                    }  // end of if
                    }*/
                    var hasLine = Ext.Array.some(this.data.lines, function (line) { return line.from == n.ID || line.to == n.ID }, this);
                    /*
                    */
                    if (hasLine) {
                        alert('该节点被其他节点使用到,不能删除!');
                        return;
                    }
                    else {
                        // 删除节点
                        Ext.Ajax.request({
                            url: '/workflow/Manage/deleteNode',
                            params: {
                                templateId: this.data.ID,
                                nodeID: n.ID
                            },
                            scope: this,
                            success: function (response, opts) {
                                var i;
                                for (i = 0; i < wfg.data.nodes.length; i++) {
                                    if (this.data.nodes[i].ID == opts.params.nodeID) //response.request.options.params.nodeID
                                        break;
                                }
                                if (i < this.data.nodes.length) {
                                    this.data.nodes.splice(i, 1);
                                }
                                this.redrawAll();
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });
                    }
                }
                else { // 不是删除节点,就是删除连线
                    n = this.captureLine(event); //取line
                    if (n && confirm("确认删除该连接吗?")) {
                        Ext.Ajax.request({
                            url: '/workflow/Manage/deleteLine',
                            params: {
                                templateId: this.data.ID,
                                LineID: n.ID
                            },
                            scope: this,
                            success: function (response, opts) {
                                var i = -1;
                                /*for (i = 0; i < wfg.data.lines.length; i++) {
                                if (wfg.data.lines[i].ID == opts.params.LineID) //response.request.options.params.nodeID
                                break;
                                }*/
                                i = Ext.Array.pluck(this.data.lines, "ID").indexOf(opts.params.LineID); //没找到返回－1
                                if (i >= 0) {
                                    this.data.lines.splice(i, 1);
                                    //Ext.Array.remove(wfg.data.lines, wfg.data.lines[i]);
                                }
                                this.redrawAll();
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });
                    }
                }
            },
            mousemove: function (event) {
                this.redrawAll();
                var pos = this.tranp({ x: event.x, y: event.y })
                Ext.getCmp('graphStatus').setText('[' + pos.x + ',' + pos.y + ']'); //这句不应该回这里
                var overEl;
                if (overEl = this.captureNode(event)) {
                    this.drawNodeRec(overEl);
                }
                else {
                    if (overEl = this.captureLine(event)) {
                        this.drawLineBold(overEl);
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
        this.operateState.params = null;

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
CanvasRenderingContext2D.prototype.drawLineWithArrow = function (start, end, middlePoint) {
    //画线
    this.beginPath();
    this.moveTo(start.x, start.y);
    if (middlePoint) {
        this.lineTo(middlePoint.x, middlePoint.y);
        start = middlePoint;
    }
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
    //this.fillStyle = "rgba(0, 0, 0, 1)";
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
    WFNodeHandle: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/handle.png"),
    WFNodeXORSplit: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/split.png"),
    WFNodeStart: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/start.png"),
    WFNodeEvent: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/event24.png"),
    WFNodeFinish: function (imgSrc) { var img = new Image(); img.src = imgSrc; return img; } ("/content/canvas/stop.png")
};

Array.prototype.remove = function () { }