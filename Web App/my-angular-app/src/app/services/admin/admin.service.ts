import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http:HttpClient) { }

  //baseURL:string = "https://localhost:7156/api";
  baseURL:string = "https://davidaqc.bsite.net/api";

  loginAdmin(email:string, pass:string){
    const url = `${this.baseURL}/Administradores/Login`;
		  return this.http.post(url, {
			"correo" : email,
			"admin_password" : pass
		  });
	  }

	obtenerProductos(){
		const url = `${this.baseURL}/Productos/GetProductos`;
		return this.http.get<any[]>(url);
	}

  crearProducto(nuevoProducto: any) {
    console.log(nuevoProducto);
    
    const url = `${this.baseURL}/Productos/AddProducto`;
    return this.http.post<any>(url, {
      "NombreProducto": nuevoProducto.nombre,
      "Descripcion": nuevoProducto.descripcion,
      "Detalles": nuevoProducto.detalles,
      "Precio": nuevoProducto.precio,
      "Stock": nuevoProducto.stock,
      "IdCategoria": nuevoProducto.id_categoria
    });
  }

  eliminarProducto(producto:any){
		const url = `${this.baseURL}/Productos/DeleteProducto/${producto.id_producto}`;
		return this.http.delete(url);
	}

  actualizarProducto(producto: any){
		const url = `${this.baseURL}/Productos/UpdateProducto/${producto.id_producto}`;
		return this.http.put(url,{
      "NombreProducto": producto.nombre,
      "Descripcion": producto.descripcion,
      "Detalles": producto.detalles,
      "Precio": producto.precio,
      "Stock": producto.stock,
      "IdCategoria": producto.id_categoria
		});
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

  eliminarCategoria(categoria:any){
		const url = `${this.baseURL}/Categorias/DeleteCategoria/${categoria.idCategoria}`;
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

  getProductosCount(){
    const url = `${this.baseURL}/Productos/GetProductosCount`;
		return this.http.get<any[]>(url);
  }

  getCategoriasCount(){
    const url = `${this.baseURL}/Categorias/GetCategoriasCount`;
		return this.http.get<any[]>(url);
  }

}
