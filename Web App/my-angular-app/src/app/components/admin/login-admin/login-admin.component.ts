import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/services/admin/admin.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login-admin',
  templateUrl: './login-admin.component.html',
  styleUrls: ['./login-admin.component.css']
})
export class LoginAdminComponent implements OnInit {

  constructor(private router:Router, private api: AdminService) { }

  public datosUsuario = {
    Email:"",
    Password:""
  }

  ngOnInit(): void {
  }

  errorInicioSesion = false; // Controlar la visibilidad del mensaje de error

  verificarCredenciales():void {

    if(this.datosUsuario.Email != "" && this.datosUsuario.Password != ""){
      this.api.loginAdmin(this.datosUsuario.Email, this.datosUsuario.Password)
      .subscribe(response=>{ 
        if (response !== null){
          this.router.navigate(["/admin/dashboard"]);
        }else{
          alert("Usuario o contraseña incorrecta, intente de nuevo");
        }
        
      },(error:any)=>{
          //alert("¡Error al intentar conectar con el server!");
          alert("Usuario o contraseña incorrecta, intente de nuevo");
          this.errorInicioSesion = true; // Mostrar mensaje de error
          this.datosUsuario.Password = ''; // Limpiar el campo de contraseña
        });
    }else{
      alert("Debe ingresar su correo y contraseña");
    }
}

}
