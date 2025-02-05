import { NgModule } from '@angular/core'
// Custom
import { SaleFormComponent } from '../../user-interface/sale-form/sale-form.component'
import { SaleListComponent } from '../../user-interface/sale-list/sale-list.component'
import { SaleRoutingModule } from './sale.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        SaleListComponent,
        SaleFormComponent,
    ],
    imports: [
        SharedModule,
        SaleRoutingModule
    ]
})

export class SaleModule { }
