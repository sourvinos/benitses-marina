import { Component, EventEmitter, Output } from '@angular/core'
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { map, Observable, startWith } from 'rxjs'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'sale-form-item',
    templateUrl: './sale-form-item.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css']
})

export class SaleFormItemComponent {

    public itemForm: FormGroup
    public isAutoCompleteDisabled = true
    public dropdownBanks: Observable<SimpleEntity[]>
    @Output() outputItem = new EventEmitter()

    constructor(private formBuilder: FormBuilder, private dexieService: DexieService, private helperService: HelperService, private messageHintService: MessageInputHintService) { }

    ngOnInit(): void {
        this.initForm()
        this.populateDropdowns()
    }

    private initForm(): void {
        this.itemForm = this.formBuilder.group({
            id: 0,
            bank: ['', [Validators.required, ValidationService.requireAutocomplete]],
        })
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response.filter(x => x.isActive)
            this[filteredTable] = this.itemForm.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('banks', 'dropdownBanks', 'bank', 'description', 'description')
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    public enableOrDisableAutoComplete(event: any): void {
        this.isAutoCompleteDisabled = this.helperService.enableOrDisableAutoComplete(event)
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        console.log(event.target.value)
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.itemForm, element, trigger)
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    get bank(): AbstractControl {
        return this.itemForm.get('bank')
    }

}
