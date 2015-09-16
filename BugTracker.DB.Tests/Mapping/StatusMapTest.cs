using BugTracker.DB.Mapping;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BugTracker.DB.Tests.Mapping
{
    [TestFixture]
    public class StatusMapTest
    {
        [Test]
        public void CanGenerateXmlMapping()
        {
            var mapper = new ModelMapper();
            mapper.AddMapping<StatusMap>();

            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            var xmlSerializer = new XmlSerializer(mapping.GetType());

            xmlSerializer.Serialize(Console.Out, mapping);
        }
    }
}
