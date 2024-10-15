import { NgModule } from '@angular/core'
// Custom
import { BerthListComponent } from '../../user-interface/berth-list.component'
import { BerthRoutingModule } from './berth.routing.module'
import { SharedModule } from '../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        BerthListComponent
    ],
    imports: [
        SharedModule,
        BerthRoutingModule
    ]
})

export class BerthModule { }
