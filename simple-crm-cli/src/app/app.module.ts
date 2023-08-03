import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatListModule} from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { AppIconsService } from './app-icons.service';
import { AccountRoutingModule } from './account/account-routing.module';
import { JwtInterceptorInterceptor } from './account/jwt-interceptor.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { StoreModule } from '@ngrx/store';
import { layoutFeatureKey, layoutReducer } from './customer/store/layout.store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EffectsModule } from '@ngrx/effects';

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
    AccountRoutingModule,
    StoreModule.forRoot({}), // for no global state, use an empty object,  {}.
    StoreModule.forFeature(layoutFeatureKey, layoutReducer),
    StoreDevtoolsModule.instrument({
      name: 'Nexul Academy - Simple CRM'}),
    EffectsModule.forRoot([])
  ],
  providers: [AppIconsService, {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptorInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(iconService: AppIconsService) {}
 }
