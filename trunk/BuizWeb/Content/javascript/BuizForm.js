function sleep(milliSeconds) {
    var startTime = new Date().getTime(); // get the current time
    while (new Date().getTime() < startTime + milliSeconds); // hog cpu
}



var MB = function () { };
MB.form = function () { };


MB.Checked = function () { alert("adsfasdf") };

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
            { xtype: 'textareafield', name: 'moduleDescription', columnWidth: 1, fieldLabel: '模块说明' }
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
                blankText: '请选择所属模块',
                emptyText: '请选择所属模块',
                valueField: 'ID',
                displayField: 'moduleName',
                editable: false,
                store: new Ext.data.Store({
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
            { xtype: 'textareafield', name: 'resourceDescription', columnWidth: 1, fieldLabel: '功能说明' }
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
                blankText: '请选择所属部门',
                emptyText: '请选择所属部门',
                valueField: 'ID',
                displayField: 'Name',
                editable: false,
                store: new Ext.data.Store({
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
MB.form.Privilege = function (config) {
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
            { xtype: 'textfield', name: 'privilegeName', fieldLabel: '操作名称', value: '' },
            { xtype: 'checkboxfield', name: 'needAuth', boxLabel: '是否需要授权', checked: true },
            { xtype: 'checkboxfield', name: 'isMenuEntry', boxLabel: '是否菜单入口', checked: false },
            { xtype: 'combo', name: 'resourceID', fieldLabel: '所属功能',
                forceSelection: true,
                blankText: '请选择所属功能',
                emptyText: '请选择所属功能',
                valueField: 'ID',
                displayField: 'resourceName',
                editable: false,
                //                listConfig: new Ext.view.BoundList({}),
                store: new Ext.data.Store({
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
            { xtype: 'textareafield', name: 'privilegeDescription', columnWidth: 1, fieldLabel: '模块说明' }
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
            { xtype: 'textareafield', name: 'roleDescription', columnWidth: 1, fieldLabel: '角色说明' }
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
            { xtype: 'textareafield', name: 'Parameters', columnWidth: 1, fieldLabel: '角色权限参数' }
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

MB.form.Event = function (config) {
    this.name = "事件管理";
    this.container = config.container;
    this.form = new Ext.form.FormPanel({
        params: { sid: config.id, jid: 'ssss' },
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        height: 400,
        width: '100%',
        autoHeight: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'none', anchor: '100%', margin: "4px 10px" },
        items: [
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'textfield', name: 'Name', width: '100%', fieldLabel: '标题' },
            { xtype: 'htmleditor', name: 'Content', flex: 1, fieldLabel: '内容' },
            {
                xtype: 'fieldcontainer',
                layout: 'hbox',
                fieldDefaults: { labelAlign: 'left', msgTarget: 'none', labelWidth: 60 },
                items: [
                    { xtype: 'combo', flex: 1, name: 'Type', fieldLabel: '事件类型',
                        store: new Ext.data.Store({
                            fields: ['type'],
                            data: [
                                    { type: '计划任务' },
                                    { type: '工作日志' },
                                    { type: '其他' }
                                ]
                        }),
                        queryMode: 'local',
                        displayField: 'type',
                        valueField: 'type',
                        value: '计划任务'
                    },
                    { xtype: 'splitter', width: 40 },
                    { xtype: 'combo', flex: 1, name: 'Parent', fieldLabel: '父事件',
                        forceSelection: true,
                        blankText: '请选择',
                        emptyText: '请选择',
                        valueField: 'ID',
                        displayField: 'Name',
                        editable: false,
                        queryMode: 'local',
                        store: new Ext.data.Store({
                            fields: ['ID', 'Name'],
                            proxy: {
                                type: 'ajax',
                                url: '/data/Carlendar/list'
                            },
                            autoLoad: true,
                            listeners: {
                                load: function (store, records, successful) {
                                    //debugger;
                                    if (!successful) {
                                        alert("Error!");
                                    }
                                    else {
                                    }
                                }
                            }
                        })
                    }
                ]
            },
            {
                xtype: 'fieldcontainer',
                layout: 'hbox',
                fieldDefaults: { labelAlign: 'left', msgTarget: 'none', labelWidth: 60 },
                items: [
                    { xtype: 'datefield', flex: 1, name: 'StartDate', fieldLabel: '开始时间' },
                    { xtype: 'splitter', width: 10 },
                    { xtype: 'timefield', name: 'StartTime', width: 80 },
                    { xtype: 'splitter', width: 40 },
                    { xtype: 'datefield', flex: 1, name: 'FinishDate', fieldLabel: '结束时间' },
                    { xtype: 'splitter', width: 10 },
                    { xtype: 'timefield', name: 'FinishTime', width: 80 }
                ]
            },
            {
                xtype: 'fieldcontainer',
                layout: 'hbox',
                fieldDefaults: { labelAlign: 'left', msgTarget: 'none', labelWidth: 60 },
                items: [
                    { xtype: 'combo', flex: 1, name: 'Master', fieldLabel: '责任人',
                        forceSelection: true,
                        blankText: '请选择',
                        emptyText: '请选择',
                        valueField: 'ID',
                        displayField: 'Name',
                        editable: false,
                        queryMode: 'local',
                        store: new Ext.data.Store({
                            fields: ['ID', 'Name'],
                            proxy: {
                                type: 'ajax',
                                url: '/data/user/list'
                            },
                            autoLoad: true,
                            listeners: {
                                load: function (store, records, successful) {
                                    //debugger;
                                    if (!successful) {
                                        alert("Error!");
                                    }
                                    else {
                                    }
                                }
                            }
                        })
                    },
                    { xtype: 'splitter', width: 40 },
                    { xtype: 'combo', flex: 1, name: 'Proctor', fieldLabel: '督办人',
                        forceSelection: true,
                        blankText: '请选择',
                        emptyText: '请选择',
                        valueField: 'ID',
                        displayField: 'Name',
                        editable: false,
                        queryMode: 'local',
                        store: new Ext.data.Store({
                            fields: ['ID', 'Name'],
                            proxy: {
                                type: 'ajax',
                                url: '/data/user/list'
                            },
                            autoLoad: true,
                            listeners: {
                                load: function (store, records, successful) {
                                    //debugger;
                                    if (!successful) {
                                        alert("Error!");
                                    }
                                    else {
                                    }
                                }
                            }
                        })
                    }
                ]
            }
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: config.id ? '/office/myOffice/UpdateEvent' : '/office/myOffice/saveEvent',
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
            url: '/office/myOffice/getEvent',
            params: { id: config.id }
        }
        );
    }

    return this.form;
}

MB.form.EventShare = function (config) {
    this.name = "事件共享";
    this.container = config.container;
    this.form = new Ext.form.FormPanel({
        params: { sid: config.id, jid: 'ssss' },
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        height: 200,
        width: '100%',
        autoHeight: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'none', anchor: '100%', margin: "4px 10px" },
        items: [
            { xtype: 'hiddenfield', name: 'eventId', value: config.eventId, hidden: true },
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'combo', name: 'Subject', fieldLabel: '共享给',
                forceSelection: true,
                blankText: '请选择',
                emptyText: '请选择',
                valueField: 'ID',
                displayField: 'Name',
                editable: false,
                queryMode: 'local',
                store: new Ext.data.Store({
                    fields: ['ID', 'Name'],
                    proxy: {
                        type: 'ajax',
                        url: '/data/Subject/list'
                    },
                    autoLoad: true,
                    listeners: {
                        load: function (store, records, successful) {
                            //debugger;
                            if (!successful) {
                                alert("Error!");
                            }
                            else {
                            }
                        }
                    }
                })
            },
            { xtype: 'checkboxfield', name: 'NeedRemind', boxLabel: '是否参与提醒', checked: true }
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: '/office/myOffice/saveEventShare',
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
            url: '/office/myOffice/getEventShare',
            params: { id: config.id }
        }
        );
    }

    return this.form;
}

MB.form.EventRemind = function (config) {
    this.name = "事件提醒";
    this.container = config.container;
    this.form = new Ext.form.FormPanel({
        params: { sid: config.id, jid: 'ssss' },
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        height: 400,
        width: '100%',
        autoHeight: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'none', anchor: '100%', margin: "4px 10px" },
        items: [
            { xtype: 'hiddenfield', name: 'eventId', value: config.eventId, hidden: true },
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            {
                xtype: 'fieldcontainer',
                layout: 'hbox',
                fieldDefaults: { labelAlign: 'left', msgTarget: 'none', labelWidth: 80 },
                items: [
                    { xtype: 'datefield', flex: 1, name: 'RemindDate', fieldLabel: '提醒时间' },
                    { xtype: 'splitter', width: 40 },
                    { xtype: 'timefield', width: 100, name: 'RemindTime' },
                    { xtype: 'fieldcontainer', flex: 1 }
                    ]
            },
            { xtype: 'textareafield', name: 'RemindContent', flex: 1, fieldLabel: '内容' },
            {
                xtype: 'fieldcontainer',
                layout: 'hbox',
                fieldLabel: '发送提醒给:',
                items: [
                    { xtype: 'checkboxfield', flex: 1, name: 'ReceiverTypeMaster', boxLabel: '责任人', value: '责任人', checked: true },
                    { xtype: 'checkboxfield', flex: 1, name: 'ReceiverTypeProctor', boxLabel: '督办人', value: '督办人', checked: true },
                    { xtype: 'checkboxfield', flex: 1, name: 'ReceiverTypeShare', boxLabel: '共享人', value: '共享人', checked: true}]
            }
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: '/office/myOffice/saveEventRemind',
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
            url: '/office/myOffice/getRemind',
            params: { id: config.id }
        }
        );
    }

    return this.form;
}

MB.form.EventState = function (config) {
    this.name = "事件状态";
    this.container = config.container;
    this.form = new Ext.form.FormPanel({
        params: { sid: config.id, jid: 'ssss' },
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        height: 400,
        width: '100%',
        autoHeight: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'left', msgTarget: 'none', margin: "4px 10px", labelWidth: 80 },
        items: [
            { xtype: 'hiddenfield', name: 'eventId', value: config.eventId, hidden: true },
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            {
                xtype: 'fieldcontainer',
                layout: 'hbox',
                fieldDefaults: { labelAlign: 'left', msgTarget: 'none', labelWidth: 80 },
                items: [
                    { xtype: 'datefield', flex: 1, name: 'StateDate', fieldLabel: '状态时间' },
                    { xtype: 'splitter', width: 40 },
                    { xtype: 'timefield', width: 100, name: 'StateTime' },
                    { xtype: 'fieldcontainer', flex: 1 }
                    ]
            },
            {
                xtype: 'fieldcontainer',
                layout: 'hbox',
                fieldDefaults: { labelAlign: 'left', msgTarget: 'none', labelWidth: 80 },
                items: [
                    { xtype: 'textfield', flex: 1, name: 'PlanRadio', fieldLabel: '计划完成率' },
                    { xtype: 'splitter', width: 40 },
                    { xtype: 'textfield', flex: 1, name: 'AcutalRadio', fieldLabel: '实际完成率' }
                ]
            },
            { xtype: 'textareafield', labelAlign: 'top', name: 'Description', flex: 1, fieldLabel: '状态描述' }
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: '/office/myOffice/saveEventState',
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
            url: '/office/myOffice/getEventState',
            params: { id: config.id }
        }
        );
    }

    return this.form;
}

MB.form.Memo = function (config) {
    this.name = "便笺管理";
    this.container = config.container;
    this.form = new Ext.form.FormPanel({
        params: { sid: config.id, jid: 'ssss' },
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        height: 400,
        width: '100%',
        autoHeight: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'top', msgTarget: 'none', margin: "4px 10px", labelWidth: 80 },
        items: [
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'textfield', name: 'Name', fieldLabel: '标题' },
            { xtype: 'htmleditor', name: 'Content', flex: 1, fieldLabel: '内容' }
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: '/office/myOffice/saveMemo',
                    success: function (form, action) { if (config && config.submitSccess) config.submitSccess(form, action) },
                    failure: function (form, action) { if (config && config.submitFailure) config.submitFailure(form, action) }
                }
                );
            }
            },
            { text: '取消', handler: function () { if (config && config.close) config.close() } }
        ],
        listeners: {
            beforerender: function (form) {
                //debugger;
                if (config.id) {
                    form.getForm().load({
                        url: '/office/myOffice/getMemo',
                        params: { id: config.id }
                    });
                }
            }
        }
    })

    //debugger;



    return this.form;
}


MB.form.WFNodeHandle = function (config) {
    this.name = "流程处理节点表单";
    this.form = new Ext.form.FormPanel({
        //url: '/system/auth/CreateResource',
        params: { sid: config.id, jid: 'ssss' },
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        height: 400,
        width: '100%',
        autoHeight: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'left', msgTarget: 'none', anchor: '100%', margin: 4, labelWidth: 60 },
        items: [
            { xtype: 'hiddenfield', name: 'templateId', value: config.templateId, hidden: true },
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'hiddenfield', name: 'x', value: config.x, hidden: true },
            { xtype: 'hiddenfield', name: 'y', value: config.y, hidden: true },
            { xtype: 'textfield', name: 'Name', fieldLabel: '节点名称' },
             {
                 xtype: 'fieldcontainer',
                 layout: 'hbox',
                 fieldDefaults: { labelAlign: 'left', msgTarget: 'none', labelWidth: 60, flex: 1 },
                 items: [
                    { xtype: 'textfield', name: 'ViewCode', fieldLabel: '视图表单' },
                    { xtype: 'splitter', width: 30 },
                    { xtype: 'checkboxfield', name: 'IsCountersign', fieldLabel: '是否会签' },
                    ]
             },
             { 
                xtype: 'combo',
                 fieldLabel: '操作人', 
                 name: 'Handlers',
                 store: new Ext.data.Store({
                     fields: ['value', 'text'],
                     proxy: {
                         type: 'ajax',
                         url: '/data/Privilege/subject_IdNames',
                         reader: {
                             type: 'json',
                             root: 'data'
                         }
                         //extraParams: {mrId:'',depth:0}, //传参数,在Request.Params["mrId"]
                     },
                     autoLoad: true,
                     listeners: {
                         load: function (store, records, successful) {
                             if (!successful) {
                                 alert('数据加载失败');
                             }
                             else {
                                 //alert('数据加载成功');
                             }
                         }
                     }
                 }),
                 queryMode: 'local',
                 displayField: 'text',
                 valueField: 'value',
                 multiSelect: true
             },
        //{ xtype: 'textareafield', name: 'Description', flex: 1, fieldLabel: '节点描述' },
        {
        xtype: 'tabpanel',
        plain: true,
        activeTab: 0,
        margin: 4,
        flex: 1,
        items: [
            {
                title: '访问权限',
                layout: 'fit',
                items: [
                {
                    xtype: 'grid',
                    tbar: [
                        { xtype: 'button', iconCls: 'iconNew', tooltip: '新增', tooltipType: 'title', handler: function (source, e) { } },
                    ],
                    store: new Ext.data.Store({
                        fields: ['ID', 'Code', 'Name', 'ACL'],
                        proxy: {
                            type: 'ajax',
                            extraParams: { id: config.id, templateId: config.templateId },
                            url: '/workflow/manage/getNodeACLs',
                            reader: {
                                type: 'json',
                                root: 'data'
                            }
                        },
                        autoLoad: true,
                        listeners: {
                            load: function (store, records, successful) {
                                if (!successful) {
                                    alert('数据加载失败');
                                }
                                else {
                                }
                            }
                        }
                    }),
                    columns: [
                        new Ext.grid.RowNumberer(),
                        {
                            header: '字段编码',
                            dataIndex: 'Code',
                            width: 80
                        }, {
                            header: '字段名称',
                            dataIndex: 'Name',
                            flex: 1
                        }, {
                            header: '访问权限',
                            dataIndex: 'ACL',
                            flex: 1
                        }, {
                            xtype: 'actioncolumn',
                            icon: '/content/images/delete.gif',
                            width: 30,
                            tooltip: '删除',
                            handler: function (grid, rowIndex, colIndex) {
                                //var rec = grid.getStore().getAt(rowIndex);
                                //alert("Terminate " + rec.get('firstname'));
                                alert(rowIndex);
                            }
                        }
                        ]
                }
                ]
            }]
    }
        ],
    buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: '/workflow/manage/addNodeHandle',
                    success: function (form, action) { form.setValues({ ID: action.result.data }); if (config && config.submitSccess) config.submitSccess(form, action) },
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

MB.form.WFNodeXORSplit = function (config) {
    this.name = "流程分支节点表单";
    this.form = new Ext.form.FormPanel({
        params: { sid: config.id, jid: 'ssss' },
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        height: 400,
        width: '100%',
        autoHeight: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'left', msgTarget: 'none', anchor: '100%', margin: 4, labelWidth: 60 },
        items: [
            { xtype: 'hiddenfield', name: 'templateId', value: config.templateId, hidden: true },
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'hiddenfield', name: 'x', value: config.x, hidden: true },
            { xtype: 'hiddenfield', name: 'y', value: config.y, hidden: true },
            { xtype: 'textfield', name: 'Name', fieldLabel: '节点名称' }
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: '/workflow/manage/addNodeXOR',
                    success: function (form, action) { form.setValues({ ID: action.result.data }); if (config && config.submitSccess) config.submitSccess(form, action) },
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

MB.form.WFNodeAction = function (config) {
    this.name = "流程分支节点表单";
    this.form = new Ext.form.FormPanel({
        params: { sid: config.id, jid: 'ssss' },
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        height: 400,
        width: '100%',
        autoHeight: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'left', msgTarget: 'none', anchor: '100%', margin: 4, labelWidth: 60 },
        items: [
            { xtype: 'hiddenfield', name: 'templateId', value: config.templateId, hidden: true },
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'hiddenfield', name: 'from', value: config.from, hidden: true },
            { xtype: 'hiddenfield', name: 'to', value: config.to, hidden: true },
            { xtype: 'textfield', name: 'Code', fieldLabel: '动作代码' },
            { xtype: 'textfield', name: 'Name', fieldLabel: '动作名称' }
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: '/workflow/manage/addNodeAction',
                    success: function (form, action) { form.setValues({ ID: action.result.data }); if (config && config.submitSccess) config.submitSccess(form, action) },
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

MB.form.WFNodeExpression = function (config) {
    this.name = "流程分支节点表单";
    this.form = new Ext.form.FormPanel({
        params: { sid: config.id, jid: 'ssss' },
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        height: 400,
        width: '100%',
        autoHeight: true,
        stateful: false,
        fieldDefaults: { labelAlign: 'left', msgTarget: 'none', anchor: '100%', margin: 4, labelWidth: 60 },
        items: [
            { xtype: 'hiddenfield', name: 'templateId', value: config.templateId, hidden: true },
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'hiddenfield', name: 'from', value: config.from, hidden: true },
            { xtype: 'hiddenfield', name: 'to', value: config.to, hidden: true },
            { xtype: 'textfield', name: 'Expression', fieldLabel: '表达式' },
            { xtype: 'textfield', name: 'Description', fieldLabel: '说明' },
            { xtype: 'textfield', name: 'OrderNO', fieldLabel: '顺序号' ,value:'10' }
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: '/workflow/manage/addNodeExpression',
                    success: function (form, action) { form.setValues({ ID: action.result.data }); if (config && config.submitSccess) config.submitSccess(form, action) },
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

MB.form.WFTemplateEdit = function (config) {
    this.name = "流程模板编辑";
    this.form = new Ext.form.FormPanel({
        params: { sid: config.id, jid: 'ssss' },
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        height: 200,
        autoHeight: true,
        width: '100%',
        stateful: false,
        fieldDefaults: { labelAlign: 'left', msgTarget: 'none', anchor: '100%', margin: 4, labelWidth: 60 },
        items: [
            { xtype: 'hiddenfield', name: 'ID', value: config.id, hidden: true },
            { xtype: 'textfield', name: 'Name', fieldLabel: '模板名称' },
            { xtype: 'textfield', name: 'BuizCode', fieldLabel: '业务编码' },
            { xtype: 'textfield', name: 'BuizName', fieldLabel: '业务名称'}
        ],
        buttons: [
            { text: '保存', handler: function () {
                this.up('form').getForm().submit({
                    url: '/workflow/manage/saveWFTemplate',
                    success: function (form, action) { form.setValues({ ID: action.result.data }); if (config && config.submitSccess) config.submitSccess(form, action) },
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
            url: '/workflow/manage/WFTemplate',
            params: { id: config.id }
        }
        );
    }

    return this.form;
}