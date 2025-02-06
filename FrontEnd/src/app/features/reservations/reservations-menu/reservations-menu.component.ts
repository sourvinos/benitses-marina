import { Component } from '@angular/core'
import { Router } from '@angular/router'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { Menu } from 'src/app/shared/classes/menu'
import { MessageMenuService } from 'src/app/shared/services/message-menu.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Component({
    selector: 'reservations-menu',
    templateUrl: './reservations-menu.component.html',
    styleUrls: ['./reservations-menu.component.css']
})

export class ReservationsMenuComponent {

    //#region variables

    public menuItems: Menu[] = []

    //#endregion

    constructor(private cryptoService: CryptoService,private messageMenuService: MessageMenuService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.buildMenu()
    }

    //#endregion

    //#region public methods

    public doNavigation(feature: string, featureIsEnabled: boolean): void {
        this.router.navigate(featureIsEnabled == false
            ? (['not-ready-yet'])
            : ([feature]))
    }

    public getLabel(id: string): string {
        return this.messageMenuService.getDescription(this.menuItems, id)
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public isLoggedIn(): boolean {
        return this.sessionStorageService.getItem('userId') ? true : false
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
