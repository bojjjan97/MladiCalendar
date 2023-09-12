import { TestBed } from '@angular/core/testing';

import { HttpDecoratorService } from './http-decorator.service';

describe('HttpDecoratorService', () => {
  let service: HttpDecoratorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HttpDecoratorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
