import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable  } from 'rxjs';
import { environment } from '../../../environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http:HttpClient) { }

  private baseURL = environment.apiUrl;

  loginAdmin(email: string, pass: string): Observable<any> {
    const url = `${this.baseURL}/Administradores/Login`;
    return this.http.post<any>(url, {
      correo: email,
      admin_password: pass
    });
  }
  
	obtenerProductos(){
		const url = `${this.baseURL}/Productos/GetProductos`;
		return this.http.get<any[]>(url);
	}

  crearProducto(nuevoProducto: FormData) {
    const url = `${this.baseURL}/Productos/AddProducto`;
    return this.http.post<any>(url, nuevoProducto); 
  }

  eliminarProducto(id_producto:number){
		const url = `${this.baseURL}/Productos/DeleteProducto/${id_producto}`;
		return this.http.delete(url);
	}

  actualizarProducto(updatedProducto: any){
    const url = `${this.baseURL}/Productos/UpdateProducto`;
    return this.http.put<any>(url, updatedProducto); 
	}

  obtenerUsuarios(){
		const url = `${this.baseURL}/Usuarios/GetUsuarios`;
		return this.http.get<any[]>(url);
	}

  eliminarUsuario(correo:string){
		const url = `${this.baseURL}/Usuarios/DeleteUsuario/${correo}`;
		return this.http.delete(url);
	}

  obtenerUsuarioPorNombre(nombre:string){
		const url = `${this.baseURL}/Usuarios/GetUsuariosByNombre/${nombre}`;
		return this.http.get<any[]>(url);
	}

  obtenerCategorias(){
		const url = `${this.baseURL}/Categorias/GetCategorias`;
		return this.http.get<any[]>(url);
	}

  eliminarCategoria(idCategoria:number){
		const url = `${this.baseURL}/Categorias/DeleteCategoria/${idCategoria}`;
		return this.http.delete(url);
	}

  crearCategoria(nuevaCategoria: any) {
    const url = `${this.baseURL}/Categorias/CreateCategoria`;
    return this.http.post<any>(url, {
      "NombreCategoria": nuevaCategoria.nombre
    });
  }

  getUsuariosCount(){
    const url = `${this.baseURL}/Usuarios/GetUsuariosCount`;
		return this.http.get<any[]>(url);
  }

  getProductosCount(): Observable<any> {
      const url = `${this.baseURL}/Productos/GetProductosCount`;
      return this.http.get<any>(url);
  }

  getCategoriasCount(){
    const url = `${this.baseURL}/Categorias/GetCategoriasCount`;
		return this.http.get<any[]>(url);
  }

}
