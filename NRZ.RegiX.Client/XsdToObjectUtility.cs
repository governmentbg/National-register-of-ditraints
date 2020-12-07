using NRZ.RegiX.Client.ResponseModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NRZ.RegiX.Client
{
    public static class XsdToObjectUtility
    {
        public static BaseResponse GetResponseObjectFromXsd<T>(RegiXResponse response, string content = null)
        {
            if (String.IsNullOrWhiteSpace(response.RawResponse.MessageContent))
            {
                throw new Exception("Missing RegiX raw response");
            }

            XElement node = null;
            if (String.IsNullOrWhiteSpace(content))
            {
                 node = GetResponseNodeFromRawResponse<T>(response.RawResponse.MessageContent);
            }
            else
            {
                node = GetResponseNodeFromRawResponse<T>(content);
            }
            

            var parsedObject = ParseResponseToObject<T>(node);
            return parsedObject;
        }



        private static XElement GetResponseNodeFromRawResponse<T>(string rawResponse)
        {
            if (rawResponse != null)
            {
                XDocument soap = XDocument.Parse(rawResponse);  // Текстът не трябва да започва с BOM.
                XmlTypeAttribute xmlAttribute = (XmlTypeAttribute)Attribute.GetCustomAttribute(
                                   typeof(T),
                                   typeof(XmlTypeAttribute)
                                 );
                XNamespace ns = xmlAttribute.Namespace;
                string typeName = typeof(T).Name;
                XElement node = soap.Descendants(ns + typeName).FirstOrDefault();
                return node;
            }
            return null;
        }

        private static BaseResponse ParseResponseToObject<T>(XElement node)
        {
            try
            {
                if (node == null)
                {
                    throw new Exception("Node element is null");
                }

                BaseResponse parsedObject = null;
                if (typeof(T) == typeof(AircraftsResponse))
                {
                    parsedObject = new AircraftsResponse();
                    using (StringReader reader = new StringReader(node.ToString()))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(AircraftsResponse));
                        parsedObject = (AircraftsResponse)xmlSerializer.Deserialize(reader);
                        return parsedObject;
                    }
                }
                else if (typeof(T) == typeof(MotorVehicleRegistrationResponse))
                {
                    parsedObject = new MotorVehicleRegistrationResponse();
                    using (StringReader reader = new StringReader(node.ToString()))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(MotorVehicleRegistrationResponse));
                        parsedObject = (MotorVehicleRegistrationResponse)xmlSerializer.Deserialize(reader);
                        return parsedObject;
                    }
                }
                else if (typeof(T) == typeof(GetMotorVehicleRegistrationInfoV3Response))
                {
                    parsedObject = new GetMotorVehicleRegistrationInfoV3Response();
                    using (StringReader reader = new StringReader(node.ToString()))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(GetMotorVehicleRegistrationInfoV3Response));
                        parsedObject = (GetMotorVehicleRegistrationInfoV3Response)xmlSerializer.Deserialize(reader);
                        return parsedObject;
                    }
                }
                else if (typeof(T) == typeof(ValidPersonResponse))
                {
                    parsedObject = new ValidPersonResponse();
                    using (StringReader reader = new StringReader(node.ToString()))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ValidPersonResponse));
                        parsedObject = (ValidPersonResponse)xmlSerializer.Deserialize(reader);
                        return parsedObject;
                    }
                }
                else if (typeof(T) == typeof(ValidUICResponse))
                {
                    parsedObject = new ValidUICResponse();
                    using (StringReader reader = new StringReader(node.ToString()))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ValidUICResponse));
                        parsedObject = (ValidUICResponse)xmlSerializer.Deserialize(reader);
                        return parsedObject;
                    }
                }
                else if (typeof(T) == typeof(RegistrationInfoByOwnerResponse))
                {
                    parsedObject = new RegistrationInfoByOwnerResponse();
                    using (StringReader reader = new StringReader(node.ToString()))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(RegistrationInfoByOwnerResponse));
                        parsedObject = (RegistrationInfoByOwnerResponse)xmlSerializer.Deserialize(reader);
                        return parsedObject;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing response to object: " + ex.Message);
            }


        }



    }
}
