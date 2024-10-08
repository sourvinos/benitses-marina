import { NgModule } from '@angular/core'
// Custom
import { ButtonModule } from 'primeng/button'
import { CheckboxModule } from 'primeng/checkbox'
import { ContextMenuModule } from 'primeng/contextmenu'
import { DropdownModule } from 'primeng/dropdown'
import { MultiSelectModule } from 'primeng/multiselect'
import { RadioButtonModule } from 'primeng/radiobutton'
import { TableModule } from 'primeng/table'

@NgModule({
    exports: [
        ButtonModule,
        CheckboxModule,
        ContextMenuModule,
        DropdownModule,
        MultiSelectModule,
        RadioButtonModule,
        TableModule
    ]
})

export class PrimeNgModule { }
