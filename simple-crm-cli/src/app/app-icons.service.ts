import { Injectable } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
@Injectable({
  providedIn: 'root'
})
export class AppIconsService {

  constructor(
    private iconRegistry: MatIconRegistry,
    private sanitizer: DomSanitizer
  ) {
      this.iconRegistry.addSvgIcon('checkout', this.sanitizer.bypassSecurityTrustResourceUrl('assets/shopping-cart-svgrepo-com.svg'));
      this.iconRegistry.addSvgIcon('cancel', this.sanitizer.bypassSecurityTrustResourceUrl('assets/shopping-cart-empty-svgrepo-com.svg'));
      this.iconRegistry.addSvgIcon('full', this.sanitizer.bypassSecurityTrustResourceUrl('assets/shopping-cart-full-svgrepo-com.svg'));
    }
}
