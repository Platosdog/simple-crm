import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { CustomerService } from '../customer.service';
import { switchMap, map } from 'rxjs';

// ngrx effects that trigger side effects for specific actions.
@Injectable()
export class CustomerStoreEffects {
  constructor(
    private actions$: Actions, // <-- this event stream is where to listen for dispatched actions
    private custSvc: CustomerService // <-- this is your service to be called for some actions
  ) { }

  searchCustomers$ = createEffect(() => this.actions$.pipe(
    ofType(searchCustomersAction),
    switchMap(({criteria}) =>  // <-- NEW, use rxjs, accept action payload
      this.custSvc.search(criteria.term).pipe(  // <-- NEW, make service call
        map( // <-- NEW: create action payload with API response data
          data => searchCustomersCompleteAction({result: data})
        )
      )
    )
  ));
}
