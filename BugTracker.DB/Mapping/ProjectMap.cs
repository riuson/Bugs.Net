using BugTracker.DB.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Mapping
{
    internal class ProjectMap : ClassMapping<Project>
    {
        public ProjectMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Name);

            Bag(x => x.Tickets,
                c =>
                {
                    c.Inverse(true);
                    c.Key(
                        k =>
                        {
                            k.Column(typeof(Project).Name + "_Id");
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
