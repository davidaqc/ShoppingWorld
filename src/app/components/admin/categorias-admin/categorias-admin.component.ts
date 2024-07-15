import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AdminService } from 'src/app/services/admin/admin.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { Categoria } from 'src/app/interfaces/user/categoria';

@Component({
  selector: 'app-categorias-admin',
  templateUrl: './categorias-admin.component.html',
  styleUrls: ['./categorias-admin.component.css']
})
export class CategoriasAdminComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef;

  constructor(private formBuilder: FormBuilder, private modalService: NgbModal, private api: AdminService) {
    this.categoriaForm = this.formBuilder.group({
      nombre: ['', [Validators.required, Validators.maxLength(20)]]
    });
  }

  ngOnInit(): void {
    this.cargarCategorias();
  }

  public categorias: Categoria[] = [];
  public actualizar = false;
  public closeResult = '';
  categoriaForm!: FormGroup;
  showSuccess = false;
  successMessage = '';
  formSubmitted = false;
  nuevaCategoria: any = {
    nombre: null
  };

  showConfirmation = false;
  categoriaAEliminar: Categoria | undefined;

  cargarCategorias(): void {
    this.api.obtenerCategorias().subscribe((response: any[]) => {
      this.categorias = response;
    });
  }

  eliminarCategoria(idCategoria: any) {
    this.api.eliminarCategoria(idCategoria).subscribe((response) => {
      const index = this.categorias.findIndex((c) => c.idCategoria === idCategoria);
      if (index !== -1) {
        this.categorias.splice(index, 1);
      }
      this.mostrarSnackbar('¡Categoría eliminada exitosamente!');
    });
  }

  guardarCategoria(): void {
    this.formSubmitted = true;
    if (this.categoriaForm.valid) {
      const formData = {
        nombre: this.categoriaForm.value.nombre
      };

      this.api.crearCategoria(formData).subscribe((response) => {
        this.mostrarSnackbar('¡Categoría agregada exitosamente!');
        this.modalService.dismissAll();
        this.cargarCategorias();
        this.limpiarForm();
      });
    }
  }

  limpiarForm(): void {
    this.nuevaCategoria.nombre = null;
    this.categoriaForm.reset();
  }

  open(content: any) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
      this.formSubmitted = false;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      this.formSubmitted = false;
    });
  }

  crearNuevo(content: any) {
    this.limpiarForm();
    this.actualizar = false;
    this.open(content);
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

  actualizarCategoria(content: any, categoria: any) {
    this.nuevaCategoria.idCategoria = categoria.idCategoria;
    this.nuevaCategoria.nombre = categoria.nombre;

    this.actualizar = true;

    this.categoriaForm.patchValue({
      nombre: this.nuevaCategoria.nombre
    });

    this.open(content);
  }

  mostrarSnackbar(message: string) {
    this.successMessage = message;
    this.showSuccess = true;
    setTimeout(() => {
      this.showSuccess = false;
    }, 3000);
  }

  mostrarModalConfirmacion(categoria: Categoria) {
    this.categoriaAEliminar = categoria;
    this.showConfirmation = true;
  }

  cerrarConfirmacion() {
    this.showConfirmation = false;
  }

  confirmarEliminacion() {
    if (this.categoriaAEliminar && this.categoriaAEliminar.idCategoria) {
      this.eliminarCategoria(this.categoriaAEliminar.idCategoria);
    }
  }

  cancelarEliminacion() {
    this.categoriaAEliminar = undefined;
    this.cerrarConfirmacion();
  }
}
