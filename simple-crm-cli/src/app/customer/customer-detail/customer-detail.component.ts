import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CustomerService } from '../customer.service';
import { Customer } from '../customer.model';

@Component({
  selector: 'crm-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss']
})
export class CustomerDetailComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private route: ActivatedRoute
  ) { }

    customerId!: number;
    customer!: Customer;

    ngOnInit(): void {

       this.customerId = +this.route.snapshot.params['id'];

       this.customerService
          .get(this.customerId)
          .subscribe(cust => {
             if (cust) {
               this.customer = cust;
             }
          });
        }
      }
