import { NgModule } from '@angular/core'
// Custom
import { SafeFormComponent } from '../../user-interface/safe-form.component'
import { SafeListComponent } from '../../user-interface/safe-list.component'
import { SafeRoutingModule } from './safe.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        SafeListComponent,
        SafeFormComponent
    ],
    imports: [
        SharedModule,
        SafeRoutingModule
    ]
})

export class SafeModule { }
