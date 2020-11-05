using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NhibernateBasics.Mappings
{
    /// <summary>
    /// 
    /// </summary>
    public class EmployeeMapping : ClassMap<Model.Employee>
    {
        public EmployeeMapping()
        {
            this.Id(x => x.Id);
            //????
            this.Map(x => x.FirstName).Not.Nullable();
            this.Map(x => x.LastName).Not.Nullable();
            this.Map(x => x.Email).Not.Nullable();

            Table("Employee");

        }
    }
}
