﻿var MB = function () { };
MB.form = function () { };

/**用于存放表单的通用配置项
*/
MB.form.FormConfig = {
};

/** 模块数据表单
config:
    submitSccess: fn // 提交成功
    submitFailure: fn //提交失败
    close: fn //关闭
*/
MB.form.Module = function (config) {
    this.name = "模块表单";
    this.form = new Ext.form.FormPanel({
        //url: '/system/auth/CreateModule',
        params: { sid: config.id, jid: 'ssss' },
        layout: 'column',
        //autoHeight: true,
        autoScroll: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'none', width: '100%', anchor: '100%', columnWidth: .5, margin: "4px 10px" },
        items: [
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'textfield', name: 'moduleCode', fieldLabel: '模块编码' },
            { xtype: 'textfield', name: 'moduleName', fieldLabel: '模块名称' },
            { xtype: 'textareafield', name: 'moduleDescription', columnWidth: 1, fieldLabel: '模块说明'}
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: config.id ? '/system/auth/UpdateModule' : '/system/auth/CreateModule',
                    success: function (form, action) { if (config && config.submitSccess) config.submitSccess(form, action) },
                    failure: function (form, action) { if (config && config.submitFailure) config.submitFailure(form, action) }
                }
                );
            }
            },
            { text: '取消', handler: function () { if (config && config.close) config.close() } }
        ]
    })

    //debugger;

    if (config.id) {
        this.form.getForm().load({
            url: '/system/auth/getModule',
            params: { id: config.id }
        }
        );
    }

    return this.form;
}

/** 用户数据表单
*/
MB.form.User = function (config) {
    this.name = "用户表单";
    this.form = new Ext.form.FormPanel({
        layout: 'column',
        //autoHeight: true,
        autoScroll: true,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'side', width: '100%', anchor: '100%', columnWidth: .5, margin: "4px 10px" },
        items: [
            { xtype: 'textfield', name: '用户编码', fieldLabel: '用户编码', allowBlank: false },
            { xtype: 'textfield', name: '用户名称', fieldLabel: '用户名称', allowBlank: false },
            { xtype: 'textfield', name: '初始口令', fieldLabel: '初始口令', allowBlank: false },
            { xtype: 'textfield', name: '确认初始口令', fieldLabel: '确认初始口令', allowBlank: false },
            { xtype: 'datefield', name: '创建时间', fieldLabel: '创建时间', columnWidth: .34, allowBlank: false },
            { xtype: 'datefield', name: '生效时间', fieldLabel: '生效时间', columnWidth: .33, allowBlank: false },
            { xtype: 'datefield', name: '到期时间', fieldLabel: '到期时间', columnWidth: .33, allowBlank: false },
            { xtype: 'fieldset', title: '联系方式', collapsible: true, columnWidth: 1, layout: 'column', margin: 10, items: [
                { xtype: 'textfield', name: '手机', fieldLabel: '手机', columnWidth: .34, allowBlank: false },
                { xtype: 'textfield', name: '办公电话一', columnWidth: .33, fieldLabel: '办公电话一', allowBlank: false },
                { xtype: 'textfield', name: '办公电话二', columnWidth: .33, fieldLabel: '办公电话二', allowBlank: false },
                { xtype: 'textfield', name: '电子邮箱', fieldLabel: '电子邮箱', columnWidth: .34, allowBlank: false },
                { xtype: 'textfield', name: 'QQ', columnWidth: .33, fieldLabel: 'QQ', allowBlank: false },
                { xtype: 'textfield', name: 'MSN', columnWidth: .33, fieldLabel: 'MSN', allowBlank: false}]
            },
            { xtype: 'htmleditor', name: '备注', columnWidth: 1, fieldLabel: '备注', allowBlank: false }
        ],
        buttons: [
            { text: '保存', handler: function () { alert('保存') } },
            { text: '取消', handler: function () { alert('取消') } }
        ]
    })

    return this.form;
}

/** 权限数据表单
*/
MB.form.Privilege= function (config) {
    this.name = "权限表单";
    this.form = new Ext.form.FormPanel({
        layout: 'column',
        //autoHeight: true,
        autoScroll: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'none', width: '100%', anchor: '100%', columnWidth: .5, margin: "4px 10px" },
        items: [
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'textfield', name: 'privilegeCode', fieldLabel: '操作编码' },
            { xtype: 'textfield', name: 'privilegeName', fieldLabel: '操作名称' },
            { xtype: 'checkboxfield', name: 'needAuth', boxLabel: '是否需要授权', checked: true, },
            { xtype: 'checkboxfield', name: 'isMenuEntry', boxLabel: '是否菜单入口', checked:false },
            { xtype: 'combo', name: 'resourceID', fieldLabel: '所属功能', 
                forceSelection: true,
                blankText:'请选择所属功能',
                emptyText:'请选择所属功能',
                valueField: 'ID',
                displayField: 'resourceName',
                editable: false,
//                listConfig: new Ext.view.BoundList({}),
                store:  new Ext.data.Store({
                    fields: ['ID', 'resourceName'],
                    proxy: {
                        type: 'ajax',
                        url: '/data/Privilege/resource',
                        //extraParams: {mrId:'',depth:0}, //传参数,在Request.Params["mrId"]
                        reader: {
                            type: 'json',
                            root: 'data'
                        }
                    },
                    autoLoad: true
                }),
                lisenters: {
                    //afterrender: function(){ select(0)}
                }
            },
            { xtype: 'textareafield', name: 'privilegeDescription', columnWidth: 1, fieldLabel: '模块说明'}
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: config.id ? '/system/auth/UpdatePrivilege' : '/system/auth/CreatePrivilege',
                    success: function (form, action) { if (config && config.submitSccess) config.submitSccess(form, action) },
                    failure: function (form, action) { if (config && config.submitFailure) config.submitFailure(form, action) }
                }
                );
            }
            },
            { text: '取消', handler: function () { if (config && config.close) config.close() } }
        ]
    })

    if (config.id) {
        this.form.getForm().load({
            url: '/system/auth/getPrivilege',
            params: { id: config.id }
        }
        );
    }

    return this.form;
}