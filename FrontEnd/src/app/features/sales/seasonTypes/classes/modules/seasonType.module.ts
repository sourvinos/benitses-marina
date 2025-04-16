import { NgModule } from '@angular/core'
// Custom
import { SeasonTypeFormComponent } from '../../user-interface/seasonType-form.component'
import { SeasonTypeListComponent } from '../../user-interface/seasonType-list.component'
import { SeasonTypeRoutingModule } from './seasonType.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        SeasonTypeListComponent,
        SeasonTypeFormComponent
    ],
    imports: [
        SharedModule,
        SeasonTypeRoutingModule
    ]
})

export class SeasonTypeModule { }
