import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject, map } from 'rxjs';
import { AuthService } from '../auth-service/auth.service';
import { CreateEvent, CreateTrainer, EventPanelSubjectModel, IEvent, UpdateEventDto, UpdateTrainer } from './event.model';
import { HttpDecoratorService } from '../http-decorator/http-decorator.service';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  eventSubject$ = new BehaviorSubject<EventPanelSubjectModel>({rightPanelOpened:false, events:[], selectedDate:null});
  openEventSubject$ = new Subject<any>();
  eventCreationSubject = new Subject();
  eventDeletionSubject$ = new Subject();
  constructor(private http:HttpDecoratorService,private auth:AuthService) {

  }

  colors: any = {
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


  getEventById(eventId:number){
    return this.http.get<any>(`/api/Event/GetEventById?eventId=${eventId}`);
  }

  getEvent(upcoming: boolean){
    return this.http.get<any[]>(`/api/Event/api/allevents?upcoming=${upcoming}`);
  }

  getAllEvents(upcoming: boolean,currentPage:number){
    return this.http.get<any[]>(`/api/Event/api/allevents?upcoming=${upcoming}&page=${currentPage}`);
  }

  getEvents(from:string,to:string){
    return this.http.get<any[]>(`/api/Event?From=${from}&To=${to}`)
    .pipe(
      map(x => this.transformEvent(x))
    )
  }

  private transformEvent(events:any[]){
    return events.map(x=> {
      let transformedEvent = {
        title: x.name,
        color: this.colors.yellow,
        start: new Date(x.eventDateTime),
        meta:x
      }
      return transformedEvent;
    })
  }

  deleteEvent(eventId:number){
    return this.http.delete(`/api/Event?eventId=${eventId}`);
  }
  updateEvent(data: UpdateEventDto,eventId:number){
    return this.http.put(`/api/Event?eventId=${eventId}`,data);
  }

  createEvent(data: CreateEvent){
    return this.http.post('/api/Event/CreateEvent',data);
  }

  createTrainer(data: CreateTrainer){
    return this.http.post('/api/Trainer',data);
  }

  updateTrainer(data: any){
    let id = data.id;
    delete data['id'];
    return this.http.put(`/api/Trainer?trainerId=${id}`,data);
  }

  getCountries(){
    return this.http.get<any[]>('/api/Country');
  }

  getTrainers(){
    return this.http.get<any[]>('/api/Trainer');
  }

  getTrainerById(trainerId:number){
    return this.http.get<any>(`/api/Trainer/GetTrainerById?trainerId=${trainerId}`);
  }

  deleteTrainer(trainerId : number){
    return this.http.delete<any[]>(`/api/Trainer/${trainerId}`);
  }

  registerUserOnEvent(eventId: number) {
    return this.http.post<any>(`/api/Event/AddUserOnEvent?eventId=${eventId}`,{});
  }

  unregisterUserFromEvent(eventId: number) {
    return this.http.post<any>(`/api/Event/UnregisterUserFromEvent?eventId=${eventId}`,{});
  }

  sendEmails(userIds:string[],eventId:number){
    return this.http.post(`/api/Event/SendCertificates?eventId=${eventId}`,userIds)
  }
}
