import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { JwtInterceptor } from './interceptors/jwt.interceptor';

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
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ConfirmationModalComponent } from './components/confirmation-modal/confirmation-modal.component';
import { InternalServerErrorComponent } from './components/internal-server-error/internal-server-error.component';
import { SpinnerComponent } from './components/spinner/spinner.component';

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
    CategoriasAdminComponent,
    ConfirmationModalComponent,
    InternalServerErrorComponent,
    SpinnerComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatSnackBarModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
