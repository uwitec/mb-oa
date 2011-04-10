using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestProject1
{
    public class metaConfig
    {
        private string _xmlFilePath = string.Empty;

        private metaConfig() { }

        public metaConfig(string xmlFilePath)
        {
            this._xmlFilePath = xmlFilePath;
        }

        public XmlDocument getXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this._xmlFilePath);
            return doc;
        }

        public string getCSharpCode()
        {
            StringBuilder code = new StringBuilder();
            XmlDocument doc = new XmlDocument();
            doc.Load(this._xmlFilePath);
            XmlNodeList simpleList = doc.SelectNodes(@"/metas/metaData/metaItem[@type='simple']");
            XmlNodeList clsList = doc.SelectNodes(@"/metas/metaData/metaItem[@type!='simple']");

            #region 添加名称空间
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.ComponentModel.DataAnnotations;");
            // 下面是上下文需要的名称空间
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Data.Entity;");
            code.AppendLine("using System.Data.Entity.ModelConfiguration;");
            code.AppendLine("\r\n");
            #endregion

            #region 生成实体类
            code.AppendLine("namespace EntityObjectLib");
            code.AppendLine("{");
            foreach (XmlNode clsNode in clsList)
            {
                code.AppendLine(string.Format("\t/// {0}", clsNode.Attributes["name"].Value));
                code.AppendLine(string.Format("\tpublic class {0}", clsNode.Attributes["code"].Value));
                code.AppendLine("\t{");
                code.AppendLine("\t\t[Key]");
                code.AppendLine("\t\tpublic string ID {get;set;}");
                foreach(XmlNode pnode in clsNode.SelectNodes("metaItem"))
                {
                    string type = pnode.Attributes["type"].Value;
                    if (pnode.Attributes["amount"] != null && pnode.Attributes["amount"].Value.Equals("*"))
                    {
                        type = string.Format("ICollection<{0}>", type);
                    }
                    code.AppendLine(string.Format("\t\t/// {0}", pnode.Attributes["name"].Value));
                    code.AppendLine(string.Format(
                        "\t\tpublic {0}{1} {2} {{get;set;}}",
                        doc.SelectSingleNode(@"/metas/metaData/metaItem[@type='simple' and @code='" + pnode.Attributes["type"].Value + "']") == null ? "virtual " : string.Empty,
                        type,
                        pnode.Attributes["code"].Value)
                        );
                }

                #region 元数据完备补足
                XmlNodeList list = doc.SelectNodes("//metaItem[@type='" + clsNode.Attributes["code"].Value + "' and @amount='*']");
                foreach (XmlNode n in list)
                {
                    string typename = n.ParentNode.Attributes["code"].Value;
                    string fcode = string.Format("{0}_{1}", typename, n.Attributes["code"].Value).ToLower();
                    code.AppendLine(string.Format("\t\t/// {0}", n.ParentNode.Attributes["name"].Value));
                    code.AppendLine(string.Format(
                        "\t\tpublic {0}{1} {2} {{get;set;}}",
                        "virtual ",
                        typename,
                        fcode)
                        );

                }
                list = doc.SelectNodes("//metaItem[@type='" + clsNode.Attributes["code"].Value + "' and @amount!='*']");
                foreach (XmlNode n in list)
                {
                    string typename = n.ParentNode.Attributes["code"].Value;
                    string fcode = string.Format("{0}_{1}", typename, n.Attributes["code"].Value).ToLower();
                    code.AppendLine(string.Format("\t\t/// {0}", n.ParentNode.Attributes["name"].Value));
                    code.AppendLine(string.Format(
                        "\t\tpublic {0} ICollection<{1}> {2} {{get;set;}}",
                        "virtual",
                        typename,
                        fcode)
                        );

                }
                #endregion
                code.AppendLine("\t}\r\n");
            }
            code.AppendLine("}");
            #endregion

            #region 生成实体类上下文
            code.AppendLine("namespace EntityObjectContext");
            code.AppendLine("{");
            code.AppendLine("\tpublic class MyDB : DbContext");
            code.AppendLine("\t{");
            // 属性
            foreach (XmlNode clsNode in clsList)
            {
                code.AppendLine(string.Format("\t\tpublic DbSet<{0}> {0}s {{ get; set; }}", clsNode.Attributes["code"].Value));
            }
            code.AppendLine("\t\tprotected override void OnModelCreating(DbModelBuilder modelBuilder)");
            code.AppendLine("\t\t{");
            /*
             * modelBuilder.Entity<Resource>()
                .HasOptional(res => res.module)
                .WithMany(m => m.resources)
                .HasForeignKey(res => res.moduleID);
             */
            foreach (XmlNode clsNode in clsList)
            {
                foreach (XmlNode pnode in clsNode.SelectNodes("metaItem[@amount!='*']"))
                {
                    if (doc.SelectSingleNode(@"/metas/metaData/metaItem[@type!='simple' and @code='" + pnode.Attributes["type"].Value + "']") != null)
                    {
                        code.AppendLine(string.Format("\t\t\tmodelBuilder.Entity<{0}>()", clsNode.Attributes["code"].Value));
                        code.AppendLine(string.Format("\t\t\t\t.HasOptional(m => m.{0})", pnode.Attributes["code"].Value));
                        code.AppendLine(string.Format("\t\t\t\t.WithMany(m => m.{0}_{1})", clsNode.Attributes["code"].Value, pnode.Attributes["code"].Value));
                        code.AppendLine(string.Format("\t\t\t\t.Map(m => {{ m.MapKey(\"{0}\"); }});", pnode.Attributes["code"].Value));
                        code.AppendLine();
                    }
                }

                foreach (XmlNode pnode in clsNode.SelectNodes("metaItem[@amount='*']"))
                {
                    if (doc.SelectSingleNode(@"/metas/metaData/metaItem[@type!='simple' and @code='" + pnode.Attributes["type"].Value + "']") != null)
                    {
                        code.AppendLine(string.Format("\t\t\tmodelBuilder.Entity<{0}>()", pnode.Attributes["type"].Value));
                        code.AppendLine(string.Format("\t\t\t\t.HasOptional(m => m.{0}_{1})", clsNode.Attributes["code"].Value, pnode.Attributes["code"].Value));
                        code.AppendLine(string.Format("\t\t\t\t.WithMany(m => m.{0})", pnode.Attributes["code"].Value));
                        code.AppendLine(string.Format("\t\t\t\t.Map(m => {{ m.MapKey(\"{0}_{1}\"); }});", clsNode.Attributes["code"].Value, pnode.Attributes["code"].Value));
                        code.AppendLine();
                    }
                }
            }
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine("}");
            #endregion

            return code.ToString();
        }
    }
}
