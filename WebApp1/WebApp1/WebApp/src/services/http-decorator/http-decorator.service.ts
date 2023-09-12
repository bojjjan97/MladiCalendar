import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth-service/auth.service';
import { catchError, tap } from 'rxjs';
import { SpinnerServiceService } from 'components/spinner/spinner-service.service';
import { AlertService } from 'components/alert-component/alert.service';
import { TokenService } from '../auth-service/token-services';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpDecoratorService {
  private url: string =  environment.apiUrl;
  headers! :HttpHeaders;

  constructor(private http:HttpClient,private spinner:SpinnerServiceService, private alert:AlertService,private tokenService:TokenService) {
  }

  get<T>(route:string){
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.tokenService.userToken}`
    })
    this.spinner.showSpinner();
    return this.http.get<T>(`${this.url}${route}`,{ headers: this.headers}).pipe(
      catchError(res => {
        if(res.status != 200){
          this.spinner.hideSpinner();
          this.alert.showAlert({Message:"Error", AlertType:0})
        }
        return [];
      }),
      tap(x=>this.spinner.hideSpinner())
    )
  }
  post<T>(route:string, body:any){
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.tokenService.userToken}`
    })
    return this.http.post<T>(`${this.url}${route}`,body,{ headers: this.headers}).pipe(
      catchError(res => {
        if(res.status != 200){
          this.alert.showAlert({Message:"Error", AlertType:0})
        }
        return [];
      })
      );
  }

  delete<T>(route:string){
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.tokenService.userToken}`
    })
    return this.http.delete<T>(`${this.url}${route}`,{ headers: this.headers})
    .pipe(
      catchError(res => {
        if(res.status != 200){
          this.alert.showAlert({Message:"Error", AlertType:0})
        }
        return [];
      })
      );
  }

  put<T>(route:string,body:any){
    console.log(route,body)
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.tokenService.userToken}`
    })
    return this.http.put<T>(`${this.url}${route}`, body,{ headers: this.headers})
    .pipe(
      catchError(res => {
        if(res.status != 200){
          this.alert.showAlert({Message:"Error", AlertType:0})
        }
        return [];
      })
      );
  }
}
