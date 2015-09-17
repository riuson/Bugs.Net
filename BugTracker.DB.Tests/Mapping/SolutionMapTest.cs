﻿using BugTracker.DB.Mapping;
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
    public class SolutionMapTest
    {
        [Test]
        public void CanGenerateXmlMapping()
        {
            var mapper = new ModelMapper();
            mapper.AddMapping<SolutionMap>();

            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            var xmlSerializer = new XmlSerializer(mapping.GetType());

            xmlSerializer.Serialize(Console.Out, mapping);
        }
    }
}