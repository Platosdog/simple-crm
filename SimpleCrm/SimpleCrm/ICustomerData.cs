﻿using System.Collections.Generic;

namespace SimpleCrm
{
    public interface ICustomerData
    {
        Customer Get(int id);
        List<Customer> GetAll(int pageIndex, int take, string orderBy);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int customerId);
        void Commit();
        void Delete();
    }
}