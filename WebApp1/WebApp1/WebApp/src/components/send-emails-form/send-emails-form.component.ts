import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { format } from 'date-fns';
import { UserOnEvent } from 'services/event-service/event.model';
import { EventService } from 'services/event-service/event.service';
import { AlertData, AlertService } from '../alert-component/alert.service';

@Component({
  selector: 'app-send-emails-form',
  templateUrl: './send-emails-form.component.html',
  styleUrls: ['./send-emails-form.component.css']
})
export class SendEmailsFormComponent {

  eventDate:string = "";
  selectedUserIds!: string[];
  usersOnEvents!: UserOnEvent[];
  usersWithSentEmails!: UserOnEvent[];

  constructor(private eventService: EventService, @Inject(MAT_DIALOG_DATA) public data: any, private dialogRef:MatDialogRef<any>, private alertService: AlertService) {
  }

  ngOnInit() {
    this.usersOnEvents = this.data.selectedEvent.usersOnEvent.filter((x:any) => !x.emailSent)
    this.usersWithSentEmails = this.data.selectedEvent.usersOnEvent.filter((x:any) => x.emailSent)
    this.selectedUserIds! = [];
    this.eventDate = format( new Date(this.data.selectedEvent.eventDateTime), "yyyy-MM-dd")
  }


  addUser(data:any) {
    var beforeLenght = this.selectedUserIds!.length

    this.selectedUserIds!.forEach( (el, index ) => {
      if (el == data.userId ) {
        this.selectedUserIds!.splice(index,1);
      } 
    })

    if (beforeLenght == this.selectedUserIds!.length) {
      this.selectedUserIds!.push(data.userId)
    } 
  }

  sendEmails(){
    this.eventService.sendEmails(this.selectedUserIds,this.data.selectedEvent.eventId).subscribe((x:any)=>{
      this.dialogRef.close();
      this.alertService.showAlert({Message:x.message,AlertType:x.responseType})
    })
  }

  closeDialog() {
    this.dialogRef.close();
  }
}
