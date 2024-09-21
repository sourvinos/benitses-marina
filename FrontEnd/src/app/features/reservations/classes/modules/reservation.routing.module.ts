import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { ReservationFormComponent } from '../../user-interface/reservation-form.component'
import { ReservationFormResolver } from '../resolvers/reservation-form.resolver'
import { ReservationListComponent } from '../../user-interface/reservation-list.component'
import { ReservationListResolver } from '../resolvers/reservation-list.resolver'

const routes: Routes = [
    { path: '', component: ReservationListComponent, canActivate: [AuthGuardService], resolve: { reservationList: ReservationListResolver } },
    { path: 'new', component: ReservationFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: ReservationFormComponent, canActivate: [AuthGuardService], resolve: { reservationForm: ReservationFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ReservationRoutingModule { }