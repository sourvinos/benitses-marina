// Base
import { NgModule } from '@angular/core'
import { NoPreloading, RouteReuseStrategy, RouterModule, Routes } from '@angular/router'
// Components
import { EmptyPageComponent } from '../shared/components/empty-page/empty-page.component'
import { ForgotPasswordFormComponent } from '../features/bookings/users/user-interface/forgot-password/forgot-password-form.component'
import { HomeComponent } from '../shared/components/home/home.component'
import { LoginFormComponent } from '../features/bookings/login/user-interface/login-form.component'
import { ResetPasswordFormComponent } from '../features/bookings/users/user-interface/reset-password/reset-password-form.component'
// Utils
import { AuthGuardService } from '../shared/services/auth-guard.service'
import { CustomRouteReuseStrategyService } from '../shared/services/route-reuse-strategy.service'

const appRoutes: Routes = [
    // Login
    { path: '', component: LoginFormComponent, pathMatch: 'full' },
    // Auth
    { path: 'login', component: LoginFormComponent },
    { path: 'forgotPassword', component: ForgotPasswordFormComponent },
    { path: 'resetPassword', component: ResetPasswordFormComponent },
    // Home
    { path: 'home', component: HomeComponent, canActivate: [AuthGuardService] },
    // Main menu
    { path: 'users', loadChildren: () => import('../features/bookings/users/classes/modules/user.module').then(m => m.UserModule) },
    // Empty
    { path: '**', component: EmptyPageComponent }
]

@NgModule({
    declarations: [],
    exports: [
        RouterModule
    ],
    imports: [
        RouterModule.forRoot(appRoutes, { onSameUrlNavigation: 'reload', preloadingStrategy: NoPreloading, useHash: true })
    ],
    providers: [
        { provide: RouteReuseStrategy, useClass: CustomRouteReuseStrategyService }
    ]
})

export class AppRoutingModule { }
