import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { DocumentTypeHelperService } from '../classes/services/documentType-helper.service'
import { DocumentTypeHttpService } from '../classes/services/documentType-http.service'
import { DocumentTypeReadDto } from '../classes/dtos/documentType-read-dto'
import { DocumentTypeWriteDto } from '../classes/dtos/documentType-write-dto'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'documentType-form',
    templateUrl: './documentType-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css']
})

export class DocumentTypeFormComponent {

    //#region common

    private record: DocumentTypeReadDto
    private recordId: string
    public feature = 'saleDocumentTypeForm'
    public featureIcon = 'documentTypes'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/saleDocumentTypes'

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private documentTypeHelperService: DocumentTypeHelperService, private documentTypeHttpService: DocumentTypeHttpService, private dexieService: DexieService, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
    }

    ngAfterViewInit(): void {
        this.focusOnField()
    }

    //#endregion

    //#region public methods

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.documentTypeHttpService.delete(this.form.value.id).subscribe({
                    complete: () => {
                        this.dexieService.remove('documentTypes', this.form.value.id)
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

    //#endregion

    //#region private methods

    private flattenForm(): DocumentTypeWriteDto {
        return {
            id: this.form.value.id != '' ? this.form.value.id : null,
            discriminatorId: parseInt(this.form.value.discriminatorId),
            abbreviation: this.form.value.abbreviation,
            description: this.form.value.description,
            batch: this.form.value.batch,
            customers: this.form.value.customers,
            isDefault: this.form.value.isDefault,
            isMyData: this.form.value.isMyData,
            isStatistic: this.form.value.isStatistic,
            isActive: this.form.value.isActive,
            table8_1: this.form.value.table8_1,
            table8_8: this.form.value.table8_8,
            table8_9: this.form.value.table8_9,
            putAt: this.form.value.putAt
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getRecord(): Promise<any> {
        if (this.recordId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['saleDocumentTypeForm']
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
            id: '',
            discriminatorId: ['', [Validators.required]],
            abbreviation: ['', [Validators.required, Validators.maxLength(5)]],
            description: ['', [Validators.required, Validators.maxLength(128)]],
            batch: ['', [Validators.required, Validators.maxLength(5)]],
            customers: ['', [Validators.maxLength(1), ValidationService.shouldBeEmptyPlusOrMinus]],
            isStatistic: false,
            isMyData: true,
            isDefault: true,
            isActive: true,
            table8_1: ['', [Validators.maxLength(32)]],
            table8_8: ['', [Validators.maxLength(32)]],
            table8_9: ['', [Validators.maxLength(32)]],
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private populateFields(): void {
        if (this.record != undefined) {
            this.form.setValue({
                abbreviation: this.record.abbreviation,
                batch: this.record.batch,
                customers: this.record.customers,
                description: this.record.description,
                discriminatorId: this.record.discriminatorId.toString(),
                id: this.record.id,
                isActive: this.record.isActive,
                isDefault: this.record.isDefault,
                isMyData: this.record.isMyData,
                isStatistic: this.record.isStatistic,
                table8_1: this.record.table8_1,
                table8_8: this.record.table8_8,
                table8_9: this.record.table8_9,
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

    private saveRecord(documentType: DocumentTypeWriteDto): void {
        this.documentTypeHttpService.save(documentType).subscribe({
            next: (response) => {
                this.documentTypeHelperService.updateBrowserStorageAfterApiUpdate('saleDocumentTypes', response.body)
                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
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

    get abbreviation(): AbstractControl {
        return this.form.get('abbreviation')
    }

    get description(): AbstractControl {
        return this.form.get('description')
    }

    get batch(): AbstractControl {
        return this.form.get('batch')
    }

    get customers(): AbstractControl {
        return this.form.get('customers')
    }

    get table8_1(): AbstractControl {
        return this.form.get('table8_1')
    }

    get table8_8(): AbstractControl {
        return this.form.get('table8_8')
    }

    get table8_9(): AbstractControl {
        return this.form.get('table8_9')
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
