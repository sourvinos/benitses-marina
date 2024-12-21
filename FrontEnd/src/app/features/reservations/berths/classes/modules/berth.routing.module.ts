import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { BerthAvailableListComponent } from '../../user-interface/berth-available-list.component'
import { BerthListResolver } from '../resolvers/berth-list.resolver'

const routes: Routes = [
    { path: '', component: BerthAvailableListComponent, canActivate: [AuthGuardService], resolve: { berthAvailableList: BerthListResolver } },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class BerthRoutingModule { }