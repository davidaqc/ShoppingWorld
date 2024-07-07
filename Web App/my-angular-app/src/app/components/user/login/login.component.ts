import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit(): void {
  }

  datosUsuario = {
    Email: '',
    Password: ''
  };

  errorInicioSesion = false;

  verificarCredenciales(): void {
    this.userService.verificarCredenciales(this.datosUsuario.Email, this.datosUsuario.Password)
      .subscribe(
        response => {
          this.router.navigate(['/inicio']);
        },
        error => {
          console.log(error);
          this.errorInicioSesion = true;
          this.datosUsuario.Password = ''; 
        }
      );
  }

}
