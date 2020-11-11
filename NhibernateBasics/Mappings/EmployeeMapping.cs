using FluentNHibernate.Mapping;
using NhibernateBasics.Model;

namespace NhibernateBasics.Mappings
{
    /// <summary>
    /// Правила отображения <see cref="Employee"/> на таблицу в БД.
    /// </summary>
    public class EmployeeMapping : ClassMap<Employee>
    {
        public EmployeeMapping()
        {
            this.Table("Employee");
            this.Id(x => x.Id);

            this.Map(x => x.FirstName).Not.Nullable();
            this.Map(x => x.LastName).Not.Nullable();
            this.Map(x => x.Email).Not.Nullable();
        }
    }
}
