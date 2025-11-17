import { Routes } from '@angular/router';
import { Crud } from './crud/crud';
import { Login } from './account/login/login';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'crud', pathMatch: 'full' },
  { path: 'login', component: Login, title: 'Login' },
  { path: 'crud', component: Crud, title: 'Students', canActivate: [authGuard] },
  { path: '**', redirectTo: 'crud' }
];
