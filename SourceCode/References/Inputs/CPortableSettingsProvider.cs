using System;
using System.Collections;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace Portable
{
    /// <summary>
    /// Provides portable, persistent application settings.
    /// </summary>
    public class PortableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        public static string ApplicationSettingsFile;

        private XDocument GetXmlDoc()
        {
            // to deal with multiple settings providers accessing the same file, reload on every set or get request.
            XDocument xmlDoc = null;
            bool initnew = false;
            if (File.Exists(ApplicationSettingsFile))
            {
                try
                {
                    xmlDoc = XDocument.Load(ApplicationSettingsFile);
                }
                catch { initnew = true; }
            }
            else
                initnew = true;
            if (initnew)
            {
                xmlDoc = new XDocument(new XElement("configuration", new XElement("userSettings")));
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            return xmlDoc;
        }

        public override string ApplicationName { get { return Assembly.GetExecutingAssembly().GetName().Name; } set { } }

        public override string Name => "PortableSettingsProvider";

        /// <summary>
        /// Applies this settings provider to each property of the given settings.
        /// </summary>
        /// <param name="settingsList">An array of settings.</param>
        public static void ApplyProvider(params ApplicationSettingsBase[] settingsList)
        {
            foreach (var settings in settingsList)
            {
                var provider = new PortableSettingsProvider();
                settings.Providers.Add(provider);
                foreach (SettingsProperty prop in settings.Properties)
                    prop.Provider = provider;
                settings.Reload();
            }
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            throw new NotImplementedException();
        }

        public void Reset(SettingsContext context)
        {
            XDocument xmlDoc = GetXmlDoc();
            try
            {
                //determine the location of the settings property
                XElement xmlSettings = xmlDoc.Element("configuration").Element("userSettings");
                XElement xmlScope = xmlSettings.Element((string)context["GroupName"]);
                if (xmlScope != null)
                    xmlScope.RemoveAll();

                // Make sure that special chars such as '\r\n' are preserved by replacing them with char entities.
                using (var writer = XmlWriter.Create(ApplicationSettingsFile,
                    new XmlWriterSettings() { NewLineHandling = NewLineHandling.Entitize, Indent = true }))
                {
                    xmlDoc.Save(writer);
                }
            }
            catch { }
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        { /* don't do anything here*/ }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            XDocument xmlDoc = GetXmlDoc();
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();
            // iterate through settings to be retrieved
            foreach (SettingsProperty setting in collection)
            {
                SettingsPropertyValue value = new SettingsPropertyValue(setting);
                value.IsDirty = false;
                //Set serialized value to xml element from file. This will be deserialized by SettingsPropertyValue when needed.
                var loadedValue = getXmlValue(xmlDoc, XmlConvert.EncodeLocalName((string)context["GroupName"]), setting);
                if (loadedValue != null)
                    value.SerializedValue = loadedValue;
                else value.PropertyValue = null;
                values.Add(value);
            }
            return values;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            XDocument xmlDoc = GetXmlDoc();
            foreach (SettingsPropertyValue value in collection)
            {
                setXmlValue(xmlDoc, XmlConvert.EncodeLocalName((string)context["GroupName"]), value);
            }
            try
            {
                string dir = Path.GetDirectoryName(ApplicationSettingsFile);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

                // Make sure that special chars such as '\r\n' are preserved by replacing them with char entities.
                using (var writer = XmlWriter.Create(ApplicationSettingsFile,
                    new XmlWriterSettings() { NewLineHandling = NewLineHandling.Entitize, Indent = true }))
                {
                    xmlDoc.Save(writer);
                }
            }
            catch
            {
            }
        }

        private object getXmlValue(XDocument xmlDoc, string scope, SettingsProperty prop)
        {
            object result = null;
            if (!IsUserScoped(prop))
            {
                return result;
            }

            if (prop.Name == "setF_culture")
                scope = "Global";


            //determine the location of the settings property
            XElement xmlSettings = xmlDoc.Element("configuration").Element("userSettings");
            // retrieve the value or set to default if available
            if (xmlSettings != null && xmlSettings.Element(scope) != null && xmlSettings.Element(scope).Element(prop.Name) != null)
            {
                using (var reader = xmlSettings.Element(scope).Element(prop.Name).CreateReader())
                {
                    reader.MoveToContent();
                    switch (prop.SerializeAs)
                    {
                        case SettingsSerializeAs.Xml:
                            result = reader.ReadInnerXml();
                            break;
                        case SettingsSerializeAs.Binary:
                            result = reader.ReadInnerXml();
                            result = Convert.FromBase64String(result as string);
                            break;
                        default:
                            result = reader.ReadElementContentAsString();
                            break;
                    }
                }
            }
            else
                result = prop.DefaultValue;
            return result;
        }

        private void setXmlValue(XDocument xmlDoc, string scope, SettingsPropertyValue value)
        {
            if (!IsUserScoped(value.Property))
                return;

            if (value.Name == "setF_culture")
                scope = "Global";

            //determine the location of the settings property
            XElement xmlSettings = xmlDoc.Element("configuration").Element("userSettings");

            // the serialized value to be saved
            XNode serialized;
            if (value.SerializedValue == null) serialized = new XText("");
            else if (value.Property.SerializeAs == SettingsSerializeAs.Xml)
            {
                if ((string)value.SerializedValue == "")
                    serialized = new XText("");
                else
                    serialized = XElement.Parse((string)value.SerializedValue);
            }
            else if (value.Property.SerializeAs == SettingsSerializeAs.Binary)
                serialized = new XText(Convert.ToBase64String((byte[])value.SerializedValue));
            else serialized = new XText((string)value.SerializedValue);

            XElement xmlScope = xmlSettings.Element(scope);
            if (xmlScope != null)
            {
                XElement xmlElem = xmlScope.Element(value.Name);
                if (xmlElem == null) xmlScope.Add(new XElement(value.Name, serialized));
                else xmlElem.ReplaceAll(serialized);
            }
            else
            {
                xmlSettings.Add(new XElement(scope, new XElement(value.Name, serialized)));
            }
        }

        // Iterates through the properties' attributes to determine whether it's user-scoped or application-scoped.
        private bool IsUserScoped(SettingsProperty prop)
        {
            foreach (DictionaryEntry d in prop.Attributes)
            {
                Attribute a = (Attribute)d.Value;
                if (a.GetType() == typeof(UserScopedSettingAttribute))
                    return true;
                
            }
            return false;
        }
    }
}
