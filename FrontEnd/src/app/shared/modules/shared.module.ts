import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { NgModule } from '@angular/core'
import { NgxCurrencyDirective, NgxCurrencyInputMode, provideEnvironmentNgxCurrency } from 'ngx-currency'
import { PrimeNgModule } from './primeng.module'
import { RouterModule } from '@angular/router'
// Custom
import { AbsPipe } from '../pipes/abs.pipe'
import { EmojiDirective } from '../directives/emoji.directive'
import { EmptyPageComponent } from '../components/empty-page/empty-page.component'
import { HomeButtonAndTitleComponent } from '../components/home-button-and-title/home-button-and-title.component'
import { InputMaxLengthDirective } from '../directives/input-maxLength.directive'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { LoadingSpinnerComponent } from '../components/loading-spinner/loading-spinner.component'
import { LogoComponent } from '../components/logo/logo.component'
import { MainFooterComponent } from '../components/home/main-footer.component'
import { MainMenuComponent } from '../components/main-menu/main-menu.component'
import { MaterialModule } from './material.module'
import { MetadataPanelComponent } from '../components/metadata-panel/metadata-panel.component'
import { ModalDialogComponent } from '../components/modal-dialog/modal-dialog.component'
import { PadNumberPipe } from '../pipes/pad-number.pipe'
import { PrettyPrintPipe } from '../pipes/json-pretty.pipe'
import { ReplaceZeroPipe } from '../pipes/replace-zero.pipe'
import { SafeStylePipe } from '../pipes/safe-style.pipe'
import { TableTotalFilteredRecordsComponent } from '../components/table-total-filtered-records/table-total-filtered-records.component'
import { TrimStringPipe } from './../pipes/string-trim.pipe'
import { ThemeSelectorComponent } from '../components/theme-selector/theme-selector.component'

@NgModule({
    declarations: [
        AbsPipe,
        EmojiDirective,
        EmptyPageComponent,
        HomeButtonAndTitleComponent,
        InputMaxLengthDirective,
        InputTabStopDirective,
        LoadingSpinnerComponent,
        LogoComponent,
        MainFooterComponent,
        MainMenuComponent,
        MetadataPanelComponent,
        ModalDialogComponent,
        PadNumberPipe,
        PrettyPrintPipe,
        ReplaceZeroPipe,
        SafeStylePipe,
        ThemeSelectorComponent,
        TableTotalFilteredRecordsComponent,
        TrimStringPipe
    ],
    imports: [
        CommonModule,
        FormsModule,
        MaterialModule,
        NgxCurrencyDirective,
        PrimeNgModule,
        ReactiveFormsModule,
        RouterModule
    ],
    exports: [
        AbsPipe,
        CommonModule,
        EmojiDirective,
        EmptyPageComponent,
        FormsModule,
        HomeButtonAndTitleComponent,
        InputMaxLengthDirective,
        InputTabStopDirective,
        LoadingSpinnerComponent,
        LogoComponent,
        MainFooterComponent,
        MainMenuComponent,
        MaterialModule,
        MetadataPanelComponent,
        NgxCurrencyDirective,
        PadNumberPipe,
        PrettyPrintPipe,
        PrimeNgModule,
        ReactiveFormsModule,
        ReplaceZeroPipe,
        RouterModule,
        RouterModule,
        TableTotalFilteredRecordsComponent,
        ThemeSelectorComponent,
        TrimStringPipe
    ], providers: [
        provideEnvironmentNgxCurrency({
            align: 'right',
            allowNegative: false,
            allowZero: true,
            decimal: '.',
            inputMode: NgxCurrencyInputMode.Natural,
            max: null,
            min: null,
            nullable: true,
            precision: 2,
            prefix: '',
            suffix: '',
            thousands: ','
        })
    ]
})

export class SharedModule { }
