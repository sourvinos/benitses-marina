import { NgModule } from '@angular/core'
// Custom
import { InsurancePolicyListComponent } from '../../user-interface/insurance-policy-list.component'
import { InsurancePolicyRoutingModule } from './insurance-policy.routing.module'
import { SharedModule } from '../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        InsurancePolicyListComponent
    ],
    imports: [
        SharedModule,
        InsurancePolicyRoutingModule
    ]
})

export class InsurancePolicyModule { }
