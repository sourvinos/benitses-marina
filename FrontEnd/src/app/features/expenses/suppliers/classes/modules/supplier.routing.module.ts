import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { SupplierFormComponent } from '../../user-interface/supplier-form.component'
import { SupplierFormResolver } from '../resolvers/supplier-form.resolver'
import { SupplierListComponent } from '../../user-interface/supplier-list.component'
import { SupplierListResolver } from '../resolvers/supplier-list.resolver'

const routes: Routes = [
    { path: '', component: SupplierListComponent, canActivate: [AuthGuardService], resolve: { supplierList: SupplierListResolver } },
    { path: 'new', component: SupplierFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: SupplierFormComponent, canActivate: [AuthGuardService], resolve: { supplierForm: SupplierFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class SupplierRoutingModule { }