import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';
import {Router} from '@angular/router';

import {Usuario} from "src/app/interfaces/user/usuario";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private router:Router, private api: UserService) { }

  ngOnInit(): void {
  }

  public datosUsuario = {
    nombre: '',
    apellidos: '',
    numeroIdentificacion: undefined,
    email: '',
    contra: '',
    confirmeContra: ''
  }  

  registrar() {
    const user = this.datosUsuario;
  
    // Verificar que todos los campos estén llenos
    if (
      !user.nombre ||
      !user.apellidos ||
      !user.email ||
      !user.contra ||
      !user.confirmeContra ||
      user.numeroIdentificacion === null
    ) {
      alert("Todos los campos deben estar llenos.");
      return;
    }
  
    // Verificar que las contraseñas coincidan
    if (user.contra !== user.confirmeContra) {
      alert("Las contraseñas no coinciden.");
      return;
    }
  
    const usuarioFinal: Usuario = {
      correo: user.email,
      nombre: user.nombre,
      apellidos: user.apellidos,
      user_password: user.contra,
      numero_identificacion: user.numeroIdentificacion
    };
  
    this.api.registrarUsuario(usuarioFinal)
      .subscribe(response => {
        this.router.navigate(["/login"]);
        alert("Usuario registrado exitosamente!");
        /*localStorage.setItem("email-cliente", user.email);
        localStorage.setItem("pass-cliente", user.contra);
        this.router.navigate(["/perfil"]);*/
      });
  }

}
