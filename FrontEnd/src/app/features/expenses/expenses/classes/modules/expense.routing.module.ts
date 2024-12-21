import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { ExpenseFormComponent } from '../../user-interface/expense-form.component'
import { ExpenseFormResolver } from '../resolvers/expense-form.resolver'
import { ExpenseListComponent } from '../../user-interface/expense-list.component'
import { ExpenseListResolver } from '../resolvers/expense-list.resolver'

const routes: Routes = [
    { path: '', component: ExpenseListComponent, canActivate: [AuthGuardService], resolve: { expenseList: ExpenseListResolver } },
    { path: 'new', component: ExpenseFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: ExpenseFormComponent, canActivate: [AuthGuardService], resolve: { expenseForm: ExpenseFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ExpenseRoutingModule { }