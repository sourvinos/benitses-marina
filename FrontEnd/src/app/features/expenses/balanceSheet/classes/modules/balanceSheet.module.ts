import { NgModule } from '@angular/core'
// Custom
import { BalanceSheetCompanyTableComponent } from '../../user-interface/list/balanceSheet-company-table.component'
import { BalanceSheetCriteriaDialogComponent } from '../../user-interface/criteria/balanceSheet-criteria.component'
import { BalanceSheetParentComponent } from '../../user-interface/list/balanceSheet-parent.component'
import { BalanceSheetRoutingModule } from './balanceSheet.routing.module'
import { LedgerFromBalanceSheetComponent } from '../../user-interface/ledger/ledger-table.component'
import { LedgerModalDialogComponent } from '../../user-interface/ledger-modal-dialog/ledger-modal-dialog.component'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        BalanceSheetCompanyTableComponent,
        BalanceSheetCriteriaDialogComponent,
        BalanceSheetParentComponent,
        LedgerModalDialogComponent,
        LedgerFromBalanceSheetComponent
    ],
    imports: [
        SharedModule,
        BalanceSheetRoutingModule
    ]
})

export class BalanceSheetModule { }
