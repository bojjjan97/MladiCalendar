import { Component, OnInit } from '@angular/core';
import { AlertData, AlertService } from './alert.service';

@Component({
  selector: 'app-alert-component',
  templateUrl: './alert-component.component.html',
  styleUrls: ['./alert-component.component.css']
})
export class AlertComponentComponent implements OnInit {
    data:AlertData | null = null;

    constructor(private service:AlertService){

    }

    ngOnInit(){
      this.service.alertSubject.subscribe(alert=>{
        this.data = alert;
        setTimeout(()=>{
          this.data = null;
        },2000)
      });
    }
}
