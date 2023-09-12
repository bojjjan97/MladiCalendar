import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SpinnerServiceService {

  loading = new BehaviorSubject(false);

  constructor() { }

  showSpinner(){
    this.loading.next(true);
  }

  hideSpinner(){
    this.loading.next(false);
  }
}
