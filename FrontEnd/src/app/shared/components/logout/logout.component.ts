import { Component } from '@angular/core'
// Custom
import { AccountService } from 'src/app/shared/services/account.service'
import { DialogService } from '../../services/modal-dialog.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { Menu } from '../../classes/menu'
import { MessageDialogService } from '../../services/message-dialog.service'
import { MessageMenuService } from '../../services/message-menu.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Component({
    selector: 'logout',
    templateUrl: './logout.component.html',
    styleUrls: ['./logout.component.css']
})

export class LogoutComponent {

    //#region variables

    public menuItems: Menu[] = []

    //#endregion

    constructor(private accountService: AccountService, private dialogService: DialogService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageMenuService: MessageMenuService, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.buildMenu()
    }

    //#endregion

    //#region public methods

    public getIconColor(): string {
        return this.localStorageService.getItem('theme') == 'light' ? 'black' : 'white'
    }

    public getLabel(id: string): string {
        return this.messageMenuService.getDescription(this.menuItems, id)
    }

    public isLoggedIn(): boolean {
        return this.sessionStorageService.getItem('userId') ? true : false
    }

    public onLogoutRequest(): void {
        this.dialogService.open(this.messageDialogService.confirmLogout(), 'question', ['abort', 'ok']).subscribe(response => {
            response ? this.accountService.logout() : null
        })
    }

    //#endregion

    //#region private methods

    private buildMenu(): void {
        this.messageMenuService.getMessages().then((response) => {
            this.createMenu(response)
        })
    }

    private createMenu(items: Menu[]): void {
        this.menuItems = []
        items.forEach(item => {
            this.menuItems.push(item)
        })
    }

    //#endregion

}
