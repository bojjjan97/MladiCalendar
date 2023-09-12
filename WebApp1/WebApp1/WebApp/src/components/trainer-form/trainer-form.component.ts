import { Component, Inject, Input } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EventService } from 'services/event-service/event.service';

@Component({
  selector: 'app-trainer-form',
  templateUrl: './trainer-form.component.html',
  styleUrls: ['./trainer-form.component.css']
})
export class TrainerFormComponent {
  trainerForm!: FormGroup;
  countries : any[] = [];
  updateFuntion: boolean = false;

  constructor(private fb: FormBuilder,private eventService: EventService,@Inject(MAT_DIALOG_DATA) public data: any, private dialogRef:MatDialogRef<any>) {
    this.countries = data.countries;
  }


  ngOnInit() {
    if (this.data.trainerToUpdate) {
      this.updateFuntion = true;
    } else {
      this.updateFuntion = false;
    }
     
    this.trainerForm = this.fb.group({
      id: this.fb.control(this.data.trainerToUpdate?.id),
      firstName: this.fb.control(this.data.trainerToUpdate?.firstName ?? ''),
      lastName:  this.fb.control(this.data.trainerToUpdate?.lastName ?? ''),
      email:  this.fb.control(this.data.trainerToUpdate?.email ?? ''),
      phoneNumber:  this.fb.control(this.data.trainerToUpdate?.phoneNumber ?? ''),
      country: this.fb.control(this.data.trainerToUpdate?.country ?? '')
    })
  }

  compareFunction(o1: any, o2: any) {
    return (o1.id == o2.id);
   }

  update(){
    let trainerFrom = this.trainerForm.getRawValue()
    trainerFrom.countryId = trainerFrom.country.id;
    delete trainerFrom['country'];

    if (this.updateFuntion) {
      this.eventService.updateTrainer(trainerFrom).subscribe(x=>{
        this.dialogRef.close()
    });
    } else {
      delete trainerFrom['id'];
      this.eventService.createTrainer(trainerFrom).subscribe((x:any)=>{
        this.dialogRef.close()
      })
      
    }

  }

}
