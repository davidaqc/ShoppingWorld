import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Usuario } from 'src/app/interfaces/user/usuario';
import { environment } from '../../../environments/environment'; 

@Injectable({
  providedIn: 'root',
})
export class UserService {

  constructor(private http: HttpClient) {}

  private baseURL = environment.apiUrl;

  verificarCredenciales(email: string, password: string) {
    const url = `${this.baseURL}/Usuarios/Login`;
    return this.http.post(url, {
      Correo: email,
      User_Password: password,
      Nombre: '',
      Apellidos: '',
      numero_identificacion: ''
    });
  }

  registrarUsuario(user: Usuario) {
    const url = `${this.baseURL}/Usuarios/Registrar`;
    return this.http.post(url, {
      Correo: user.correo,
      User_Password: user.user_password,
      Nombre: user.nombre,
      Apellidos: user.apellidos,
      NumeroIdentificacion: user.numero_identificacion
    });
  }
}
