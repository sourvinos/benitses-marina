import { NgModule } from '@angular/core'
// Custom
import { LedgerCriteriaDialogComponent } from '../../user-interface/criteria/ledger-criteria.component'
import { LedgerParentBillingComponent } from '../../user-interface/list/ledger-parent.component'
import { LedgerRoutingModule } from './ledger.routing.module'
import { LedgerShipOwnerTableComponent } from '../../user-interface/list/ledger-shipOwner-table.component'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        LedgerCriteriaDialogComponent,
        LedgerParentBillingComponent,
        LedgerShipOwnerTableComponent
    ],
    imports: [
        SharedModule,
        LedgerRoutingModule
    ]
})

export class LedgerModule { }
