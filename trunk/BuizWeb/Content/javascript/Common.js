var tabActiveOrders = new Array();
function addTab(url, text) {
    var Id = "tab" + hashCode(url); // Ext.id([Mixed el], [String prefix])
    var mainTabPanel = Ext.getCmp('mainTabPanel');
    var tab = Ext.getCmp(Id);
    if (tab) {
        //mainTabPanel.remove(Id); // 如果该选项卡面板里已有选项卡，先将其移除
        mainTabPanel.setActiveTab(tab); //激活
    }
    else {
        tab = Ext.create('Ext.panel.Panel', {
            id: Id, // 此id 不是html元素的ID
            closable: true,
            autoDestroy: true,
            autoShow:true,
            border: 0,
            title: text,
            hideMode: 'offsets',//在隐藏时保持面板尺寸
            html: '<iframe id="' + Id + '" name="' + Id + '" src="' + url + '" width="100%" height="100%" frameborder="0" ></iframe>',
            listeners: {
                beforedestroy: function (source, e) { source.body.dom.getElementsByTagName("iframe")[0].src = "javascript:false"; } ,
                activate: function () { this.doLayout(); }
                /*render: function(){ this.ownerCt.doLayout(); }*/
            }
                });
        mainTabPanel.add(tab);

//        tab = mainTabPanel.add(Ext.create('Ext.ux.SimpleIFrame', {
//            id: Id, // 此id 不是html元素的ID
//            closable: true,
//            //autoDestroy: true,
//            autoShow: true,
//            title: text,
//            border: false,
//            src: url,
//            listeners: {
//                deactivate: function () { this.doLayout(); }
//            }
//        }));

        mainTabPanel.setActiveTab(tab);
        //mainTabPanel.doLayout();
        tab.on("deactivate", function (t) {
            tabActiveOrders.push(t.id);
        });
        tab.on("removed", function (t, ownerCt) {
            //debugger;
            var tab = Ext.getCmp(tabActiveOrders.pop());
            while (!tab) {
                tab = Ext.getCmp(tabActiveOrders.pop());
            }
            if (tab)
                ownerCt.setActiveTab(tab);
        });
    }
}

//<a href="javascript:this.parent.addTab('/system/auth/privilege','test')">parent.addTab</a>
//<a href="javascript:this.parent.closeMe(window)">parent.closeme()</a> 
function closeMe(iframe) {
    Ext.getCmp('mainTabPanel').remove(Ext.getCmp(iframe.name), true);
}

function hashCode(str) {
    //同一URL返回不同的值
    var now = new Date();
    //str = str + now.toTimeString() + now.getMilliseconds();

    var hash = 18951163698;

    for (var i = 0; i < str.length; i++) {
        hash ^= ((hash << 5) + str.charCodeAt(i) + (hash >> 2));
    }

    return (hash & 0x7FFFFFFF);
}


Ext.example = function () {
    var msgCt;

    function createBox(t, s) {
        return '<div class="msg"><h3>' + t + '</h3><p>' + s + '</p></div>';
    }
    return {
        msg: function (title, format) {
            if (!msgCt) {
                msgCt = Ext.core.DomHelper.insertFirst(document.body, { id: 'msg-div' }, true);
            }
            var s = Ext.String.format.apply(String, Array.prototype.slice.call(arguments, 1));
            var m = Ext.core.DomHelper.append(msgCt, createBox(title, s), true);
            m.hide();
            m.slideIn('b').ghost("b", { delay: 3000, remove: true });
            //m.slideIn('b').slideOut("r", { delay: 3000, remove: true });
        },

        init: function () {
        }
    };
} ();


// vim: sw=2:ts=2:nu:nospell:fdc=2:expandtab
/**
* @class Ext.ux.SimpleIFrame
* @extends Ext.Panel
*
* A simple ExtJS 4 implementaton of an iframe providing basic functionality.
* For example:
*
* var panel=Ext.create('Ext.ux.SimpleIFrame', {
*   border: false,
*   src: 'http://localhost'
* });
* panel.setSrc('http://www.sencha.com');
* panel.reset();
* panel.reload();
* panel.getSrc();
* panel.update('<div><b>Some Content....</b></div>');
* panel.destroy();
*
* @author    Conor Armstrong
* @copyright (c) 2011 Conor Armstrong
* @date      12 April 2011
* @version   0.1
*
* @license Ext.ux.SimpleIFrame.js is licensed under the terms of the Open Source
* LGPL 3.0 license. Commercial use is permitted to the extent that the 
* code/component(s) do NOT become part of another Open Source or Commercially
* licensed development library or toolkit without explicit permission.
* 
* <p>License details: <a href="http://www.gnu.org/licenses/lgpl.html"
* target="_blank">http://www.gnu.org/licenses/lgpl.html</a></p>
*
*/

Ext.require([
	'Ext.panel.*'
]);

Ext.define('Ext.ux.SimpleIFrame', {
    extend: 'Ext.Panel',
    alias: 'widget.simpleiframe',
    src: 'about:blank',
    loadingText: 'Loading ...',
    initComponent: function () {
        this.updateHTML();
        this.callParent(arguments);
    },
    updateHTML: function () {
        this.html = '<iframe id="iframe-' + this.id + '"' +
        ' style="overflow:auto;width:100%;height:100%;"' +
        ' frameborder="0" ' +
        ' src="' + this.src + '"' +
        '></iframe>';
    },
    reload: function () {
        this.setSrc(this.src);
    },
    reset: function () {
        var iframe = this.getDOM();
        var iframeParent = iframe.parentNode;
        if (iframe && iframeParent) {
            iframe.src = 'about:blank';
            iframe.parentNode.removeChild(iframe);
        }

        iframe = document.createElement('iframe');
        iframe.frameBorder = 0;
        iframe.src = this.src;
        iframe.id = 'iframe-' + this.id;
        iframe.style.overflow = 'auto';
        iframe.style.width = '100%';
        iframe.style.height = '100%';
        iframeParent.appendChild(iframe);
    },
    setSrc: function (src, loadingText) {
        this.src = src;
        var iframe = this.getDOM();
        if (iframe) {
            iframe.src = src;
        }
    },
    getSrc: function () {
        return this.src;
    },
    getDOM: function () {
        return document.getElementById('iframe-' + this.id);
    },
    getDocument: function () {
        var iframe = this.getDOM();
        iframe = (iframe.contentWindow) ? iframe.contentWindow : (iframe.contentDocument.document) ? iframe.contentDocument.document : iframe.contentDocument;
        return iframe.document;
    },
    destroy: function () {
        var iframe = this.getDOM();
        if (iframe && iframe.parentNode) {
            iframe.src = 'about:blank';
            iframe.parentNode.removeChild(iframe);
        }
        this.callParent(arguments);
    },
    update: function (content) {
        this.setSrc('about:blank');
        try {
            var doc = this.getDocument();
            doc.open();
            doc.write(content);
            doc.close();
        } catch (err) {
            // reset if any permission issues
            this.reset();
            var doc = this.getDocument();
            doc.open();
            doc.write(content);
            doc.close();
        }
    }
});

function toLocalWeekDay(en) {
    switch (en.toLowerCase()) {
        case 'sunday':
            return '星期天';
            break;
        case 'monday':
            return '星期一';
            break;
        case 'tuesday':
            return '星期二';
            break;
        case 'wednesday':
            return '星期三';
            break;
        case 'thursday':
            return '星期四';
            break;
        case 'friday':
            return '星期五';
            break;
        case 'saturday':
            return '星期六';
            break;
        default: break;
    }
}