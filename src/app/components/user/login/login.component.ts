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

  mostrarContrasena: boolean = false;
  successMessage = '';
  showSuccess = false;
  datosUsuario = {
    Email: '',
    Password: ''
  };

  verificarCredenciales(): void {
    if (this.datosUsuario.Email !== "" && this.datosUsuario.Password !== "") {
      this.userService.verificarCredenciales(this.datosUsuario.Email, this.datosUsuario.Password)
        .subscribe(
          response => {
            this.router.navigate(['/inicio']);
          });
    }else{
      this.mostrarSnackbar('Debe ingresar su correo y contraseña.');
    }
  }

  togglePasswordVisibility() {
    this.mostrarContrasena = !this.mostrarContrasena;
  }

  mostrarSnackbar(message: string) {
    this.successMessage = message;
    this.showSuccess = true;
    setTimeout(() => {
      this.showSuccess = false;
    }, 3000);
  }
  
}
