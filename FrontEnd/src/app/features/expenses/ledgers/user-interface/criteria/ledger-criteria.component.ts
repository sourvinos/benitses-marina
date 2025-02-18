import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Component, ElementRef, EventEmitter, NgZone, Output, Renderer2 } from '@angular/core'
import { MatDialogRef } from '@angular/material/dialog'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { Observable, map, startWith } from 'rxjs'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { LedgerCriteriaVM } from './../../classes/view-models/criteria/ledger-criteria-vm'
import { LedgerFormCriteriaVM } from './../../classes/view-models/criteria/ledger-form-criteria-vm'
import { MessageInputHintService } from './../../../../../shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'ledger-criteria',
    templateUrl: './ledger-criteria.component.html',
    styleUrls: ['./ledger-criteria.component.css']
})

export class LedgerCriteriaDialogComponent {

    //#region variables

    @Output() outputSelected = new EventEmitter()

    public feature = 'ledgerCriteria'
    public featureIcon = 'ledgers'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/home'

    //#endregion

    //#region autocompletes

    public isAutoCompleteDisabled = true
    public dropdownCompanies: Observable<SimpleEntity[]>
    public dropdownSuppliers: Observable<SimpleEntity[]>

    //#endregion

    constructor(private dateHelperService: DateHelperService, private dexieService: DexieService, private dialogRef: MatDialogRef<LedgerCriteriaDialogComponent>, private elementRef: ElementRef, private formBuilder: FormBuilder, private helperService: HelperService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private ngZone: NgZone, private renderer: Renderer2, private sessionStorageService: SessionStorageService,) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateFormFromStoredFields(this.getCriteriaFromStorage())
        this.populateDropdowns()
        this.addTabIndexToInput()
    }

    //#endregion

    //#region public methods

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    public enableOrDisableAutoComplete(event: any): void {
        this.isAutoCompleteDisabled = this.helperService.enableOrDisableAutoComplete(event)
    }

    public getDateRange(): any[] {
        const x = []
        x.push(this.form.value.fromDate)
        x.push(this.form.value.toDate)
        return x
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onClose(): void {
        this.dialogRef.close()
    }

    public onSearch(): void {
        this.ngZone.run(() => {
            this.sessionStorageService.saveItem(this.feature, JSON.stringify(this.form.value))
            this.dialogRef.close(this.createCriteriaObject(this.form.value))
        })
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    public patchFormWithSelectedDateRange(event: any): void {
        this.form.patchValue({
            fromDate: this.dateHelperService.formatDateToIso(new Date(event.value.fromDate)),
            toDate: this.dateHelperService.formatDateToIso(new Date(event.value.toDate))
        })
    }

    //#endregion

    //#region private methods

    private addTabIndexToInput() {
        this.helperService.addTabIndexToInput(this.elementRef, this.renderer)
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private getCriteriaFromStorage(): any {
        return this.sessionStorageService.getItem(this.feature) ? JSON.parse(this.sessionStorageService.getItem(this.feature)) : ''
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            company: ['', [Validators.required, ValidationService.requireAutocomplete]],
            supplier: ['', [Validators.required, ValidationService.requireAutocomplete]],
            fromDate: ['', [Validators.required]],
            toDate: ['', [Validators.required]],
        })
    }

    private createCriteriaObject(criteria: LedgerFormCriteriaVM): LedgerCriteriaVM {
        return {
            companyId: criteria.company.id,
            supplierId: criteria.supplier.id,
            fromDate: criteria.fromDate,
            toDate: criteria.toDate
        }
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('companiesCriteria', 'dropdownCompanies', 'company', 'description', 'description')
        this.populateDropdownFromDexieDB('suppliersCriteria', 'dropdownSuppliers', 'supplier', 'description', 'description')
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateFormFromStoredFields(object: any): void {
        this.form.patchValue({
            company: object.company,
            supplier: object.supplier,
            fromDate: object.fromDate,
            toDate: object.toDate,
        })
    }

    private updateFormControls(event: any): void {
        this.form.patchValue({
            fromDate: this.dateHelperService.formatDateToIso(new Date(event.value.fromDate)),
            toDate: this.dateHelperService.formatDateToIso(new Date(event.value.toDate))
        })
    }

    //#endregion

    //#region getters

    get company(): AbstractControl {
        return this.form.get('company')
    }

    get supplier(): AbstractControl {
        return this.form.get('supplier')
    }

    //#endregion

}
