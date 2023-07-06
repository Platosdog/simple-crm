import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { HttpClientModule } from '@angular/common/http';
import { CustomerService } from './customer.service';
import { Observable } from 'rxjs';
import { Customer } from './customer.model';
import { CustomerMockService } from './customer-mock.service';
import {MatIconModule} from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CustomerCreateDialogComponent } from './customer-create-dialog/customer-create-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { StatusIconPipe } from './status-icon.pipe';
import { NotAuthorizedComponent } from '../account/not-authorized/not-authorized.component';
import { EffectsModule } from '@ngrx/effects';
import { CustomerStoreEffects } from './store/customer.store.effects';
import { StoreModule } from '@ngrx/store';
import { customerFeatureKey } from '../store/customer.store.selectors'
import { customerReducer } from '../store/customer.store'

@NgModule({
  declarations: [
    CustomerListPageComponent,
    CustomerCreateDialogComponent,
    CustomerDetailComponent,
    StatusIconPipe,
    NotAuthorizedComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    CustomerRoutingModule,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    FlexLayoutModule,
    MatDialogModule,
    MatInputModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatSnackBarModule,
    EffectsModule.forFeature([CustomerStoreEffects]),
    StoreModule.forFeature(customerFeatureKey, customerReducer)
    ],
  providers: [CustomerService]
})
export class CustomerModule { }
