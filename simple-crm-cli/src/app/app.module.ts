import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatListModule} from '@angular/material/list';
import { CustomerModule } from './customer/customer.module';
import { MatButtonModule } from '@angular/material/button';
import { AppIconsService } from './app-icons.service';
import { AccountRoutingModule } from './account/account-routing.module';
import { JwtInterceptorInterceptor } from './account/jwt-interceptor.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule,
    CustomerModule,
    AccountRoutingModule,
  ],
  providers: [AppIconsService, {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptorInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(iconService: AppIconsService) {}
 }
