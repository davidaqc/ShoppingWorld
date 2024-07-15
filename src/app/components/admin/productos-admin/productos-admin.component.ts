import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AdminService } from 'src/app/services/admin/admin.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { Producto } from 'src/app/interfaces/admin/producto';

@Component({
  selector: 'app-productos-admin',
  templateUrl: './productos-admin.component.html',
  styleUrls: ['./productos-admin.component.css']
})
export class ProductosAdminComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef;

  constructor(private formBuilder: FormBuilder, private modalService: NgbModal, private api: AdminService) {
    this.productoForm = this.formBuilder.group({
      nombre: ['', [Validators.required, Validators.maxLength(20)]],
      descripcion: ['', [Validators.required, Validators.maxLength(50)]],
      detalles: ['', [Validators.maxLength(50)]],
      precio: ['', [Validators.required, Validators.pattern(/^\d+(\.\d+)?$/)]],
      stock: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      id_categoria: ['', [Validators.required]],
      imagen: ['', [Validators.required, this.validateFileExtension(['png'])]]
    });
  }

  ngOnInit(): void {
    this.obtenerProductos();
    this.cargarCategorias();
  }

  public productos: Producto[] = [];
  public actualizar = false;
  public closeResult = '';
  public categoriasDisponibles: any[] = [];
  productoForm!: FormGroup;
  showSuccess = false;
  successMessage = '';
  selectedFile!: File;
  formSubmitted = false;
  maxFileSizeInKB = 500;
  nuevoProducto: any = {
    nombre: null,
    descripcion: null,
    detalles: null,
    precio: null,
    stock: null,
    id_categoria: null
  };

  showConfirmation = false;
  usuarioAEliminar: Producto | undefined;
  loading = false;

  obtenerProductos() {
    this.api.obtenerProductos().subscribe((response) => {
      this.productos = response;
    });
  }

  eliminarProducto(id_producto: any) {
    this.api.eliminarProducto(id_producto).subscribe((response) => {
      const index = this.productos.findIndex((p) => p.id_producto === id_producto);
      if (index !== -1) {
        this.productos.splice(index, 1);
      }
      this.mostrarSnackbar('¡Producto eliminado exitosamente!');
    });
  }

  guardarProducto(): void {
    this.formSubmitted = true;
    if (this.productoForm.valid) {
      this.loading = true;
      const formData = new FormData();
      formData.append('NombreProducto', this.productoForm.value.nombre);
      formData.append('Descripcion', this.productoForm.value.descripcion);
      formData.append('Detalles', this.productoForm.value.detalles);
      formData.append('Precio', this.productoForm.value.precio);
      formData.append('Stock', this.productoForm.value.stock);
      formData.append('IdCategoria', this.productoForm.value.id_categoria);
      formData.append('file', this.selectedFile, this.selectedFile.name);

      if (this.actualizar) {
        formData.append('IdProducto', this.nuevoProducto.id_producto);
        if (this.nuevoProducto.image_url !== null) formData.append('ImageUrl', this.nuevoProducto.image_url);
        this.api.actualizarProducto(formData).subscribe((response) => {
          this.mostrarSnackbar('¡Producto actualizado exitosamente!');
          this.modalService.dismissAll();
          this.obtenerProductos();
          this.loading = false;
          this.limpiarForm();
        });
      } else {
        this.api.crearProducto(formData).subscribe((response) => {
          this.mostrarSnackbar('¡Producto agregado exitosamente!');
          this.modalService.dismissAll();
          this.obtenerProductos();
          this.loading = false;
          this.limpiarForm();
        });
      }
    } 
  }

  cargarCategorias(): void {
    this.api.obtenerCategorias().subscribe((response: any[]) => {
      this.categoriasDisponibles = response;
    });
  }

  limpiarForm(): void {
    this.nuevoProducto.nombre = null;
    this.nuevoProducto.descripcion = null;
    this.nuevoProducto.detalles = null;
    this.nuevoProducto.precio = null;
    this.nuevoProducto.stock = null;
    this.nuevoProducto.categoria = null;
    this.nuevoProducto.id_categoria = null;
    this.productoForm.reset();
    if (this.fileInput) {
      this.fileInput.nativeElement.value = '';
    }
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

  actualizarProducto(content: any, producto: any) {
    this.nuevoProducto.id_producto = producto.id_producto;
    this.nuevoProducto.nombre = producto.nombre_producto;
    this.nuevoProducto.descripcion = producto.descripcion;
    this.nuevoProducto.detalles = producto.detalles;
    this.nuevoProducto.precio = producto.precio;
    this.nuevoProducto.stock = producto.stock;
    this.nuevoProducto.categoria = producto.nombre_categoria;
    this.nuevoProducto.image_url = producto.image_url;
    this.nuevoProducto.id_categoria = producto.id_categoria;
    this.actualizar = true;
  
    // Update the form values
    this.productoForm.patchValue({
      nombre: this.nuevoProducto.nombre,
      descripcion: this.nuevoProducto.descripcion,
      detalles: this.nuevoProducto.detalles,
      precio: this.nuevoProducto.precio,
      stock: this.nuevoProducto.stock,
      id_categoria: this.nuevoProducto.id_categoria,
      imagen: '',
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

  mostrarModalConfirmacion(usuario: Producto) {
    this.usuarioAEliminar = usuario;
    this.showConfirmation = true;
  }

  cerrarConfirmacion() {
    this.showConfirmation = false;
  }

  confirmarEliminacion() {
    if (this.usuarioAEliminar && this.usuarioAEliminar.id_producto) {
      this.eliminarProducto(this.usuarioAEliminar.id_producto);
    }
  }

  cancelarEliminacion() {
    this.usuarioAEliminar = undefined;
    this.cerrarConfirmacion();
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];

    // Validate file size
    const fileControl = this.productoForm.get('imagen');
    if (this.selectedFile && this.selectedFile.size > 1024*this.maxFileSizeInKB) {
      fileControl?.setErrors({ fileSizeExceeded: true });
    }
  }

  validateFileExtension(extensions: string[]) {
    return (control: any) => {
      const file = control.value as string;
      if (file) {
        const extension = file.split('.').pop()?.toLowerCase();
        if (!extension || !extensions.includes(extension)) {
          return { invalidExtension: true };
        }
      }
      return null;
    };
  }

}
