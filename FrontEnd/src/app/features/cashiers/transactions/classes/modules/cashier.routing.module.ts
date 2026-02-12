import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { CashierFormComponent } from '../../user-interface/cashier-form/cashier-form.component'
import { CashierFormResolver } from '../resolvers/cashier-form.resolver'
import { CashierListComponent } from '../../user-interface/cashier-list/cashier-list.component'
import { CashierListResolver } from '../resolvers/cashier-list.resolver'

const routes: Routes = [
    { path: '', component: CashierListComponent, canActivate: [AuthGuardService], resolve: { cashierList: CashierListResolver } },
    { path: 'new', component: CashierFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: CashierFormComponent, canActivate: [AuthGuardService], resolve: { cashierForm: CashierFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class CashierRoutingModule { }