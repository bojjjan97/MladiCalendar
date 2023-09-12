import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'services/auth-service/auth.service';
import { TokenService } from 'services/auth-service/token-services';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {


  userLoggedIn = false;
  adminLoggedIn = false;
  constructor(private tokenService:TokenService,private auth:AuthService, private router:Router){
  }
  ngOnInit(){
    this.tokenService.authenticatedSubject.subscribe(authenticated=>{
      if(authenticated){
        this.userLoggedIn = this.tokenService.userRole === "User";
        this.adminLoggedIn = this.tokenService.userRole === "Admin";
      }
      else{
        this.router.navigate(['login']);
      }
    });
    this.tokenService.checkUserStatus();

  }

  logout(){
    this.auth.logout();
    this.userLoggedIn = false;
    this.adminLoggedIn = false;
  }

  calendar(){
    this.router.navigate(['calendar'])
  }

  dashboard(){
    this.router.navigate(['dashboard'])
  }

  trainer(){
    this.router.navigate(['trainer'])
  }
  registerAdmin() {
    this.router.navigate(['register-admin'])
  }

  updateUser(){
    this.router.navigate(['update-user'])
  }
}
