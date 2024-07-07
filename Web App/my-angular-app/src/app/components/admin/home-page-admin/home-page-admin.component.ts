import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/services/admin/admin.service';

@Component({
  selector: 'app-home-page-admin',
  templateUrl: './home-page-admin.component.html',
  styleUrls: ['./home-page-admin.component.css']
})
export class HomePageAdminComponent implements OnInit {

  constructor(private api: AdminService) { }

  public metricas = {
    productosCount: "",
    categoriasCount: "",
    usuariosCount: ""
  }

  ngOnInit(): void {
    this.getUsuariosCount();
    this.getProductosCount();
    this.getCategoriasCount();
  }

  getCategoriasCount(): void {
    this.api.getCategoriasCount()
      .subscribe(
        (response: any) => {
          this.metricas.categoriasCount = response;
        },
        error => {
          console.error('Error al obtener categorías:', error);
          // Manejo de errores si es necesario
        }
      );
  }

  getUsuariosCount(): void {
    this.api.getUsuariosCount()
      .subscribe(
        (response: any) => {
          this.metricas.usuariosCount = response;
        },
        error => {
          console.error('Error al obtener categorías:', error);
          // Manejo de errores si es necesario
        }
      );
  }

  getProductosCount(): void {
    this.api.getProductosCount()
      .subscribe(
        (response: any) => {
          this.metricas.productosCount = response;
        },
        error => {
          console.error('Error al obtener categorías:', error);
          // Manejo de errores si es necesario
        }
      );
  }

}
