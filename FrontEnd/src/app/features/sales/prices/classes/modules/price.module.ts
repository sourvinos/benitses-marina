import { NgModule } from '@angular/core'
// Custom
import { PriceFormComponent } from '../../user-interface/price-form/price-form.component'
import { PriceListComponent } from '../../user-interface/price-list/price-list.component'
import { PriceRoutingModule } from './price.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        PriceListComponent,
        PriceFormComponent
    ],
    imports: [
        SharedModule,
        PriceRoutingModule
    ]
})

export class PriceModule { }
