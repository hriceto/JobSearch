using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Serialization;
using System.IO;
using System.Web;
using System.Web.Configuration;


namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class XsltTemplating
    {
        public enum TemplatePath { Email, }
        public Dictionary<string, string> Paths;

        public XsltTemplating()
        {
            String[] templateTypes = Enum.GetNames(typeof(TemplatePath));

            Paths = new Dictionary<string, string>();
            foreach (string templateType in templateTypes)
            {
                Paths.Add(templateType, WebConfigurationManager.AppSettings["TEMPLATE_PATH_" + templateType.ToUpper()]);
            }
        }

        public string GetTransformedTemplate(TemplatePath path, string templateName, Dictionary<string, string> stringParameters, params object[] objectParameters)
        {
            string result = "";

            try
            {
                //serialize the input parameters into xml
                XmlDocument inputXmlDocument = new XmlDocument();
                XmlNode inputXmlDocumentRootNode = inputXmlDocument.CreateNode(XmlNodeType.Element, "root", "");
                inputXmlDocument.AppendChild(inputXmlDocumentRootNode);

                if (objectParameters != null)
                {
                    foreach (object inputParameter in objectParameters)
                    {
                        XmlSerializer serializer = new XmlSerializer(inputParameter.GetType());
                        using (StringWriter writer = new StringWriter())
                        {
                            serializer.Serialize(writer, inputParameter);
                            XmlDocument inputParameterDocument = new XmlDocument();
                            inputParameterDocument.LoadXml(writer.ToString());

                            XmlNode inputXmlDocumentChildNode = inputXmlDocument.CreateNode(XmlNodeType.Element, inputParameterDocument.DocumentElement.Name, "");
                            inputXmlDocumentChildNode.InnerXml = inputParameterDocument.DocumentElement.InnerXml;
                            inputXmlDocumentRootNode.AppendChild(inputXmlDocumentChildNode);
                        }
                    }
                }

                //prepare string parameters for transformation
                XsltArgumentList xmltTransformArguments = new XsltArgumentList();
                if (stringParameters != null)
                {
                    foreach (KeyValuePair<string, string> stringParameter in stringParameters)
                    {
                        xmltTransformArguments.AddParam(stringParameter.Key, "", stringParameter.Value);
                    }
                }

                //get transform
                XslCompiledTransform xslTransform = GetXsltTransform(path, templateName);

                //run xslt transformation on the xml
                StreamReader reader = null;
                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream();
                    xslTransform.Transform(inputXmlDocument.CreateNavigator(), xmltTransformArguments, memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    reader = new StreamReader(memoryStream);
                    result = reader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    ExceptionManager exceptionManager = new ExceptionManager();
                    exceptionManager.AddException(ex);
                    result = "";
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (memoryStream != null)
                    {
                        memoryStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //add exception to exception log.
                ExceptionManager exceptionManager = new ExceptionManager();
                exceptionManager.AddException(ex);
                result = "";
            }
            return result;
        }

        public XslCompiledTransform GetXsltTransform(TemplatePath path, string templateName)
        {
            string pathToTemplates = Paths[path.ToString()];
            XslCompiledTransform xslTransform = new XslCompiledTransform();
            XmlDocument xsltTemplate = new XmlDocument();
                
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(HttpContext.Current.Server.MapPath(pathToTemplates + templateName.ToLower() + ".xslt"));
                xsltTemplate.Load(sr);
                xslTransform.Load(xsltTemplate.CreateNavigator());
            }
            catch (Exception ex)
            {
                ExceptionManager exceptionManager = new ExceptionManager();
                exceptionManager.AddException(ex);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }

            return xslTransform;
        }
    }
}
