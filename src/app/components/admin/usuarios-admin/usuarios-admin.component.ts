import { Component, OnInit } from '@angular/core';
import { Usuario } from 'src/app/interfaces/user/usuario';
import { AdminService } from 'src/app/services/admin/admin.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-usuarios-admin',
  templateUrl: './usuarios-admin.component.html',
  styleUrls: ['./usuarios-admin.component.css']
})
export class UsuariosAdminComponent implements OnInit {

  constructor(private api: AdminService, private snackBar: MatSnackBar) { }
  
  public usuarios: Usuario[] = [];
  public campoNombre = '';
  showSuccess = false;
  showConfirmation = false;
  successMessage = '';
  usuarioAEliminar: Usuario | undefined;
  public showHayUsuariosEnBD = false;
  public showHayUsuarioEnBD = false;

  ngOnInit(): void {
    this.obtenerUsuarios();
  }

  actualizarEstadoUsuarios(){
    if(this.usuarios.length == 0)
      this.showHayUsuariosEnBD = true;
    else
      this.showHayUsuariosEnBD = false;
  }

  obtenerUsuarios() {
    this.api.obtenerUsuarios()
      .subscribe(
        (response: any[]) => {
          this.usuarios = response;
          this.actualizarEstadoUsuarios();
        }
      );
  }

  eliminarUsuario(correo: string) {
    this.api.eliminarUsuario(correo)
      .subscribe(
        response => {
          // Elimina el usuario del arreglo en memoria
          const index = this.usuarios.findIndex(u => u.correo === correo);
          if (index !== -1) {
            this.usuarios.splice(index, 1);
          }
          this.actualizarEstadoUsuarios();
          this.mostrarSnackbar('¡Usuario eliminado exitosamente!');
          this.cerrarConfirmacion();
        });
  }

  obtenerUsuarioPorNombre() {   
    if(this.campoNombre != ""){
      this.api.obtenerUsuarioPorNombre(this.campoNombre)
        .subscribe(
          (response: any[]) => {
            this.usuarios = response; 
            if(this.usuarios.length == 0)
              this.showHayUsuarioEnBD = true;
            else
              this.showHayUsuarioEnBD = false;
          });
    } else {
      this.mostrarSnackbar('Debe ingresar un nombre de usuario.');
    }
  }

  mostrarSnackbar(message: string) {
    this.successMessage = message;
    this.showSuccess = true;
    setTimeout(() => {
      this.showSuccess = false;
    }, 3000);
  }

  // Métodos para manejar el modal de confirmación
  mostrarModalConfirmacion(usuario: Usuario) {
    this.usuarioAEliminar = usuario;
    this.showConfirmation = true;
  }

  cerrarConfirmacion() {
    this.showConfirmation = false;
  }

  confirmarEliminacion() {
    if (this.usuarioAEliminar && this.usuarioAEliminar.correo) {
      this.eliminarUsuario(this.usuarioAEliminar.correo);
    }
  }

  cancelarEliminacion() {
    this.usuarioAEliminar = undefined;
    this.cerrarConfirmacion();
  }

}
