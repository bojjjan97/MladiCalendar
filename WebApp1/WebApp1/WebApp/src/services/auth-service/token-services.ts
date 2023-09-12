import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Subject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class TokenService{
  public authenticatedSubject: BehaviorSubject<boolean> = new BehaviorSubject(false);
  public userRoleSubject: BehaviorSubject<string> = new BehaviorSubject("");
  private authChangeSub = new Subject<boolean>();
  public authChanged = this.authChangeSub.asObservable();

  constructor(private router:Router){

  }

  checkUserStatus() {
    const token = localStorage.getItem("token");
    if (token == null || this.tokenExpired(token!))
      this.authenticatedSubject.next(false);
    else
      this.authenticatedSubject.next(true);
  }

  private tokenExpired(token: string) {
    const expiry = (JSON.parse(atob(token.split('.')[1]))).exp;
    return (Math.floor((new Date).getTime() / 1000)) >= expiry;
  }


  get authenticated() {
    const token = localStorage.getItem("token");
    if (token == null || this.tokenExpired(token!))
      return false;
    return true;
  }

  get isUserAuthenticated(): Observable<boolean> {
    return this.authenticatedSubject.asObservable();
  }

  get userToken(): string | null {
    return localStorage.getItem("token");
  }

  get userRole():string | null{
    return localStorage.getItem("role");
  }

   authenticateUser(token: string) {
    const tokenData = (JSON.parse(atob(token.split('.')[1])))
    const userRole = tokenData['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    localStorage.setItem("token", token);
    localStorage.setItem("role",userRole[0]);
    this.userRoleSubject.next(userRole[0]);
    this.router.navigate(['main']);
    this.authenticatedSubject.next(true);
  }
}
