import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { SafeFormComponent } from '../../user-interface/safe-form.component'
import { SafeFormResolver } from '../resolvers/safe-form.resolver'
import { SafeListComponent } from '../../user-interface/safe-list.component'
import { SafeListResolver } from '../resolvers/safe-list.resolver'

const routes: Routes = [
    { path: '', component: SafeListComponent, canActivate: [AuthGuardService], resolve: { safeList: SafeListResolver } },
    { path: 'new', component: SafeFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: SafeFormComponent, canActivate: [AuthGuardService], resolve: { safeForm: SafeFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class SafeRoutingModule { }