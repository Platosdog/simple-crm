import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotAuthorizedComponent } from './account/not-authorized/not-authorized.component';

const routes: Routes = [
  {
    path: 'customers',
    loadChildren: () => import('./customer/customer.module').then(mod => mod.CustomerModule)
  },
  {
  path: '',
  redirectTo: 'customers',
  pathMatch: 'full'
  },
  {
    path: 'not-authorized',
    component: NotAuthorizedComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
