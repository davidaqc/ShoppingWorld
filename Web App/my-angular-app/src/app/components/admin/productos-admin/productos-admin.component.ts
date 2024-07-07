import { Component, OnInit } from '@angular/core';
import { NgForm } from "@angular/forms";
import { AdminService } from 'src/app/services/admin/admin.service'
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-productos-admin',
  templateUrl: './productos-admin.component.html',
  styleUrls: ['./productos-admin.component.css']
})
export class ProductosAdminComponent implements OnInit {

  constructor(private modalService: NgbModal, private api: AdminService) { }

  ngOnInit(): void {
    this.obtenerProductos();
  }

  public productos: any[] = [];
  public actualizar: boolean = false;
  public closeResult: string = '';
  public categoriasDisponibles: any[] = [];

  nuevoProducto: any = {
    nombre: null,
    descripcion: null,
    detalles: null,
    precio: null,
    stock: null,
    id_categoria: null
  };

  obtenerProductos() {
    this.api.obtenerProductos()
      .subscribe(
        (response: any[]) => {
          this.productos = response;
        },
        error => {
          console.error('Error al obtener productos:', error);
          // Manejo de errores si es necesario
        }
      );
  }

  eliminarProducto(producto: any) {
    if (confirm('¿Está seguro que quiere eliminar el producto?')) {
      this.api.eliminarProducto(producto)
        .subscribe(
          response => {
            // Elimina el producto del arreglo en memoria
            const index = this.productos.findIndex(p => p.id_producto === producto.id_producto);
            if (index !== -1) {
              this.productos.splice(index, 1);
            }
            console.log("Producto eliminado exitosamente!");
          },
          error => {
            console.error("Error eliminando el producto:", error);
            alert("No se pudo eliminar el producto.");
          }
        );
    }
  }

  limpiarForm(): void {
    this.nuevoProducto.nombre = null,
    this.nuevoProducto.descripcion = null,
    this.nuevoProducto.detalles = null,
    this.nuevoProducto.precio = null,
    this.nuevoProducto.stock = null,
    this.nuevoProducto.categoria = null
  }

  open(content: any) {
    this.cargarCategorias();
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  crearNuevo(content: any) {
    this.limpiarForm();
    this.actualizar = false;
    this.open(content);
  }

  cargarCategorias(): void {
    this.api.obtenerCategorias()
      .subscribe(
        (response: any[]) => {
          this.categoriasDisponibles = response;
        },
        error => {
          console.error('Error al obtener categorías:', error);
          // Manejo de errores si es necesario
        }
      );
  }

  guardarProducto(form: NgForm): void {
    if (form.valid) {
      if(this.actualizar){
        /*this.api.actualizarProducto(this.nuevoProducto).subscribe(response=>location.reload(),(error:any)=>{
              alert("No se puede actualizar el empleado")});*/
        this.api.actualizarProducto(this.nuevoProducto)
              .subscribe(
                response => {
                  console.log("Producto actualizado correctamente!");
                  // Cerrar el modal
                  this.modalService.dismissAll();
                  // Actualizar la lista de productos
                  this.obtenerProductos();
                },
                error => {
                  console.error("Error actualizando el producto:", error);
                  alert("No se pudo actualizar el producto.");
                }
              );
      }
      else{
        this.api.crearProducto(this.nuevoProducto)
        .subscribe(
          response => {
            console.log("Producto agregado correctamente!");
            // Cerrar el modal
            this.modalService.dismissAll();
            // Actualizar la lista de productos
            this.obtenerProductos();
          },
          error => {
            console.error("Error agregando el producto:", error);
            alert("No se pudo agregar el producto.");
          }
        );
      }  
      
    } else {
      alert("Formulario no válido. Rellene todos los campos.");
    }
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on the backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  actualizarProducto(content:any, producto:any){
    this.nuevoProducto.id_producto = producto.id_producto;
		this.nuevoProducto.nombre = producto.nombre_producto;
    this.nuevoProducto.descripcion = producto.descripcion;
    this.nuevoProducto.detalles = producto.detalles;
    this.nuevoProducto.precio = producto.precio;
    this.nuevoProducto.stock = producto.stock;
    this.nuevoProducto.categoria = producto.nombre_categoria;
    this.actualizar = true;
		this.open(content);
	}
}
