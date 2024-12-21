import { NgModule } from '@angular/core'
// Custom
import { ExpenseRoutingModule } from './expense.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'
import { ExpenseListComponent } from '../../user-interface/expense-list.component'
import { ExpenseFormComponent } from '../../user-interface/expense-form.component'

@NgModule({
    declarations: [
        ExpenseListComponent,
        ExpenseFormComponent
    ],
    imports: [
        SharedModule,
        ExpenseRoutingModule
    ]
})

export class ExpenseModule { }
