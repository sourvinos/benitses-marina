import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { LeaseRenewalListComponent } from '../../user-interface/lease-renewal-list.component'
import { LeaseRenewalListResolver } from '../resolvers/lease-renewal-list.resolver'

const routes: Routes = [
    { path: '', component: LeaseRenewalListComponent, canActivate: [AuthGuardService], resolve: { leaseRenewalList: LeaseRenewalListResolver }, runGuardsAndResolvers: 'always' }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class LeaseRenewalRoutingModule { }