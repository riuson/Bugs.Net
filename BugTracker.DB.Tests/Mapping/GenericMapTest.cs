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
    [TestFixture(typeof(AttachmentMap))]
    [TestFixture(typeof(BlobContentMap))]
    [TestFixture(typeof(ChangeMap))]
    [TestFixture(typeof(PriorityMap))]
    [TestFixture(typeof(ProblemTypeMap))]
    [TestFixture(typeof(ProjectMap))]
    [TestFixture(typeof(SolutionMap))]
    [TestFixture(typeof(StatusMap))]
    [TestFixture(typeof(TicketMap))]
    public class GenericMapTest<T> where T : IConformistHoldersProvider, new()
    {
        [Test]
        public void CanGenerateXmlMapping()
        {
            var mapper = new ModelMapper();
            mapper.AddMapping<T>();

            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            var xmlSerializer = new XmlSerializer(mapping.GetType());

            xmlSerializer.Serialize(Console.Out, mapping);
        }
    }
}
