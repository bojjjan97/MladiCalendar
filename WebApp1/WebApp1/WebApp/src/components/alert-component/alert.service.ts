import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject, Observable } from 'rxjs';


export interface AlertData{
  Message:string,
  AlertType:number,
}

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  public alertSubject = new BehaviorSubject<any>(null);
  constructor() { }

  showAlert(data:AlertData){
    this.alertSubject.next(data);
  }
  hideAlert(){
    this.alertSubject.next(null);
  }
}
