import { Component, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { AuthService } from 'services/auth-service/auth.service';
import { CreateEvent, IEvent, UpdateEventDto } from 'services/event-service/event.model';
import { EventService } from 'services/event-service/event.service';

@Component({
  selector: 'app-create-event',
  templateUrl: './create-event.component.html',
  styleUrls: ['./create-event.component.css']
})
export class CreateEventComponent {
  @Input() selectedDate: string = "";
  @Input() eventToUpdate: IEvent | null = null;
  @Input() updateEvent:boolean  = false;
  createEventForm!: FormGroup;

  constructor(private fb: FormBuilder, private eventService: EventService, private auth: AuthService) {

  }

  countries: any[] = [];
  trainers: any[] = [];
  selectedTrainerIds: number[] = [];
  eventUserType = [
    {
      id: 1,
      name: "Student"
    },
    {
      id: 2,
      name: "Highschool"
    }
  ]
  submitButtonText : string = "Create Event";
  ngOnInit() {
    if(!!this.eventToUpdate){
      this.submitButtonText = "Update Event";
    }
    this.createEventForm = this.fb.group({
      id: this.fb.control(this.eventToUpdate?.eventId),
      description: this.fb.control(this.eventToUpdate?.description ?? '', Validators.required),
      name: this.fb.control(this.eventToUpdate?.name ?? '',Validators.required),
      location: this.fb.control(this.eventToUpdate?.location ?? '',Validators.required),
      onlineMeetingUrl: this.fb.control(this.eventToUpdate?.onlineMeetingUrl ?? '',Validators.required),
      numberOfParticipants: this.fb.control(this.eventToUpdate?.numberOfParticipants ?? '',Validators.required),
      eventDateTime: this.fb.control(this.selectedDate,Validators.required),
      eventForUserType: this.fb.control(this.eventToUpdate?.eventUserType ?? '',Validators.required),
      trainerIds: this.fb.control([],Validators.required),
      eventStartTime: this.fb.control('',Validators.required)
    })
    this.eventService.getCountries().subscribe(c => {
      this.countries = c;
    })
    this.eventService.getTrainers().subscribe(t => {
      this.trainers = t;
    })

    this.createEventForm.controls['eventStartTime'].valueChanges.subscribe(val =>{
      if(this.createEventForm.controls['eventStartTime'].hasError('required') && this.createEventForm.controls['eventStartTime'].touched )
        this.timeValid = false;
      else
      this.timeValid = true;

     })
  }

  addTrainer(trainerId: any) {
    if (this.selectedTrainerIds.includes(trainerId))
      this.selectedTrainerIds = this.selectedTrainerIds.filter(x => x != trainerId)
    else
      this.selectedTrainerIds.push(trainerId);
    this.createEventForm.patchValue({trainerIds:this.selectedTrainerIds})
  }

  func(v:any){
    console.log(v);
  }

  createEvent() {
    console.log(this.createEventForm)
    if(this.updateEvent){
      let eventId = Number(this.createEventForm.get('id')?.value);
      let fromValue: CreateEvent = this.createEventForm.getRawValue();
      fromValue.trainerIds = this.selectedTrainerIds.map(x => Number(x));
      fromValue.eventForUserType = Number(fromValue.eventForUserType);
      fromValue.eventDateTime = `${this.selectedDate}T${this.createEventForm.get("eventStartTime")?.value}:11.905Z`;
      if(!this.createEventForm.valid) return;
      this.eventService.updateEvent(fromValue,eventId).subscribe(x=>{
        this.eventService.eventCreationSubject.next(true);
        this.updateEvent = false;
      });
    }
    else{
      let fromValue: CreateEvent = this.createEventForm.getRawValue();
      fromValue.trainerIds = this.selectedTrainerIds.map(x => Number(x));
      fromValue.eventForUserType = Number(fromValue.eventForUserType);
      fromValue.eventDateTime = `${this.selectedDate}T${this.createEventForm.get("eventStartTime")?.value}:11.905Z`;
      if(!this.createEventForm.valid) return;
      this.eventService.createEvent(fromValue).subscribe(x=>{
        this.eventService.eventCreationSubject.next(true);
      });
    }

  }

  prepareDataForApi() {

  }
  matcher = new MyErrorStateMatcher();
  timeValid = true;

}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    return (control && control.invalid && control.touched) ?? false;
  }
}
