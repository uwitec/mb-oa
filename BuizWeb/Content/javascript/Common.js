function addTab(url, text) {
    var Id = "tab" + hashCode(url); // Ext.id([Mixed el], [String prefix])
    var mainTabPanel = Ext.getCmp('mainTabPanel');
    var tab = Ext.getCmp(Id);
    if (tab) {
        //mainTabPanel.remove(Id); // 如果该选项卡面板里已有选项卡，先将其移除
        mainTabPanel.setActiveTab(tab); //激活
    }
    else {
//        tab = Ext.create('Ext.panel.Panel', {
//            id: Id, // 此id 不是html元素的ID
//            closable: true,
//            autoDestroy: true,
//            //autoShow:true,
//            border: 0,
//            title: text,
//html: '<iframe src="' + url + '" width="100%" height="100%" frameborder="0" ></iframe>',
//            listeners: {
//                beforedestroy: function (source, e) { source.body.dom.getElementsByTagName("iframe")[0].src = "javascript:false"; }/*,
//                render: function(){ this.ownerCt.doLayout(); }*/
//            }
        //        });

        
        tab = mainTabPanel.add(Ext.create('Ext.ux.SimpleIFrame', {
            id: Id, // 此id 不是html元素的ID
            closable: true,
            //autoDestroy: true,
            //autoShow:true,
            title: text,
            border: false,
            src: url
        }));
        tab.show();
        mainTabPanel.doLayout();
        mainTabPanel.setActiveTab(tab);
    }
}

function hashCode(str) {
    //同一URL返回不同的值
    var now = new Date();
    str = str + now.toTimeString() + now.getMilliseconds();

    var hash = 18951163698;

    for (var i = 0; i < str.length; i++) {
        hash ^= ((hash << 5) + str.charCodeAt(i) + (hash >> 2));
    }

    return (hash & 0x7FFFFFFF);
}


// from: http://www.rockstown.com/node/4
// vim: sw=2:ts=2:nu:nospell:fdc=2:expandtab
/**
* @class Ext.ux.SimpleIFrame
* @extends Ext.Panel
*
* A simple ExtJS 4 implementaton of an iframe providing basic functionality.
* For example:
*
* var panel=Ext.create('Ext.ux.SimpleIFrame', {
*   border: false,
*   src: '<a href="http://localhost">http://localhost</a>'
* });
* panel.setSrc('<a href="http://www.sencha.com">http://www.sencha.com</a>');
* panel.reset();
* panel.reload();
* panel.getSrc();
* panel.update('<div><b>Some Content....</b></div>');
* panel.destroy();
*
* @author    Conor Armstrong
* @copyright (c) 2011 Conor Armstrong
* @date      12 April 2011
* @version   0.1
*
* @license Ext.ux.SimpleIFrame.js is licensed under the terms of the Open Source
* LGPL 3.0 license. Commercial use is permitted to the extent that the 
* code/component(s) do NOT become part of another Open Source or Commercially
* licensed development library or toolkit without explicit permission.
* 
* <p>License details: <a href="<a href="http://www.gnu.org/licenses/lgpl.html">http://www.gnu.org/licenses/lgpl.html</a>"
* target="_blank"><a href="http://www.gnu.org/licenses/lgpl.html</a></p>">http://www.gnu.org/licenses/lgpl.html</a></p></a>
*
*/
Ext.require(['Ext.panel.*'
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
        this.html = '<iframe id="iframe-' + this.id + '"' + ' width="100%" height="100%"' + ' frameborder="0" ' + ' src="' + this.src + '"' + '></iframe>';
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
        this.getDOM().src = 'javascript:false'; //chw
//        var iframe = this.getDOM();
//        if (iframe && iframe.parentNode) {
//            iframe.src = 'about:blank';
//            iframe.parentNode.removeChild(iframe);
//        }
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


var addportlet = function () {
    var portalpanel = Ext.getCmp('app-portal');
    portalpanel.items.items[0].add(
        new Ext.panel.Panel({
            title: 'adsfasdf',
            html: 'dafasdfasdf',
            frame: true,
            //tools: Portal.getTools,
            closable:true,
            draggable:true,  
            cls:'x-portlet',
            listeners: {
                'close': Ext.bind(Portal.onPortletClose, this)
            }
        })
            );
    portalpanel.doLayout();
    //debugger;
}