import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { AuthService } from 'services/auth-service/auth.service';
import { EventService } from 'services/event-service/event.service';

@Component({
  selector: 'app-register-admin',
  templateUrl: './register-admin.component.html',
  styleUrls: ['./register-admin.component.css']
})
export class RegisterAdminComponent {
  registerForm: FormGroup;
  countries: any[] = [];
  constructor(private fb: FormBuilder,private eventService:EventService, private authService: AuthService) {

    this.eventService.getCountries().subscribe(c => {
      this.countries = c;
    })
    this.registerForm = fb.group({
      email: fb.control('',[Validators.email,Validators.required]),
      firstName: fb.control('',Validators.required),
      lastName: fb.control('',Validators.required),
      phoneNumber: fb.control(''),
      password: fb.control('',Validators.required),
      repeatPassword: fb.control('',Validators.required),
      countryId: this.fb.control('',Validators.required),
    })
  }

  registerUser() {
    if(!this.registerForm.valid) return;

    const registerData = this.registerForm.getRawValue();
    delete registerData.repeatPassword;
    this.authService.registerAdmin(registerData)
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
