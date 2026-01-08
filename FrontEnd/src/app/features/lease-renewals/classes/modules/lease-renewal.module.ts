import { NgModule } from '@angular/core'
// Custom
import { LeaseRenewalListComponent } from '../../user-interface/lease-renewal-list.component'
import { LeaseRenewalRoutingModule } from './lease-renewal.routing.module'
import { SharedModule } from '../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        LeaseRenewalListComponent
    ],
    imports: [
        SharedModule,
        LeaseRenewalRoutingModule
    ]
})

export class LeaseRenewalModule { }
