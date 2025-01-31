using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace XXEExamples.Tests
{
    [TestFixture]
    public class XmlReader_Tests
    {
        private XmlReaderSettings GetSecureSettings()
        {
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null,
                MaxCharactersFromEntities = 0,
                ValidationType = ValidationType.None
            };
            settings.XmlResolver = null;
            return settings;
        }

        [Test]
        public void XMLReader_WithDTDProcessingParseAndXmlResolverSet_NotSafe()
        {
            AssertXXE.IsXMLParserSafe((string xml) =>
            {
                var settings = GetSecureSettings();
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                using (var reader = XmlReader.Create(stream, settings))
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.XmlResolver = null;
                    xmlDocument.Load(reader);
                    return xmlDocument.InnerText;
                }
            }, true);
        }

        [Test]
        public void XMLReader_WithDTDProcessingIgnored_Safe()
        {
            var exception = Assert.Throws<XmlException>(() =>
            {
                AssertXXE.IsXMLParserSafe((string xml) =>
                {
                    var settings = GetSecureSettings();
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                    using (var reader = XmlReader.Create(stream, settings))
                    {
                        var xmlDocument = new XmlDocument();
                        xmlDocument.XmlResolver = null;
                        xmlDocument.Load(reader);
                        return xmlDocument.InnerText;
                    }
                }, true);
            });

            Assert.IsTrue(exception.Message.StartsWith("For security reasons DTD is prohibited in this XML document."));
        }

        [Test]
        public void XMLReader_WithDTDProcessingProhibited_Safe()
        {
            var exception = Assert.Throws<XmlException>(() =>
            {
                AssertXXE.IsXMLParserSafe((string xml) =>
                {
                    var settings = GetSecureSettings();
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                    using (var reader = XmlReader.Create(stream, settings))
                    {
                        var xmlDocument = new XmlDocument();
                        xmlDocument.XmlResolver = null;
                        xmlDocument.Load(reader);
                        return xmlDocument.InnerText;
                    }
                }, true);
            });

            Assert.IsTrue(exception.Message.StartsWith("For security reasons DTD is prohibited in this XML document."));
        }
    }
}
