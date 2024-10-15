import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { BerthListComponent } from '../../user-interface/berth-list.component'
import { BerthListResolver } from '../resolvers/berth-list.resolver'

const routes: Routes = [
    { path: '', component: BerthListComponent, canActivate: [AuthGuardService], resolve: { birthList: BerthListResolver } },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class BerthRoutingModule { }