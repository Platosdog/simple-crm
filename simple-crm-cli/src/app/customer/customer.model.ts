export type InteractionMethod = 'phone' | 'email';
export type StatusCode = 'checkout' | 'cancel' | 'full' ;

export interface Customer {
  customerId: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  emailAddress: string;
  preferredContactMethod: InteractionMethod;
  statusCode: StatusCode;
  lastContactDate: string;
}
