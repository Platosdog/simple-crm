import { Component, Inject, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, combineLatest } from 'rxjs';
import { map, startWith, tap } from 'rxjs/operators';
import { CustomerService } from '../customer.service';
import { Router } from '@angular/router';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss']
})
export class CustomerListPageComponent implements OnInit {
  customers$!: Observable<Customer[]>;

  filteredCustomers$!: Observable<Customer[]>;
  filterInput = new FormControl();

  displayColumns = [ 'icon', 'name', 'phoneNumber', 'emailAddress', 'status', 'edit', 'lastContactDate' ];
row: any;

  constructor(
    private customerService: CustomerService,
    private router: Router,
    public dialog: MatDialog,
    ) {
    this.customers$ = this.customerService.search('');
    this.filteredCustomers$ = combineLatest([this.customers$, this.filterInput.valueChanges.pipe(startWith(""))]).pipe(
      tap(([customers, filter])=> console.log(customers)),
      map(([customers, filter])=>{
       return customers.filter((cust)=>{
         return (`${cust.firstName} ${cust.lastName}`.toLowerCase().includes(filter.toLowerCase()) ||
              cust.emailAddress.toLowerCase().includes(filter.toLowerCase()) ||
              cust.phoneNumber.includes(filter)
            )
        });
      })
    )
  }

  ngOnInit(): void { }
  openDetail(item: Customer): void {
    if(item) {
      this.router.navigate([`./customer/${item.customerId}`])
    }
  }

  addCustomer(): void {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      width: '250px',
      disableClose: true,
      data: null
    });
  }
}

