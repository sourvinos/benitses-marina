import { NgModule } from '@angular/core'
// Custom
import { InvoiceFormComponent } from '../../user-interface/invoice-form/invoice-form.component'
import { InvoiceListComponent } from '../../user-interface/invoice-list/invoice-list.component'
import { InvoiceRoutingModule } from './invoice.routing.module'
import { InvoiceCriteriaDialogComponent } from '../../user-interface/criteria/invoice-criteria.component'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        InvoiceListComponent,
        InvoiceFormComponent,
        InvoiceCriteriaDialogComponent
    ],
    imports: [
        SharedModule,
        InvoiceRoutingModule
    ]
})

export class InvoiceModule { }
