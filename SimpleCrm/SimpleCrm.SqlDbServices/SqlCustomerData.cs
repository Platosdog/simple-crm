using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Dynamic.Core;

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
            simpleCrmDbContext.SaveChanges();
        }

        public void Commit()
        {
            simpleCrmDbContext.SaveChanges();
        }

        public List<Customer> GetAll(CustomerListParameters resourceParameters)
        {
            var allowedFields = new string[] { "firstname", "lastname", "phonenumber", "optinnewsletter", "type", "emailaddress", "preferredcontactmethod", "ststuscode" };

            if (string.IsNullOrWhiteSpace(resourceParameters.OrderBy))
            {
                resourceParameters.OrderBy = "lastname asc";
            }
            string[] expressions = resourceParameters.OrderBy.ToLower().Split(',');
            foreach (var expression in expressions)
            { //expresion like "lastName DESC"
                var propertyDirectionArr = expression.Split(' ');
                if (propertyDirectionArr.Length > 2)
                {
                    throw new System.Exception("invalid search");
                }
                if (propertyDirectionArr.Length > 1 && propertyDirectionArr[1] != "asc" && propertyDirectionArr[1] != "desc")
                {
                    throw new System.Exception("invalid sort direction");
                }
     
            }
                
            if (string.IsNullOrWhiteSpace(resourceParameters.OrderBy))
            {
                resourceParameters.OrderBy = "LastName asc";
            }

            return simpleCrmDbContext.Customers
              .OrderBy(resourceParameters.OrderBy)
              .Skip((resourceParameters.Page -1) * resourceParameters.Take)
              .Take(resourceParameters.Take)
              .ToList();
        }

        public void Delete(int id)
        {
            var cust = new Customer { Id = id };
            simpleCrmDbContext.Attach(cust);
            simpleCrmDbContext.Remove(cust);
        }
        public void Delete(Customer item)
        {
            simpleCrmDbContext.Remove(item);
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
