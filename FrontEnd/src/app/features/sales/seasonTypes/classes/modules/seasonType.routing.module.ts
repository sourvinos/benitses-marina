import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { SeasonTypeFormComponent } from '../../user-interface/seasonType-form.component'
import { SeasonTypeFormResolver } from '../resolvers/seasonType-form.resolver'
import { SeasonTypeListComponent } from '../../user-interface/seasonType-list.component'
import { SeasonTypeListResolver } from '../resolvers/seasonType-list.resolver'

const routes: Routes = [
    { path: '', component: SeasonTypeListComponent, canActivate: [AuthGuardService], resolve: { seasonTypeList: SeasonTypeListResolver } },
    { path: 'new', component: SeasonTypeFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: SeasonTypeFormComponent, canActivate: [AuthGuardService], resolve: { seasonTypeForm: SeasonTypeFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class SeasonTypeRoutingModule { }