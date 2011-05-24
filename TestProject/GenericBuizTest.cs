using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityObjectContext;
using EntityObjectLib;
using System.Data.Entity;
using EntityObjectLib.WF;

namespace TestProject1
{
    [TestClass]
    public class GenericBuizTest
    {
        [TestMethod]
        public void Append()
        {
            using (MyDB mydb = new MyDB())
            {
                WFTemplate template = new WFTemplate
                {
                    ID = Guid.NewGuid().ToString(),
                    BuizCode = "GenericBuiz",
                    BuizName = "通用业务",
                    Name = "通用业务模板一",
                    Creator = mydb.Users.First(u => u.Code == "chw"),
                    CreateTime = DateTime.Now
                };

                mydb.WFTemplates.Add(template);

                GenericModel model = new GenericModel
                {
                    Name = "test1",
                    Model = @"[
	{code:'ID',name:'ID',type:'string'},
	{code:'amount',name:'申请金额(元)',type:'numeric'},
	{code:'description',name:'申请说明',type:'string'},
	{code:'applyTime',name:'申请时间',type:'date'},
	{code:'detail',name:'明细',type:'array',ctype:[
		{code:'ID',name:'ID',type:'string'},
		{code:'item',name:'项目',type:'string'},
		{code:'item',name:'费用(元)',type:'numeric'},
	]
	}
]",
                    Views = new[]
                    {
                        new GenericView
                        {
                             Name="view1",
                             Content=@"<script type='text/javascript'>
	var data={};
</script>
<input type='hidden' name='ID' id='ID' value=''/>
<table border='1'>
<tr>
<td><span>申请金额(元)</span></td><td><input type='text' name='amount' id='amount' value=''/></td></tr>
<tr>
<td><span>申请说明</span></td><td><input type='text' name='description' id='description' value=''/></td></tr>
<tr>
<td><span>申请时间</span></td><td><input type='text' name='applyTime' id='applyTime' value=''/></td>
</tr>
</table>
<!--以下是明细-->"
                        },
                        new GenericView
                        {
                            Name = "view2",
                            Content=""
                        }
                    },
                    Buizs = new[]
                    {
                        new GenericBuiz
                        {
                             Name="业务实例1",
                             Data="",
                             Creator = mydb.Users.First(u => u.Code == "chw"),
                             WFTemplate = template
                        },
                        new GenericBuiz
                        {
                             Name="业务实例2",
                             Data="",
                             Creator = mydb.Users.First(u => u.Code == "chw"),
                             WFTemplate = template
                        }
                    }
                };

                mydb.GenericModels.Add(model);
                mydb.SaveChanges();
            }
        }

        [TestMethod]
        public void Clear()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.GenericBuizs.Load();
                mydb.GenericBuizs.Local.Clear();

                mydb.GenericViews.Load();
                mydb.GenericViews.Local.Clear();

                mydb.GenericModels.Load();
                mydb.GenericModels.Local.Clear();

                mydb.WFTemplates.Remove(mydb.WFTemplates.First(t => t.BuizCode == "GenericBuiz"));

                mydb.SaveChanges();
            }
        }

        [TestMethod]
        public void test1()
        {
            using (MyDB mydb = new MyDB())
            {
                GenericBuiz buiz = mydb.GenericBuizs.First();
            }
        }
    }
}
