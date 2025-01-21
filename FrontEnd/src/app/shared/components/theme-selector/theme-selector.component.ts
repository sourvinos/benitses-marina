import { Component, Inject } from '@angular/core'
import { DOCUMENT } from '@angular/common'
// Common
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { Menu } from '../../classes/menu'
import { MessageMenuService } from '../../services/message-menu.service'

@Component({
    selector: 'theme-selector',
    templateUrl: './theme-selector.component.html',
    styleUrls: ['./theme-selector.component.css']
})

export class ThemeSelectorComponent {

    //#region variables

    public menuItems: Menu[] = []
    public defaultTheme = 'dark'

    //#endregion

    constructor(@Inject(DOCUMENT) private document: Document, private localStorageService: LocalStorageService, private messageMenuService: MessageMenuService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.buildMenu()
        this.storeDefaultTheme()
        this.setTheme()
        this.attachStylesheetToHead()
    }

    //#endregion

    //#region public methods

    public getIconColor(): string {
        return this.localStorageService.getItem('theme') == 'light' ? 'black' : 'white'
    }

    public getLabel(id: string): string {
        return this.messageMenuService.getDescription(this.menuItems, id)
    }

    public getThemeThumbnail(): string {
        return this.localStorageService.getItem('theme') == '' ? this.defaultTheme : this.localStorageService.getItem('theme')
    }

    public onChangeTheme(): void {
        this.toggleTheme()
        this.storeDefaultTheme()
        this.setTheme()
        this.attachStylesheetToHead()
    }

    //#endregion

    //#region private methods

    private attachStylesheetToHead(): void {
        const headElement = this.document.getElementsByTagName('head')[0]
        const newLinkElement = this.document.createElement('link')
        newLinkElement.rel = 'stylesheet'
        newLinkElement.href = this.defaultTheme + '.css'
        headElement.appendChild(newLinkElement)
    }

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

    private storeDefaultTheme(): void {
        if (this.localStorageService.getItem('theme') == '') {
            this.localStorageService.saveItem('theme', this.defaultTheme)
        }
    }

    private toggleTheme(): void {
        this.localStorageService.saveItem('theme', this.localStorageService.getItem('theme') == 'light' ? 'dark' : 'light')
    }

    private setTheme(): void {
        this.defaultTheme = this.localStorageService.getItem('theme')
    }

    //#endregion

}
