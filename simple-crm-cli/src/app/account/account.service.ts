import { PlatformLocation } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { anonymousUser, MicrosoftOptions, UserSummaryViewModel, GoogleOptions } from './account.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  logout() {
    throw new Error('Method not implemented.');
  }
  private baseUrl: string;
  private cachedUser = new BehaviorSubject<UserSummaryViewModel>(anonymousUser());

  constructor(
    private http: HttpClient,
    private router: Router,
    private platformLocation: PlatformLocation,
    private snackBar: MatSnackBar
    ) {
      this.baseUrl = `${environment.server}${environment.apiUrl}auth`;
      const cu = localStorage.getItem('currentUser');
      if (cu) {
        const curUser = JSON.parse(cu);
        this.cachedUser.next(curUser);
        this.verifyUser(curUser).subscribe({next: (user) => this.setUser(user)});
      }
     }

     get user(): BehaviorSubject<UserSummaryViewModel> {
       return this.cachedUser;
     }

    setUser(user: UserSummaryViewModel): void {
      this.cachedUser.next(user);
      localStorage.setItem('currentUser', JSON.stringify(user));
    }

    public loginMicrosoftOptions(): Observable<MicrosoftOptions> {
      // TODO: Add interface for MicrosoftOptions to account.model.ts
      return this.http.get<MicrosoftOptions>(`${this.baseUrl}/external/microsoft`);
    }

    // TODO: Do this right (using an RxJs pipe from the cachedUser)
    get isAnonymous(): boolean {
      if (this.cachedUser.value.name === 'Anonymous') return true;
      return false;
    }
    public loginComplete(data: UserSummaryViewModel, successMessage: string): void {
      this.cachedUser.next(data);
      localStorage.setItem('currentUser', JSON.stringify(data));
      if (!data.roles || data.roles.length === 0) {
        this.router.navigate(['account', 'not-authorized']);
        this.snackBar.open('No Access', '', { duration: 3000 });
      } else {
        this.snackBar.open(successMessage, '', { duration: 3000 });
        const returnUrl = localStorage.getItem('loginReturnUrl') || './constituent/profile';
        console.log(returnUrl);
        this.router.navigate([returnUrl]);
      }
    }
}
