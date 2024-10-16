import { NgModule } from '@angular/core'
// Custom
import { BerthAvailableListComponent } from '../../user-interface/berth-available-list.component'
import { BerthRoutingModule } from './berth.routing.module'
import { SharedModule } from '../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        BerthAvailableListComponent
    ],
    imports: [
        SharedModule,
        BerthRoutingModule
    ]
})

export class BerthModule { }
