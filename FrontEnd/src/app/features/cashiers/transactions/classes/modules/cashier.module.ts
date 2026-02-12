import { NgModule } from '@angular/core'
// Custom
import { CashierCriteriaDialogComponent } from '../../user-interface/cashier-criteria/cashier-criteria.component'
import { CashierFormComponent } from '../../user-interface/cashier-form/cashier-form.component'
import { CashierListComponent } from '../../user-interface/cashier-list/cashier-list.component'
import { CashierRoutingModule } from './cashier.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        CashierListComponent,
        CashierFormComponent,
        CashierCriteriaDialogComponent
    ],
    imports: [
        SharedModule,
        CashierRoutingModule
    ]
})

export class CashierModule { }
