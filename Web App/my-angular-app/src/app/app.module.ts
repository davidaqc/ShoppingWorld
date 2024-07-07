import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { LoginAdminComponent } from './components/admin/login-admin/login-admin.component';
import { HomePageAdminComponent } from './components/admin/home-page-admin/home-page-admin.component';
import { NabvarAdminComponent } from './components/admin/nabvar-admin/nabvar-admin.component';
import { ProductosAdminComponent } from './components/admin/productos-admin/productos-admin.component';
import { HomePageComponent } from './components/user/home-page/home-page.component';
import { NabvarComponent } from './components/user/nabvar/nabvar.component';
import { ProductosComponent } from './components/user/productos/productos.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ContactoComponent } from './components/user/contacto/contacto.component';
import { SobreNosotrosComponent } from './components/user/sobre-nosotros/sobre-nosotros.component';
import { UsuariosAdminComponent } from './components/admin/usuarios-admin/usuarios-admin.component';
import { CategoriasAdminComponent } from './components/admin/categorias-admin/categorias-admin.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    LoginAdminComponent,
    HomePageAdminComponent,
    NabvarAdminComponent,
    ProductosAdminComponent,
    HomePageComponent,
    NabvarComponent,
    ProductosComponent,
    NotFoundComponent,
    ContactoComponent,
    SobreNosotrosComponent,
    UsuariosAdminComponent,
    CategoriasAdminComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
