import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Customer } from './customer.model';
import { CustomerService } from './customer.service';

@Injectable()

export class CustomerMockService extends CustomerService {
  customers: Customer[] = [];

  constructor(http: HttpClient) {
    super(http);
    console.warn('Warning: You are using the CustomerMockService, not intended for production use.');
    const localCustomers = localStorage.getItem('customers');
    if (localCustomers) {
      this.customers = JSON.parse(localCustomers);
    } else {
      this.customers = [
        {
          customerId: 1,
          firstName: 'John',
          lastName: 'Smith',
          phoneNumber: '314-555-1234',
          emailAddress: 'john@nexulacademy.com',
          statusCode: 'Prospect',
          preferredContactMethod: 'phone',
          lastContactDate: new Date().toISOString()
        },
        {
          customerId: 2,
          firstName: 'Tory',
          lastName: 'Amos',
          phoneNumber: '314-555-9873',
          emailAddress: 'tory@example.com',
          statusCode: 'Prospect',
          preferredContactMethod: 'email',
          lastContactDate: new Date().toISOString()
        }];
    }

  }

  override search(term: string): Observable<Customer[]> {
    const results = this.customers.filter(c =>
      term == '' ||
      (c.firstName.indexOf(term) > 0 ||
        c.lastName.indexOf(term) > 0 ||
        c.phoneNumber.indexOf(term) > 0 ||
        c.emailAddress.indexOf(term) > 0) ||
      (`${c.firstName} ${c.lastName}`).indexOf(term) > 0);
    return of(results);
  }

  override insert(customer: Customer): Observable<Customer> {
    customer.customerId = Math.max(...this.customers.map(c => c.customerId)) + 1;
    this.customers = [...this.customers, customer];
    localStorage.setItem('customers', JSON.stringify(this.customers));
    return of(customer);
  }

  override update(customer: Customer): Observable<Customer> {
    const foundCustomer = this.customers.find(c => c.customerId === customer.customerId);
    if (foundCustomer) {
      this.customers = this.customers.map(c => c.customerId === customer.customerId ? customer : c);
    } else {
      this.customers = [...this.customers, customer];
    }
    localStorage.setItem('customers', JSON.stringify(this.customers));
    return of(customer);
  }
  override get(customerId: number): Observable<Customer | undefined> {
    const item = this.customers.find(c => c.customerId === customerId);
    return of(item);
  }
}
