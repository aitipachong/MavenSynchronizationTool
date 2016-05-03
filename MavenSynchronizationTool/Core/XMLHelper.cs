using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MavenSynchronizationTool.Core
{
    public class XMLHelper
    {
        #region xml和string的互转

        /// <summary>
        /// XML文件转String
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetXml2String(string xmlPath)
        {
            string result = string.Empty;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                result = doc.InnerXml;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// String字符串转XML文件
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static bool SetString2Xml(string xml, string savePath)
        {
            bool result = false;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                doc.Save(savePath);
                result = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region XML文件序列化和反序列化

        /// <summary>
        /// 序列化对象为XML字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SerializeEntitiesToXmlString<T>(T t) where T : class
        {
            XmlSerializer xs = new XmlSerializer(t.GetType());
            //设置编码格式
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                //去掉默认命名控件：xmlns:xsd和xmlns:xsi
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                //序列化
                xs.Serialize(writer, t, ns);
            }

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <summary>
        /// 序列化对象为XML文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="savePath"></param>
        /// <param name="t"></param>
        public static void SerializeEntitiesToXmlFile<T>(string savePath, T t) where T : class
        {
            XmlSerializer xs = new XmlSerializer(t.GetType());
            ////设置编码格式
            //MemoryStream stream = new MemoryStream();
            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Encoding = new UTF8Encoding(false);
            //settings.Indent = true;

            using (StreamWriter writer = new StreamWriter(savePath))
            {
                //去掉默认命名控件:xmlns:xsd和xmlns:xsi
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                //序列化
                xs.Serialize(writer, t, ns);
            }
        }

        /// <summary>
        /// XML文件反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeserializerXmlToString<T>(string fileName) where T : class
        {
            FileStream fs = null;
            if(fileName == null || fileName.Trim() == string.Empty || !File.Exists(fileName))
            {
                FileNotFoundException ex = new FileNotFoundException(string.Format("XML文件不存在，路径：{0}", fileName));
                throw ex;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return (T)serializer.Deserialize(fs);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        #endregion

        #region 通过XPath读取XML

        public static List<string> GetAttributeValueByXPath(string xmlPath, string xPath, string attribute)
        {
            List<string> results = new List<string>();
            if(string.IsNullOrEmpty(xmlPath) || !File.Exists(xmlPath))
            {
                FileNotFoundException ex = new FileNotFoundException(string.Format("XML文件不存在，路径：{0}", xmlPath));
                throw ex;
            }

            try
            {
                string value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);

                XmlNodeList nodeList = xmlDoc.SelectNodes(xPath);
                if(nodeList != null)
                {
                    for(int loopi = 0;loopi < nodeList.Count;loopi++)
                    {
                        value = nodeList[loopi].Attributes.GetNamedItem(attribute).Value;
                        results.Add(value);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return results;
        }

        public static XmlNodeList GetXmlNodes(string xmlPath, string xPath)
        {
            XmlNodeList result = null;
            if (string.IsNullOrEmpty(xmlPath) || !File.Exists(xmlPath))
            {
                FileNotFoundException ex = new FileNotFoundException(string.Format("XML文件不存在，路径：{0}", xmlPath));
                throw ex;
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);

                if (xmlDoc == null) return result;
                result = xmlDoc.SelectNodes(xPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public static string GetXmlNodeInfo(XmlNode node, string type="xml")
        {
            string result = string.Empty;
            if (node == null) return result;

            switch(type.Trim().ToUpper())
            {
                case "TEXT":
                    result = node.InnerText;
                    break;
                default:
                    result = node.InnerXml;
                    break;
            }

            return result;
        }
        #endregion
    }
}