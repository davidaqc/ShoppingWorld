import { Component, OnInit } from '@angular/core';
import { Usuario } from 'src/app/interfaces/user/usuario';
import { AdminService } from 'src/app/services/admin/admin.service'

@Component({
  selector: 'app-usuarios-admin',
  templateUrl: './usuarios-admin.component.html',
  styleUrls: ['./usuarios-admin.component.css']
})
export class UsuariosAdminComponent implements OnInit {

  constructor(private api: AdminService) { }
  public usuarios: Usuario[] = [];

  ngOnInit(): void {
    this.obtenerUsuarios();
  }

  public campoNombre = '';

  obtenerUsuarios() {
    this.api.obtenerUsuarios()
      .subscribe(
        (response: any[]) => {
          this.usuarios = response;  
        },
        error => {
          console.error('Error al obtener productos:', error);
          // Manejo de errores si es necesario
        }
      );
  }

  eliminarUsuario(correo: any) {
    if (confirm('¿Está seguro que quiere eliminar el usuario?')) {
      this.api.eliminarUsuario(correo)
        .subscribe(
          response => {
            // Elimina el producto del arreglo en memoria
            const index = this.usuarios.findIndex(p => p.correo === correo);
            if (index !== -1) {
              this.usuarios.splice(index, 1);
            }
            console.log("Usuario eliminado exitosamente!");
          },
          error => {
            console.error("Error eliminando el usuario:", error);
            alert("No se pudo eliminar el usuario.");
          }
        );
    }
  }

  obtenerUsuarioPorNombre() {   
    if(this.campoNombre != ""){
      this.api.obtenerUsuarioPorNombre(this.campoNombre)
      .subscribe(
        (response: any[]) => {
          this.usuarios = response;  
        },
        error => {
          console.error('Error al obtener productos:', error);
          // Manejo de errores si es necesario
        }
      );
    }else{
      alert("Debe ingresar un nombre de usuario");
    }
  }

}
