import { NgModule } from '@angular/core'
// Custom
import { LedgerCompanyTableComponent } from '../../user-interface/list/ledger-company-table.component'
import { LedgerCriteriaDialogComponent } from '../../user-interface/criteria/ledger-criteria.component'
import { LedgerParentBillingComponent } from '../../user-interface/list/ledger-parent.component'
import { LedgerRoutingModule } from './ledger.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        LedgerCriteriaDialogComponent,
        LedgerParentBillingComponent,
        LedgerCompanyTableComponent
    ],
    imports: [
        SharedModule,
        LedgerRoutingModule
    ]
})

export class LedgerModule { }
