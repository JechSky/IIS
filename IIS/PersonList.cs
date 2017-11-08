using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISFormForFun
{
    public class PersonList
    {
        List<Person> list = new List<Person>();
        public PersonList()
        {
            list.Add(new Person() { Name = "afu", Age = 21, Gender = true });
            list.Add(new Person() { Name = "bfu", Age = 22, Gender = false });
            list.Add(new Person() { Name = "cfu", Age = 23, Gender = true });
            list.Add(new Person() { Name = "dfu", Age = 26, Gender = false });
        }
        public List<Person> GetData()
        {
            return list;
        }

    }
}
