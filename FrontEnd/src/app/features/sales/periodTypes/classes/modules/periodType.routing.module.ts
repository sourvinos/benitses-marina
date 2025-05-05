import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { PeriodTypeFormComponent } from '../../user-interface/periodType-form.component'
import { PeriodTypeFormResolver } from '../resolvers/periodType-form.resolver'
import { PeriodTypeListComponent } from '../../user-interface/periodType-list.component'
import { PeriodTypeListResolver } from '../resolvers/periodType-list.resolver'

const routes: Routes = [
    { path: '', component: PeriodTypeListComponent, canActivate: [AuthGuardService], resolve: { periodTypeList: PeriodTypeListResolver } },
    { path: 'new', component: PeriodTypeFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: PeriodTypeFormComponent, canActivate: [AuthGuardService], resolve: { periodTypeForm: PeriodTypeFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class PeriodTypeRoutingModule { }