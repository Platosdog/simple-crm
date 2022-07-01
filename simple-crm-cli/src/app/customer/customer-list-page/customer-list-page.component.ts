import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { CustomerService } from '../customer.service';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss']
})
export class CustomerListPageComponent implements OnInit {
  customers$!: Observable<Customer[]>;

  displayColumns = ['name', 'phoneNumber', 'emailAddress', 'status'];

  constructor(private customerService: CustomerService) {
    this.customers$ = this.customerService.search('');
  }

  ngOnInit(): void { }

}
