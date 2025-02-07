import { NgModule } from '@angular/core'
// Custom
import { ButtonModule } from 'primeng/button'
import { CheckboxModule } from 'primeng/checkbox'
import { ContextMenuModule } from 'primeng/contextmenu'
import { DropdownModule } from 'primeng/dropdown'
import { KnobModule } from 'primeng/knob'
import { MessagesModule } from 'primeng/messages'
import { MultiSelectModule } from 'primeng/multiselect'
import { SelectButtonModule } from 'primeng/selectbutton'
import { TableModule } from 'primeng/table'

@NgModule({
    exports: [
        ButtonModule,
        CheckboxModule,
        ContextMenuModule,
        DropdownModule,
        KnobModule,
        MessagesModule,
        MultiSelectModule,
        SelectButtonModule,
        TableModule
    ]
})

export class PrimeNgModule { }
