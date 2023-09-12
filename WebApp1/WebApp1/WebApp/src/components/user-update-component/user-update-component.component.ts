import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { AuthService } from 'services/auth-service/auth.service';
import { TokenService } from 'services/auth-service/token-services';
import { EventService } from 'services/event-service/event.service';

@Component({
  selector: 'app-user-update-component',
  templateUrl: './user-update-component.component.html',
  styleUrls: ['./user-update-component.component.css']
})
export class UserUpdateComponentComponent {
  registerForm: FormGroup;
  countries: any[] = [];
  user : any;

  isAdmin : boolean = false;
  constructor(private fb: FormBuilder,private eventService:EventService, private authService: AuthService,private tokenService:TokenService) {

    this.isAdmin = tokenService.userRole == 'Admin';
    this.eventService.getCountries().subscribe(c => {
      this.countries = c;
      this.authService.getUserById().subscribe((u:any) =>{
        this.registerForm.patchValue({
          firstName:u?.firstName,
          lastName:u?.lastName,
          userType:this.eventUserType.filter(x=>x.id == u.userType)[0],
          countryId: this.countries.filter(x=>x.id == u.country.id)[0],
          phoneNumber:u.phoneNumber
        })
      });
    })
    
   

    this.registerForm = fb.group({
      firstName: fb.control('',Validators.required),
      lastName: fb.control('',Validators.required),
      phoneNumber: fb.control(''),
      userType: this.fb.control('',Validators.required),
      countryId: this.fb.control(null),
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
    if(this.isAdmin) this.registerForm.get('userType')?.setValue({id:4});
    if(!this.registerForm.valid) return;
    const changeDataForm = this.registerForm.getRawValue();
   changeDataForm.countryId = changeDataForm.countryId.id;
   changeDataForm.userType = changeDataForm.userType.id;
    this.authService.changeUserDate(changeDataForm)
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
