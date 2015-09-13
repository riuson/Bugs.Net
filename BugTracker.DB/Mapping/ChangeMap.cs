﻿using BugTracker.DB.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Mapping
{
    internal class ChangeMap : ClassMapping<Change>
    {
        public ChangeMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Created);
            ManyToOne(x => x.Author, m =>
            {
                m.Column(typeof(Member).Name + "_Id");
                m.Class(typeof(Member));
                m.Cascade(Cascade.None);
                m.Access(Accessor.Property);
                m.Fetch(FetchKind.Join);
                m.Lazy(LazyRelation.Proxy);
                m.NotNullable(true);
            });
            ManyToOne(x => x.Description, m =>
            {
                m.Column(typeof(BlobContent).Name + "_Id");
                m.Class(typeof(BlobContent));
                m.Cascade(Cascade.All | Cascade.Persist | Cascade.Remove);
                m.Unique(true);
                m.Access(Accessor.Property);
                m.Fetch(FetchKind.Join);
                m.Lazy(LazyRelation.Proxy);
                m.NotNullable(true);
            });
            ManyToOne(x => x.Ticket, m =>
            {
                m.Column(typeof(Ticket).Name + "_Id");
                m.Class(typeof(Ticket));
                m.Unique(false);
                m.Access(Accessor.Property);
                m.Fetch(FetchKind.Join);
                m.Lazy(LazyRelation.Proxy);
                //m.Cascade(Cascade.All);
            });
        }
    }
}
