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
            html: '<iframe src="' + url + '" width="100%" height="100%" frameborder="0" ></iframe>',
            listeners: {
                beforedestroy: function (source, e) { source.body.dom.getElementsByTagName("iframe")[0].src = "javascript:false"; }
            }
        });
        mainTabPanel.add(tab);
        //mainTabPanel.show();
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