import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from 'components/auth-components/login/login.component';
import { RegisterComponent } from 'components/auth-components/register/register.component';
import { DashboardComponent } from 'components/dashboard/dashboard.component';
import { EventCalendarComponent } from 'components/event-calendar/event-calendar.component';
import { CalendarContainerComponent } from 'components/layout-components/calendar-container/calendar-container.component';
import { RegisterAdminComponent } from 'components/register-admin/register-admin/register-admin.component';
import { TrainerComponentComponent } from 'components/trainer-component/trainer-component.component';
import { UserUpdateComponentComponent } from 'components/user-update-component/user-update-component.component';
import { AuthGuardGuard } from 'services/auth.guard';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'calendar', component: CalendarContainerComponent, /*canActivate: [AuthGuardGuard]*/},
  { path: 'dashboard', component: DashboardComponent, /*canActivate: [AuthGuardGuard] */},
  { path: 'trainer', component: TrainerComponentComponent, /*canActivate: [AuthGuardGuard]*/},
  { path: 'register-admin', component: RegisterAdminComponent, /*canActivate: [AuthGuardGuard]*/},
  { path: 'update-user', component: UserUpdateComponentComponent, /*canActivate: [AuthGuardGuard]*/},

  { path: '**', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
