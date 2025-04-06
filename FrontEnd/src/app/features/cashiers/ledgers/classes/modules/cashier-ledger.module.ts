import { NgModule } from '@angular/core'
// Custom
import { CashierLedgerCompanyTableComponent } from '../../user-interface/list/cashier-ledger-company-table.component'
import { CashierLedgerCriteriaDialogComponent } from '../../user-interface/criteria/cashier-ledger-criteria.component'
import { CashierLedgerParentComponent } from '../../user-interface/list/cashier-ledger-parent.component'
import { CashierLedgerRoutingModule } from './cashier-ledger.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        CashierLedgerCriteriaDialogComponent,
        CashierLedgerParentComponent,
        CashierLedgerCompanyTableComponent
    ],
    imports: [
        SharedModule,
        CashierLedgerRoutingModule
    ]
})

export class CashierLedgerModule { }
