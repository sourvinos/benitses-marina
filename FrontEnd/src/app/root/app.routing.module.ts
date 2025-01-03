// Base
import { NgModule } from '@angular/core'
import { NoPreloading, RouteReuseStrategy, RouterModule, Routes } from '@angular/router'
// Components
import { EmptyPageComponent } from '../shared/components/empty-page/empty-page.component'
import { ForgotPasswordFormComponent } from '../features/users/user-interface/forgot-password/forgot-password-form.component'
import { HomeComponent } from '../shared/components/home/home.component'
import { LoginFormComponent } from '../shared/components/login/user-interface/login-form.component'
import { ResetPasswordFormComponent } from '../features/users/user-interface/reset-password/reset-password-form.component'
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
    { path: 'insurancePolicies', loadChildren: () => import('../features/reservations/insurancePolicies/classes/modules/insurance-policy.module').then(m => m.InsurancePolicyModule) },
    { path: 'upcomingLeaseTerminations', loadChildren: () => import('../features/reservations/leases/classes/modules/upcoming-lease-termination.module').then(m => m.UpcomingLeaseTerminationModule) },
    { path: 'berths', loadChildren: () => import('../features/reservations/berths/classes/modules/berth.module').then(m => m.BerthModule) },
    { path: 'reservations', loadChildren: () => import('../features/reservations/reservations/classes/modules/reservation.module').then(m => m.ReservationModule) },
    { path: 'suppliers', loadChildren: () => import('../features/expenses/suppliers/classes/modules/supplier.module').then(m => m.SupplierModule) },
    { path: 'ledgers', loadChildren: () => import('../features/expenses/ledgers/classes/modules/ledger.module').then(m => m.LedgerModule) },
    { path: 'balanceSheet', loadChildren: () => import('../features/expenses/balanceSheet/classes/modules/balanceSheet.module').then(m => m.BalanceSheetModule) },
    { path: 'invoices', loadChildren: () => import('../features/expenses/invoices/classes/modules/invoice.module').then(m => m.InvoiceModule) },
    { path: 'users', loadChildren: () => import('../features/users/classes/modules/user.module').then(m => m.UserModule) },
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
