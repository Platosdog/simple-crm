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
            //validate the orderby,array of strings column names need to be declared if not throw exception
            string[] expressions = orderBy.Split(',');
            foreach (var expression in expressions)
            { //expresion like lastName DESC
                var propertyDirectionArr = expression.Split(' ');
            }
            var query = simpleCrmDbContext.Customers
                .Where(x => x.Status == status);
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                query = query.OrderBy(orderBy);
            }
            if (expressions.Length != 2)
            {
                throw new System.Exception("invalid search"); 
            }
            if (expressions[1] != "ASC" || expressions[1] != "DESC")
            {
                throw new System.Exception("Invalid sort function");
            }

            if (!allowedFields.Contains(orderBy))
            {
                throw new System.Exception("Invalid sort fields");
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
