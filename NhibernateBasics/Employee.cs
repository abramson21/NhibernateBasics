namespace NhibernateBasics
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    /// <summary>
    /// Класс сотрудников.
    /// </summary>
    class Employee
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
    }
}
