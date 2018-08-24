using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ClaimsGrabber_
{
    class SAML
    {
        public String Url { get; set; }
        public String Rpt { get; set; }
        public List<SamlAttribute> Attributes { get; set; }

        public SAML (string url, string rpt)
        {
            // Set obj property
            Url = url;
            Rpt = rpt;

            //Setup browser
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
            request.Accept = "text/html, application/xhtml+xml, */*";

            CookieContainer cookies = new CookieContainer();
            request.CookieContainer = cookies;

            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();

            // Browse to application
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string httpResponse = reader.ReadToEnd();
            reader.Close();
            response.Close();

            // Check for httpresponse error
            if (httpResponse.IndexOf("<span>There was a problem accessing the site.  Try to browse to the site again.</span>") != -1)
            {
                throw (new SAMLError("##ERROR## Invalid httpresponse from RPT."));
            }

            // Extract SAML token
            string samlToken = GetSamlTokenFromHttpResponse(httpResponse);

            // Get attributes list
            List<SamlAttribute> attributesList = new List<SamlAttribute>();
            attributesList = GetAttributesListFromSAML(samlToken);

            // Set obj property
            Attributes = attributesList;
            

        }

        private string GetSamlTokenFromHttpResponse (string httpResponse)
        {
            string samlToken = string.Empty;

            // Format response
            httpResponse = httpResponse.Replace("&lt;", "<").Replace("&quot;", @"""");

            // Check for base64 encoding
            string samlCheck = ("name=" + @"""" + "SAMLResponse" + @"""");
            if (httpResponse.Contains(samlCheck))
            {
                // Get SAMLResponse Value
                httpResponse = Base64GetSAMLResponseValue(httpResponse);
                httpResponse = DecodeBase64(httpResponse);

                // Format SAML to support xml reading
                samlToken = httpResponse.Replace("samlp:", "");

            }
            else
            {
                // Format httpResponse
                int startSaml = httpResponse.IndexOf("<saml:AttributeStatement>");
                int endSaml = httpResponse.IndexOf("</saml:AttributeStatement>");
                samlToken = httpResponse.Substring(startSaml, (endSaml - startSaml + 26)).Replace("saml:", "");
            }

            if (samlToken.IndexOf("<EncryptedAssertion ") != -1)
            {
                //Throw error
                throw (new SAMLError("Error: SAML is encrypted. This type of RPT is not supported."));
                
            }
            else
            {
                return samlToken;
            }

            
        }

        // returns subject and attributes from a WS-Fed token (Ws-Fed endpoint)
        private List<SamlAttribute> GetAttributesListFromSAML(string samlToken)
        {
            // Setup return list
            List<SamlAttribute> SamlAttribs = new List<SamlAttribute>();
            
            // Convert to xml for easy parsing
            XmlDocument samlXml = new XmlDocument();
            samlXml.LoadXml(samlToken);

            // Parse SAML token for subject (WS-Fed)
            try
            {
                string subject = samlXml.SelectSingleNode("/AttributeStatement/Subject/NameIdentifier").InnerText;

                SamlAttribute samlAttribSubject = new SamlAttribute()
                {
                    Name = "Subject",
                    Value = subject,
                    Description = "N/A"
                };
                SamlAttribs.Add(samlAttribSubject);
            }
            catch { }

            // Parse SAML token for subject (SAML)
            try
            {
                //string subject = samlXml.SelectSingleNode("/Response/Assertion/Subject/NameID").InnerText; - Failing for some reason

                XmlNodeList subject = samlXml.GetElementsByTagName("NameID");
                string sSubject = subject[0].InnerText;

                // Create SAML object
                SamlAttribute samlAttribSubject = new SamlAttribute()
                {
                    Name = "Subject",
                    Value = sSubject,
                    Description = "N/A"
                };

                // Add to list to be returned
                SamlAttribs.Add(samlAttribSubject);
            }
            catch { }

            // Parse Attribs and add to list
            try   // WS-Fed endpoint
            {
                XmlNodeList elemList = samlXml.GetElementsByTagName("Attribute");
                for (int i = 0; i < elemList.Count; i++)
                {
                    SamlAttribute samlAttrib = new SamlAttribute()
                    {
                        Name = elemList[i].Attributes.GetNamedItem("AttributeName").Value,
                        Value = elemList[i].InnerText,
                        Description = elemList[i].Attributes.GetNamedItem("AttributeNamespace").Value
                    };

                    // Add to list for gridview bind
                    SamlAttribs.Add(samlAttrib);
                }
            }
            catch   // SAML endpoint
            {
                XmlNodeList elemList = samlXml.GetElementsByTagName("Attribute");
                for (int i = 0; i < elemList.Count; i++)
                {
                    SamlAttribute samlAttrib = new SamlAttribute()
                    {
                        Name = "N/A - SAML endpoint",
                        Value = elemList[i].InnerText,
                        Description = elemList[i].Attributes.GetNamedItem("Name").Value
                    };

                    // Add to list for gridview bind
                    SamlAttribs.Add(samlAttrib);
                }
            }

            return SamlAttribs;

        }
                
        // Decodes base64
        private string DecodeBase64 (string base64)
        {
            byte[] data = Convert.FromBase64String(base64);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        // Gets the value attribute in a httpReponse
        private string Base64GetSAMLResponseValue (string httpResponse)
        {
            string sPattern = @"value=[""""][\S]+";
            string value = Regex.Match(httpResponse, sPattern).Value;
            value = value.Substring(7, (value.Length - 8));
            return value;
        }

        // Write the SAML token details to xml file specified
        public void WriteToXml (string filelocation)
        {

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            // Create RPT element
            XmlNode rptNode = doc.CreateElement("Rpt");

            // Add rpt name attrib
            XmlAttribute rptAttribute = doc.CreateAttribute("name");
            rptAttribute.Value = this.Rpt;
            rptNode.Attributes.Append(rptAttribute);

            // Add rpt url attrib
            XmlAttribute urlAttribute = doc.CreateAttribute("test");
            urlAttribute.Value = this.Url;
            rptNode.Attributes.Append(urlAttribute);

            // Append RPT element to parent
            doc.AppendChild(rptNode);

            // Create sub element for attribs
            foreach (SamlAttribute attrib in this.Attributes)
            {
                XmlNode attribNode = doc.CreateElement("Attribute");
                rptNode.AppendChild(attribNode);

                XmlNode nameNode = doc.CreateElement("Name");
                nameNode.AppendChild(doc.CreateTextNode(attrib.Name));
                attribNode.AppendChild(nameNode);

                XmlNode descriptionNode = doc.CreateElement("Name");
                descriptionNode.AppendChild(doc.CreateTextNode(attrib.Description));
                attribNode.AppendChild(descriptionNode);

                XmlNode valueNode = doc.CreateElement("Value");
                valueNode.AppendChild(doc.CreateTextNode(attrib.Value));
                attribNode.AppendChild(valueNode);
            }

            doc.Save(filelocation);

        }

        public class SamlAttribute
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
        }

        // Custom Errors - throw (new SAMLError("Some Text")) ; catch (SAMLError e)
        public class SAMLError : ApplicationException
        {
            public SAMLError(string message)
                : base(message)
            {
            }
        }

    }
}
