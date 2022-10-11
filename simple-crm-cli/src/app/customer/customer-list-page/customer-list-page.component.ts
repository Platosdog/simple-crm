import { Component, Inject, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { CustomerService } from '../customer.service';
import { Router } from '@angular/router';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss']
})
export class CustomerListPageComponent implements OnInit {
  customers$!: Observable<Customer[]>;

  displayColumns = ['name', 'phoneNumber', 'emailAddress', 'status', 'edit', 'lastContactDate'];

  constructor(
    private customerService: CustomerService,
    private router: Router,
    public dialog: MatDialog,
    ) {
    this.customers$ = this.customerService.search('');
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
