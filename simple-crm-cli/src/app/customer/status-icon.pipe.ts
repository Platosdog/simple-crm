import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusIcon'
})
export class StatusIconPipe implements PipeTransform {

  transform(value: string|undefined|null, ...args: unknown[]): string {
    if (!value) {
      return 'checkout';
    }
    if (value === 'checkout') {
      return 'checkout';
    }
    if (value.search(/prospect/i) === 0) {
      return 'cancel';
    }
    if (value.search(/ordered/i) === 0) {
      return 'full';
    } else {
      return 'checkout';
    }
  }
}

