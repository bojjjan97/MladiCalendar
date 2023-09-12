import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CalendarEvent, CalendarEventTimesChangedEvent, CalendarView } from 'angular-calendar';
import { isSameMonth,startOfMonth,endOfMonth,subDays, format, addDays, parseISO } from 'date-fns';
import { Subject } from 'rxjs';
import { EventService } from 'services/event-service/event.service';

export const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3'
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF'
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA'
  }
};


@Component({
  selector: 'app-event-calendar',
  templateUrl: './event-calendar.component.html',
  styleUrls: ['./event-calendar.component.css']
})
export class EventCalendarComponent implements OnInit{
  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  viewDate: Date = new Date();

  refresh: Subject<any> = new Subject();
  events!: CalendarEvent[];
  activeDayIsOpen: boolean = true;

  constructor(private eventService: EventService) {
    let month = this.viewDate.getMonth();
    let year = this.viewDate.getFullYear();


  }
  ngOnInit(): void {
    this.eventService.eventCreationSubject.subscribe(x=>{
      this.initEventsForViewDate();
    })
    this.eventService.eventDeletionSubject$.subscribe(eventId=>{
      this.events = this.events.filter(x=>x.meta.eventId != eventId);
    });

    let fromDate = new Date(format(startOfMonth(this.viewDate), "yyyy-MM-dd hh:mm:ss"))
    let a = format(subDays(fromDate,7),"yyyy-MM-dd hh:mm:ss");

    let toDate = new Date(format(endOfMonth(this.viewDate), "yyyy-MM-dd hh:mm:ss"))
    let b = format(addDays(toDate,7),"yyyy-MM-dd hh:mm:ss");
    this.eventService.getEvents(a, b ).subscribe(response => {
      this.events = response
    })
  }



  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    this.eventService.eventSubject$.next({ rightPanelOpened: true, events: events.map(x => x.meta), selectedDate: date })

    if (isSameMonth(date, this.viewDate)) {
      this.viewDate = date;
    }
  }

  handleEvent(action: string, event: CalendarEvent): void {
  }

  selectedDate:Date | null = null;
  openCellView(e:any){
    this.selectedDate = e == this.selectedDate ? null : e;
  }
  eventClick(jsEvent: any, e:any, date:Date) {
    jsEvent.stopPropagation()
    let dayEvents = this.events.filter(x=>x.start.getDate() == date.getDate() && x.start.getMonth() == date.getMonth()).map(x=>x.meta)
    this.eventService.openEventSubject$.next({ rightPanelOpened: true, events: e.meta, allevents : dayEvents, selectedDate: date })
  }

  viewAllEvents(events:any){
    this.eventService.eventSubject$.next({ rightPanelOpened: true, events:events.map((x:any)=>x.meta), selectedDate: this.selectedDate })
  }

  deleteEvent(eventToDelete: CalendarEvent) {
    this.events = this.events.filter(event => event !== eventToDelete);
  }

  setView(view: CalendarView) {
    this.view = view;
  }

  closeOpenMonthViewDay() {
    this.activeDayIsOpen = false;
    this.initEventsForViewDate();
  }

  initEventsForViewDate(){
    let fromDate = new Date(format(startOfMonth(this.viewDate), "yyyy-MM-dd hh:mm:ss"))
    let from = format(subDays(fromDate,7),"yyyy-MM-dd hh:mm:ss");

    let toDate = new Date(format(endOfMonth(this.viewDate), "yyyy-MM-dd hh:mm:ss"))
    let to = format(addDays(toDate,7),"yyyy-MM-dd hh:mm:ss");
    this.eventService.getEvents(from,to).subscribe(response => {
      this.events = response
    })
  }


  initCurrentDateEvents() {
    let fromDate = new Date(format(startOfMonth(this.viewDate), "yyyy-MM-dd hh:mm:ss"))
    let a = format(subDays(fromDate,7),"yyyy-MM-dd hh:mm:ss");

    let toDate = new Date(format(endOfMonth(this.viewDate), "yyyy-MM-dd hh:mm:ss"))
    let b = format(addDays(toDate,7),"yyyy-MM-dd hh:mm:ss");
    this.eventService.getEvents(a, b ).subscribe(response => {
      this.events = response
    })
  }

  getCellDate(date: Date) {
    return format(date, 'd');
  }
}

