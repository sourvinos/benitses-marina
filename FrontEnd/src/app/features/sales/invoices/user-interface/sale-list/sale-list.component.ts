import { Component, ViewChild } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { MenuItem } from 'primeng/api'
import { Router } from '@angular/router'
import { Table } from 'primeng/table'
import { formatNumber } from '@angular/common'
// Custom
import { DateHelperService } from '../../../../../shared/services/date-helper.service'
import { DateRangePickerComponent } from 'src/app/shared/components/date-range-picker/date-range-picker.component'
import { DialogService } from '../../../../../shared/services/modal-dialog.service'
import { EmojiService } from '../../../../../shared/services/emoji.service'
import { HelperService } from '../../../../../shared/services/helper.service'
import { InteractionService } from '../../../../../shared/services/interaction.service'
import { LocalStorageService } from '../../../../../shared/services/local-storage.service'
import { MessageDialogService } from '../../../../../shared/services/message-dialog.service'
import { MessageLabelService } from '../../../../../shared/services/message-label.service'
import { SaleHttpDataService } from '../../classes/services/sale-http-data.service'
import { SaleListCriteriaVM } from '../../classes/view-models/criteria/sale-list-criteria-vm'
import { SaleListVM } from '../../classes/view-models/list/sale-list-vm'
import { SessionStorageService } from '../../../../../shared/services/session-storage.service'

@Component({
    selector: 'sale-list',
    templateUrl: './sale-list.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './sale-list.component.css']
})

export class SaleListComponent {

    //#region variables

    @ViewChild('table') table: Table

    private criteria: SaleListCriteriaVM
    private url = 'sales'
    private virtualElement: any
    public feature = 'saleList'
    public featureIcon = 'sales'
    public icon = 'home'
    public parentUrl = '/home'
    public records: SaleListVM[] = []
    public selectedRecords: SaleListVM[] = []
    public recordsFilteredCount = 0
    public recordsFiltered: SaleListVM[]

    //#endregion

    //#region dropdown filters

    public dropdownDates = []
    public dropdownCustomers = []
    public dropdownDocumentTypes = []

    //#endregion

    //#region context menu

    public menuItems!: MenuItem[]
    public selectedRecord!: SaleListVM

    //#endregion

    constructor(private dateHelperService: DateHelperService, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private interactionService: InteractionService, private saleHttpService: SaleHttpDataService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.setTabTitle()
        this.subscribeToInteractionService()
        this.getStoredCriteria()
        this.buildCriteriaVM(this.criteria).then((response) => {
            this.loadRecords(response).then(() => {
                this.createDateObjects()
                this.initFilteredRecordsCount()
                this.filterTableFromStoredFilters()
                this.populateDropdownFilters()
                this.clearSelectedRecords()
                this.doVirtualTableTasks()
            })
        })
    }

    //#endregion

    //#region public methods

    public editRecord(id: string): void {
        this.storeScrollTop()
        this.storeSelectedId(id)
        this.navigateToRecord(id)
    }

    public onClearFilterTasks(): void {
        this.clearFilters()
        this.deleteStoredFilters()
        this.clearSelectedRecords()
        this.initFilteredRecordsCount()
    }

    public onFilter(event: any, column: string, matchMode: string): void {
        if (event) this.table.filter(event, column, matchMode)
    }

    public onFilterRecords(event: any): void {
        setTimeout(() => {
            this.sessionStorageService.saveItem(this.feature + '-' + 'filters', JSON.stringify(this.table.filters))
            this.recordsFiltered = event.filteredValue
            this.recordsFilteredCount = event.filteredValue.length
        }, 500)
    }

    public onRefreshList(): void {
        this.buildCriteriaVM(this.criteria).then((response) => {
            this.loadRecords(response).then(() => {
                this.createDateObjects()
                this.initFilteredRecordsCount()
                this.filterTableFromStoredFilters()
                this.populateDropdownFilters()
                this.clearSelectedRecords()
                this.doVirtualTableTasks()
            })
        })
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

    public onHighlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public onNewRecord(): void {
        this.router.navigate([this.url + '/new'])
    }

    public addSelectedRecordsToEmailQueue(): void {
        if (this.isAnyRowSelected()) {
            const ids = []
            this.selectedRecords.forEach(record => {
                ids.push(record.invoiceId)
            })
            this.saleHttpService.patchSalesWithEmailPending(ids).subscribe({
                next: () => {
                    this.onRefreshList()
                    this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, false)
                },
                error: (errorFromInterceptor) => {
                    this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                }
            })
        }
    }

    // public buildAndOpenSelectedRecords(): void {
    //     if (this.isAnyRowSelected()) {
    //         const ids = []
    //         this.selectedRecords.forEach(record => {
    //             ids.push(record.invoiceId)
    //         })
    //         this.saleHttpPdfService.buildPdf(ids).subscribe({
    //             next: () => {
    //                 ids.forEach(id => {
    //                     this.saleHttpPdfService.openPdf(id + '.pdf').subscribe({
    //                         next: (response) => {
    //                             const blob = new Blob([response], { type: 'application/pdf' })
    //                             const fileURL = URL.createObjectURL(blob)
    //                             window.open(fileURL, '_blank')
    //                         },
    //                         error: (errorFromInterceptor) => {
    //                             this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
    //                         }
    //                     })
    //                 })
    //             },
    //             error: (errorFromInterceptor) => {
    //                 this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
    //             }
    //         })
    //     }
    // }

    public onShowCriteriaDialog(): void {
        const dialogRef = this.dialog.open(DateRangePickerComponent, {
            data: 'saleListCriteria',
            height: '36.0625rem',
            panelClass: 'dialog',
            width: '32rem',
        })
        dialogRef.afterClosed().subscribe(criteria => {
            if (criteria !== undefined) {
                this.onClearFilterTasks()
                this.buildCriteriaVM(criteria).then((response) => {
                    this.loadRecords(response).then(() => {
                        this.createDateObjects()
                        this.initFilteredRecordsCount()
                        this.filterTableFromStoredFilters()
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

    private addSelectedRecordToSelectedRecords(record: SaleListVM): void {
        this.selectedRecords = []
        this.selectedRecords.push(record)
    }

    private buildCriteriaVM(event: SaleListCriteriaVM): Promise<any> {
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
            ? this.helperService.clearTableTextFilters(this.table, ['saleNo', 'grossAmount'])
            : null
    }

    private clearSelectedRecords(): void {
        this.selectedRecords = []
    }

    private createDateObjects(): void {
        this.records.forEach(record => {
            record.date = {
                id: this.dateHelperService.convertIsoDateToUnixTime(record.date.toString()),
                description: this.formatDateToLocale(record.date.toString()),
                isActive: true
            }
        })
    }

    private deleteStoredFilters(): void {
        this.sessionStorageService.deleteItems([{ 'item': 'saleList-filters', 'when': 'always' }])
    }

    private doVirtualTableTasks(): void {
        setTimeout(() => {
            this.getVirtualElement()
            this.scrollToSavedPosition()
            this.hightlightSavedRow()
        }, 1000)
    }

    private filterColumn(element: any, field: string, matchMode: string): void {
        if (element != undefined && (element.value != null || element.value != undefined)) {
            this.table.filter(element.value, field, matchMode)
        }
    }

    private filterTableFromStoredFilters(): void {
        const filters = this.sessionStorageService.getFilters(this.feature + '-' + 'filters')
        if (filters) {
            setTimeout(() => {
                this.filterColumn(filters.date, 'date', 'in')
                this.filterColumn(filters.customer, 'customer', 'in')
                this.filterColumn(filters.destination, 'destination', 'in')
                this.filterColumn(filters.ship, 'ship', 'in')
                this.filterColumn(filters.shipOwner, 'shipOwner', 'in')
                this.filterColumn(filters.documentType, 'documentType', 'in')
                this.filterColumn(filters.saleNo, 'saleNo', 'contains')
                this.filterColumn(filters.grossAmount, 'grossAmount', 'contains')
            }, 1000)
        }
    }

    private formatDateToLocale(date: string): string {
        return this.dateHelperService.formatISODateToLocale(date)
    }

    private getStoredCriteria(): void {
        const storedCriteria: any = this.sessionStorageService.getItem('saleListCriteria') ? JSON.parse(this.sessionStorageService.getItem('saleListCriteria')) : ''
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

    private loadRecords(criteria: SaleListCriteriaVM): Promise<SaleListVM[]> {
        return new Promise((resolve) => {
            this.saleHttpService.getForList(criteria).subscribe(response => {
                this.records = response
                resolve(this.records)
            })
        })
    }

    private navigateToRecord(id: any): void {
        this.router.navigate([this.url, id])
    }

    private populateDropdownFilters(): void {
        this.dropdownDates = this.helperService.getDistinctRecords(this.records, 'date', 'description')
        this.dropdownCustomers = this.helperService.getDistinctRecords(this.records, 'customer', 'description')
        this.dropdownDocumentTypes = this.helperService.getDistinctRecords(this.records, 'documentType', 'description')
    }

    private scrollToSavedPosition(): void {
        this.helperService.scrollToSavedPosition(this.virtualElement, this.feature)
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private storeSelectedId(id: string): void {
        this.sessionStorageService.saveItem(this.feature + '-id', id.toString())
    }

    private storeScrollTop(): void {
        this.sessionStorageService.saveItem(this.feature + '-scrollTop', this.virtualElement.scrollTop)
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshTabTitle.subscribe(() => {
            this.setTabTitle()
        })
        this.interactionService.emitDateRange.subscribe((response) => {
            if (response) {
                this.criteria.fromDate = response.fromDate
                this.criteria.toDate = response.toDate
            }
        })
    }

    //#endregion

}
