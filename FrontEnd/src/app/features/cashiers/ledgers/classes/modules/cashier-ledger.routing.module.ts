import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { CashierLedgerParentComponent } from '../../user-interface/list/cashier-ledger-parent.component'

const routes: Routes = [
    { path: '', component: CashierLedgerParentComponent, canActivate: [AuthGuardService], runGuardsAndResolvers: 'always' }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class CashierLedgerRoutingModule { }
