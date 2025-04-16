import { NgModule } from '@angular/core'
// Custom
import { HullTypeFormComponent } from '../../user-interface/hullType-form.component'
import { HullTypeListComponent } from '../../user-interface/hullType-list.component'
import { HullTypeRoutingModule } from './hullType.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        HullTypeListComponent,
        HullTypeFormComponent
    ],
    imports: [
        SharedModule,
        HullTypeRoutingModule
    ]
})

export class HullTypeModule { }
