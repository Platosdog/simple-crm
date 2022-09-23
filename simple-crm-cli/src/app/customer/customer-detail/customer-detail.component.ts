import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CustomerService } from '../customer.service';
import { Customer } from '../customer.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'crm-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss']
})
export class CustomerDetailComponent implements OnInit {
  customerId!: number;
  customer!: Customer;
  detailForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
  ) {
    this.detailForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: [''],
      emailAddress: ['', [Validators.required, Validators.email]],
      preferredContactMethod: ['email']
    });
  }


  ngOnInit(): void {

    this.customerId = +this.route.snapshot.params['id'];

    this.customerService.get(this.customerId).subscribe(cust => {
      if (cust) {
        this.customer = cust;
        this.detailForm.patchValue(cust);
      }
    });

  }
  public save(): void {
    if (!this.detailForm.valid) return;
    const customer = { ...this.customer, ...this.detailForm.value };
    this.customerService.update(customer).subscribe({
      next:(cust) => {
      this.snackBar.open('Customer Saved', 'OK');
      }
    })

  }

  public cancel(): void{}
}
