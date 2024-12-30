import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SupplierHttpService } from '../classes/services/supplier-http.service'
import { SupplierReadDto } from '../classes/dtos/supplier-read-dto'
import { SupplierWriteDto } from '../classes/dtos/supplier-write-dto'
import { ValidationService } from 'src/app/shared/services/validation.service'
import { map, Observable, startWith } from 'rxjs'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'

@Component({
    selector: 'supplier-form',
    templateUrl: './supplier-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './supplier-form.component.css']
})

export class SupplierFormComponent {

    //#region common

    private record: SupplierReadDto
    private recordId: number
    public feature = 'supplierForm'
    public featureIcon = 'suppliers'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/suppliers'

    //#endregion

    //#region autocompletes

    public dropdownBanks: Observable<SimpleEntity[]>
    public isAutoCompleteDisabled = true

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private supplierHttpService: SupplierHttpService, private dexieService: DexieService, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
    }

    ngAfterViewInit(): void {
        this.focusOnField()
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


    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getRemarksLength(): any {
        return this.form.value.remarks != null ? this.form.value.remarks.length : 0
    }

    public onDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.supplierHttpService.delete(this.form.value.id).subscribe({
                    complete: () => {
                        this.dexieService.remove('suppliers', this.form.value.id)
                        this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
                    },
                    error: (errorFromInterceptor) => {
                        this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                    }
                })
            }
        })
    }

    public onSave(): void {
        this.saveRecord(this.flattenForm())
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    //#endregion

    //#region private methods

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): SupplierWriteDto {
        return {
            id: this.form.value.id,
            bankId: this.form.value.bank.id,
            iban: this.form.value.iban,
            description: this.form.value.description,
            longDescription: this.form.value.longDescription,
            vatNumber: this.form.value.vatNumber,
            phones: this.form.value.phones,
            email: this.form.value.email,
            remarks: this.form.value.remarks,
            isActive: this.form.value.isActive,
            putAt: this.form.value.putAt
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getRecord(): Promise<any> {
        if (this.recordId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['supplierForm']
                if (formResolved.error == null) {
                    this.record = formResolved.record.body
                    resolve(this.record)
                } else {
                    this.dialogService.open(this.messageDialogService.filterResponse(formResolved.error), 'error', ['ok']).subscribe(() => {
                        this.resetForm()
                        this.goBack()
                    })
                }
            })
        }
    }

    private goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            id: 0,
            bank: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            iban: [''],
            description: ['', [Validators.required, Validators.maxLength(128)]],
            longDescription: ['', [Validators.maxLength(128)]],
            vatNumber: ['', [Validators.required, Validators.maxLength(36)]],
            phones: ['', [Validators.maxLength(128)]],
            email: ['', [Validators.maxLength(128)]],
            remarks: ['', Validators.maxLength(2048)],
            isActive: true,
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.recordId == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('banks', 'dropdownBanks', 'bank', 'description', 'description')
    }

    private populateFields(): void {
        if (this.record != undefined) {
            this.form.setValue({
                id: this.record.id,
                bank: { 'id': this.record.bank.id, 'description': this.record.bank.description },
                iban: this.record.iban,
                description: this.record.description,
                longDescription: this.record.longDescription,
                vatNumber: this.record.vatNumber,
                phones: this.record.phones,
                email: this.record.email,
                remarks: this.record.remarks,
                isActive: this.record.isActive,
                postAt: this.record.postAt,
                postUser: this.record.postUser,
                putAt: this.record.putAt,
                putUser: this.record.putUser
            })
        }
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(supplier: SupplierWriteDto): void {
        this.supplierHttpService.save(supplier).subscribe({
            next: (response) => {
                this.dexieService.update('suppliers', {
                    'id': parseInt(response.body.id),
                    'description': response.body.description,
                    'email': response.body.email,
                    'vatPercent': response.body.vatPercent,
                    'isActive': response.body.isActive
                })
                this.helperService.doPostSaveFormTasks(
                    response.code == 200 ? this.messageDialogService.success() : this.messageDialogService.supplierVatNumberIsDuplicate(),
                    response.code == 200 ? 'ok' : 'ok',
                    this.parentUrl,
                    true)
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    private setRecordId(): void {
        this.activatedRoute.params.subscribe(x => {
            this.recordId = x.id
        })
    }

    //#endregion

    //#region getters

    get description(): AbstractControl {
        return this.form.get('description')
    }

    get longDescription(): AbstractControl {
        return this.form.get('longDescription')
    }

    get bank(): AbstractControl {
        return this.form.get('bank')
    }

    get iban(): AbstractControl {
        return this.form.get('iban')
    }

    get vatNumber(): AbstractControl {
        return this.form.get('vatNumber')
    }

    get phones(): AbstractControl {
        return this.form.get('phones')
    }

    get email(): AbstractControl {
        return this.form.get('email')
    }

    get remarks(): AbstractControl {
        return this.form.get('remarks')
    }

    get postAt(): AbstractControl {
        return this.form.get('postAt')
    }

    get postUser(): AbstractControl {
        return this.form.get('postUser')
    }

    get putAt(): AbstractControl {
        return this.form.get('putAt')
    }

    get putUser(): AbstractControl {
        return this.form.get('putUser')
    }

    //#endregion

}
