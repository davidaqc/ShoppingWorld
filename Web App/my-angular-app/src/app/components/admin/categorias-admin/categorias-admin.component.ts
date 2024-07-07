import { Component, OnInit } from '@angular/core';
import { NgForm } from "@angular/forms";
import { AdminService } from 'src/app/services/admin/admin.service'
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-categorias-admin',
  templateUrl: './categorias-admin.component.html',
  styleUrls: ['./categorias-admin.component.css']
})
export class CategoriasAdminComponent implements OnInit {

  constructor(private modalService: NgbModal, private api: AdminService) { }

  ngOnInit(): void {
    this.cargarCategorias();
  }

  public categorias: any[] = [];
  public actualizar: boolean = false;
  public closeResult: string = '';

  nuevaCategoria: any = {
    nombre: null
  };

  cargarCategorias(): void {
    this.api.obtenerCategorias()
      .subscribe(
        (response: any[]) => {          
          this.categorias = response;
        },
        error => {
          console.error('Error al obtener categorías:', error);
          // Manejo de errores si es necesario
        }
      );
  }

  eliminarCategoria(categorias: any) {
    if (confirm('¿Está seguro que quiere eliminar la categoria? Al hacerlo, también se eliminarán los productos asociados.')) {
      this.api.eliminarCategoria(categorias)
        .subscribe(
          response => {
            // Elimina el producto del arreglo en memoria
            const index = this.categorias.findIndex(p => p.id_categoria === categorias.id_categoria);
            if (index !== -1) {
              this.categorias.splice(index, 1);
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
    this.nuevaCategoria.nombre = null
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

  guardarProducto(form: NgForm): void {

    //this.modalService.dismissAll();

    if (form.valid) {
      if(this.actualizar){

      }
      else{
        this.api.crearCategoria(this.nuevaCategoria)
        .subscribe(
          response => {
            console.log("Producto agregado correctamente!");
            // Cerrar el modal
            this.modalService.dismissAll();
            // Actualizar la lista de productos
            this.cargarCategorias();
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

}
