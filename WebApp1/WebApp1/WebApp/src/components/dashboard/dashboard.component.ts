import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { IEvent } from 'services/event-service/event.model';
import { EventService } from 'services/event-service/event.service';
import { SendEmailsFormComponent } from '../send-emails-form/send-emails-form.component';
import { forkJoin } from 'rxjs';
import { format } from 'date-fns';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit{
  rightHandPanel:boolean = true;
  group!: string[];
  events!: IEvent[];
  upcomingEvents:boolean = true;
  currentPage : number = 1;
  total:number = 0;
  totaLCertificatesSent:number = 0;
  numberOfPages:number = 0;
  constructor(private eventService: EventService, public dialog: MatDialog){
  }

  ngOnInit(): void {
    this.showUpcomingEvents();
    this.upcomingEvents = true;
  }

  showUpcomingEvents() {
    this.eventService.getAllEvents(true, 1).subscribe((response:any) => {
      this.events! = response.data;
      this.currentPage = 1;
      this.upcomingEvents = true;
      this.total = response.total;
      this.numberOfPages = Math.floor(this.total / 10)+1;
    })
  }

  showPastEvents() {
    this.eventService.getAllEvents(false, 1).subscribe((response:any) => {
      this.events! = response.data;
      this.totaLCertificatesSent = response.totalCertificatesSent;
      this.upcomingEvents = false;
      this.currentPage = 1;
      this.total = response.total;
      this.numberOfPages = Math.floor(this.total / 10)+1;
    })
  }

  sendEmails(event:IEvent){
    forkJoin([
      this.eventService.getEventById(event.eventId),
      this.eventService.getCountries()]
    ).subscribe(results =>{
      this.dialog.open(SendEmailsFormComponent,{
        data: {selectedEvent:results[0],countries:results[1]}
      }).afterClosed().subscribe(result => {
        this.eventService.getAllEvents(false,this.currentPage).subscribe((events: any) => {
          this.events = events.data;
        });
    })
   }
    );
  }
  previousPage(){
    if(this.currentPage == 1) return;
    this.currentPage--;
    this.eventService.getAllEvents(this.upcomingEvents,this.currentPage).subscribe((response:any) => {
      this.events! = response.data;
      this.total = response.total;
      this.numberOfPages = Math.floor(this.total / 10)+1;
    })
  }
  nextPage(){
    if(this.currentPage == this.numberOfPages) return;
    this.currentPage++;
    this.eventService.getAllEvents(this.upcomingEvents,this.currentPage).subscribe((response:any) => {
      this.events! = response.data;
      this.total = response.total;
      this.numberOfPages = Math.floor(this.total / 10)+1;
    })
  }

  convertDateTime(date: string) {
    return format( new Date(date), "yyyy-MM-dd")
  }


  getTrainerNames(trainers: any[]) {
    const slicedArray = trainers.slice(0, 3);
    var names = slicedArray.map(i => {
      return i.firstName
    }).join(", ")

    return names
  }

  getUsersCount(usersOnEvent: any[]) {
    return usersOnEvent.filter(x => x.emailSent == true).length;
  }
}

