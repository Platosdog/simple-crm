import { EntityState, EntityAdapter, createEntityAdapter } from "@ngrx/entity";
import { Customer } from "../customer.model";
import { customerSearchCriteria } from "src/app/store/customer.store.model";

export interface CustomerState extends EntityState<Customer> {
  searchStatus: string,
  criteria: customerSearchCriteria
}

    export const customerStateAdapter: EntityAdapter<Customer> = createEntityAdapter<Customer>({
      selectId: (item: Customer) => item.id // <-- defines the key property
    });

    export const initialCustomerState: CustomerState = customerStateAdapter.getInitialState({
      searchStatus: '',
      criteria: {term: ''}
    });
