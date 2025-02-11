import { Component } from '@angular/core'
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { map, Observable, startWith } from 'rxjs'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'hello',
    templateUrl: './sale-form-item.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css']
})

export class SaleFormItemComponent {

    //#region variables

    public form: FormGroup
    public dropdownBanks: Observable<SimpleEntity[]>
    public isAutoCompleteDisabled = true

    //#endregion

    constructor(private dexieService: DexieService, private formBuilder: FormBuilder, private helperService: HelperService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.addLesson()
    }

    //#endregion

    //#region public methods

    public addLesson() {
        const lessonForm = this.formBuilder.group({
            bank: ['', [Validators.required, ValidationService.requireAutocomplete]],
            level: ['', Validators.required]
        });
        this.populateDropdowns()
        this.lessons.push(lessonForm);
    }

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public onShowFormValue(): void {
        // this.debugDialogService.open(this.fg.value, '', ['ok'])
    }

    public enableOrDisableAutoComplete(event: any): void {
        this.isAutoCompleteDisabled = this.helperService.enableOrDisableAutoComplete(event)
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        console.log(event.target.value)
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    //#endregion

    //#region private methods

    private initForm(): void {
        this.form = this.formBuilder.group({
            lessons: this.formBuilder.array([])
        })
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response.filter(x => x.isActive)
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdowns(): void {
        console.log('populating dropdown')
        this.populateDropdownFromDexieDB('banks', 'dropdownBanks', 'bank', 'description', 'description')
    }

    //#endregion

    //#region getters

    get lessons() {
        return this.form.controls["lessons"] as FormArray;
    }

    //#endregion

}