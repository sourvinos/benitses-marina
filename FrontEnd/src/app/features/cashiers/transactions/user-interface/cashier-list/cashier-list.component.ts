import { Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { Table } from 'primeng/table'
import { formatNumber } from '@angular/common'
// Custom
import { CashierCriteriaDialogComponent } from '../cashier-criteria/cashier-criteria.component'
import { CashierHttpService } from '../../classes/services/cashier-http.service'
import { CashierListCriteriaVM } from '../../classes/view-models/criteria/cashier-list-criteria-vm'
import { CashierListVM } from '../../classes/view-models/list/cashier-list-vm'
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MatDialog } from '@angular/material/dialog'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'cashier-list',
    templateUrl: './cashier-list.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', 'cashier-list.component.css']
})

export class CashierListComponent {

    //#region variables

    @ViewChild('table') table: Table

    private criteria: CashierListCriteriaVM
    private url = 'cashiers'
    private virtualElement: any
    public feature = 'cashierList'
    public featureIcon = 'cashiers'
    public icon = 'home'
    public parentUrl = '/home'
    public records: CashierListVM[] = []
    public recordsFilteredCount = 0

    public selectedRecords: CashierListVM[] = []
    public distinctCompanies: SimpleEntity[] = []
    public distinctSafes: SimpleEntity[] = []

    //#endregion

    constructor(private cashierHttpService: CashierHttpService, private cryptoService: CryptoService, private dateHelperService: DateHelperService, private emojiService: EmojiService, private helperService: HelperService, private localStorageService: LocalStorageService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        if (this.getStoredCriteria()) {
            this.buildCriteriaVM(this.criteria).then((response) => {
                this.loadRecords(response).then(() => {
                    this.initFilteredRecordsCount()
                    this.populateDropdownFilters()
                    this.doVirtualTableTasks()
                })
            })
        }
        if (this.getStoredIsTodaysRecords()) {
            this.onGetRecordsForToday().then(() => {
                this.initFilteredRecordsCount()
                this.populateDropdownFilters()
                this.doVirtualTableTasks()
            })
        }
        this.setSidebarsHeight()
    }

    ngAfterViewInit(): void {
        setTimeout(() => {
            this.getVirtualElement()
            this.scrollToSavedPosition()
            this.hightlightSavedRow()
            this.enableDisableFilters()
        }, 500)
    }

    //#endregion

    //#region public methods

    public onClearFilterTasks(): void {
        this.clearFilters()
        this.deleteStoredFilters()
        this.clearSelectedRecords()
        this.initFilteredRecordsCount()
    }

    public onGetRecordsForToday(): Promise<CashierListVM[]> {
        return new Promise((resolve) => {
            this.cashierHttpService.getForToday().subscribe(response => {
                this.records = response
                resolve(this.records)
                this.sessionStorageService.deleteItems([
                    { 'item': 'cashiersListCriteria', 'when': 'always' },
                    { 'item': 'cashiersList-filters', 'when': 'always' }
                ])
                this.sessionStorageService.saveItem('isTodaysRecords', 'true')
            })
        })
    }

    public formatDateToLocale(date: string, showWeekday = false, showYear = false, returnEmptyString = false): string {
        return returnEmptyString && date == '2199-12-31' ? '' : this.dateHelperService.formatISODateToLocale(date, showWeekday, showYear)
    }

    public formatNumberToLocale(number: number, decimals = true): string {
        return formatNumber(number, this.localStorageService.getItem('language'), decimals ? '1.2' : '1.0')
    }

    public getCriteria(): string {
        return this.criteria
            ? this.dateHelperService.formatISODateToLocale(this.criteria.fromDate) + ' - ' + this.dateHelperService.formatISODateToLocale(this.criteria.toDate)
            : ''
    }

    public getEmoji(anything: any): string {
        return typeof anything == 'string'
            ? this.emojiService.getEmoji(anything)
            : anything ? this.emojiService.getEmoji('green-box') : this.emojiService.getEmoji('red-box')
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public onEditRecord(id: string): void {
        this.storeScrollTop()
        this.storeSelectedId(id)
        this.navigateToRecord(id)
    }

    public onFilter(event: any, column: string, matchMode: string): void {
        if (event) this.table.filter(event, column, matchMode)
    }

    public onFilterRecords(event: any): void {
        this.recordsFilteredCount = event.filteredValue.length
    }

    public onHighlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public onNewRecord(): void {
        this.router.navigate([this.url + '/new'])
    }

    public onShowCriteriaDialog(): void {
        const dialogRef = this.dialog.open(CashierCriteriaDialogComponent, {
            data: 'cashierListCriteria',
            height: '36.0625rem',
            panelClass: 'dialog',
            width: '32rem',
        })
        dialogRef.afterClosed().subscribe(criteria => {
            if (criteria !== undefined) {
                this.onClearFilterTasks()
                this.buildCriteriaVM(criteria).then((response) => {
                    this.loadRecords(response).then(() => {
                        this.initFilteredRecordsCount()
                        this.populateDropdownFilters()
                        this.clearSelectedRecords()
                        this.doVirtualTableTasks()
                    })
                })
            }
        })
    }

    public onResetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table, ['date', 'debit', 'credit'])
    }

    //#endregion

    //#region private methods

    private buildCriteriaVM(event: CashierListCriteriaVM): Promise<any> {
        return new Promise((resolve) => {
            this.criteria = {
                fromDate: event.fromDate,
                toDate: event.toDate
            }
            resolve(this.criteria)
        })
    }

    private clearFilters(): void {
        this.table != undefined
            ? this.helperService.clearTableTextFilters(this.table, ['documentNo', 'amount'])
            : null
    }

    private clearSelectedRecords(): void {
        this.selectedRecords = []
    }

    private deleteStoredFilters(): void {
        this.sessionStorageService.deleteItems([{ 'item': 'cashierList-filters', 'when': 'always' }])
    }

    private doVirtualTableTasks(): void {
        setTimeout(() => {
            this.getVirtualElement()
            this.scrollToSavedPosition()
            this.hightlightSavedRow()
        }, 500)
    }

    private enableDisableFilters(): void {
        this.records.length == 0 ? this.helperService.disableTableFilters() : this.helperService.enableTableFilters()
    }

    private getStoredIsTodaysRecords(): boolean {
        const storedCriteria: any = this.sessionStorageService.getItem('isTodaysRecords') ? JSON.parse(this.sessionStorageService.getItem('isTodaysRecords')) : ''
        return storedCriteria
    }

    private getVirtualElement(): void {
        this.virtualElement = document.getElementsByClassName('p-scroller-inline')[0]
    }

    private getStoredCriteria(): boolean {
        const storedCriteria: any = this.sessionStorageService.getItem('cashiersListCriteria') ? JSON.parse(this.sessionStorageService.getItem('cashiersListCriteria')) : ''
        if (storedCriteria) {
            this.criteria = {
                fromDate: storedCriteria.fromDate,
                toDate: storedCriteria.toDate
            }
            return true
        }
    }

    private hightlightSavedRow(): void {
        this.helperService.highlightSavedRow(this.feature)
    }

    private initFilteredRecordsCount(): void {
        this.recordsFilteredCount = this.records.length
    }

    private loadRecords(criteria: CashierListCriteriaVM): Promise<CashierListVM[]> {
        return new Promise((resolve) => {
            this.cashierHttpService.getForList(criteria).subscribe(response => {
                this.records = response
                resolve(this.records)
            })
        })
    }

    private navigateToRecord(id: any): void {
        this.router.navigate([this.url, id])
    }

    private populateDropdownFilters(): void {
        this.distinctCompanies = this.helperService.getDistinctRecords(this.records, 'company', 'description')
        this.distinctSafes = this.helperService.getDistinctRecords(this.records, 'safe', 'description')
    }

    private scrollToSavedPosition(): void {
        this.helperService.scrollToSavedPosition(this.virtualElement, this.feature)
    }

    private setSidebarsHeight(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    private storeSelectedId(id: string): void {
        this.sessionStorageService.saveItem(this.feature + '-id', id)
    }

    private storeScrollTop(): void {
        this.sessionStorageService.saveItem(this.feature + '-scrollTop', this.virtualElement.scrollTop)
    }

    //#endregion

}
