import { TestBed, getTestBed } from '@angular/core/testing';
import { CustomerMockService } from './customer-mock.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('CustomerMockService', () => {
  let injector: TestBed;
  let service: CustomerMockService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [ CustomerMockService ]
    });
    injector = getTestBed();
    service = TestBed.inject(CustomerMockService);
    httpMock = injector.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
