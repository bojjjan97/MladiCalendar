import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EventService } from 'services/event-service/event.service';
import { TrainerFormComponent } from '../trainer-form/trainer-form.component';
import { forkJoin } from 'rxjs';
import { Trainer } from 'services/event-service/event.model';



@Component({
  selector: 'app-trainer-component',
  templateUrl: './trainer-component.component.html',
  styleUrls: ['./trainer-component.component.css']
})
export class TrainerComponentComponent {
  displayedColumns = [ 'fullName', 'email', 'phoneNumber','country','edit','delete'];
  dataSource: MatTableDataSource<TrainerData> = new MatTableDataSource();
  countries : any[] = [];
  paginator:MatPaginator | null = null;
  sort:MatSort | null = null;

  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.dataSource.paginator = this.paginator;
  }
  @ViewChild(MatSort) set content(content: MatSort) {
    this.sort = content;
    if (this.sort) {
      this.dataSource.sort = this.sort;
    }
  };

  constructor(private service: EventService,  public dialog: MatDialog) {
    this.service.getCountries().subscribe(c =>{
      this.countries = c;
    })


  }

  /**
   * Set the paginator and sort after the view init since this component will
   * be able to query its view for the initialized paginator and sort.
   */
  ngAfterViewInit() {
    this.initTable();
  }

  initTable(){
    this.service.getTrainers().subscribe((trainers: any) => {
      // Assign the data to the data source for the table to render

      this.dataSource.data = trainers;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;

    });
  }
  deleteTrainer(trainerId:number){
    this.service.deleteTrainer(trainerId).subscribe(x=>{
      this.dataSource.data = this.dataSource.data.filter(x=>x.id != trainerId);
      this.dataSource._updateChangeSubscription();
    });
  }


  editTrainer(trainer:TrainerData){
    forkJoin([
      this.service.getTrainerById(trainer.id),
      this.service.getCountries()]
    ).subscribe(results =>{
      this.dialog.open(TrainerFormComponent,{
        data: {trainerToUpdate:results[0],countries:results[1]}
      }).afterClosed().subscribe(result => {
        this.service.getTrainers().subscribe((trainers: any) => {
          this.initTable();
        });
    })
   }
    );
  }

  createTrainer(){
    this.dialog.open(TrainerFormComponent,{
      data: {trainerToUpdate: null,countries: this.countries}
    }).afterClosed().subscribe(result => {
      this.service.getTrainers().subscribe((trainers: any) => {
        // Assign the data to the data source for the table to render
        this.dataSource = new MatTableDataSource(trainers);
      });
    });
  }

  applyFilter(filterValue: any) {
    let val = filterValue.value
    val = val.trim();
    val = val.toLowerCase();
    this.dataSource.filter = val;
  }
}

export interface TrainerData {
  id: number;
  email: string;
  name: string;
  country: string;
}

