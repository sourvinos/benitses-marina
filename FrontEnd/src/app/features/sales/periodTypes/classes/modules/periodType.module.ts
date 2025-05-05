import { NgModule } from '@angular/core'
// Custom
import { PeriodTypeFormComponent } from '../../user-interface/periodType-form.component'
import { PeriodTypeListComponent } from '../../user-interface/periodType-list.component'
import { PeriodTypeRoutingModule } from './periodType.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        PeriodTypeListComponent,
        PeriodTypeFormComponent
    ],
    imports: [
        SharedModule,
        PeriodTypeRoutingModule
    ]
})

export class PeriodTypeModule { }
