using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SimpleCrm
{
    public class InMemoryCustomerData : ICustomerData
    {
        static IList<Customer> customers;
        
        static InMemoryCustomerData()
        {
            customers = new List<Customer>
                  {
                      new Customer { Id =1, FirstName ="Bob", LastName = "Jones", PhoneNumber = "555-555-2345" },
                      new Customer { Id =2, FirstName ="Jane", LastName = "Smith", PhoneNumber = "555-555-5256" },
                      new Customer { Id =3, FirstName ="Mike", LastName = "Doe", PhoneNumber = "555-555-8547" },
                      new Customer { Id =4, FirstName ="Karen", LastName = "Jamieson", PhoneNumber = "555-555-9134" },
                      new Customer { Id =5, FirstName ="James", LastName = "Dean", PhoneNumber = "555-555-7245" },
                      new Customer { Id =6, FirstName ="Michelle", LastName = "Leary", PhoneNumber = "555-555-3457" }
                  };
        }

        public Customer Get(int id)
        {
            return customers.FirstOrDefault(cust => cust.Id == id);
            
        }

        public IEnumerable<Customer> GetAll()
        {
            return customers;
        }

        public void Save (Customer customer)
        {
            customer.Id = customers.Max(x => x.Id) + 1;
            customers.Add(customer);
        }
    }
}