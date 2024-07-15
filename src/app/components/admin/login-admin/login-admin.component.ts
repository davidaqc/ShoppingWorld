import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/services/admin/admin.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-admin',
  templateUrl: './login-admin.component.html',
  styleUrls: ['./login-admin.component.css']
})
export class LoginAdminComponent implements OnInit {

  constructor(private router: Router, private api: AdminService) { }

  public datosUsuario = {
    Email: "",
    Password: ""
  }
  mostrarContrasena: boolean = false;
  successMessage = '';
  showSuccess = false;

  ngOnInit(): void {
  }

  verificarCredenciales(): void {
    if (this.datosUsuario.Email !== "" && this.datosUsuario.Password !== "") {
      this.api.loginAdmin(this.datosUsuario.Email, this.datosUsuario.Password)
      .subscribe(response => {
          localStorage.setItem('jwtToken', response.token); // Guardar el token
          this.router.navigate(["/admin/dashboard"]);
        });
    } else {
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
