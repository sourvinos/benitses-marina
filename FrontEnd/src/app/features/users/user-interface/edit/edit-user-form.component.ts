import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmailQueueDto } from 'src/app/shared/classes/email-queue-dto'
import { EmailQueueHttpService } from 'src/app/shared/services/email-queue-http.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { UpdateUserDto } from '../../classes/dtos/update-user-dto'
import { UserReadDto } from '../../classes/dtos/user-read-dto'
import { UserService } from '../../classes/services/user.service'
import { ValidationService } from '../../../../shared/services/validation.service'

@Component({
    selector: 'edit-user-form',
    templateUrl: './edit-user-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './edit-user-form.component.css']
})

export class EditUserFormComponent {

    //#region common

    private record: UserReadDto
    public feature = 'editUserForm'
    public featureIcon = 'users'
    public form: FormGroup
    public icon = ''
    public input: InputTabStopDirective
    public parentUrl = ''

    //#endregion

    //#region specific

    private mirrorRecord: UserReadDto
    private mustGoBackAfterSave = true

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dexieService: DexieService, private dialogService: DialogService, private emailQueueHttpService: EmailQueueHttpService, private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService, private userService: UserService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.getRecord()
        this.populateFields()
        this.updateReturnUrl()
        this.cloneRecord()
        this.setSidebarsHeight()
    }

    ngAfterViewInit(): void {
        this.focusOnField()
    }

    //#endregion

    //#region public methods

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public onChangePassword(): void {
        if (this.cryptoService.decrypt(this.sessionStorageService.getItem('userId')) != this.record.id.toString()) {
            this.dialogService.open(this.messageDialogService.passwordCanBeChangedOnlyByAccountOwner(), 'error', ['ok'])
        } else {
            if (this.helperService.deepEqual(this.form.value, this.mirrorRecord)) {
                this.router.navigate(['/users/' + this.record.id.toString() + '/changePassword'])
            } else {
                this.mustGoBackAfterSave = false
                this.dialogService.open(this.messageDialogService.formIsDirty(), 'error', ['ok'])
            }
        }
    }

    public onDeleteDexieDB(): void {
        this.dialogService.open(this.messageDialogService.confirmDeleteDexieDB(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.dexieService.delete().then(() => {
                    window.location.reload()
                })
            }
        })
    }

    public onSave(): void {
        this.saveRecord(this.flattenForm())
    }

    public onEmailUserDetais(): void {
        this.userService.getUserFromEmail(this.form.value.email).subscribe({
            next: (x) => {
                if (x.body != '') {
                    this.emailQueueHttpService.save(this.createEmailQueueObject(x)).subscribe(() => {
                        this.helperService.doPostSaveFormTasks(this.messageDialogService.emailSent(), 'ok', this.parentUrl, true)
                    })
                } else {
                    this.helperService.doPostSaveFormTasks(this.messageDialogService.emailSent(), 'ok', this.parentUrl, true)
                }
            }
        })
    }

    //#endregion

    //#region private methods

    private createEmailQueueObject(z: { body: any }): EmailQueueDto {
        return {
            initiator: 'UserDetails',
            entityId: z.body,
            priority: 2,
            isSent: false
        }
    }

    private editUserFromList(): void {
        this.parentUrl = '/users'
        this.icon = 'arrow_back'
    }

    private editUserFromTopMenu(): void {
        this.parentUrl = '/home'
        this.icon = 'home'
    }

    private cloneRecord(): void {
        this.mirrorRecord = this.form.value
    }

    private flattenForm(): UpdateUserDto {
        return {
            id: this.form.value.id,
            username: this.form.value.username,
            displayname: this.form.value.displayname,
            email: this.form.value.email,
            isFirstFieldFocused: this.form.value.isFirstFieldFocused,
            isAdmin: this.form.value.isAdmin,
            isActive: this.form.value.isActive
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getRecord(): Promise<any> {
        return new Promise((resolve) => {
            const formResolved: FormResolved = this.activatedRoute.snapshot.data['userEditForm']
            if (formResolved.error === null) {
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

    private goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            id: '',
            username: ['', [ValidationService.containsIllegalCharacters, Validators.maxLength(32), Validators.required]],
            displayname: ['', [ValidationService.beginsOrEndsWithSpace, Validators.maxLength(32), Validators.required]],
            email: ['', [Validators.email, Validators.maxLength(128), Validators.required]],
            isFirstFieldFocused: false,
            isAdmin: false,
            isActive: true,
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private populateFields(): void {
        this.form.setValue({
            id: this.record.id,
            username: this.record.username,
            displayname: this.record.displayname,
            email: this.record.email,
            isAdmin: this.record.isAdmin,
            isFirstFieldFocused: this.record.isFirstFieldFocused,
            isActive: this.record.isActive,
            postAt: this.record.postAt,
            postUser: this.record.postUser,
            putAt: this.record.putAt,
            putUser: this.record.putUser
        })
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(user: UpdateUserDto): void {
        this.userService.save(user).subscribe({
            complete: () => {
                this.sessionStorageService.saveItem('isFirstFieldFocused', user.isFirstFieldFocused.toString())
                this.mirrorRecord = this.form.value
                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, this.mustGoBackAfterSave)
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    private setSidebarsHeight(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    private updateReturnUrl(): void {
        this.sessionStorageService.getItem('returnUrl') == '/' ? this.editUserFromTopMenu() : this.editUserFromList()
    }

    //#endregion

    //#region getters

    get username(): AbstractControl {
        return this.form.get('username')
    }

    get displayname(): AbstractControl {
        return this.form.get('displayname')
    }

    get email(): AbstractControl {
        return this.form.get('email')
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
