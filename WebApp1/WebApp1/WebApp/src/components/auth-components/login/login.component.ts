import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'services/auth-service/auth.service';
import { LoginData } from 'services/auth-service/models';
import { TokenService } from 'services/auth-service/token-services';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm!: FormGroup;

  constructor(private fb: FormBuilder, private service: AuthService, private router:Router,private tokenService:TokenService) {
    this.loginForm = this.fb.group({
      email: this.fb.control(''),
      password: this.fb.control('')
    })
  }

  ngOnInit(): void {
    if(this.tokenService.authenticated){
      this.router.navigate(["calendar"]);
    }
  }

  login() {
    this.service.login(this.loginForm.value as LoginData);
  }

  register() {
    this.router.navigateByUrl('/register');
  }
}
