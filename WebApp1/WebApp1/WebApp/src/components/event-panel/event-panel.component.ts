import { Component, OnInit } from '@angular/core';
import { format } from 'date-fns';
import { AuthService } from 'services/auth-service/auth.service';
import { IEvent } from 'services/event-service/event.model';
import { EventService } from 'services/event-service/event.service';
import { AlertService } from '../alert-component/alert.service';
import { TokenService } from 'services/auth-service/token-services';

@Component({
  selector: 'app-event-panel',
  templateUrl: './event-panel.component.html',
  styleUrls: ['./event-panel.component.css']
})
export class EventPanelComponent implements OnInit {
  rightHandPanel:boolean = false;
  createEvent:boolean = false;
  trainersHidden:boolean = true;
  pariticipantsHIdden:boolean = true;
  events:IEvent[] = []
  selectedDate:string = "";
  selectedEvent:any | null  = null;
  selectedDateForForm :string = "";

  userLoggedIn = false;
  adminLoggedIn = false;
  constructor(private service:EventService,private auth:TokenService,private alert:AlertService){
    this.userLoggedIn = auth.userRole === "User";
    this.adminLoggedIn = auth.userRole === "Admin";
  }

  selectedEventVisible = false;
  eventsPanelVisible = false;

  ngOnInit(): void {
    this.service.eventSubject$.subscribe(x=>{
      this.selectedEventVisible = false;
      this.eventsPanelVisible = true;
      this.createEvent = false;
      this.update = false;
      this.rightHandPanel = x.rightPanelOpened;
      this.events = x.events;
      this.selectedDate = format(x.selectedDate ?? new Date(), "dd LLL");
      this.selectedDateForForm = format(x.selectedDate ?? new Date(), "yyyy-MM-dd")

    })

    this.service.eventCreationSubject.subscribe(x=>{
      this.closePanel();
    })

    this.service.openEventSubject$.subscribe(ev=>{
      this.selectedDate = format(ev.selectedDate ?? new Date(), "dd LLL");
      this.selectedDateForForm = format(ev.selectedDate ?? new Date(), "yyyy-MM-dd")
      this.rightHandPanel = true;
      this.selectedEventVisible = true;
      this.eventsPanelVisible = false;
      this.events = ev.allevents;
      this.selectedEvent = ev.events;
    })
  }

  getClass(user:any){
    switch(user.country.name){
      case "Montenegro":
        return 'fi fi-me';
        case "Serbia":
          return 'fi fi-rs';
        case "Bosnia and Herzegovina":
          return 'fi fi-ba';
        case "North Macedonia":
          return 'fi fi-mk';
        default:
          return "";
    }
  }

  openEvent(eventId: Number){
    this.rightHandPanel = true;
    this.createEvent = false;
    this.selectedEvent = this.events.find(x => x.eventId == eventId) || null
    this.update = false;
    this.eventsPanelVisible = false;
    this.selectedEventVisible = true;
  }

  goBack(){
    this.trainersHidden = true;
    this.pariticipantsHIdden = true;
    this.createEvent = false;
    this.selectedEvent = null;
    this.eventsPanelVisible = true;
    this.selectedEventVisible = false;
    if(this.update)
      this.update = false;
  }

  closePanel(){
    this.createEvent = false;
    this.rightHandPanel = false;
    this.update = false;
  }

  openCreateEvent(){
    this.createEvent = true;
    this.eventsPanelVisible = false;
  }
  update = false;
  openUpdateEvent(){
    this.createEvent = true;
    this.eventsPanelVisible = false;
    this.update = true;
  }

  showParticipants() {
    this.pariticipantsHIdden = !this.pariticipantsHIdden;
  }

  showTrainers(){
    this.trainersHidden = !this.trainersHidden;
  }

  register(eventId:number | undefined){
    if(!eventId) return;
    this.service.registerUserOnEvent(eventId).subscribe(x=>{

      this.alert.showAlert({Message:x.message,AlertType:x?.responseType})

      if(x.responseType == 0){
        let event = this.events.find(x=>x.eventId == eventId);
        event!.currentUserAlreadyOnEvent = true;
        event!.numberOfRegisteredParticipants++;
        if(event?.numberOfRegisteredParticipants == event!.numberOfParticipants)
          event!.freePlaces = false;
      }


    });

  }

  deleteEvent(ev:any,eventId:number){
    ev.stopPropagation();
    this.service.deleteEvent(eventId).subscribe(x=>{
      this.events = this.events.filter(x=>x.eventId != eventId);
      this.service.eventDeletionSubject$.next(eventId);
    });
  }

  unregister(eventId:number | undefined){
    if(!eventId) return;
    this.service.unregisterUserFromEvent(eventId).subscribe(x=>{
      this.alert.showAlert({Message:x.message,AlertType:x?.responseType})
      let event = this.events.find(x=>x.eventId == eventId);
      event!.currentUserAlreadyOnEvent = false;
      event!.freePlaces = true;
      event!.numberOfRegisteredParticipants--;
    });
  }

  get_date(date: any ) {
    return format( new Date(date), "yyyy-MM-dd")
  }
}
