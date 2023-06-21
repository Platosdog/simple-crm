import { PlatformLocation } from '@angular/common';
import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AccountService } from '../account.service';
import { UserSummaryViewModel } from '../account.model';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'crm-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginType: 'undecided' | 'password' | 'microsoft' | 'google' = 'undecided';
  currentUser: Observable<UserSummaryViewModel>;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    public snackBar: MatSnackBar,
    private router: Router,
    private platformLocation: PlatformLocation,
) {
    this.currentUser = this.accountService.user;
    this.loginForm = this.fb.group({
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
}

  ngOnInit(): void {
  }

  useMicrosoft(): void {
    this.loginType = 'microsoft';
    this.snackBar.open('Signing In with Microsoft...', '', { duration: 2000 });
    const baseUrl =
      'https://login.microsoftonline.com/common/oauth2/v2.0/authorize?';
    this.accountService.loginMicrosoftOptions().subscribe((opts) => {
      const options: { [key: string]: string } = {
        ...opts,
        response_type: 'code',
        redirect_uri:
          window.location.origin +
          this.platformLocation.getBaseHrefFromDOM() +
          'signin-microsoft',
      };
      console.log(options.redirect_uri);
      let params = new HttpParams();
      for (const key of Object.keys(options)) {
        params = params.set(key, options[key]); // encodes values automatically.
      }

      window.location.href = baseUrl + params.toString();
    });
  }

  useGoogle(): void {
    this.loginType = 'google';
    this.snackBar.open('Signing In with Google...', '', { duration: 2000 });
    const baseUrl =
      'https://accounts.google.com/o/oauth2/v2/auth/oauthchooseaccount?';
    this.accountService.loginGoogleOptions().subscribe((opts) => {
      const options: { [key: string]: string } = {
        ...opts,
        response_type: 'code',
        prompt: 'consent',
        access_type: 'offline',
        flowName: 'GeneralOAuthFlow',
        redirect_uri:
          window.location.origin +
          this.platformLocation.getBaseHrefFromDOM() +
          'account/signin-google',
      };
      console.log(options.redirect_uri);
      let params = new HttpParams();
      for (const key of Object.keys(options)) {
        params = params.set(key, options[key]); // encodes values automatically.
      }

      window.location.href = baseUrl + params.toString();
    });
  }
}
