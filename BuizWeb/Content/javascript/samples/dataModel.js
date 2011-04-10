Ext.regModel('Privileges', {
    fields: [
            { name: 'ID', type: 'string' },
            { name: 'privilegeName', type: 'string' },
            { name: 'resourceName', type: 'string' },
            { name: 'moduleName', type: 'string' },
            { name: 'privilegeCode', type: 'string' },
            { name: 'orderNO', type: 'int' }
    ]
});

Ext.regModel('sample', {
    fields: [
            { name: 'company' },
            { name: 'price', type: 'float' },
            { name: 'change', type: 'float' },
            { name: 'pctChange', type: 'float' },
            { name: 'lastChange', type: 'date', dateFormat: 'n/j h:ia' }
    ]
});