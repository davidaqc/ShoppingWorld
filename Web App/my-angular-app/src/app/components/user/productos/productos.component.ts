import { Component, OnInit } from '@angular/core';
import { Producto } from 'src/app/interfaces/admin/producto';
import { AdminService } from 'src/app/services/admin/admin.service'

@Component({
  selector: 'app-productos',
  templateUrl: './productos.component.html',
  styleUrls: ['./productos.component.css']
})
export class ProductosComponent implements OnInit {

  constructor(private api: AdminService) { }

  public productosEnVenta: Producto[] = [];
  public categorias: any[] = [];
  
  ngOnInit(): void {
    // Cargar los productos
    this.obtenerProductos();
  }
  
  getProductosPorCategoria(categoria: string): Producto[] {
      return this.productosEnVenta.filter(p => p.nombre_categoria === categoria);
  }

  obtenerProductos() {
    this.api.obtenerProductos()
      .subscribe(
        (response: any[]) => {
          this.productosEnVenta = response;  
          this.categorias = [...new Set(this.productosEnVenta.map(p => p.nombre_categoria))];
        },
        error => {
          console.error('Error al obtener productos:', error);
          // Manejo de errores si es necesario
        }
      );
  }

}
