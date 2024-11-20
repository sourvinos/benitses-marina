import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { InsurancePolicyListComponent } from '../../user-interface/insurance-policy-list.component'
import { InsurancePolicyListResolver } from '../resolvers/insurance-policy-list.resolver'

const routes: Routes = [
    { path: '', component: InsurancePolicyListComponent, canActivate: [AuthGuardService], resolve: { insurancePolicyList: InsurancePolicyListResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class InsurancePolicyRoutingModule { }