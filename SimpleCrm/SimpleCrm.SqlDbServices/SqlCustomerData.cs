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
        private readonly CustomerStatus status;

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

        public List<Customer> GetAll(int pageIndex, int take, string orderBy)
        {
            var allowedFields = new string[] { "firstname", "lastname", "phonenumber", "optinnewsletter", "type", "emailaddress", "preferredcontactmethod", "ststuscode" };
            
            string[] expressions = orderBy.ToLower().Split(',');
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
                if (!allowedFields.Contains(propertyDirectionArr[0]))
                {
                    throw new System.Exception("Invalid Column Name");
                }
            }
            var query = simpleCrmDbContext.Customers
                .Where(x => x.Status == status);
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                query = query.OrderBy(orderBy);
            }

            return query.Skip(pageIndex * take)
              .Take(take)
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

        public IEnumerable<Customer> GetAll(int v)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
