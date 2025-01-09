import { NgModule } from '@angular/core'
// Custom
import { SharedModule } from '../../../../../shared/modules/shared.module'
import { StatisticsCompanyTableComponent } from '../../user-interface/list/statistics-company-table.component'
import { StatisticsCriteriaDialogComponent } from '../../user-interface/criteria/statistics-criteria-dialog.component'
import { StatisticsParentComponent } from '../../user-interface/list/statistics-parent.component'
import { StatisticsRoutingModule } from './statistics.routing.module'

@NgModule({
    declarations: [
        StatisticsCriteriaDialogComponent,
        StatisticsParentComponent,
        StatisticsCompanyTableComponent
    ],
    imports: [
        SharedModule,
        StatisticsRoutingModule
    ]
})

export class StatisticsModule { }
