using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SimpleCrm.SqlDbServices
{
    public class SqlCustomerData : ICustomerData
    {
        private readonly SimpleCrmDbContext simpleCrmDbContext;

        public SqlCustomerData(SimpleCrmDbContext simpleCrmDbContext)
        {
            this.simpleCrmDbContext = simpleCrmDbContext;
        }

        public Customer Get(int id)
        {
            return simpleCrmDbContext.Customers.FirstOrDefault(cust => cust.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return simpleCrmDbContext.Customers.ToList();
        }

        public void Add(Customer customer)
        {
            simpleCrmDbContext.Add(customer);
        }

        public void Update(Customer customer)
        {
           
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetByStatus(CustomerStatus status, int pageIndex, int take, string orderBy)
        {
            throw new NotImplementedException();
        }

        public void Delete(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
