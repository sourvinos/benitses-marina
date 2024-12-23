import { NgModule } from '@angular/core'
// Custom
import { InvoiceRoutingModule } from './invoice.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'
import { InvoiceListComponent } from '../../user-interface/invoice-list.component'
import { InvoiceFormComponent } from '../../user-interface/invoice-form.component'

@NgModule({
    declarations: [
        InvoiceListComponent,
        InvoiceFormComponent
    ],
    imports: [
        SharedModule,
        InvoiceRoutingModule
    ]
})

export class InvoiceModule { }
