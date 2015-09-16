using BugTracker.DB.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Mapping
{
    internal class TicketMap : ClassMapping<Ticket>
    {
        public TicketMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Title);
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

            ManyToOne(x => x.Status, m =>
            {
                m.Column(typeof(Status).Name + "_Id");
                m.Class(typeof(Status));
                m.Cascade(Cascade.None);
                m.Access(Accessor.Property);
                m.Fetch(FetchKind.Join);
                m.Lazy(LazyRelation.Proxy);
                m.NotNullable(true);
            });

            ManyToOne(x => x.Type, m =>
            {
                m.Column(typeof(ProblemType).Name + "_Id");
                m.Class(typeof(ProblemType));
                m.Cascade(Cascade.None);
                m.Access(Accessor.Property);
                m.Fetch(FetchKind.Join);
                m.Lazy(LazyRelation.Proxy);
                m.NotNullable(true);
            });

            ManyToOne(x => x.Priority, m =>
            {
                m.Column(typeof(Priority).Name + "_Id");
                m.Class(typeof(Priority));
                m.Cascade(Cascade.None);
                m.Access(Accessor.Property);
                m.Fetch(FetchKind.Join);
                m.Lazy(LazyRelation.Proxy);
                m.NotNullable(true);
            });

            ManyToOne(x => x.Solution, m =>
            {
                m.Column(typeof(Solution).Name + "_Id");
                m.Class(typeof(Solution));
                m.Cascade(Cascade.None);
                m.Access(Accessor.Property);
                m.Fetch(FetchKind.Join);
                m.Lazy(LazyRelation.Proxy);
                m.NotNullable(true);
            });

            ManyToOne(x => x.Project, m =>
            {
                m.Column(typeof(Project).Name + "_Id");
                m.Class(typeof(Project));
                m.Unique(false);
                m.Access(Accessor.Property);
                m.Fetch(FetchKind.Join);
                m.Lazy(LazyRelation.Proxy);
            });

            Bag(x => x.Changes,
                c =>
                {
                    c.Inverse(true);
                    c.Key(
                        k =>
                        {
                            k.Column(typeof(Ticket).Name + "_Id");
                            k.Unique(true);
                            k.Update(true);
                        });
                    c.Cascade(Cascade.All);
                },
                r =>
                {
                    r.OneToMany();
                });

            Bag(x => x.Attachments,
                c =>
                {
                    c.Inverse(true);
                    c.Key(
                        k =>
                        {
                            k.Column(typeof(Ticket).Name + "_Id");
                            k.Unique(true);
                            k.Update(true);
                        });
                    c.Cascade(Cascade.All);
                },
                r =>
                {
                    r.OneToMany();
                });
        }
    }
}
