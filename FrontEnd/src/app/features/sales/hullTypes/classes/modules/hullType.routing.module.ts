import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { HullTypeFormComponent } from '../../user-interface/hullType-form.component'
import { HullTypeFormResolver } from '../resolvers/hullType-form.resolver'
import { HullTypeListComponent } from '../../user-interface/hullType-list.component'
import { HullTypeListResolver } from '../resolvers/hullType-list.resolver'

const routes: Routes = [
    { path: '', component: HullTypeListComponent, canActivate: [AuthGuardService], resolve: { hullTypeList: HullTypeListResolver } },
    { path: 'new', component: HullTypeFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: HullTypeFormComponent, canActivate: [AuthGuardService], resolve: { hullTypeForm: HullTypeFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class HullTypeRoutingModule { }