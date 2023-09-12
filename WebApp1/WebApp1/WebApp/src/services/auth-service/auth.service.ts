import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Subject, Observable } from 'rxjs';
import { LoginData, LoginResponse, RegisterResponse } from './models';
import { HttpClient } from '@angular/common/http';
import { AlertService } from 'components/alert-component/alert.service';
import { isThisHour } from 'date-fns';
import { HttpDecoratorService } from '../http-decorator/http-decorator.service';
import { TokenService } from './token-services';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginUrl: string = environment.apiUrl + `/api/Auth/Login`;
  private registerUrl: string = environment.apiUrl + `/api/Auth/Register`;
  public authenticatedSubject: BehaviorSubject<boolean> = new BehaviorSubject(false);
  public userRoleSubject: BehaviorSubject<string> = new BehaviorSubject("");
  private authChangeSub = new Subject<boolean>();
  public authChanged = this.authChangeSub.asObservable();

  constructor(private router: Router, private http:HttpDecoratorService, private alertService:AlertService,private tokenService:TokenService) {

  }



  login(loginData: LoginData) {
    this.http.post<LoginResponse>('/api/Auth/Login', loginData).subscribe(response => {
        if(response?.responseObject?.token && response.responseType == 0){
          this.tokenService.authenticateUser(response.responseObject.token);
          this.router.navigate(['/calendar']);
          this.authenticatedSubject.next(true);

        }
        this.alertService.showAlert({Message:response.message,AlertType : response.responseType});

    })
  }

  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    this.authenticatedSubject.next(false);
    this.router.navigate(['/']);
  }

  register(regData:any) {
    this.http.post('/api/Auth/Register', regData).subscribe((response:any) => {
      if (response) {
        this.alertService.showAlert({AlertType : response.responseType,Message:response.message});
      }
    })
  }

  registerAdmin(regData:any){
    this.http.post('/api/Auth/RegisterAdmin', regData).subscribe((response:any) => {
      if (response) {
        this.alertService.showAlert({AlertType : response.responseType,Message:response.message});
      }
    })
  }

  changeUserDate(changeData:any){
    this.http.post('/api/Auth/ChangeUserData', changeData).subscribe((response:any) => {
      if (response) {
        this.alertService.showAlert({AlertType : response.responseType,Message:response.message});
      }
    })
  }

  getUserById(){
    return this.http.get('/api/User/GetUserById');
  }
}
