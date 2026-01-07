import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { Table } from 'primeng/table'
import { formatNumber } from '@angular/common'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InvoiceListExportService } from '../../classes/services/invoice-list-export.service'
import { InvoiceListVM } from '../../classes/view-models/list/invoice-list-vm'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { MatDialog } from '@angular/material/dialog'
import { InvoiceCriteriaDialogComponent } from '../criteria/invoice-criteria.component'
import { InvoiceListCriteriaVM } from '../../classes/view-models/criteria/invoice-list-criteria-vm'
import { InvoiceHttpService } from '../../classes/services/invoice-http.service'

@Component({
    selector: 'invoice-list',
    templateUrl: './invoice-list.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', 'invoice-list.component.css']
})

export class InvoiceListComponent {

    //#region variables

    @ViewChild('table') table: Table

    private criteria: InvoiceListCriteriaVM
    private url = 'invoices'
    private virtualElement: any
    public feature = 'invoiceList'
    public featureIcon = 'invoices'
    public icon = 'home'
    public parentUrl = '/home'
    public records: InvoiceListVM[] = []
    public recordsFilteredCount = 0

    public selectedRecords: InvoiceListVM[] = []
    public distinctCompanies: SimpleEntity[] = []
    public distinctDocumentTypes: SimpleEntity[] = []
    public distinctPaymentMethods: SimpleEntity[] = []
    public distinctSuppliers: SimpleEntity[] = []

    //#endregion

    constructor(private invoiceHttpService: InvoiceHttpService, private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateHelperService: DateHelperService, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private invoiceListExportService: InvoiceListExportService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.getStoredCriteria()
        this.buildCriteriaVM(this.criteria).then((response) => {
            this.loadRecords(response).then(() => {
                // this.createDateObjects()
                this.initFilteredRecordsCount()
                // this.filterTableFromStoredFilters()
                this.populateDropdownFilters()
                // this.clearSelectedRecords()
                this.doVirtualTableTasks()
            })
        })
        // this.loadRecords()
        // this.populateDropdownFilters()
        // this.doVirtualTableTasks()
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

    public exportSelected(): void {
        if (this.isAnyRowSelected()) {
            this.invoiceListExportService.exportToExcel(this.invoiceListExportService.buildList(this.selectedRecords))
        }
    }

    public onFilterTodayRecords(): void {
        this.helperService.clearTableTextFilters(this.table, ['date', 'no', 'amount'])
        this.table.filter(this.dateHelperService.formatDateToIso(new Date()), 'putAt', 'equals')
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

    public onEditRecord(invoiceId: string): void {
        this.storeScrollTop()
        this.storeSelectedId(invoiceId)
        this.navigateToRecord(invoiceId)
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
        this.router.navigate([this.url, 'new'])
    }

    public onResetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table, ['date', 'no', 'amount'])
    }

    public onShowCriteriaDialog(): void {
        const dialogRef = this.dialog.open(InvoiceCriteriaDialogComponent, {
            data: 'invoiceListCriteria',
            height: '36.0625rem',
            panelClass: 'dialog',
            width: '32rem',
        })
        dialogRef.afterClosed().subscribe(criteria => {
            if (criteria !== undefined) {
                this.onClearFilterTasks()
                this.buildCriteriaVM(criteria).then((response) => {
                    this.loadRecords(response).then(() => {
                        // this.createDateObjects()
                        this.initFilteredRecordsCount()
                        this.populateDropdownFilters()
                        this.clearSelectedRecords()
                        this.doVirtualTableTasks()
                    })
                })
            }
        })
    }

    //#endregion

    //#region private methods

    private buildCriteriaVM(event: InvoiceListCriteriaVM): Promise<any> {
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

    // private createDateObjects(): void {
    //     this.records.forEach(record => {
    //         record.date = {
    //             id: this.dateHelperService.convertIsoDateToUnixTime(record.date.toString()),
    //             description: this.formatDateToLocale(record.date.toString()),
    //             isActive: true
    //         }
    //     })
    // }

    private deleteStoredFilters(): void {
        this.sessionStorageService.deleteItems([{ 'item': 'invoiceList-filters', 'when': 'always' }])
    }

    private doVirtualTableTasks(): void {
        setTimeout(() => {
            this.getVirtualElement()
            this.scrollToSavedPosition()
            this.hightlightSavedRow()
        }, 500)
    }

    private enableDisableFilters(): void {
        // this.records.length == 0 ? this.helperService.disableTableFilters() : this.helperService.enableTableFilters()
    }

    private getStoredCriteria(): void {
        const storedCriteria: any = this.sessionStorageService.getItem('invoicesListCriteria') ? JSON.parse(this.sessionStorageService.getItem('invoicesListCriteria')) : ''
        if (storedCriteria) {
            this.criteria = {
                fromDate: storedCriteria.fromDate,
                toDate: storedCriteria.toDate
            }
        } else {
            this.criteria = {
                fromDate: this.dateHelperService.formatDateToIso(new Date()),
                toDate: this.dateHelperService.formatDateToIso(new Date())
            }
        }
    }

    private getVirtualElement(): void {
        this.virtualElement = document.getElementsByClassName('p-scroller-inline')[0]
    }

    private goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    private hightlightSavedRow(): void {
        this.helperService.highlightSavedRow(this.feature)
    }

    private initFilteredRecordsCount(): void {
        this.recordsFilteredCount = this.records.length
    }

    private isAnyRowSelected(): boolean {
        if (this.selectedRecords.length == 0) {
            this.dialogService.open(this.messageDialogService.noRecordsSelected(), 'error', ['ok'])
            return false
        }
        return true
    }

    private loadRecords(criteria: InvoiceListCriteriaVM): Promise<InvoiceListVM[]> {
        return new Promise((resolve) => {
            this.invoiceHttpService.getForList(criteria).subscribe(response => {
                this.records = response
                resolve(this.records)
            })
        })
    }

    // private loadRecords(): Promise<any> {
    //     return new Promise((resolve) => {
    //         const listResolved: ListResolved = this.activatedRoute.snapshot.data[this.feature]
    //         if (listResolved.error == null) {
    //             this.records = listResolved.list
    //             this.recordsFilteredCount = this.records.length
    //             resolve(this.records)
    //         } else {
    //             this.dialogService.open(this.messageDialogService.filterResponse(listResolved.error), 'error', ['ok']).subscribe(() => {
    //                 this.goBack()
    //             })
    //         }
    //     })
    // }

    private navigateToRecord(id: any): void {
        this.router.navigate([this.url, id])
    }

    public onClearFilterTasks(): void {
        this.clearFilters()
        this.deleteStoredFilters()
        this.clearSelectedRecords()
        this.initFilteredRecordsCount()
    }

    private populateDropdownFilters(): void {
        this.distinctCompanies = this.helperService.getDistinctRecords(this.records, 'company', 'description')
        this.distinctDocumentTypes = this.helperService.getDistinctRecords(this.records, 'documentType', 'description')
        this.distinctPaymentMethods = this.helperService.getDistinctRecords(this.records, 'paymentMethod', 'description')
        this.distinctSuppliers = this.helperService.getDistinctRecords(this.records, 'supplier', 'description')
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
