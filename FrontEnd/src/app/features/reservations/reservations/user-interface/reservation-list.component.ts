import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { Table } from 'primeng/table'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { ReservationListVM } from '../classes/view-models/reservation-list-vm'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'reservation-list',
    templateUrl: './reservation-list.component.html',
    styleUrls: ['../../../../../assets/styles/custom/lists.css', 'reservation-list.component.css']
})

export class ReservationListComponent {

    //#region variables

    @ViewChild('table') table: Table

    private url = 'reservations'
    private virtualElement: any
    public feature = 'reservationList'
    public featureIcon = 'reservations'
    public icon = 'home'
    public parentUrl = '/home'
    public records: ReservationListVM[] = []
    public recordsFilteredCount = 0
    public filterDate = ''

    public selectedRecords: ReservationListVM[] = []
    public distinctPaymentStatuses: SimpleEntity[] = []

    public occupied = 0
    public dryDock = 0
    public fishingBoats = 0
    public athenian = 0

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateHelperService: DateHelperService, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.loadRecords()
        this.filterOccupied()
        this.filterDryDock()
        this.filterFishingBoats()
        this.filterAthenian()
        this.populateDropdownFilters()
        this.stringifyBerths()
        this.setTabTitle()
        this.doVirtualTableTasks()
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

    public formatDateToLocale(date: string, showWeekday = false, showYear = false, returnEmptyString = false): string {
        return returnEmptyString && date == '2199-12-31' ? '' : this.dateHelperService.formatISODateToLocale(date, showWeekday, showYear)
    }

    public getEmoji(anything: any): string {
        return typeof anything == 'string'
            ? this.emojiService.getEmoji(anything)
            : anything ? this.emojiService.getEmoji('green-box') : this.emojiService.getEmoji('red-box')
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getKnobBackgroundColor(): string {
        return "LightSlateGray"
    }

    public getOverdueDescription(isOverdue: boolean): string {
        return isOverdue ? 'YES' : ''
    }

    public getPaymentDescriptionColor(paymentStatusDescription: string): string {
        switch (paymentStatusDescription) {
            case 'NONE':
                return 'color-red'
            case 'PARTIAL':
                return 'color-orange'
            case 'FULL':
                return 'color-green'
        }
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public onEditRecord(reservationId: string): void {
        this.storeScrollTop()
        this.storeSelectedId(reservationId)
        this.navigateToRecord(reservationId)
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

    public onResetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table, ['description', 'email', 'phones'])
    }

    //#endregion

    //#region private methods

    private doVirtualTableTasks(): void {
        setTimeout(() => {
            this.getVirtualElement()
            this.scrollToSavedPosition()
            this.hightlightSavedRow()
        }, 1000)
    }

    private enableDisableFilters(): void {
        this.records.length == 0 ? this.helperService.disableTableFilters() : this.helperService.enableTableFilters()
    }

    private filterOccupied(): void {
        this.occupied = this.records.filter((x) => x.isDocked).length
    }

    private filterDryDock(): void {
        this.dryDock = this.records.filter((x) => x.isDryDock).length
    }

    private filterFishingBoats(): void {
        this.fishingBoats = this.records.filter((x) => x.isFishingBoat).length
    }

    private filterAthenian(): void {
        this.athenian = this.records.filter((x) => x.isAthenian).length
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

    private stringifyBerths(): void {
        this.records.forEach(record => {
            const joinedBerths = record.berths.map((berth) => {
                return berth.description
            }).join(', ')
            record.joinedBerths = joinedBerths
        })
    }

    private loadRecords(): Promise<any> {
        return new Promise((resolve) => {
            const listResolved: ListResolved = this.activatedRoute.snapshot.data[this.feature]
            if (listResolved.error == null) {
                this.records = listResolved.list
                this.recordsFilteredCount = this.records.length
                resolve(this.records)
            } else {
                this.dialogService.open(this.messageDialogService.filterResponse(listResolved.error), 'error', ['ok']).subscribe(() => {
                    this.goBack()
                })
            }
        })
    }

    private navigateToRecord(id: any): void {
        this.router.navigate([this.url, id])
    }

    private populateDropdownFilters(): void {
        this.distinctPaymentStatuses = this.helperService.getDistinctRecords(this.records, 'paymentStatus', 'description')
    }

    private scrollToSavedPosition(): void {
        this.helperService.scrollToSavedPosition(this.virtualElement, this.feature)
    }

    private setSidebarsHeight(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private storeSelectedId(reservationId: string): void {
        this.sessionStorageService.saveItem(this.feature + '-id', reservationId)
    }

    private storeScrollTop(): void {
        this.sessionStorageService.saveItem(this.feature + '-scrollTop', this.virtualElement.scrollTop)
    }

    //#endregion

}
