import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('jwtToken');
    // Verificar si el token es válido
    return !!token;
  }

  // Cerrar sesión y eliminar el token
  logout(): void {
    localStorage.removeItem('jwtToken');
  }
}
