import { NgModule } from '@angular/core'
// Custom
import { SharedModule } from '../../../../shared/modules/shared.module'
import { UpcomingLeaseTerminationListComponent } from '../../user-interface/upcoming-lease-termination-list.component'
import { UpcomingLeaseTrminationRoutingModule } from './upcoming-lease-termination.routing.module'

@NgModule({
    declarations: [
        UpcomingLeaseTerminationListComponent
    ],
    imports: [
        SharedModule,
        UpcomingLeaseTrminationRoutingModule 
    ]
})

export class UpcomingLeaseTerminationModule { }
