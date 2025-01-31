import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { SaleFormComponent } from '../../user-interface/sale-form/sale-form.component'
import { SaleFormResolver } from '../resolvers/sale-form.resolver'
import { SaleListComponent } from '../../user-interface/sale-list/sale-list.component'
import { SaleListResolver } from '../resolvers/sale-list.resolver'

const routes: Routes = [
    { path: '', component: SaleListComponent, canActivate: [AuthGuardService], resolve: { saleList: SaleListResolver }, runGuardsAndResolvers: 'always' },
    { path: 'new', component: SaleFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: SaleFormComponent, canActivate: [AuthGuardService], resolve: { saleForm: SaleFormResolver } },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class SaleRoutingModule { }
