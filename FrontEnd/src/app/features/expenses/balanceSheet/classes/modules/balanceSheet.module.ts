import { NgModule } from '@angular/core'
// Custom
import { BalanceSheetCompanyTableComponent } from '../../user-interface/list/balanceSheet-company-table.component'
import { BalanceSheetCriteriaDialogComponent } from '../../user-interface/criteria/balanceSheet-criteria.component'
import { BalanceSheetParentComponent } from '../../user-interface/list/balanceSheet-parent.component'
import { BalanceSheetRoutingModule } from './balanceSheet.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        BalanceSheetCriteriaDialogComponent,
        BalanceSheetParentComponent,
        BalanceSheetCompanyTableComponent
    ],
    imports: [
        SharedModule,
        BalanceSheetRoutingModule
    ]
})

export class BalanceSheetModule { }
