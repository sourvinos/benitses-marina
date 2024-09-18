import { NgModule } from '@angular/core'
// Custom
import { BookingFormComponent } from '../../user-interface/booking-form.component'
import { BookingListComponent } from '../../user-interface/booking-list.component'
import { BookingRoutingModule } from './booking.routing.module'
import { SharedModule } from '../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        BookingListComponent,
        BookingFormComponent
    ],
    imports: [
        SharedModule,
        BookingRoutingModule
    ]
})

export class BookingModule { }
