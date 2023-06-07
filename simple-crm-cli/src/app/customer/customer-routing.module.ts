import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { AuthenticatedGuard } from '../account/authenticated.guard';
import { NotAuthorizedComponent } from './not-authorized/not-authorized.component';


const routes: Routes = [
  {
    path: 'customers',
    pathMatch: 'full',
    component: CustomerListPageComponent,
    canActivate: [AuthenticatedGuard]
  },
  {
    path: 'customer/:id',
    pathMatch: 'full',
    component: CustomerDetailComponent,
    canActivate: [AuthenticatedGuard]
  }
  {
    path: 'not-authorized',
    canActivate: [AuthenticatedGuard],
    pathMatch: 'full',
    component: NotAuthorizedComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerRoutingModule { }
