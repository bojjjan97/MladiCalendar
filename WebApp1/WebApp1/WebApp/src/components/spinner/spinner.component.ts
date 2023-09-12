import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SpinnerServiceService } from './spinner-service.service';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.css']
})
export class SpinnerComponent {

  constructor(protected service:SpinnerServiceService){
  }
}
