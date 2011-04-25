function sleep(milliSeconds) {
    var startTime = new Date().getTime(); // get the current time
    while (new Date().getTime() < startTime + milliSeconds); // hog cpu
}



var MB = function () { };
MB.form = function () { };


MB.Checked = function(){alert("adsfasdf")};

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

/** 资源数据表单
config:
    submitSccess: fn // 提交成功
    submitFailure: fn //提交失败
    close: fn //关闭
*/
MB.form.Resource = function (config) {
    this.name = "资源表单";
    this.form = new Ext.form.FormPanel({
        //url: '/system/auth/CreateResource',
        params: { sid: config.id, jid: 'ssss' },
        layout: 'column',
        //autoHeight: true,
        autoScroll: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'none', width: '100%', anchor: '100%', columnWidth: .5, margin: "4px 10px" },
        items: [
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'textfield', name: 'resourceCode', fieldLabel: '功能编码' },
            { xtype: 'textfield', name: 'resourceName', fieldLabel: '功能名称' },
            { xtype: 'combo', name: 'moduleID', fieldLabel: '所属模块', 
                forceSelection: true,
                blankText:'请选择所属模块',
                emptyText:'请选择所属模块',
                valueField: 'ID',
                displayField: 'moduleName',
                editable: false,
                store:  new Ext.data.Store({
                    fields: ['ID', 'moduleName'],
                    proxy: {
                        type: 'ajax',
                        url: '/data/Privilege/module',
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
            { xtype: 'textareafield', name: 'resourceDescription', columnWidth: 1, fieldLabel: '功能说明'}
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: config.id ? '/system/auth/UpdateResource' : '/system/auth/CreateResource',
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
            url: '/system/auth/getResource',
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
        fieldDefaults: { labelAlign: 'top', width: '100%', anchor: '100%', columnWidth: .5, margin: "4px 10px" },
        items: [
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'textfield', name: 'Code', fieldLabel: '用户编码' },
            { xtype: 'textfield', name: 'Name', fieldLabel: '用户名称' },
            { xtype: 'textfield', name: 'Password', fieldLabel: '初始口令' },
            { xtype: 'textfield', name: 'Password1', fieldLabel: '确认初始口令' },
            { xtype: 'combo', name: 'OrgID', fieldLabel: '所在部门', 
                forceSelection: true,
                blankText:'请选择所属部门',
                emptyText:'请选择所属部门',
                valueField: 'ID',
                displayField: 'Name',
                editable: false,
                store:  new Ext.data.Store({
                    fields: ['ID', 'Name'],
                    proxy: {
                        type: 'ajax',
                        url: '/data/Privilege/Organization',
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
            { xtype: 'datefield', name: 'ExpireDate', fieldLabel: '到期时间' },
            { xtype: 'fieldset', title: '联系方式', collapsible: true, columnWidth: 1, layout: 'column', margin: 10, items: [
                { xtype: 'textfield', name: 'Mobile', fieldLabel: '手机', columnWidth: .34 },
                { xtype: 'textfield', name: 'OfficePhone', columnWidth: .33, fieldLabel: '办公电话' },
                { xtype: 'textfield', name: 'HomePhone', columnWidth: .33, fieldLabel: '住宅电话' },
                { xtype: 'textfield', name: 'Email', fieldLabel: '电子邮箱', columnWidth: .34 },
                { xtype: 'textfield', name: 'QQ', columnWidth: .33, fieldLabel: 'QQ' },
                { xtype: 'textfield', name: 'MSN', columnWidth: .33, fieldLabel: 'MSN'}]
            },
            { xtype: 'textareafield', name: 'Description', columnWidth: 1, fieldLabel: '备注' }
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: config.id ? '/system/auth/UpdateUser' : '/system/auth/CreateUser',
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
            url: '/system/auth/getUser',
            params: { id: config.id }
        }
        );
    }

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
        //stateful: false,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'none', width: '100%', anchor: '100%', columnWidth: .5, margin: "4px 10px" },
        items: [
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'textfield', name: 'privilegeCode', fieldLabel: '操作编码', value: '' },
            { xtype: 'textfield', name: 'privilegeName', fieldLabel: '操作名称', value: ''  },
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
                    success: function (form, action) { if (config && config.submitSccess) config.submitSccess() },
                    failure: function (form, action) { if (config && config.submitFailure) config.submitFailure() }
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

MB.form.Role = function (config) {
    this.name = "角色表单";
    this.container = config.container;
    this.form = new Ext.form.FormPanel({
        params: { sid: config.id, jid: 'ssss' },
        layout: 'column',
        //autoHeight: true,
        autoScroll: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'none', width: '100%', anchor: '100%', columnWidth: .5, margin: "4px 10px" },
        items: [
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'textfield', name: 'roleCode', fieldLabel: '角色编码' },
            { xtype: 'textfield', name: 'roleName', fieldLabel: '角色名称' },
            { xtype: 'textareafield', name: 'roleDescription', columnWidth: 1, fieldLabel: '角色说明'}
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: config.id ? '/system/auth/UpdateRole' : '/system/auth/CreateRole',
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
            url: '/system/auth/getRole',
            params: { id: config.id }
        }
        );
    }

    return this.form;
}

MB.form.RolePrivilegeParam = function (config) {
    this.name = "角色权限参数表单";
    this.form = new Ext.form.FormPanel({
        layout: 'column',
        autoScroll: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'none', width: '100%', anchor: '100%', columnWidth: .5, margin: "4px 10px" },
        items: [
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'textareafield', name: 'Parameters', columnWidth: 1, fieldLabel: '角色权限参数'}
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: 'updateRolePrivilegeParam',
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
            url: '/system/auth/getRolePrivilegeParam',
            params: { id: config.id }
        }
        );
    }

    return this.form;
}

