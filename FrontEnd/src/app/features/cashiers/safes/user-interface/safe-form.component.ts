import { ActivatedRoute, Router } from '@angular/router'
import { Component, ElementRef, Renderer2 } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { map, Observable, startWith } from 'rxjs'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SafeHttpService } from '../classes/services/safe-http.service'
import { SafeReadDto } from '../classes/dtos/safe-read-dto'
import { SafeWriteDto } from '../classes/dtos/safe-write-dto'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'safe-form',
    templateUrl: './safe-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './safe-form.component.css']
})

export class SafeFormComponent {

    //#region common

    private record: SafeReadDto
    private recordId: number
    public feature = 'safeForm'
    public featureIcon = 'safes'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/safes'

    //#endregion

    //#region autocompletes

    public dropdownSafes: Observable<SimpleEntity[]>
    public isAutoCompleteDisabled = true

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dexieService: DexieService, private dialogService: DialogService, private elementRef: ElementRef, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private renderer: Renderer2, private router: Router, private sessionStorageService: SessionStorageService, private safeHttpService: SafeHttpService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
    }

    ngAfterViewInit(): void {
        this.addTabIndexToInput()
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

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public onDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.safeHttpService.delete(this.form.value.id).subscribe({
                    complete: () => {
                        this.dexieService.remove('safes', this.form.value.id)
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

    private addTabIndexToInput(): void {
        this.helperService.addTabIndexToInput(this.elementRef, this.renderer)
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): SafeWriteDto {
        return {
            id: this.form.value.id,
            description: this.form.value.description,
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
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['safeForm']
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
            description: ['', [Validators.required, Validators.maxLength(128)]],
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
        this.populateDropdownFromDexieDB('safes', 'dropdownSafes', 'safe', 'description', 'description')
    }

    private populateFields(): void {
        if (this.record != undefined) {
            this.form.setValue({
                id: this.record.id,
                description: this.record.description,
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

    private saveRecord(safe: SafeWriteDto): void {
        this.safeHttpService.save(safe).subscribe({
            next: (response) => {
                this.dexieService.update('safes', {
                    'id': parseInt(response.body.id),
                    'description': response.body.description,
                    'email': response.body.email,
                    'vatPercent': response.body.vatPercent,
                    'isActive': response.body.isActive
                })
                this.helperService.doPostSaveFormTasks(
                    this.messageDialogService.success(),
                    'ok',
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
