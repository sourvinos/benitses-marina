import { NgModule } from '@angular/core'
// Custom
import { AccordionModule } from 'primeng/accordion'
import { ButtonModule } from 'primeng/button'
import { CheckboxModule } from 'primeng/checkbox'
import { ContextMenuModule } from 'primeng/contextmenu'
import { DropdownModule } from 'primeng/dropdown'
import { KnobModule } from 'primeng/knob'
import { MessagesModule } from 'primeng/messages'
import { MultiSelectModule } from 'primeng/multiselect'
import { PanelModule } from 'primeng/panel';
import { RadioButtonModule } from 'primeng/radiobutton'
import { SelectButtonModule } from 'primeng/selectbutton'
import { TableModule } from 'primeng/table'

@NgModule({
    exports: [
        AccordionModule,
        ButtonModule,
        CheckboxModule,
        ContextMenuModule,
        DropdownModule,
        KnobModule,
        MessagesModule,
        MultiSelectModule,
        PanelModule,
        RadioButtonModule,
        SelectButtonModule,
        TableModule
    ]
})

export class PrimeNgModule { }
