import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './components/user/login/login.component';
import { HomePageComponent } from './components/user/home-page/home-page.component';
import { RegisterComponent } from './components/user/register/register.component';
import { ProductosComponent } from './components/user/productos/productos.component';
import { ContactoComponent } from './components/user/contacto/contacto.component';
import { SobreNosotrosComponent } from './components/user/sobre-nosotros/sobre-nosotros.component';

import { LoginAdminComponent } from './components/admin/login-admin/login-admin.component';
import { HomePageAdminComponent } from './components/admin/home-page-admin/home-page-admin.component';
import { ProductosAdminComponent } from './components/admin/productos-admin/productos-admin.component';
import { UsuariosAdminComponent } from './components/admin/usuarios-admin/usuarios-admin.component';
import { CategoriasAdminComponent } from './components/admin/categorias-admin/categorias-admin.component';

import { NotFoundComponent } from './components/not-found/not-found.component';
import { InternalServerErrorComponent } from './components/internal-server-error/internal-server-error.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'inicio', component: HomePageComponent},
  { path: 'signup', component: RegisterComponent},
  { path: 'productos', component: ProductosComponent},
  { path: 'contacto', component: ContactoComponent},
  { path: 'sobre-nosotros', component: SobreNosotrosComponent},

  { path: 'admin', component: LoginAdminComponent},
  { path: 'admin/dashboard', component: HomePageAdminComponent, canActivate: [AuthGuard] },
  { path: 'admin/productos', component: ProductosAdminComponent, canActivate: [AuthGuard] },
  { path: 'admin/usuarios', component: UsuariosAdminComponent, canActivate: [AuthGuard] },
  { path: 'admin/categorias', component: CategoriasAdminComponent, canActivate: [AuthGuard] },

  { path: 'internal-server-error', component: InternalServerErrorComponent},
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
