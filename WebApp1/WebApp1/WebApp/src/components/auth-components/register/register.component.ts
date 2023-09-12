import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { AuthService } from 'services/auth-service/auth.service';
import { EventService } from 'services/event-service/event.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  countries: any[] = [];
  constructor(private fb: FormBuilder,private eventService:EventService, private authService: AuthService) {

    this.eventService.getCountries().subscribe(c => {
      this.countries = c;
    })
    this.registerForm = fb.group({
      email: fb.control('',Validators.email),
      firstName: fb.control('',Validators.required),
      lastName: fb.control('',Validators.required),
      phoneNumber: fb.control(''),
      password: fb.control('',Validators.required),
      repeatPassword: fb.control('',Validators.required),
      userType: this.fb.control('',Validators.required),
      countryId: this.fb.control(''),
    })
  }

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

  registerUser() {
    if(!this.registerForm.valid) return;

    const registerData = this.registerForm.getRawValue();
    delete registerData.repeatPassword;
    this.authService.register(registerData)
  }
  ngOnInit(): void {
  }
  matcher = new MyErrorStateMatcher();
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return (control && control.invalid && control.touched) ?? false;
  }
}

	
	

