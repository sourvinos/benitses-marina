import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { UpcomingLeaseTerminationListComponent } from '../../user-interface/upcoming-lease-termination-list.component'
import { UpcomingLeaseTerminationListResolver } from '../resolvers/upcoming-lease-termination-list.resolver'

const routes: Routes = [
    { path: '', component: UpcomingLeaseTerminationListComponent, canActivate: [AuthGuardService], resolve: { upcomingLeasesList: UpcomingLeaseTerminationListResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class UpcomingLeaseTrminationRoutingModule { }