import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { EventCalendarComponent } from 'components/event-calendar/event-calendar.component';
import { EventPanelComponent } from 'components/event-panel/event-panel.component';
import { HeaderComponent } from 'components/layout-components/header/header.component';
import { FooterComponent } from 'components/layout-components/footer/footer.component';
import { LoginComponent } from 'components/auth-components/login/login.component';
import { RegisterComponent } from 'components/auth-components/register/register.component';
import { DashboardComponent } from 'components/dashboard/dashboard.component';
import { AuthService } from 'services/auth-service/auth.service';
import { EventService } from 'services/event-service/event.service';
import { HttpClientModule } from  '@angular/common/http';
import { CalendarContainerComponent } from 'components/layout-components/calendar-container/calendar-container.component';
import { CreateEventComponent } from 'components/create-event/create-event.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatSelectModule} from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AlertComponentComponent } from 'components/alert-component/alert-component.component';
import { AlertService } from 'components/alert-component/alert.service';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { HttpDecoratorService } from 'services/http-decorator/http-decorator.service';
import { SpinnerComponent } from 'components/spinner/spinner.component';
import { TrainerComponentComponent } from 'components/trainer-component/trainer-component.component';
import {NgxMatTimepickerModule} from 'ngx-mat-timepicker';
import {MatTableModule} from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import {MatDialogModule} from '@angular/material/dialog';
import { TrainerFormComponent } from 'components/trainer-form/trainer-form.component';
import { EventPipe } from 'components/event-calendar/eventPipe/eventPipe';
import { SendEmailsFormComponent } from 'components/send-emails-form/send-emails-form.component';
import { ErrorStateMatcher, ShowOnDirtyErrorStateMatcher } from '@angular/material/core';
import { TokenService } from 'services/auth-service/token-services';
import { RegisterAdminComponent } from 'components/register-admin/register-admin/register-admin.component';
import { UserUpdateComponentComponent } from 'components/user-update-component/user-update-component.component';

@NgModule({
  declarations: [
    AppComponent,
    EventCalendarComponent,
    EventPanelComponent,
    HeaderComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    DashboardComponent,
    CalendarContainerComponent,
    CreateEventComponent,
    AlertComponentComponent,
    SpinnerComponent,
    TrainerComponentComponent,
    TrainerFormComponent,
    EventPipe,
    SendEmailsFormComponent,
    RegisterAdminComponent,
    UserUpdateComponentComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CalendarModule.forRoot({ provide: DateAdapter, useFactory: adapterFactory }),
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgbDropdownModule,
    BrowserAnimationsModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
    NgxMatTimepickerModule,
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule
  ],
  providers: [
    AuthService,
    EventService,
    AlertService,
    HttpDecoratorService,
    TokenService,
    { provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
