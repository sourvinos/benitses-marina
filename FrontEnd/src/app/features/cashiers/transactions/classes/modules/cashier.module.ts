import { NgModule } from '@angular/core'
// Custom
import { CashierFormComponent } from '../../user-interface/cashier-form.component'
import { CashierListComponent } from '../../user-interface/cashier-list.component'
import { CashierRoutingModule } from './cashier.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        CashierListComponent,
        CashierFormComponent
    ],
    imports: [
        SharedModule,
        CashierRoutingModule
    ]
})

export class CashierModule { }
