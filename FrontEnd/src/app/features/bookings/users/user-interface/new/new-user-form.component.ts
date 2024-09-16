import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Component } from '@angular/core'
// Custom
import { ConfirmValidParentMatcher, ValidationService } from '../../../../../shared/services/validation.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { UserNewDto } from '../../classes/dtos/new-user-dto'
import { UserService } from '../../classes/services/user.service'

@Component({
    selector: 'new-user-form',
    templateUrl: './new-user-form.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css']
})

export class NewUserFormComponent {

    //#region common #6

    public feature = 'newUserForm'
    public featureIcon = 'users'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/users'

    //#endregion

    //#region specific #2

    public confirmValidParentMatcher = new ConfirmValidParentMatcher()
    public hidePassword = true

    //#endregion

    constructor(private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private userService: UserService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
    }

    ngAfterViewInit(): void {
        this.clearInvisibleFieldsAndRestoreVisibility()
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

    public onSave(): void {
        this.saveRecord(this.flattenForm())
    }

    //#endregion

    //#region private methods

    private clearInvisibleFieldsAndRestoreVisibility(): void {
        this.helperService.clearInvisibleFieldsAndRestoreVisibility(this.form, ['email'])
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): UserNewDto {
        return {
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

    private initForm(): void {
        this.form = this.formBuilder.group({
            username: ['', [Validators.required, ValidationService.containsSpace, Validators.maxLength(256)]],
            displayname: ['', [Validators.required, ValidationService.beginsOrEndsWithSpace, Validators.maxLength(32)]],
            email: ['x@x.com', [Validators.email, Validators.maxLength(128), Validators.required]],
            isFirstFieldFocused: false,
            isAdmin: false,
            isActive: true
        })
    }

    private saveRecord(user: UserNewDto): void {
        this.userService.saveUser(user).subscribe({
            complete: () => {
                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
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

    get passwords(): AbstractControl {
        return this.form.get('passwords')
    }

    get password(): AbstractControl {
        return this.form.get('passwords.password')
    }

    get confirmPassword(): AbstractControl {
        return this.form.get('passwords.confirmPassword')
    }

    get matchingPasswords(): boolean {
        return this.form.get('passwords.password').value == this.form.get('passwords.confirmPassword').value
    }

    //#endregion

}
