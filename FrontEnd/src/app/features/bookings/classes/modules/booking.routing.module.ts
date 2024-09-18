import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { BookingFormComponent } from '../../user-interface/booking-form.component'
import { BookingFormResolver } from '../resolvers/booking-form.resolver'
import { BookingListComponent } from '../../user-interface/booking-list.component'
import { BookingListResolver } from '../resolvers/booking-list.resolver'

const routes: Routes = [
    { path: '', component: BookingListComponent, canActivate: [AuthGuardService], resolve: { bookingList: BookingListResolver } },
    { path: 'new', component: BookingFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: BookingFormComponent, canActivate: [AuthGuardService], resolve: { bookingForm: BookingFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class BookingRoutingModule { }