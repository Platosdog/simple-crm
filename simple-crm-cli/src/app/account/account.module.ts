import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { SigninMicrosoftComponent } from './signin-microsoft/signin-microsoft.component';
import { SigninGoogleComponent } from './signin-google/signin-google.component';
import { RegistationComponent } from './registation/registation.component';
import { RegistrationComponent } from './registration/registration.component';

@NgModule({
  declarations: [
    LoginComponent,
    SigninMicrosoftComponent,
    SigninGoogleComponent,
    RegistationComponent,
    RegistrationComponent,
  ],
  imports: [
    CommonModule
  ]
})
export class AccountModule { }
