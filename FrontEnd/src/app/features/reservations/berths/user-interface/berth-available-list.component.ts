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

@Component({
    selector: 'berth-available-list',
    templateUrl: './berth-available-list.component.html',
    styleUrls: ['../../../../../assets/styles/custom/lists.css', 'berth-available-list.component.css']
})

export class BerthAvailableListComponent {

    //#region variables

    @ViewChild('table') table: Table

    private url = 'berths'
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
    public isOccupied = 0
    public isAvailable = 0
    public isAthenian = 0
    public isFishingBoat = 0
    public isDryDock = 0

    public berthAvailabilityOptions: any[] = [
        { label: this.getLabel("allFilter"), value: 'all' },
        { label: this.getLabel("occupiedFilter"), value: 'occupied' },
        { label: this.getLabel("availableFilter"), value: 'available' }
    ]

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateHelperService: DateHelperService, private dialogService: DialogService, private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.loadRecords().then(() => {
            this.isOccupied = this.records.filter(x => x.boatName != 'AVAILABLE').length
            this.isAvailable = this.records.filter(x => x.boatName == 'AVAILABLE').length + this.records.filter(x => x.isDryDock).length
            this.isAthenian = this.records.filter(x => x.isAthenian).length
            this.isFishingBoat = this.records.filter(x => x.isFishingBoat).length
            this.isDryDock = this.records.filter(x => x.isDryDock).length
            this.setSidebarsHeight()
        })
    }

    //#endregion

    //#region public methods

    public filterBySelection(): void {
        if (this.form.value.value == 'all') { this.helperService.clearTableTextFilters(this.table, ['description', 'boatName', 'toDate']) }
        if (this.form.value.value == 'occupied') { this.table.filter('AVAILABLE', 'boatName', 'notEquals') }
        if (this.form.value.value == 'available') { this.table.filter('AVAILABLE', 'boatName', 'equals') }
    }

    public formatDateToLocale(date: string, showWeekday: boolean, showYear: boolean, returnEmptyString: boolean): string {
        return returnEmptyString && date == '2199-12-31' ? '' : this.dateHelperService.formatISODateToLocale(date, showWeekday, showYear)
    }

    public getKnobBackgroundColor(): string {
        return "LightSlateGray"
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getYesOrNo(isOverdue: boolean): string {
        return isOverdue ? 'YES' : ''
    }

    public getBoatName(boatName: string): string {
        return ' ' + boatName
    }

    public getColor(boatName: string, isDryDock: boolean): string {
        switch (boatName) {
            case 'AVAILABLE':
                return 'color-green'
            default:
                return isDryDock ? 'color-orange' : 'color-red'
        }
    }

    public getIcon(): string {
        return 'adjust'
    }

    public getStar(field: boolean): string {
        return field ? 'star' : ''
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
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

    private goBack(): void {
        this.router.navigate([this.parentUrl])
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

    private setSidebarsHeight(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    //#endregion

}
