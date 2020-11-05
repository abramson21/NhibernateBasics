using System;
using System.Collections.Generic;
using System.Text;

namespace NhibernateBasics.Model
{
    public class Employee
    {
        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
    }
}
