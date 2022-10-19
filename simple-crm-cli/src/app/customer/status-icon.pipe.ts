import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusIcon'
})
export class StatusIconPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): string {
    if (value === 'checkout') {
      return 'checkout';
    }
    if (value === 'cancel') {
      return 'cancel';
    }
    if (value === 'ordered') {
      return 'full';
    } else {
      return 'checkout';
    }
  }
}
