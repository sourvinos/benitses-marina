import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { FormBuilder, FormControl, FormGroup } from '@angular/forms'
import { MenuItem } from 'primeng/api'
import { Table } from 'primeng/table'
// Custom
import { BerthListVM } from '../classes/view-models/berth-list-vm'
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { filter } from 'rxjs'

@Component({
    selector: 'berth-available-list',
    templateUrl: './berth-available-list.component.html',
    styleUrls: ['../../../../assets/styles/custom/lists.css', 'berth-available-list.component.css']
})

export class BerthAvailableListComponent {

    //#region variables

    @ViewChild('table') table: Table

    private url = 'berths'
    private virtualElement: any
    public feature = 'berthAvailableList'
    public featureIcon = 'berths'
    public form: FormGroup
    public icon = 'home'
    public parentUrl = '/home'
    public records: BerthListVM[] = []
    public recordsFilteredCount = 0

    //#endregion

    //#region context menu

    public menuItems!: MenuItem[]
    public selectedRecord!: BerthListVM

    //#endregion

    //#region availability filters

    public selectedRecords: BerthListVM[] = []
    public occupied = 0
    public available = 0

    public berthAvailabilityOptions: any[] = [
        { label: 'All', value: 'all' },
        { label: 'Occupied', value: 'occupied' },
        { label: 'Available', value: 'available' }
    ]

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateHelperService: DateHelperService, private dialogService: DialogService, private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.loadRecords().then(() => {
            this.occupied = this.records.filter((x) => x.boatName != 'AVAILABLE').length
            this.available = this.records.filter((x) => x.boatName == 'AVAILABLE').length
            this.setSidebarsHeight()
        })
    }

    ngAfterViewInit(): void {
        // setTimeout(() => {
        //     this.getVirtualElement()
        //     this.scrollToSavedPosition()
        //     this.hightlightSavedRow()
        //     this.enableDisableFilters()
        // }, 500)
    }

    //#endregion

    //#region public methods

    public fixMe(): void {
        if (this.form.value.value == 'all') { this.helperService.clearTableTextFilters(this.table, ['description', 'boatName', 'toDate']) }
        if (this.form.value.value == 'occupied') { this.table.filter('AVAILABLE', 'boatName', 'notEquals') }
        if (this.form.value.value == 'available') { this.table.filter('AVAILABLE', 'boatName', 'equals') }
    }

    public formatDateToLocale(date: string, showWeekday: boolean, showYear: boolean, returnEmptyString: boolean): string {
        return returnEmptyString && date == '2199-12-31' ? '' : this.dateHelperService.formatISODateToLocale(date, showWeekday, showYear)
    }

    public getEmoji(anything: any): string {
        return typeof anything == 'string'
            ? this.emojiService.getEmoji(anything)
            : anything ? this.emojiService.getEmoji('green-box') : this.emojiService.getEmoji('red-box')
    }

    public getWarningEmoji(isOverdue: boolean): string {
        return isOverdue ? this.emojiService.getEmoji('warning') : ''
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getOverdueDescription(isOverdue: boolean): string {
        return isOverdue ? 'YES' : ''
    }

    public getBoatNameColor(boatName: string): string {
        switch (boatName) {
            case 'AVAILABLE':
                return 'green'
            default:
                return ''
        }
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public onEditRecord(id: number): void {
        this.storeScrollTop()
        this.storeSelectedId(id)
        this.navigateToRecord(id)
    }

    public onFilter(event: any, column: string, matchMode: string): void {
        if (event) this.table.filter(event, column, matchMode)
    }

    public onFilterRecords(event: any): void {
        this.sessionStorageService.saveItem(this.feature + '-' + 'filters', JSON.stringify(this.table.filters))
        this.recordsFilteredCount = event.filteredValue.length
    }

    public onHighlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public onNewRecord(): void {
        this.router.navigate([this.url + '/new'])
    }

    public onResetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table, ['description', 'boatName', 'toDate'])
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

    private filterColumn(element: { value: any }, field: string, matchMode: string): void {
        if (element != undefined && (element.value != null || element.value != undefined)) {
            this.table.filter(element.value, field, matchMode)
        }
    }

    private filterTableFromStoredFilters(): void {
        const filters = this.sessionStorageService.getFilters(this.feature + '-' + 'filters')
        if (filters != undefined) {
            setTimeout(() => {
                this.filterColumn(filters.boatName, 'boatName', 'contains')
                this.filterColumn(filters.customer, 'customer', 'contains')
                this.filterColumn(filters.loa, 'loa', 'contains')
                this.filterColumn(filters.fromDate, 'fromDate', 'contains')
                this.filterColumn(filters.toDate, 'toDate', 'contains')
                this.filterColumn(filters.joinedBerths, 'joinedBerths', 'contains')
                this.filterColumn(filters.paymentStatus, 'paymentStatus', 'in')
                this.filterColumn(filters.isOverdue, 'isOverdue', 'contains')
            }, 500)
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

    private initContextMenu(): void {
        this.menuItems = [
            { label: this.getLabel('contextMenuEdit'), command: () => this.onEditRecord(this.selectedRecord.id) }
        ]
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            value: new FormControl('all'),
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

    private scrollToSavedPosition(): void {
        this.helperService.scrollToSavedPosition(this.virtualElement, this.feature)
    }

    private setSidebarsHeight(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private storeSelectedId(id: number): void {
        this.sessionStorageService.saveItem(this.feature + '-id', id.toString())
    }

    private storeScrollTop(): void {
        this.sessionStorageService.saveItem(this.feature + '-scrollTop', this.virtualElement.scrollTop)
    }

    //#endregion

}
